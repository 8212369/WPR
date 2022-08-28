using System;

namespace Microsoft.Phone.Tasks
{
    public abstract class ChooserBase<TTaskEventArgs> where TTaskEventArgs : TaskEventArgs
    {
        public ChooserBase()
        {
        }

        public event EventHandler<TTaskEventArgs>? Completed;
    }
}
