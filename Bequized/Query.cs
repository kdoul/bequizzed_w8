using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bequized
{
   public class Query
    {
        public String Question, RightAns, AnswerA, AnswerB, AnswerC;

        public Query(String Question, String RightAns,
            String AnswerA, String AnswerB, String AnswerC) 
        {
            this.Question = Question;
            this.RightAns = RightAns;
            this.AnswerA = AnswerA;
            this.AnswerB = AnswerB;
            this.AnswerC = AnswerC;
        }

    }
}
