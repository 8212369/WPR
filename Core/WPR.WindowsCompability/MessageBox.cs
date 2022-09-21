using System.Threading.Tasks;

namespace System.Windows
{
    public static class MessageBox
    {
        public static Func<string, string, MessageBoxButton, Task<MessageBoxResult>> ShowSimpleImpl;

        public static MessageBoxResult Show(string title, string caption, MessageBoxButton buttons)
        {
            return ShowSimpleImpl(title, caption, buttons).GetAwaiter().GetResult();
        }
    }

}
