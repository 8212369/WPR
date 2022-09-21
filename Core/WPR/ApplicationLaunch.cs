using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Runtime.Loader;
using WPR.Models;
using WPR.Common;

using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.GamerServices;

using System;
using System.IO;
using System.Threading.Tasks;
using WPR.XnaCompability;

namespace WPR
{
    public static class ApplicationLaunch
    {
        private static string CurrentProductFolder => Path.Combine(Configuration.Current.DataPath(Application.DataStoreFolder),
            WindowsCompability.Application.Current!.ProductId!);

        static ApplicationLaunch()
        {
            AssemblyLoadContext.Default.Resolving += (loadContext, name) =>
            {
                return loadContext.LoadFromAssemblyPath(Path.Combine(CurrentProductFolder, name.Name + ".dll"));
            };
        }

        public static async Task Start(Application app, Action<DisplayOrientation>? requestOrientation = null)
        {
            if (app.ApplicationType != ApplicationType.XNA)
            {
                throw new NotSupportedException("Only XNA app is supported!");
            }

            // Setting game folder path
            WindowsCompability.Application.Current.ProductId = app.ProductId;
            string folderPath = CurrentProductFolder;

            FNAPlatform.TitleLocation = folderPath;
            string curDir = Directory.GetCurrentDirectory();

            Assembly assem = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(folderPath, AssemblyNameStandardization.Process(app.Assembly)));

            Directory.SetCurrentDirectory(folderPath);

            // Instatiate
            Type? mainType = assem.GetType(app.EntryPoint);

            // Run on separate thread to not affect the UI
            await Task.Run(() =>
            {
                using (Game? obj = Activator.CreateInstance(mainType!) as Game)
                {
                    obj!.IsMouseVisible = true;
                    obj!.Window.Title = $"{app.Name} - {app.Author} (Publisher: {app.Publisher})";

#if !__MOBILE__
                    TouchPanel.MouseAsTouch = true;
#endif
                    TouchPanel.EnabledGestures = GestureType.DoubleTap | GestureType.Tap | GestureType.Hold |
                        GestureType.HorizontalDrag | GestureType.VerticalDrag | GestureType.FreeDrag |
                        GestureType.Pinch | GestureType.Flick | GestureType.DragComplete | GestureType.PinchComplete;

                    GraphicsDeviceManager2.RequestOrientation = requestOrientation;
                    SignedInGamer.Reset();

                    obj.Activated += (obj, args) =>
                    {
                        PhoneApplicationService.Current!.HandleApplicationStart(true);
                    };

                    GraphicsDeviceManager? manager = obj.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
                    if (manager != null)
                    {
                        manager.PreparingDeviceSettings += (obj, args) =>
                        {
                            GraphicsDeviceManager2.RequestOrientationChange(
                                args.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth,
                                args.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight
                            );
                        };
                    }

                    obj.Run();

                    try
                    {
                        PhoneApplicationService.Current!.HandleApplicationExit();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(LogCategory.AppList, $"Ignored clean-up exception:\n {ex}");
                    }

                    obj.Exit();
                }
            });
        }
    }
}
