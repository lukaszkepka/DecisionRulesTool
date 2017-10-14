using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class LengthFilterApplier : ValueBasedFiltersApplier
    {
        public LengthFilterApplier(int minLength, int maxLength, Relation relation) : base(minLength, maxLength, relation)
        {
        }

        public override IList<IRuleFilter> GenerateSeries()
        {
            IList<IRuleFilter> filters = new List<IRuleFilter>();
            for (int i = MinLength; i <= MaxLength; i++)
            {
                IRuleFilter ruleFilter = new LengthFilter(RelationBetweenRulesLengths, i);
                filters.Add(ruleFilter);
            }
            return filters;
        }

        //public RuleSetSubset[] ApplyFilterSeries(RuleSetSubset ruleSet)
        //{



            //var yy = actualSubsetLevel.ToList();
            //for (int j  = 0; j < yy.Count; j++)
            //{
            //    var item = yy[j];
            //    var g = item.Filters.LastOrDefault();
            //    var t = (LengthFilter)g;

            //    for (int i = GetLowerBound(t); i <= GetUpperBound(t, item); i++)
            //    {
            //        LengthFilter lengthFiler = new LengthFilter(Relation.Equality, i);
            //        //Use filter to generate new subset
            //        RuleSetSubset subset = new RuleSetSubset(item);
            //        subset.AddFilter(lengthFiler);
            //        subset.ApplyFilters();

            //        //Set new subset name 
            //        SetSubsetName(subset);

            //        //Attach new subset to parent
            //        item.Subsets.Add(subset);
            //        //Mark new subset as parent for next level
            //        actualSubsetLevel.Add(subset);
            //    }               
            //}

            //return actualSubsetLevel.ToArray();
        //}



    }
}
