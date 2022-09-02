using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Scheduler
{
    public class ScheduledActionService
    {
        private static Dictionary<string, ScheduledAction> Actions;

        static ScheduledActionService()
        {
            Actions = new Dictionary<string, ScheduledAction>();
        }

        public static ScheduledAction? Find(string name)
        {
            return Actions.ContainsKey(name) ? Actions[name] : null;
        }

        public static void Add(ScheduledAction action)
        {
            if (Actions.ContainsKey(action.Name!))
            {
                throw new ArgumentException($"The task with the name: {action.Name} has already been scheduled!");
            }

            Actions.Add(action.Name!, action);
        }
    }
}
