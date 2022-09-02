using System;

namespace WPR.WindowsCompability
{
    public class Application
    {
        private static Application? _Current;
        public event EventHandler<ApplicationUnhandledExceptionEventArgs>? UnhandledException;
        public string? ProductId { get; set; }

        public static Application Current {
            get {
                if (_Current == null)
                {
                    _Current = new Application();
                }
                return _Current;
            }
        }
    }
}