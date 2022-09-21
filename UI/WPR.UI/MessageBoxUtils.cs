using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Widget;
#else
using Avalonia.Controls;
using Avalonia.Threading;
#endif

using MessageBox.Avalonia;
using static System.Net.Mime.MediaTypeNames;

namespace WPR.UI
{
    public static class MessageBoxUtils
    {
#if __ANDROID__
        public static Activity MainActivity { get; set; }
#else
        public static Window MainWindow { get; set; }
#endif

        public static Task<MessageBox.Avalonia.Enums.ButtonResult> GetMessageDialogResult(string title,
            string text, MessageBox.Avalonia.Enums.ButtonEnum buttons = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
            MessageBox.Avalonia.Enums.Icon icon = MessageBox.Avalonia.Enums.Icon.Info, IEnumerable<string> ?buttonTexts = null,
            bool modalOnWindow = true, bool dispatchMain = false)
        {
#if __ANDROID__
            TaskCompletionSource<MessageBox.Avalonia.Enums.ButtonResult> source = new TaskCompletionSource<MessageBox.Avalonia.Enums.ButtonResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            MainActivity.RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity)!
                    .SetTitle(title)!
                    .SetMessage(text)!;

                switch (buttons)
                {
                    case MessageBox.Avalonia.Enums.ButtonEnum.Ok:
                        if (buttonTexts != null)
                        {
                            var enumerable = buttonTexts.GetEnumerator();
                            enumerable.MoveNext();

                            builder = builder.SetPositiveButton(enumerable.Current, (dialog, which) =>
                            {
                                Common.Log.Error(Common.LogCategory.AppList, "OK reported");
                                source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Ok);
                                (dialog as AlertDialog)!.Dismiss();
                            })!;
                        }
                        else
                        {
                            builder = builder.SetPositiveButton(Android.Resource.String.Ok, (dialog, which) =>
                            {
                                Common.Log.Error(Common.LogCategory.AppList, "OK reported");
                                source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Ok);
                                (dialog as AlertDialog)!.Dismiss();
                            })!;
                        }

                        break;

                    case MessageBox.Avalonia.Enums.ButtonEnum.YesNo:
                        if (buttonTexts != null)
                        {
                            var enumerable = buttonTexts.GetEnumerator();
                            enumerable.MoveNext();

                            builder = builder.SetNegativeButton(enumerable.Current, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.No);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!;

                            enumerable.MoveNext();

                            builder = builder
                                .SetPositiveButton(enumerable.Current, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Yes);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!;
                            
                        } else
                        {
                            builder = builder
                                .SetPositiveButton(Android.Resource.String.Yes, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Yes);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!
                                .SetNegativeButton(Android.Resource.String.No, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.No);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!;
                        }
                        break;

                }

                switch (icon)
                {
                    case MessageBox.Avalonia.Enums.Icon.Warning:
                        builder = builder.SetIcon(Android.Resource.Drawable.IcDialogAlert)!;
                        break;

                    case MessageBox.Avalonia.Enums.Icon.Info:
                        builder = builder.SetIcon(Android.Resource.Drawable.IcDialogInfo)!;
                        break;

                    default:
                        break;
                }

                builder.Create()!.Show();
            });

            return source.Task;
#else
            Func<Task<MessageBox.Avalonia.Enums.ButtonResult>> returnTaskFunc = () =>
            {
                var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                    title: title,
                    text: text,
                    icon: icon,
                    @enum: buttons,
                    windowStartupLocation: WindowStartupLocation.CenterScreen);

                return modalOnWindow ? msgBox.ShowDialog(MainWindow) : msgBox.Show();
            };

            if (dispatchMain)
            {
                return Dispatcher.UIThread.InvokeAsync(returnTaskFunc);
            }

            return returnTaskFunc();
#endif
        }

        public static Task<string> GetInputResult(string title, string description, string defaultText, bool isModal = true, bool dispatchMain = false)
        {
#if __ANDROID__
            TaskCompletionSource<string> source = new TaskCompletionSource<string>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            MainActivity.RunOnUiThread(() =>
            {
                EditText editField = new EditText(MainActivity);
                editField.SetText(defaultText, TextView.BufferType.Editable);

                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity)!
                    .SetTitle(title)!
                    .SetMessage(description)!
                    .SetView(editField)!
                    .SetPositiveButton(Android.Resource.String.Ok, (dialog, which) =>
                    {
                        source.SetResult(editField.Text!);
                        (dialog as AlertDialog)!.Dismiss();
                    })!
                    .SetNegativeButton(Android.Resource.String.Cancel, (dialog, which) =>
                    {
                        source.SetResult(defaultText);
                        (dialog as AlertDialog)!.Dismiss();
                    })!;

                builder.Create()!.Show();
            });

            return source.Task;
        }
#else
            Func<Task<string>> returnTaskFunc = async () =>
            {
                var msgBox = MessageBoxManager.GetMessageBoxInputWindow(
                    new MessageBox.Avalonia.DTO.MessageBoxInputParams()
                    {
                        ContentTitle = title,
                        ContentMessage = description,
                        InputDefaultValue = defaultText,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }
                );

                var result = await (isModal ? msgBox.ShowDialog(MainWindow) : msgBox.Show());
                return result.Message;
            };

            if (dispatchMain)
            {
                return Dispatcher.UIThread.InvokeAsync(returnTaskFunc);
            }

            return returnTaskFunc();
        }
#endif
    }
}
