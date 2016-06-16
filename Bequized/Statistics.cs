using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Bequized
{
    class Statistics 

    {
        private int normalTotalQ, normalTotalCorrect, normalTotalIncorrect, timedTotalCorrect, timedTotalIncorrect, timedTotalQ;
        private int normalLastQ, normalLastCorrect, normalLastIncorrect, timedLastQ, timedLastCorrect, timedLastIncorrect;
        private int highScoreTimed, highScoreNormal;
        private long fastest_time;

        public string Fastest_time
        {
            get
            {
                string TempString = ((double)fastest_time / 1000).ToString();
                return TempString + " sec";
            }
        }

        public string HighScoreNormal
        {
            get
            {
                return this.highScoreNormal.ToString();
            }
        }

        public string HighScoreTimed
        {
            get
            {
                return this.highScoreTimed.ToString();
            }
        }

        public string NormalTotalQ
        {
            get
            {
                return this.normalTotalQ.ToString();
            }
        }

        public string NormalTotalCorrect
        {
            get
            {
                return this.normalTotalCorrect.ToString();
            }
        }

        public string NormalTotalIncorrect
        {
            get
            {
                return this.normalTotalIncorrect.ToString();
            }
        }

        public string TimedTotalCorrect
        {
            get
            {
                return this.timedTotalCorrect.ToString();
            }
        }

        public string TimedTotalIncorrect
        {
            get
            {
                return this.timedTotalIncorrect.ToString();
            }
        }

        public string TimedTotalQ
        {
            get
            {
                return this.timedTotalQ.ToString();
            }
        }

        public string NormalLastCorrect
        {
            get
            {
                return this.normalLastCorrect.ToString();
            }
        }

        public string NormalLastIncorrect
        {
            get
            {
                return this.normalLastIncorrect.ToString();
            }
        }

        public string TimedLastQ
        {
            get
            {
                return this.timedLastQ.ToString();
            }
        }

        public string NormalLastQ
        {
            get
            {
                return this.normalLastQ.ToString();
            }
        }

        public string TimedLastCorrect
        {
            get
            {
                return this.timedLastCorrect.ToString();
            }
        }

        public string TimedLastIncorrect
        {
            get
            {
                return this.timedLastIncorrect.ToString();
            }
        }
    
        ApplicationDataContainer localdata = ApplicationData.Current.LocalSettings;
        public Statistics()
        {
            if (localdata.Values["TimedTotalCorrect"] != null)
                timedTotalCorrect = (int)localdata.Values["TimedTotalCorrect"];
            else
            { localdata.Values["TimedTotalCorrect"] = 0; timedTotalCorrect = 0; }

            if (localdata.Values["TimedTotalIncorrect"] != null)
                timedTotalIncorrect = (int)localdata.Values["TimedTotalIncorrect"];
            else
            { localdata.Values["TimedTotalIncorrect"] = 0; timedTotalIncorrect = 0; }

            if (localdata.Values["TimedTotalQ"] != null)
                timedTotalQ = (int)localdata.Values["TimedTotalQ"];
            else
            { localdata.Values["TimedTotalQ"] = 0; timedTotalQ = 0;}

            if (localdata.Values["TTCA"] != null)
                fastest_time = (int)localdata.Values["TTCA"];
            else
            { localdata.Values["TTCA"] = 0; fastest_time = 0; }

            if (localdata.Values["TimedHighScore"] != null)
                highScoreTimed = (int)localdata.Values["TimedHighScore"];
            else
            { localdata.Values["TimedHighScore"] = 0; highScoreTimed = 0; }

            if (localdata.Values["NormalTotalCorrect"] != null)
                normalTotalCorrect = (int)localdata.Values["NormalTotalCorrect"];
            else
            { localdata.Values["NormalTotalCorrect"] = 0; normalTotalCorrect = 0; }

            if (localdata.Values["NormalTotalIncorrect"] != null)
                normalTotalIncorrect = (int)localdata.Values["NormalTotalIncorrect"];
            else
            { localdata.Values["NormalTotalIncorrect"] = 0; normalTotalIncorrect = 0; }

            if (localdata.Values["NormalTotalQ"] != null)
                normalTotalQ = (int)localdata.Values["NormalTotalQ"];
            else
            {localdata.Values["NormalTotalQ"] = 0; normalTotalQ = 0;}

            if (localdata.Values["NormalHighScore"] != null)
                highScoreNormal = (int)localdata.Values["NormalHighScore"];
            else
            {localdata.Values["NormalHighScore"] = 0; highScoreNormal = 0;}


            if ( localdata.Values["NormalCorrectLast"] != null)
              normalLastCorrect = (int)localdata.Values["NormalCorrectLast"];
            else
            { localdata.Values["NormalCorrectLast"] = 0; normalLastCorrect = 0 ;}

            if (localdata.Values["NormalIncorrectLast"] != null)
                normalLastIncorrect = (int)localdata.Values["NormalIncorrectLast"];
            else
            { localdata.Values["NormalIncorrectLast"] = 0; normalLastIncorrect = 0; }

            if (localdata.Values["NormalLastQ"] != null)
                normalLastQ = (int)localdata.Values["NormalLastQ"];
            else
            { localdata.Values["NormalLastQ"] = 0; normalLastQ = 0; }

            if (localdata.Values["TimedCorrectLast"] != null)
                 timedLastCorrect = (int)localdata.Values["TimedCorrectLast"];
            else
            { localdata.Values["TimedCorrectLast"] = 0; timedLastCorrect = 0; }
            
            if (localdata.Values["TimedIncorrectLast"] != null)
              timedLastIncorrect = (int)localdata.Values["TimedIncorrectLast"];
            else
            { localdata.Values["TimedIncorrectLast"] = 0; timedLastIncorrect = 0; }

            if (localdata.Values["TimedLastQ"] != null)
                timedLastQ = (int)localdata.Values["TimedLastQ"];
            else
            { localdata.Values["TimedLastQ"] = 0; timedLastQ = 0; }
        }
    }
}
