using Avalonia.Controls;

namespace WPR.UI.Views
{
    public partial class ProgressWindow : Window
    {
        private Label? ProgressTextLabel;
        private ProgressBar? ProgressBar;

        public delegate void OnCancelRequested(object args);
        public event OnCancelRequested CancelRequested;

        public ProgressWindow()
        {
            InitializeComponent();

            ProgressTextLabel = this.Get<Label>("progressBarValue");
            ProgressBar = this.Get<ProgressBar>("progressBar");

            this.Get<Button>("cancelButton").Click += (obj, args) => CancelRequested!.Invoke(obj);
        }

        public int Progress
        {
            get { return (int)ProgressBar!.Value; }
            set
            {
                ProgressBar!.Value = value;
                ProgressTextLabel!.Content = $"{value}%";
            }
        }
    }
}
