using Avalonia.Controls;

namespace WPR.UI.Views
{
    public partial class MainViewMobile : UserControl
    {
        private MainViewNavigator _Navigator;

        public MainViewMobile()
        {
            InitializeComponent();

            _Navigator = new MainViewNavigator();
            _Navigator.SetupNavigation(this.Get<TabControl>("navigationControl"), this.Get<TransitioningContentControl>("contentControl"));
        }
    }
}
