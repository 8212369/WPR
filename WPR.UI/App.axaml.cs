using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using WPR.Common;

using System.IO;

namespace WPR.UI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            Filesystem.CopyFilesRecursively(Path.Combine(Directory.GetCurrentDirectory(), "Database"),
                Configuration.Current.DataPath("Database"));
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindowDesktop
                {
                    DataContext = new MainWindowViewModel(),
                };
            } else if (ApplicationLifetime is ISingleViewApplicationLifetime mobile)
            {
                mobile.MainView = new MainViewMobile
                {
                    DataContext = new MainWindowViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
