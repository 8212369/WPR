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
using Avalonia.Platform.Storage;

namespace WPR.UI.Pages
{
    public partial class ApplicationListingPage : ReactiveUserControl<ApplicationListingPageViewModel>
    {
        private List<FilePickerFileType> AppInstallFileFilters;

        public ApplicationListingPage()
        {
            InitializeComponent();
            DataContext = new ApplicationListingPageViewModel();

            AppInstallFileFilters = new List<FilePickerFileType>
            {
                new FilePickerFileType("XAP file")
                {
                    Patterns = new List<string> { "*.xap" }
                },
                new FilePickerFileType("All files")
                {
                    Patterns = new List<string> { "*.*" }
                }
            };

            this.Get<Button>("addNewAppButton").Click += async delegate
            {
                var result = await GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = "Choose XAP file",
                    FileTypeFilter = AppInstallFileFilters
                });

                if ((result != null) && (result.Count >= 1))
                {
                    var InstallProgressWindow = new ProgressView();
                    InstallProgressWindow.CancelRequested += obj => ViewModel!.CancelSource!.Cancel();

                    ViewModel!.InstallationSetProgress += progress => Dispatcher.UIThread.InvokeAsync(() => InstallProgressWindow.Progress = progress);
                    ViewModel!.DeleteExistingAppInteraction!.RegisterHandler(context => Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        {
                            Application app = context.Input;

                            MessageBox.Avalonia.Enums.ButtonResult result = await MessageBoxUtils.GetMessageDialogResult(
                                title: Properties.Resources.ApplicationAlreadyInstalled,
                                text: String.Format(Properties.Resources.ApplicationAlreadyInstalledDescription, app.Name),
                                icon: MessageBox.Avalonia.Enums.Icon.Question,
                                buttons: MessageBox.Avalonia.Enums.ButtonEnum.YesNo);

                            context.SetOutput(result == MessageBox.Avalonia.Enums.ButtonResult.Yes);
                        }
                    }));

                    InstallProgressWindow.WhenAnyValue(v => v.IsVisible)
                        .Subscribe(async v => {
                            if (!v)
                            {
                                return;
                            }

                            var err = await ViewModel!.InstallRequestCommand.Execute(await result[0].OpenReadAsync());
                            
                            string errUserStr = LocaleUtils.GetDisplayName(err);
                            bool failed = err != ApplicationInstallError.None;

                            await MessageBoxUtils.GetMessageDialogResult(
                                title: failed ? Properties.Resources.InstallationFailed : Properties.Resources.InstallationSucceed,
                                text: errUserStr,
                                icon: failed ? MessageBox.Avalonia.Enums.Icon.Error : MessageBox.Avalonia.Enums.Icon.Success);

                            ViewModel!.UpdateApplicationList(ViewModel!.SearchText);

                            DialogHost.DialogHost.Close(null);
                        });

                    await DialogHost.DialogHost.Show(InstallProgressWindow);
                }
            };
        }

        Window GetWindow() => VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
        TopLevel GetTopLevel() => VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        IStorageProvider GetStorageProvider() => GetTopLevel().StorageProvider;
    }
}
