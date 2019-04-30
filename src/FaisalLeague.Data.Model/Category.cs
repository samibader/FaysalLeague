using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class Category
    {
        public Category()
        {
            Questions = new List<Question>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }

        public IList<Question> Questions { get; set; }
    }
}
