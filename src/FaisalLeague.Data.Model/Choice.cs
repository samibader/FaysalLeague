using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class Choice
    {
        public Choice()
        {
            
        }
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
