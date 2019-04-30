using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FaisalLeague.Queries.Models
{
    public class QuestionsByCategory
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long QuestionId { get; set; }
        public bool IsAnswered { get; set; }
        public int Sort { get; set; }
    }

    public class CardScore
    {
        public int Sort { get; set; }
        public int? Score { get; set; }
    }

    public class ActiveCardModel
    {
        public long CardId { get; set; }
        public List<QuestionsByCategory> QuestionsByCategoryList { get; set; }
        public List<CardScore> CardScoreList { get; set; }
        public int TotalCardScore
        {
            get
            {
                return CardScoreList.Sum(c => c.Score.GetValueOrDefault(0));
            }
        }
    }
}
