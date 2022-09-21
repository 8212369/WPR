using Microsoft.Xna.Framework.GamerServices;
using System.Linq;

namespace WPR.UI
{
    public static class ServicesSetup
    {
        public static void Start()
        {
            Guide.ShowInputBoxFunc = async (title, description, defaultText) =>
            {
                return await MessageBoxUtils.GetInputResult(title, description, defaultText, false, true);
            };

            Guide.ShowMessageBoxFunc = async (title, description, buttonNames, currentActiveButton, icon) =>
            {
                MessageBox.Avalonia.Enums.Icon messageBoxIcon = MessageBox.Avalonia.Enums.Icon.None;
                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        messageBoxIcon = MessageBox.Avalonia.Enums.Icon.Error;
                        break;

                    case MessageBoxIcon.Alert:
                    case MessageBoxIcon.Warning:
                        messageBoxIcon = MessageBox.Avalonia.Enums.Icon.Warning;
                        break;

                    default:
                        break;
                }

                var result = await MessageBoxUtils.GetMessageDialogResult(title, description, (buttonNames.Count() <= 1) ? MessageBox.Avalonia.Enums.ButtonEnum.Ok
                    : MessageBox.Avalonia.Enums.ButtonEnum.YesNo, messageBoxIcon, buttonNames, false, true);

                if (result == MessageBox.Avalonia.Enums.ButtonResult.None)
                {
                    return currentActiveButton;
                }

                return (result == MessageBox.Avalonia.Enums.ButtonResult.Ok) ? 0 :
                    (result == MessageBox.Avalonia.Enums.ButtonResult.Yes) ? 1 : 0;
            };

            System.Windows.MessageBox.ShowSimpleImpl = async (title, caption, button) =>
            {
                MessageBox.Avalonia.Enums.ButtonEnum buttonImpl = MessageBox.Avalonia.Enums.ButtonEnum.Ok;
                switch (button)
                {
                    case System.Windows.MessageBoxButton.OK:
                        buttonImpl = MessageBox.Avalonia.Enums.ButtonEnum.Ok;
                        break;

                    case System.Windows.MessageBoxButton.OKCancel:
                        buttonImpl = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel;
                        break;

                    case System.Windows.MessageBoxButton.YesNoCancel:
                        buttonImpl = MessageBox.Avalonia.Enums.ButtonEnum.YesNoCancel;
                        break;

                    case System.Windows.MessageBoxButton.YesNo:
                        buttonImpl = MessageBox.Avalonia.Enums.ButtonEnum.YesNo;
                        break;

                    default:
                        break;
                }

                var result = await MessageBoxUtils.GetMessageDialogResult(title, caption, buttonImpl,
                    modalOnWindow: false, dispatchMain : true);

                switch (result)
                {
                    case MessageBox.Avalonia.Enums.ButtonResult.Ok:
                        return System.Windows.MessageBoxResult.OK;

                    case MessageBox.Avalonia.Enums.ButtonResult.Yes:
                        return System.Windows.MessageBoxResult.Yes;

                    case MessageBox.Avalonia.Enums.ButtonResult.No:
                        return System.Windows.MessageBoxResult.No;

                    case MessageBox.Avalonia.Enums.ButtonResult.Cancel:
                        return System.Windows.MessageBoxResult.Cancel;

                    default:
                        return System.Windows.MessageBoxResult.None;
                }
            };
        }
    }
}
