using Bequized.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using Windows.UI;
using Windows.UI.Popups;
using System.ComponentModel;
using Windows.Storage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Bequized
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainGame : Page

    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private QuizGame newGame;
        private bool isCorrect, isPaused, isTimed;
        private int CorrectCount, IncorrectCount;
        private long MinimumTime;
        private ApplicationDataContainer localdata = ApplicationData.Current.LocalSettings;
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

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


        public MainGame()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            GameItems.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            grid2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            newGame = new QuizGame();
            grid.DataContext = newGame;
            if (MainMenu.isAnimEnabled)
                BackgroundAnimate.Begin();
            StartGameAnim.Begin();
            StartGameAnim.Completed += StartGameAnim_Completed;
            newGame.GameEnd += newGame_GameEnd;
            newGame.GameTimedStop += newGame_GameTimedStop;
            newGame.RightAns += newGame_RightAns;
            newGame.WrongAns += newGame_WrongAns;
            newGame.NoMoreQuestions += newGame_NoMoreQuestions;

            if (MainMenu.isMusicEnabled)
                StartBackgroundMusic();

        }

        void newGame_NoMoreQuestions()
        {
            SelectAnsA.IsHitTestVisible = false;
            SelectAnsB.IsHitTestVisible = false;
            SelectAnsC.IsHitTestVisible = false;
            SelectAnsD.IsHitTestVisible = false;
            buttonA.IsHitTestVisible = false;
            buttonB.IsHitTestVisible = false;
            buttonC.IsHitTestVisible = false;
            buttonD.IsHitTestVisible = false;
            EndGameFade.Begin();
            EndGameFade.Completed += EndGameFade_CompletedC;         
        }
        private async void EndGameFade_CompletedC(object sender, object e)
        {
            SaveStats();
                var y = new MessageDialog("Congratulations! You managed to answer all our questions! Your score is: " + newGame.Score + ".", "Game Over.");

                y.Commands.Add(new UICommand("OK", (UICommandInvokedHandler) =>
                {
                    this.Frame.Navigate(typeof(MainMenu));
                }));
                await y.ShowAsync();
        }

        void newGame_GameEnd()
        {
            SelectAnsA.IsHitTestVisible = false;
            SelectAnsB.IsHitTestVisible = false;
            SelectAnsC.IsHitTestVisible = false;
            SelectAnsD.IsHitTestVisible = false;
            buttonA.IsHitTestVisible = false;
            buttonB.IsHitTestVisible = false;
            buttonC.IsHitTestVisible = false;
            buttonD.IsHitTestVisible = false;
            EndGameFade.Begin();
            EndGameFade.Completed += EndGameFade_CompletedB;
        }

        async void EndGameFade_CompletedB(object sender, object e)
        {
            SaveStats();
                var x = new MessageDialog("You lost all your lives. Your score is: " + newGame.Score + ".", "Game Over.");

                x.Commands.Add(new UICommand("OK", (UICommandInvokedHandler) =>
                {
                    this.Frame.Navigate(typeof(MainMenu));
                }));
                await x.ShowAsync();
        }

        void newGame_WrongAns()
        {
            IncorrectCount++;
            isCorrect = false;
            if (MainMenu.isSfxEnabled)
                SfxIncorrect.Play();
        }

        void newGame_RightAns()
        {
            CorrectCount++;
            isCorrect = true;
            if (MainMenu.isSfxEnabled)
                SfxCorrect.Play();
        }

        void newGame_GameTimedStop()
        {
            SelectAnsA.IsHitTestVisible = false;
            SelectAnsB.IsHitTestVisible = false;
            SelectAnsC.IsHitTestVisible = false;
            SelectAnsD.IsHitTestVisible = false;
            buttonA.IsHitTestVisible = false;
            buttonB.IsHitTestVisible = false;
            buttonC.IsHitTestVisible = false;
            buttonD.IsHitTestVisible = false;
            EndGameFade.Begin();
            EndGameFade.Completed += EndGameFade_CompletedA;
        }

        async void EndGameFade_CompletedA(object sender, object e)
        {
            SaveStats();
                var w = new MessageDialog("Time is up! Your score is:" + newGame.Score + ".", "Game Over.");

                w.Commands.Add(new UICommand("OK", (UICommandInvokedHandler) =>
                {
                    this.Frame.Navigate(typeof(MainMenu));
                }));
                await w.ShowAsync();
        }



        void StartGameAnim_Completed(object sender, object e)
        {
            if (MainMenu.isSfxEnabled)
                SfxWoosh1.Play();
            if (isTimed)
            {
                int GameRunningTime = 20;
                newGame.StartNewGameTimed(GameRunningTime);
                TimeBar.Maximum = GameRunningTime;
                TimeBarSnapped.Maximum = GameRunningTime;
                Lives.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                LivesSnapped.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                stopwatch.Start();
            }
            else
            {
                newGame.StartNewGame(3);
                TimeBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                TimeBarSnapped.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            GameItems.Visibility = Windows.UI.Xaml.Visibility.Visible;
            grid2.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {

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
            if (e.Parameter is bool)
                isTimed = (bool)e.Parameter;
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StartGameAnim.Completed -= StartGameAnim_Completed;
            newGame.GameEnd -= newGame_GameEnd;
            newGame.GameTimedStop -= newGame_GameTimedStop;
            newGame.RightAns -= newGame_RightAns;
            newGame.WrongAns -= newGame_WrongAns;
            newGame.NoMoreQuestions -= newGame_NoMoreQuestions;
            navigationHelper.OnNavigatedFrom(e);
        }

        private void GoBackToMain(object sender, RoutedEventArgs e)
        {
            if (MainMenu.isSfxEnabled)
                SfxClick.Play();
            if (!isPaused)
            {
                isPaused = true;
                if (isTimed)
                    newGame.PauseGame();
            }
            var z = new MessageDialog("Are you sure you want to go back? All game progress will be lost.", "Are you sure?");
            var asyncOperation = z.ShowAsync();
            z.Commands.Add(new UICommand("No", (UICommandInvokedHandler) =>
            {
                if (isPaused)
                {
                    isPaused = false;
                    if (isTimed)
                        newGame.PauseGame();
                }
                asyncOperation.Cancel();

            }));
            z.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
            {
                this.Frame.Navigate(typeof(MainMenu));
            }));
        }

        private  void ABtnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopwatch.Stop();
            newGame.CheckAns('A');
            if (isCorrect)
            {
                if (isTimed)
                {
                    if ((MinimumTime > stopwatch.ElapsedMilliseconds) || (MinimumTime == 0))
                    {
                        MinimumTime = stopwatch.ElapsedMilliseconds;
                    }
                }
                MakeAGreen.Begin();
            }
            else
                MakeARed.Begin();
            CheckLives();
            stopwatch.Reset();
            stopwatch.Start();
        }

        private void MainGame1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Window_SizeChanged;
            DetermineVisualState();
        }

        private void DetermineVisualState()
        {
            var state = string.Empty;
            var applicationView = ApplicationView.GetForCurrentView();
            var size = Window.Current.Bounds;
            if (size.Width > 1250)
            {
                GameItems1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SnappedViewMainGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Thickness margin = QuestionViewBox.Margin;
                margin.Left = (1920 - ((size.Width / size.Height) * 1080)) / 2;
                margin.Right = (1920 - ((size.Width / size.Height) * 1080)) / 2;
                QuestionViewBox.Margin = margin;
            }
            else
            {
                GameItems1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SnappedViewMainGame.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (size.Width == 500)
                state = "Snapped";
            else if (size.Width < 500)
                state = "Narrow";
            else if (size.Width == 1024)
                state = "Balls";
            else
                state = "Filled";
            //}

            

            System.Diagnostics.Debug.WriteLine("Width: {0}, New VisulState: {1}",
                size.Width, state);

            VisualStateManager.GoToState(this, state, true);


        }

        private void MainGame1_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            DetermineVisualState();
        }

        private  void BBtnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopwatch.Stop();

        	newGame.CheckAns('B');
            if (isCorrect){
                if (isTimed)
                {
                    if ((MinimumTime > stopwatch.ElapsedMilliseconds) || (MinimumTime == 0))
                        MinimumTime = stopwatch.ElapsedMilliseconds;
                }
                MakeBGreen.Begin();
            }
                
            else
                MakeBRed.Begin();
            CheckLives();
            stopwatch.Reset();
            stopwatch.Start();
        }

        private  void CBtnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopwatch.Stop();
        	newGame.CheckAns('C');
            if (isCorrect)
            {
                if (isTimed)
                {
                    if ((MinimumTime > stopwatch.ElapsedMilliseconds) || (MinimumTime == 0))
                        MinimumTime = stopwatch.ElapsedMilliseconds;
                }
                MakeCGreen.Begin();
            }
            else
                MakeCRed.Begin();
            CheckLives();            
            stopwatch.Reset();
            stopwatch.Start();

        }

        private  void DBtnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopwatch.Stop();

    	    newGame.CheckAns('D');
            if (isCorrect)
            {
                if (isTimed)
                {
                    if ((MinimumTime > stopwatch.ElapsedMilliseconds) || (MinimumTime == 0))
                        MinimumTime = stopwatch.ElapsedMilliseconds;
                }
                MakeDGreen.Begin();
            }
            else
                MakeDRed.Begin();
            CheckLives();

            stopwatch.Reset();
            stopwatch.Start();
        }

        #endregion
        private void CheckLives()
        {
            if (newGame.RemainingLives == 2)
            {
                BurnLive3.Begin();
            }
            else if (newGame.RemainingLives == 1)
            {
                BurnLive2.Begin();
            }
            else if (newGame.RemainingLives == 0)
            {
                BurnLive1.Begin();
            }

        }

        private void PauseGame(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
            if (!isPaused)
            {
                PauseGameAnim.Begin();
                if (MainMenu.isMusicEnabled)
                    PauseBackgroundMusic();
                isPaused = true;
                if (isTimed)
                    newGame.PauseGame();
            }
            else
            {
                UnPauseGameAnim.Begin();
                if (MainMenu.isMusicEnabled)
                    BackgroundMusic.Play();
                isPaused = false;
                if (isTimed)
                    newGame.PauseGame();
            }
        }

        private void OpenSettings(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SettingsFlyout2 sf = new SettingsFlyout2();
            sf.ShowIndependent();
        }

        public void StartBgAnim()
        {
            BackgroundAnimate.Begin();
        }

        public void StopBgAnim()
        {
            
            BackgroundAnimate.Stop();
        }
        
        public async void StartBackgroundMusic()
        {
            await Task.Delay(1000);
            BackgroundMusic.Play();
        }

        public void PauseBackgroundMusic()
        {
            BackgroundMusic.Pause();
        }

        private void ReloadGame(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!isPaused)
            {
                isPaused = true;
                if (isTimed)
                    newGame.PauseGame();
            }
            var z = new MessageDialog("Are you sure you want to restart? All game progress will be lost.", "Are you sure?");
            var asyncOperation = z.ShowAsync();
            z.Commands.Add(new UICommand("No", (UICommandInvokedHandler) =>
            {
                if (isPaused)
                {
                    isPaused = false;
                    if (isTimed)
                        newGame.PauseGame();
                }
                asyncOperation.Cancel();

            }));
            z.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
            {
                if (isTimed)
                    this.Frame.Navigate(typeof(MainGame), true);
                else
                    this.Frame.Navigate(typeof(MainGame), false);
            }));

        }

        private void checkTime()
        {
            
        }

        private void SaveStats()
        {
            if (isTimed)
            {
                int lastValueTotalCorrect = 0;
                int lastValueTotalIncorrect = 0;
                int lastValueTotalQuestions = 0;
                long lastValueLessTimeToCorrectAns = 0;
                int lastValueScore = 0;

                if (localdata.Values["TimedTotalCorrect"] != null)
                    lastValueTotalCorrect = (int)localdata.Values["TimedTotalCorrect"];
                else 
                    localdata.Values["TimedTotalCorrect"] = 0;

                if (localdata.Values["TimedTotalIncorrect"] != null)
                    lastValueTotalIncorrect = (int)localdata.Values["TimedTotalIncorrect"];
                else
                    localdata.Values["TimedTotalIncorrect"] = 0;

                if (localdata.Values["TimedTotalQ"] != null)
                    lastValueTotalQuestions = (int)localdata.Values["TimedTotalQ"];
                else
                    localdata.Values["TimedTotalQ"] = 0;

                if (localdata.Values["TTCA"] != null)
                    lastValueLessTimeToCorrectAns = (int)localdata.Values["TTCA"];
                else
                    localdata.Values["TTCA"] = 0;

                if (localdata.Values["TimedHighScore"] != null)
                    lastValueScore = (int)localdata.Values["TimedHighScore"];
                else
                    localdata.Values["TimedHighScore"] = 0;

                localdata.Values["TimedCorrectLast"] = CorrectCount;
                localdata.Values["TimedIncorrectLast"] = IncorrectCount;
                localdata.Values["TimedLastQ"] = CorrectCount + IncorrectCount;
                localdata.Values["TimedTotalCorrect"] = CorrectCount + lastValueTotalCorrect;
                localdata.Values["TimedTotalIncorrect"] = IncorrectCount + lastValueTotalIncorrect;
                localdata.Values["TimedTotalQ"] = CorrectCount + lastValueTotalCorrect;

                if ((lastValueLessTimeToCorrectAns > MinimumTime) || (lastValueLessTimeToCorrectAns == 0))
                {
                    localdata.Values["TTCA"] = (int)MinimumTime;
                }

                if (lastValueScore < newGame.Score)
                {
                    localdata.Values["TimedHighScore"] = newGame.Score;
                }
                
            }
            else
            {
                int lastValueTotalCorrect = 0;
                int lastValueTotalIncorrect = 0;
                int lastValueTotalQuestions = 0;
                int lastValueScore = 0;

                if (localdata.Values["NormalTotalCorrect"] != null)
                    lastValueTotalCorrect = (int)localdata.Values["NormalTotalCorrect"];
                else
                    localdata.Values["NormalTotalCorrect"] = 0;

                if (localdata.Values["NormalTotalIncorrect"] != null)
                    lastValueTotalIncorrect = (int)localdata.Values["NormalTotalIncorrect"];
                else
                    localdata.Values["NormalTotalIncorrect"] = 0;

                if (localdata.Values["NormalTotalQ"] != null)
                    lastValueTotalQuestions = (int)localdata.Values["NormalTotalQ"];
                else
                    localdata.Values["NormalTotalQ"] = 0;

                if (localdata.Values["NormalHighScore"] != null)
                    lastValueScore = (int)localdata.Values["NormalHighScore"];
                else
                    localdata.Values["NormalHighScore"] = 0;

                localdata.Values["NormalCorrectLast"] = CorrectCount;
                localdata.Values["NormalIncorrectLast"] = IncorrectCount;
                localdata.Values["NormalLastQ"] = CorrectCount + IncorrectCount;
                localdata.Values["NormalTotalCorrect"] = CorrectCount + lastValueTotalCorrect;
                localdata.Values["NormalTotalIncorrect"] = IncorrectCount + lastValueTotalIncorrect;
                localdata.Values["NormalTotalQ"] = CorrectCount + lastValueTotalCorrect;

                if (lastValueScore < newGame.Score)
                {
                    localdata.Values["NormalHighScore"] = newGame.Score;
                }
            }

        }

    }

}

