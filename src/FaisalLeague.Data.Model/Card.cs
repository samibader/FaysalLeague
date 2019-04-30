using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class Card
    {
        public Card()
        {
            CardQuestions = new List<CardQuestion>();
        }

        public long Id { get; set; }
        public virtual IList<CardQuestion> CardQuestions { get; set; }

    }
}
