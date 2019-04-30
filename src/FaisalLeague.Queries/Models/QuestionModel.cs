using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Queries.Models
{
    public class QuestionModel
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Text { get; set; }
        public string VideoCode { get; set; }
        public string DecryptionKey { get; set; }
        public List<ChoiceModel> Choices { get; set; }
    }

    public class ChoiceModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
