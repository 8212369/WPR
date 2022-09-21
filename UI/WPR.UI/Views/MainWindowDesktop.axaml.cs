using Avalonia.Controls;
using System;
using WPR.Common;
using WPR.Models;

using Newtonsoft.Json;

namespace WPR.UI.Views
{
    public partial class MainWindowDesktop : Window
    {
        private MainViewNavigator _Navigator;

        public MainWindowDesktop()
        {
            InitializeComponent();

#if !__MOBILE__
            MessageBoxUtils.MainWindow = this;
            ServicesSetup.Start();

            ApplicationLaunchRequest.Incoming += async (sender, args) =>
            {
                Hide();

                _ = NativeUI.NotificationManager.ShowNotification(new DesktopNotifications.Notification()
                {
                    Title = Properties.Resources.LaunchingInProcess,
                    Body = args.Target.Name!,
                    ImagePath = Configuration.Current!.DataPath(args.Target.IconPath)
                }, expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));

                bool runOk = true;

                var test = JsonConvert.SerializeObject(args.Target);
                Console.WriteLine(test);

                try
                {
                    await ApplicationLaunch.Start(args.Target);
                }
                catch (Exception ex)
                {
                    Log.Error(LogCategory.AppList, $"Game run error: \n{ex}");
                    runOk = false;
                }

                Show();

                if (!runOk)
                {
                    await MessageBoxUtils.GetMessageDialogResult(
                        title: Properties.Resources.AppRunError,
                        text: Properties.Resources.ExceptionRunApp,
                        icon: MessageBox.Avalonia.Enums.Icon.Error);
                }
            };
#endif

            _Navigator = new MainViewNavigator();
            _Navigator.SetupNavigation(this.Get<TabControl>("navigationControl"), this.Get<TransitioningContentControl>("contentControl"));
        }
    }
}
