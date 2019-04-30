using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class UserAnswer
    {
        public UserAnswer()
        {

        }
        public long UserCardId { get; set; }
        public virtual UserCard UserCard { get; set; }
        public long QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public long? ChoiceId { get; set; }
        public virtual Choice Choice { get; set; }
        public int AnswerPoints { get; set; }
        public int Sort { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
