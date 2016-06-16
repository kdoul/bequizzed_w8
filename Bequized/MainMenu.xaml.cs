using Bequized.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Bequized
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainMenu : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public static bool isSfxEnabled, isMusicEnabled, isAnimEnabled;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public MainMenu()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            ApplicationDataContainer localData = ApplicationData.Current.LocalSettings;
            if ((localData.Values["isSfxEnabled"] != null) && (localData.Values["isSfxEnabled"] is bool))
                isSfxEnabled = (bool)localData.Values["isSfxEnabled"];
            else
            { localData.Values["isSfxEnabled"] = true; isSfxEnabled = true; }

            if ((localData.Values["isMusicEnabled"] != null) && (localData.Values["isMusicEnabled"] is bool))
                isMusicEnabled = (bool)localData.Values["isMusicEnabled"];
            else
            { localData.Values["isMusicEnabled"] = true; isMusicEnabled = true; }

            if ((localData.Values["isAnimEnabled"] != null) && (localData.Values["isAnimEnabled"] is bool))
                isAnimEnabled = (bool)localData.Values["isAnimEnabled"];
            else
            { localData.Values["isAnimEnabled"] = true; isAnimEnabled = true;}

            Statistics stats = new Statistics();

            MainMenu1.DataContext = stats;

            if (isAnimEnabled)
                AnimateMainMenu.Begin();
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Window_SizeChanged;
            DetermineVisualState();
        }

        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            DetermineVisualState();
        }

        private void DetermineVisualState()
        {
            var state = string.Empty;
            var applicationView = ApplicationView.GetForCurrentView();
            var size = Window.Current.Bounds;

            if (applicationView.IsFullScreen)
            {
                if (applicationView.Orientation == ApplicationViewOrientation.Landscape)
                    state = "FullScreenLandscape";
                else
                    state = "FullScreenPortrait";
            }
            else
            {
                if (size.Width == 320)
                    state = "Snapped";
                else if (size.Width <= 500)
                    state = "Narrow";
                else
                    state = "Filled";
            }

            System.Diagnostics.Debug.WriteLine("Width: {0}, New VisulState: {1}",
                size.Width, state);

            VisualStateManager.GoToState(this, state, true);
        }



        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private async void StartGameBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (isSfxEnabled)
                SfxClick.Play();
            await Task.Delay(100);
        	this.Frame.Navigate((typeof(MainGame)), false);
        }

        #endregion

        private async void TimedGameStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isSfxEnabled)
                SfxClick.Play();
            await Task.Delay(100);
            this.Frame.Navigate((typeof(MainGame)), true);
        }

        private void TestClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }

        
        }
    }

