using System;

namespace Microsoft.Phone.Tasks
{
    public class TaskEventArgs : EventArgs
    {
        public TaskEventArgs(TaskResult result)
        {
            this.TaskResult = result;
        }

        public virtual TaskResult TaskResult { get; internal set; }
        public virtual Exception Error { get; internal set; }
    }
}
