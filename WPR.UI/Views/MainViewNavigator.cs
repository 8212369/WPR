using System;
using WPR.UI.Pages;
using WPR.UI.ViewModels;
using Avalonia;
using Avalonia.Controls;

namespace WPR.UI.Views
{
    public class MainViewNavigator
    {
        private int _CurrentIndex = -1;
        private UserControl[] _Pages = new UserControl[4];

        public void SetupNavigation(TabControl control, TransitioningContentControl contentControl)
        {
            _CurrentIndex = 0;

            _Pages[0] = new ApplicationListingPage();

            contentControl.Content = _Pages[0];

            control.SelectionChanged += (obj, args) =>
            {
                if (_CurrentIndex != control.SelectedIndex)
                {
                    _CurrentIndex = control.SelectedIndex;

                    if (_Pages[_CurrentIndex] == null)
                    {
                        switch (_CurrentIndex)
                        {
                            case 1:
                                _Pages[1] = new SettingsPage();
                                break;

                            case 2:
                                _Pages[2] = new AboutPage();
                                break;
                        }
                    }

                    var previousControl = contentControl.Content as UserControl;

                    // Workaround control not displaying back
                    // TODO: Report to the developers
                    contentControl.Content = _Pages[_CurrentIndex];
                    contentControl.Content = null;
                    contentControl.Content = _Pages[_CurrentIndex];
                }
            };
        }
    }
}
