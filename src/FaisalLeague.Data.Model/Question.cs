using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class Question
    {
        public Question()
        {
            Choices = new List<Choice>();
        }
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Text { get; set; }
        public string VideoCode { get; set; }
        public string DecryptionKey { get; set; }

        public virtual IList<Choice> Choices { get; set; }
    }
}
