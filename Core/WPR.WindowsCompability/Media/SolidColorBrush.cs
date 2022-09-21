namespace WPR.WindowsCompability.Media
{
    // TODO: Comply with official documentation!
    public class SolidColorBrush
    {
        public Color Color { get; set; }

        internal SolidColorBrush()
        {
            Color = new Color();
        }
    }
}
