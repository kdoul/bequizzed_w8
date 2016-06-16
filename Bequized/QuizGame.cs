using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Bequized
{
    class QuizGame : INotifyPropertyChanged
    {
        #region private fields
        DispatcherTimer timer = new DispatcherTimer();
        private DateTime gameStartTime = DateTime.MinValue;
        private TimeSpan gameLength;
        private int timerTicks;
        private SolidColorBrush timebrush = new SolidColorBrush(Color.FromArgb(255,0,128,0));
        private ObservableCollection<Query> questionList = new ObservableCollection<Query>();
        private int score, timeRemaining, Lives=-1;
        private char rightSelection;
        private string AnsA, AnsB, AnsC, AnsD;
        private string Question = "Test";
        private Random random = new Random();
        private bool Selected0, Selected1, Selected2, Selected3;
        #endregion

        #region Events
        #region Delegates
       // public delegate void GameStartHandler();
        public delegate void GameEndHandler();
        public delegate void GameTimedStartHandler();
        public delegate void GameTimedStopHandler();
        public delegate void RightAnsHandler();
        public delegate void WrongAnsHandler();
        #endregion

        #region Event Signatures
       // public event GameStartHandler GameStart;

        public event GameEndHandler GameEnd;

        public event GameTimedStartHandler NoMoreQuestions;

        public event GameTimedStopHandler GameTimedStop;

        public event RightAnsHandler RightAns;

        public event WrongAnsHandler WrongAns;
        #endregion

        public double RemainingTime
        {
            get
            {
                if (timerTicks == this.gameLength.TotalSeconds)
                {
                    return 0;
                }
                else
                {
                    if (timerTicks > this.gameLength.TotalSeconds)
                    {
                        timerTicks = 0;
                        timer.Stop();
                        if (GameTimedStop != null)
                        this.GameTimedStop();
                        return 0;
                    }
                    else
                    {
                        return (this.gameLength.TotalSeconds - timerTicks);
                    }
                }
            }
        }

        public SolidColorBrush TimeBrush
        {
            get
            {
                return this.timebrush;
            }
        }


        public double GameLength
        {
            get
            {
                return this.gameLength.TotalSeconds;

            }
        }

        public int RemainingLives
        {
            get
            {
                return Lives;
            }
        }

        public string question
        {
            get { return Question; }
        }

        public int Score
        {
            get { return score; }
        }

        public string ansA
        {
            get { return AnsA; }
        }

        public string ansB
        {
            get { return AnsB; }
        }

        public string ansC
        {
            get { return AnsC; }
        }
        public string ansD
        {
            get { return AnsD; }
        }

        #endregion

        #region Methods
        #region Constructors
        public QuizGame()
        {
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion

        #region Public Methods

        public void PauseGame()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }

        }

        public async void StartNewGameTimed(int maxTime)
        {
            this.score = 0;
            this.questionList.Clear();
            await this.LoadXml();
            DisplayQuestion();
            this.gameStartTime = DateTime.Now;
            this.gameLength = new TimeSpan(0, 0, maxTime);
            timer.Start();
            this.NotifyPropertyChanged("questionList");
            this.NotifyPropertyChanged("Score");
            this.NotifyPropertyChanged("RemainingTime");
            this.NotifyPropertyChanged("questionsToAsk");
            this.NotifyPropertyChanged("GameLength");
            this.NotifyPropertyChanged("gameLength");
        }

        public async void StartNewGame(int maxLives)
        {
            this.score = 0;
            this.questionList.Clear();
            this.Lives = maxLives;
            await this.LoadXml();
            DisplayQuestion();
            this.NotifyPropertyChanged("questionList");
            this.NotifyPropertyChanged("Score");
            this.NotifyPropertyChanged("questionsToAsk");
        }


        public void CheckAns(Char SelectedButton)
        {
            if (SelectedButton == rightSelection)
            {
                score++;
                DisplayQuestion();
                this.NotifyPropertyChanged("Score");
                this.RightAns();
            }
            else
            {
                Lives--;
                if (Lives == 0)
                {
                    if (GameEnd != null)
                    this.GameEnd();
                }
                this.NotifyPropertyChanged("RemainingLives");
                DisplayQuestion();
                this.WrongAns();
            }
        }
        #endregion

        #region Private Methods

        private async Task LoadXml()
        {
            XmlDocument doc = new XmlDocument();
            StorageFolder InstallationFolder;
            StorageFile file;
            int i = 0;

            InstallationFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            file = await InstallationFolder.GetFileAsync("QData.xml");

            if (file != null && InstallationFolder != null)
            {
                doc = await XmlDocument.LoadFromFileAsync(file);
                XmlNodeList nodes = doc.SelectNodes(@"/Query_Library/Query");
                if (nodes.Count > 0)
                {
                    foreach (XmlElement node in nodes)
                    {
                        questionList.Add(new Query(node.SelectSingleNode("Question").InnerText,
                           node.SelectSingleNode("Right_Ans").InnerText,
                           node.SelectSingleNode("Wrong_AnsA").InnerText,
                           node.SelectSingleNode("Wrong_AnsB").InnerText,
                           node.SelectSingleNode("Wrong_AnsC").InnerText));
                        i++;
                    }
                }

            }
        }

        public void timer_Tick(object sender, object e)
        {
            
            timeRemaining = (int)(gameLength.TotalSeconds) - timerTicks;
            System.Diagnostics.Debug.WriteLine(timeRemaining);
            Color newColor = new Color();
            if (gameLength.TotalSeconds != 0)
            {
                newColor.A = 255;
                newColor.B = 0;
                newColor.G = (byte)(128 * timeRemaining / (int)(gameLength.TotalSeconds));
                newColor.R = (byte)(255 * timerTicks / (int)(gameLength.TotalSeconds));
                timebrush = new SolidColorBrush(newColor);
            }
            this.NotifyPropertyChanged("RemainingTime");
            this.NotifyPropertyChanged("timeBrush");
            this.NotifyPropertyChanged("TimeBrush");
            timerTicks++;
            
        }

        private void DisplayQuestion()
        {

            if (questionList.Count > 0)
            {
                int randomIndex = random.Next(questionList.Count);

                Question = questionList[randomIndex].Question + "?";
                AnsA = Randomizer(questionList[randomIndex], 'A');
                AnsB = Randomizer(questionList[randomIndex], 'B');
                AnsC = Randomizer(questionList[randomIndex], 'C');
                AnsD = Randomizer(questionList[randomIndex], 'D');
                Selected0 = false; Selected1 = false;
                Selected2 = false; Selected3 = false;
                questionList.RemoveAt(randomIndex);

                this.NotifyPropertyChanged("ansA");
                this.NotifyPropertyChanged("ansB");
                this.NotifyPropertyChanged("ansC");
                this.NotifyPropertyChanged("ansD");
                this.NotifyPropertyChanged("question");

            }
            else
            {
                this.NoMoreQuestions();
                timer.Stop();
            }
        }

        private String Randomizer(Query query, char caller)
        {
            while (true)
            {
                switch (random.Next(4))
                {
                    case 0:
                        if (Selected0) { break; }
                        Selected0 = true;
                        rightSelection = caller;
                        return query.RightAns;
                    case 1:
                        if (Selected1) { break; }
                        Selected1 = true;
                        return query.AnswerA;
                    case 2:
                        if (Selected2) { break; }
                        Selected2 = true;
                        return query.AnswerB;
                    case 3:
                        if (Selected3) { break; }
                        Selected3 = true;
                        return query.AnswerC;
                    default:
                        return null;
                }
            }
        }




        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
        #endregion
        #endregion
    }


}

 