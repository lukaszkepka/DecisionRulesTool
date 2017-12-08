using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class LengthFilter : ValueBasedFilter
    {
        /// <summary>
        /// Constructor for length filter
        /// </summary>
        /// <param name="relation">Describes relation between desired rule length and actual rule length</param>
        /// <param name="desiredLength">Desired rule length</param>
        /// <param name="ruleLengthComparer">Provides comparision between numbers based on relation. IntegerAttributeComparer by default</param>
        public LengthFilter(Relation relation, int desiredLength, IAttributeValuesComparer ruleLengthComparer = null) : base(relation, desiredLength, ruleLengthComparer)
        {
        }

        public override string ToString()
        {
            return $"Length {Tools.GetRelationString(Relation)} {DesiredValue}";
        }

        public override bool CheckCondition(Rule rule)
        {
            return RuleAttributeComparer.AreInRelation(Relation, rule.Conditions.Count, DesiredValue);
        }

        public override string GetShortName()
        {
            return $"L_{GetShortRelationName()}_{DesiredValue}";
        }
    }
}
