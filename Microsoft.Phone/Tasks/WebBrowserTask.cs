using System;
using System.Diagnostics;

namespace Microsoft.Phone.Tasks
{
    public class WebBrowserTask
    {
        public WebBrowserTask()
        {

        }

        public string? URL { get; set; }

        public void Show()
        {
            if (URL == null)
            {
                return;
            }

            Process.Start("explorer", URL);
        }
    }
}
