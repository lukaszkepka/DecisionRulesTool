using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Attribute
    {
        public string Name { get; set; }
        public int? Accuary { get; set; }
        public Category Type { get; set; }

        public enum Category
        {
            NUMERIC,
            SYMBOLIC
        }
    }
}
