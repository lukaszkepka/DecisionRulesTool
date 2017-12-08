using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.AttributePresenceStrategy;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class AttributePresenceFilter : IRuleFilter
    {
        private string[] attributeNames;
        private IAttributePresenceStrategy attributePresenceStrategy;
        private Mode mode;

        public AttributePresenceFilter(Mode mode, params string[] attributeNames)
        {
            this.attributeNames = attributeNames;
            this.mode = mode;

            InitializeAttributePresenceStrategy();
        }

        private void InitializeAttributePresenceStrategy()
        {
            this.attributePresenceStrategy = default(IAttributePresenceStrategy);
            switch (mode)
            {
                case Mode.Exact:
                    attributePresenceStrategy = new ExactModeStrategy();
                    break;
                case Mode.Any:
                    attributePresenceStrategy = new AnyAttributeStrategy();
                    break;
                case Mode.All:
                    attributePresenceStrategy = new AllAtributesStrategy();
                    break;
                default:
                    attributePresenceStrategy = new DefaultAttributeStrategy();
                    break;
            }
        }

        public virtual bool CheckCondition(Rule rule, string[] attributeName)
        {
            return attributePresenceStrategy.CheckCondition(rule, attributeName);
        }

        public RuleSet FilterRules(RuleSet ruleSet)
        {
            RuleSet newRuleSet = new RuleSet(ruleSet.Name, ruleSet.Attributes, new List<Rule>(), ruleSet.DecisionAttribute);
            foreach (Rule rule in ruleSet.Rules)
            {
                if (CheckCondition(rule, attributeNames))
                {
                    newRuleSet.Rules.Add(rule);
                }
            }
            return newRuleSet;
        }

        public string GetShortName()
        {
            return $"A({mode.ToString()})[{attributeNames.Aggregate((x, y) => x + "_" + y)}]";
        }

        public override string ToString()
        {
            return $"Attribute filter (Mode = {mode.ToString()}) {{{attributeNames.Aggregate((x,y) => x + "_" + y)}}}";
        }

        public enum Mode
        {
            Any,
            All,
            Exact
        }
    }
}
