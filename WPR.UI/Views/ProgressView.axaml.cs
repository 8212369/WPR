using Avalonia.Controls;
using Avalonia.ReactiveUI;

using System;

namespace WPR.UI.Views
{
    public partial class ProgressView : UserControl
    {
        private Label? ProgressTextLabel;
        private ProgressBar? ProgressBar;

        private static int MaxIntendedWidth = 500;
        private static int MaxIntendedHeight = 120;

        public delegate void OnCancelRequested(object args);
        public event OnCancelRequested CancelRequested;

        public ProgressView()
        {
            InitializeComponent();

            this.AttachedToVisualTree += (obj, args) =>
            {
                Width = Math.Min(VisualRoot!.ClientSize.Width * 90 / 100, MaxIntendedWidth);
                Height = Math.Min(VisualRoot!.ClientSize.Height * 20 / 100, MaxIntendedHeight);
            };

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
