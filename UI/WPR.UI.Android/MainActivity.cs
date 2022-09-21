using Android.App;
using Android.Content;
using Android.Content.PM;
using AndroidX.Activity.Result.Contract;
using AndroidX.Activity.Result;
using Avalonia.Android;
using Avalonia;

using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

using System.IO;
using Android.OS;
using Avalonia.ReactiveUI;

using Newtonsoft.Json;
using WPR.Common;
using System.Collections.Generic;

#if !DEBUG
using Xamarin.Android.AssemblyStore;
#else
using System.IO.Compression;
#endif

namespace WPR.UI.Android
{
    internal class GameActivityResultCallback : Java.Lang.Object, IActivityResultCallback
    {
        private MainActivity _Owning;

        public GameActivityResultCallback(MainActivity activity)
        {
            _Owning = activity;
        }

        public void OnActivityResult(Java.Lang.Object result)
        {
            MessageBoxUtils.MainActivity = _Owning;
            Directory.SetCurrentDirectory(_Owning.CurrentDirectoryForMain);

            ActivityResult resultAct = (result as ActivityResult)!;
            if (resultAct.ResultCode != (int)Result.Ok)
            {
                Log.Error(LogCategory.AppList, $"Game run error: {resultAct.Data.GetStringExtra(GameActivity.ErrorDataName)!}");

                new AlertDialog.Builder(MessageBoxUtils.MainActivity!)!
                    .SetTitle(Properties.Resources.AppRunError)!
                    .SetMessage(Properties.Resources.ExceptionRunApp)!
                    .Show();
            }
        }
    }

    [Activity(Label = "WPR.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : AvaloniaActivity<App>
    {
        private ActivityResultLauncher ActivitySpawner;
        private static List<string> CopyAssemblyList = new List<string>
        {
            "FNA"
        };

        public string CurrentDirectoryForMain => Path.Combine(GetExternalFilesDir(null)!.AbsolutePath, "PatchAssemblies");

        public MainActivity()
        {
            ActivitySpawner = RegisterForActivityResult(new ActivityResultContracts.StartActivityForResult(),
                new GameActivityResultCallback(this));
        }

        // Because DLLs files are in APK. Monodroid has their own way of extracting and getting these dlls out.
        // But Cecil just read it from stream. It's hard. So we extract a subset of needed DLLs beforehand
        public void SetupDllPatchForCecil()
        {
            string basePath = CurrentDirectoryForMain;
            Directory.CreateDirectory(basePath);

            string? apkPath = Application?.ApplicationInfo?.PublicSourceDir;
            if (apkPath == null)
            {
                Log.Warn(LogCategory.Android, "Unable to copy DLLs needed for patching! Some games may fail to patch!");
                return;
            }
#if DEBUG

            using (ZipArchive archive = ZipFile.Open(apkPath, ZipArchiveMode.Read))
            {
                foreach (var dll in CopyAssemblyList)
                {
                    ZipArchiveEntry? entry = archive.GetEntry($"assemblies/{dll}.dll");
                    if (entry == null)
                    {
                        Log.Warn(LogCategory.Android, $"Fail to copy DLL ${dll} to patch assembly folder!");
                    }
                    else
                    {
                        entry.ExtractToFile(Path.Combine(basePath, dll), true);
                    }
                }
            }
#else
            AssemblyStoreExplorer explorer = new AssemblyStoreExplorer(apkPath, keepStoreInMemory: true);
            foreach (var dll in CopyAssemblyList)
            {
                string filename = $"{dll}.dll.comp";
                string filenameAuth = $"{dll}.dll";

                if (explorer.AssembliesByName.ContainsKey(dll))
                {
                    explorer.AssembliesByName[dll].ExtractImage(basePath, filename);
                }
                else
                {
                    Log.Warn(LogCategory.Android, $"Fail to copy DLL ${dll} to patch assembly folder (entry not found)!");
                    continue;
                }

                bool fileShouldMove = false;

                using (FileStream stream = new FileStream(Path.Combine(basePath, filename), FileMode.Open, FileAccess.Read))
                {
                    if (AssemblyDecompressor.IsCompressed(stream))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        
                        using (FileStream streamAuth = new FileStream(Path.Combine(basePath, filenameAuth), FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            if (!AssemblyDecompressor.Work(stream, streamAuth))
                            {
                                Log.Warn(LogCategory.Android, $"Fail to decompress DLL ${dll} to patch assembly folder (entry not found)!");
                            }
                        }
                    } else
                    {
                        fileShouldMove = true;
                    }
                }

                if (fileShouldMove)
                {
                    File.Move(Path.Combine(basePath, filename), Path.Combine(basePath, filenameAuth));
                } else
                {
                    File.Delete(Path.Combine(basePath, filename));
                }
            }
#endif

            Directory.SetCurrentDirectory(basePath);
        }

        public void SetupConfigurationAndDatabase()
        {
            Configuration.Current = new Configuration(GetExternalFilesDir(null)!.AbsolutePath);

            Filesystem.CopyFolderFromAssets(Assets!, "Database/TrueAchievements", Configuration.Current.DataPath("Database/TrueAchievements"));

            if (!File.Exists(Configuration.Current.DataPath("Database/achievements.db")))
            {
                Filesystem.CopyFileFromAssets(Assets!, "Database/achievements.db", Configuration.Current.DataPath("Database/achievements.db"));
            }

            if (!File.Exists(Configuration.Current.DataPath("Database/applications.db")))
            {
                Filesystem.CopyFileFromAssets(Assets!, "Database/applications.db", Configuration.Current.DataPath("Database/applications.db"));
            }
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            SetupConfigurationAndDatabase();
            SetupDllPatchForCecil();

            MessageBoxUtils.MainActivity = this;
            ServicesSetup.Start();
            NativeUI.Initialize(this);

            ApplicationLaunchRequest.Incoming += (sender, args) =>
            {
                RunOnUiThread(() =>
                {
                    var launchIntent = new Intent(this, typeof(GameActivity));
                    launchIntent.PutExtra(GameActivity.TargetApplicationDataName, JsonConvert.SerializeObject(args.Target));

                    ActivitySpawner.Launch(launchIntent);
                });
            };

            base.OnCreate(savedInstanceState);
        }

        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder)
                .LogToTrace()
                .UseReactiveUI()
                .WithIcons(container => container
                    .Register<FontAwesomeIconProvider>());
        }
    }
}
