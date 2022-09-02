using ReactiveUI;
using System.Threading.Tasks;
using WPR.Models;
using WPR.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Reactive;
using Avalonia.Threading;
using System;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace WPR.UI.ViewModels
{
    public class ApplicationListingPageViewModel : ViewModelBase
    {
        internal delegate void OnProgressNeedSet(int value);
        internal event OnProgressNeedSet? InstallationSetProgress;

        internal delegate void OnPrepareAppLaunch();
        internal delegate void OnAppDone(bool runOk);
        internal event OnPrepareAppLaunch? AppPrepareLaunch;
        internal event OnAppDone? AppDone;

        private string _SearchText;
        private ObservableCollection<ApplicationItemViewModel> _Applications;
        private ApplicationItemViewModel? _ChoosenApp;

        public ReactiveCommand<string, ApplicationInstallError> InstallRequestCommand;
        public ReactiveCommand<string, Unit> AppSearchCommand;
        public Interaction<Application, bool> DeleteExistingAppInteraction;

        public string SearchText
        {
            get { return _SearchText; }
            set { this.RaiseAndSetIfChanged(ref _SearchText, value); }
        }

        public ApplicationItemViewModel? ChoosenApp
        {
            get { return _ChoosenApp; }
            set
            {
                AppPrepareLaunch?.Invoke();
                _ChoosenApp = value;

                NativeUI.NotificationManager.ShowNotification(new DesktopNotifications.Notification()
                {
                    Title = Properties.Resources.LaunchingInProcess,
                    Body = _ChoosenApp!.Name!,
                    ImagePath = _ChoosenApp.App.IconPath
                }, expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));

                bool runOk = true;

                try
                {
                    ApplicationLaunch.Start(_ChoosenApp!.App);
                } catch (Exception ex)
                {
                    Log.Error(LogCategory.AppList, $"Game run error: \n{ex}");
                    runOk = false;
                }

                AppDone?.Invoke(runOk);
            }
        }

        public CancellationTokenSource? CancelSource { get; set; }
        public ObservableCollection<ApplicationItemViewModel> Applications {
            get { return _Applications; }
            set { this.RaiseAndSetIfChanged(ref _Applications, value); }
        }

        public void UpdateApplicationList(string text)
        {
            try
            {
                var enumerable = ApplicationContext.Current.Applications!
                        .Where(app => app.Name.ToLower().Contains((text != null) ? text.ToLower() : ""))
                        .OrderBy(app => app.Name.ToLower())
                        .Select(app => new ApplicationItemViewModel(app))
                        .AsEnumerable();

                _ChoosenApp = null;

                // So it can hear change. Replace the ref only does not make it refresh display
                Applications = new ObservableCollection<ApplicationItemViewModel>(enumerable);
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.AppList, $"Unable to query application database with exception:\n {ex}");
                Applications = new ObservableCollection<ApplicationItemViewModel>();
            }
        }

        public ApplicationListingPageViewModel()
        {
            Applications = new ObservableCollection<ApplicationItemViewModel>();
            DeleteExistingAppInteraction = new Interaction<Application, bool>();

            InstallRequestCommand = ReactiveCommand.CreateFromTask<string, ApplicationInstallError>(
                async path =>
                {
                    CancelSource = new CancellationTokenSource();

                    return await ApplicationInstaller.Install(path,
                        progressValue => InstallationSetProgress!.Invoke(progressValue),
                        app => DeleteExistingAppInteraction!.Handle(app),
                        CancelSource.Token);
                });

            this.WhenAnyValue(v => v.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(20))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(text => UpdateApplicationList(text));
        }
    }
}
