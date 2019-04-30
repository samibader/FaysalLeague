using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class CardQuestion
    {
        public  CardQuestion()
        {

        }

        public long CardId { get; set; }
        public virtual Card Card { get; set; }
        public long QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int Sort { get; set; }
    }
}
