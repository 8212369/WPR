using Avalonia.Controls;

namespace WPR.UI.Views
{
    public partial class MainWindowDesktop : Window
    {
        private MainViewNavigator _Navigator;

        public MainWindowDesktop()
        {
            InitializeComponent();

            _Navigator = new MainViewNavigator();
            _Navigator.SetupNavigation(this.Get<TabControl>("navigationControl"), this.Get<TransitioningContentControl>("contentControl"));
        }
    }
}
