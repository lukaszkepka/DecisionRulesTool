using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class LengthSeriesFilter : LengthFilter, IRuleSeriesFilter
    {
        private LengthFilter initialFilter;


        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public LengthSeriesFilter(int minLength, int maxLength, LengthFilter lengthFilter) : base(lengthFilter.RelationBetweenRulesLengths, lengthFilter.DesiredLength, lengthFilter.RuleLengthComparer)
        {
            this.initialFilter = lengthFilter;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public IList<IRuleFilter> GenerateSeries()
        {

            IList<IRuleFilter> h = new List<IRuleFilter>();
            for (int i = MinLength; i < MaxLength; i++)
            {
                IRuleFilter ruleFilter = new LengthFilter(initialFilter.RelationBetweenRulesLengths, i);
                h.Add(ruleFilter);
            }
            return h;



            //IList<RuleFilterAggregator> h = new List<RuleFilterAggregator>();
            //for (int i = MinLength; i < MaxLength; i++)
            //{
            //    RuleFilterAggregator r = new RuleFilterAggregator(ruleFilterAggregator.InitialRuleSet, ruleFilterAggregator.Filters);
            //    IRuleFilter ruleFilter = new LengthFilter(initialFilter.RelationBetweenRulesLengths, i);
            //    r.AddFilter(ruleFilter);

            //    h.Add(r);
            //}
            //return h;
        }
    }
}
