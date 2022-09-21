using Avalonia.Controls;
using WPR.Common;

using ReactiveUI;
using System;

using Microsoft.Xna.Framework.GamerServices;
using MessageBox.Avalonia;

namespace WPR.UI.Pages
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();

            TextBox gamerTagTextBox = this.Get<TextBox>("gamerTagTextBox");
            if (Configuration.Current.GamerTag != null)
            {
                gamerTagTextBox.Text = Configuration.Current.GamerTag;
            }

            gamerTagTextBox.WhenAnyValue(x => x.Text).Subscribe(text =>
            {
                if (Gamer.SignedInGamers.Count != 0)
                {
                    Gamer.SignedInGamers[0].Gamertag = text;
                }

                Configuration.Current.GamerTag = text;
                Configuration.Current.Save();
            });

            TextBox pathTextBox = this.Get<TextBox>("dataStoragePathText");
            pathTextBox.Text = Configuration.Current.DataStorePath;

            Button pathChangeBtn = this.Get<Button>("dataStoragePathBrowse");
            pathChangeBtn.Click += async (obj, args) =>
            {
                string? resultFolder = await new OpenFolderDialog()
                {
                    Directory = Configuration.Current.DataStorePath
                }.ShowAsync(GetWindow());

                if (resultFolder != null)
                {
                    pathTextBox.Text = resultFolder;
                    Configuration.Current.DataStorePath = resultFolder;
                    Configuration.Current.Save();

                    var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                        title: Properties.Resources.SuccessfullyChanged,
                        text: Properties.Resources.SuccessfullyChangedDataPathMsg,
                        icon: MessageBox.Avalonia.Enums.Icon.Success,
                        windowStartupLocation: WindowStartupLocation.CenterScreen);

                    await msgBox.ShowDialog(GetWindow());
                }
            };

            this.Get<Button>("restoreDefaultStoragePathBtn").Click += async (obj, args) =>
            {
                Configuration.Current.RestoreDefaultDataStoragePath();
                pathTextBox.Text = Configuration.Current.DataStorePath;

                Configuration.Current.Save();

                var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                    title: Properties.Resources.SuccessfullyChanged,
                    text: Properties.Resources.SuccessfullyChangedDataPathMsg,
                    icon: MessageBox.Avalonia.Enums.Icon.Success,
                    windowStartupLocation: WindowStartupLocation.CenterScreen);

                await msgBox.ShowDialog(GetWindow());
            };
        }
        Window GetWindow() => VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
    }
}
