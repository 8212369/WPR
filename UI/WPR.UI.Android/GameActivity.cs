using Android.App;
using Android.Content;
using Android.OS;

using Org.Libsdl.App;
using Newtonsoft.Json;

using System;
using System.Runtime.InteropServices;
using Android.Content.PM;

namespace WPR.UI.Android
{
    [Activity(Label = "Game activity", Theme = "@style/MyTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Landscape, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class GameActivity : SDLActivity
    {
        public static string TargetApplicationDataName = "TargetApplication";
        public static string ErrorDataName = "Error";

        private static Models.Application? TargetLaunchApplication;
        private static GameActivity CurrentActivity;

        [DllImport("main")]
        private static extern void SetMain(System.Action main);

        public override void LoadLibraries()
        {
            base.LoadLibraries();
            SetMain(SDLMain);
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CurrentActivity = this;
            MessageBoxUtils.MainActivity = this;

            string ?targetApplication = Intent!.GetStringExtra(TargetApplicationDataName);

            if (targetApplication == null)
            {
                return;
            }

            TargetLaunchApplication = JsonConvert.DeserializeObject<Models.Application>(targetApplication);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
        }

        public static void SDLMain()
        {
            // Should not be possible
            if (TargetLaunchApplication == null)
            {
                Common.Log.Error(Common.LogCategory.AppList, "Empty target application to launch!");
                return;
            }

            try
            {
                ApplicationLaunch.Start(TargetLaunchApplication!, orientation =>
                {
                    CurrentActivity.RunOnUiThread(() =>
                    {
                        if (orientation == Microsoft.Xna.Framework.DisplayOrientation.Portrait)
                        {
                            CurrentActivity.RequestedOrientation = ScreenOrientation.Portrait;
                        }
                        else
                        {
                            CurrentActivity.RequestedOrientation = ScreenOrientation.Landscape;
                        }
                    });
                }).Wait();
            }
            catch (Exception ex)
            {
                Intent errorIntent = new Intent();
                errorIntent.PutExtra(ErrorDataName, ex.ToString());

                CurrentActivity.SetResult(Result.FirstUser, errorIntent);
                return;
            }

            CurrentActivity.SetResult(Result.Ok);
        }
    }
}
