using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Bequized
{
    public sealed partial class SettingsFlyout2 : SettingsFlyout
    {
        ApplicationDataContainer localData = ApplicationData.Current.LocalSettings;
        public SettingsFlyout2()
        {
            this.InitializeComponent();
            Update();

            AnimSwitch.Toggled += ToggleBackgroundAnim;
            SfxSwitch.Toggled += ToggleSfx;
            MusicSwitch.Toggled += ToggleMusic;
        }

        private void ToggleBackgroundAnim(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (MainMenu.isAnimEnabled)
            {
                localData.Values["isAnimEnabled"] = false;
                MainMenu.isAnimEnabled = false;
            }
            else
            {
                localData.Values["isAnimEnabled"] = true;
                MainMenu.isAnimEnabled = true;
            }
 
            
        }

        private void ToggleSfx(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (MainMenu.isSfxEnabled)
            {
                localData.Values["isSfxEnabled"] = false;
                MainMenu.isSfxEnabled = false;
            }
            else
            {
                localData.Values["isSfxEnabled"] = true;
                MainMenu.isSfxEnabled = true;
            }
        }

        private void ToggleMusic(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var frame = (Frame)Window.Current.Content;

            if (MainMenu.isMusicEnabled)
            {
                    if (frame.Content is MainGame)
                {
                        var page = (MainGame)frame.Content;
                        page.PauseBackgroundMusic();
                }
                    localData.Values["isMusicEnabled"] = false;
                    MainMenu.isMusicEnabled = false;
            }
            else
            {
                if (frame.Content is MainGame)
                {
                    var page = (MainGame)frame.Content;
                    page.StartBackgroundMusic();
                }
                localData.Values["isMusicEnabled"] = true;
                MainMenu.isMusicEnabled = true;
            }
        }

        private void Update()
        {
            if (MainMenu.isAnimEnabled)
                AnimSwitch.IsOn = true;
            else if (!MainMenu.isAnimEnabled)
                AnimSwitch.IsOn = false;

            if (MainMenu.isSfxEnabled)
                SfxSwitch.IsOn = true;
            else if (!MainMenu.isSfxEnabled)
                SfxSwitch.IsOn = false;

            if (MainMenu.isMusicEnabled)
                MusicSwitch.IsOn = true;
            else if (!MainMenu.isMusicEnabled)
                MusicSwitch.IsOn = false;
        }

        private void ClearStats(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var y = new MessageDialog("Are you sure you want to delete all data? This will reset the game settings too.", "Are you sure?");
            var asyncOperation = y.ShowAsync();
            y.Commands.Add(new UICommand("No", (UICommandInvokedHandler) =>
            {
                asyncOperation.Cancel();
            }));
            y.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
            {
                ApplicationData.Current.ClearAsync();
                var frame = (Frame)Window.Current.Content;
                if (frame.Content is MainMenu)
                    frame.Navigate(typeof(MainMenu));
            }));

           
        }
    }
}
