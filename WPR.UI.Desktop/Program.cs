using Avalonia;
using Avalonia.ReactiveUI;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;
using System.IO;

using WPR.WindowsCompability;
using System.Linq;

using WPR.Common;

namespace WPR.UI.Desktop
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            Configuration.Current = new Configuration(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WPR"));

            Filesystem.CopyFilesRecursively(Path.Combine(Directory.GetCurrentDirectory(), "Database\\TrueAchievements"),
                Configuration.Current.DataPath("Database\\TrueAchievements"));

            if (!File.Exists(Configuration.Current.DataPath("Database\\achievements.db")))
            {
                File.Copy("Database\\achievements.db", Configuration.Current.DataPath("Database\\achievements.db"));
            }

            if (!File.Exists(Configuration.Current.DataPath("Database\\applications.db")))
            {
                File.Copy("Database\\applications.db", Configuration.Current.DataPath("Database\\applications.db"));
            }

            NativeUI.Initialize();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .WithIcons(container => container
                    .Register<FontAwesomeIconProvider>());
    }
}
