using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class SupportValueSeriesFilter : SupportValueFilter
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public SupportValueSeriesFilter(Relation relation, int desiredValue, IAttributeValuesComparer ruleLengthComparer = null) : base(relation, desiredValue, ruleLengthComparer)
        {
        }
    }
}
