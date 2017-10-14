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
    public class SupportValueFilter : ValueBasedFilter
    {
        /// <summary>
        /// Constructor for support value filter
        /// </summary>
        /// <param name="relation">Describes relation between desired support value and actual rule's support value</param>
        /// <param name="desiredValue">Desired support value</param>
        /// <param name="ruleSupportComparer">Provides comparision between numbers based on relation. IntegerAttributeComparer by default</param>
        public SupportValueFilter(Relation relation, int desiredValue, IAttributeValuesComparer ruleSupportComparer = null) : base(relation, desiredValue, ruleSupportComparer)
        {
        }

        public override bool CheckCondition(Rule rule)
        {
            return RuleAttributeComparer.AreInRelation(this.Relation, rule.SupportValue, this.DesiredValue);
        }

        public override string ToString()
        {
            return $"Support {Tools.GetRelationString(this.Relation)} {this.DesiredValue}";
        }
    }
}
