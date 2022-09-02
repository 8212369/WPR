using Avalonia.Controls;
using Avalonia.ReactiveUI;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using Avalonia.Threading;
using MessageBox.Avalonia;
using System.Reactive.Linq;
using WPR.Models;
using WPR.Common;

namespace WPR.UI.Pages
{
    public partial class ApplicationListingPage : ReactiveUserControl<ApplicationListingPageViewModel>
    {
        private List<FileDialogFilter> AppInstallFileFilters;

        public ApplicationListingPage()
        {
            InitializeComponent();
            DataContext = new ApplicationListingPageViewModel();

            ViewModel!.AppPrepareLaunch += () => GetWindow().Hide();
            ViewModel!.AppDone += async runOk =>
            {
                GetWindow().Show();
                if (!runOk)
                {
                    var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                        title: Properties.Resources.AppRunError,
                        text: Properties.Resources.ExceptionRunApp,
                        icon: MessageBox.Avalonia.Enums.Icon.Error,
                        windowStartupLocation: WindowStartupLocation.CenterScreen);

                    await msgBox.ShowDialog(GetWindow());
                }
            };

            AppInstallFileFilters = new List<FileDialogFilter>
            {
                new FileDialogFilter
                {
                    Name = "XAP file",
                    Extensions = new List<string> { "xap" }
                },
                new FileDialogFilter
                {
                    Name = "All files",
                    Extensions = new List<string> { "*" }
                }
            };

            this.Get<Button>("addNewAppButton").Click += async delegate
            {
                var result = await new OpenFileDialog()
                {
                    Title = "Choose XAP file",
                    Filters = AppInstallFileFilters
                }.ShowAsync(GetWindow());

                if (result != null)
                {
                    var InstallProgressWindow = new ProgressWindow();
                    InstallProgressWindow.Title = Properties.Resources.InstallingApp;
                    InstallProgressWindow.CancelRequested += obj => ViewModel!.CancelSource!.Cancel();

                    ViewModel!.InstallationSetProgress += progress => Dispatcher.UIThread.InvokeAsync(() => InstallProgressWindow.Progress = progress);
                    ViewModel!.DeleteExistingAppInteraction!.RegisterHandler(context => Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        {
                            Application app = context.Input;

                            var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                                title: Properties.Resources.ApplicationAlreadyInstalled,
                                text: String.Format(Properties.Resources.ApplicationAlreadyInstalledDescription, app.Name),
                                icon: MessageBox.Avalonia.Enums.Icon.Question,
                                windowStartupLocation: WindowStartupLocation.CenterScreen,
                                @enum: MessageBox.Avalonia.Enums.ButtonEnum.YesNo);

                            MessageBox.Avalonia.Enums.ButtonResult result = await msgBox.ShowDialog(InstallProgressWindow);
                            context.SetOutput(result == MessageBox.Avalonia.Enums.ButtonResult.Yes);
                        }
                    }));

                    InstallProgressWindow.WhenAnyValue(v => v.IsVisible)
                        .Subscribe(async v => {
                            if (!v)
                            {
                                return;
                            }

                            var err = await ViewModel!.InstallRequestCommand.Execute(result[0]);
                            
                            string errUserStr = LocaleUtils.GetDisplayName(err);
                            bool failed = err != ApplicationInstallError.None;

                            var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                                title: failed ? Properties.Resources.InstallationFailed : Properties.Resources.InstallationSucceed,
                                text: errUserStr,
                                icon: failed ? MessageBox.Avalonia.Enums.Icon.Error : MessageBox.Avalonia.Enums.Icon.Success,
                                windowStartupLocation: WindowStartupLocation.CenterScreen);

                            await msgBox.ShowDialog(InstallProgressWindow);
                            ViewModel!.UpdateApplicationList(ViewModel!.SearchText);

                            InstallProgressWindow.Close();
                        });

                    await InstallProgressWindow.ShowDialog(GetWindow());
                }
            };
        }

        Window GetWindow() => VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
    }
}
