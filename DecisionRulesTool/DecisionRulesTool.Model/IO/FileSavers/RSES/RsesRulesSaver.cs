using DecisionRulesTool.Model.FileLoaders.RSES;
using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DecisionRulesTool.Model.IO;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.Model.FileSavers.RSES
{
    public class RsesRulesSaver : RsesFileSaver<RuleSet>
    {
        public override void Save(RuleSet content, StreamWriter streamWriter)
        {
            WriteHeader(content, streamWriter);
            WriteAtributes(content, streamWriter);
            WriteDecisionValues(content, streamWriter);
            WriteRules(content, streamWriter);
        }

        public void WriteHeader(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"{fileFormat.RuleFileHeader} {content.Name}");
        }

        public void WriteAtributes(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"{fileFormat.AttributesSectionHeader} {content.Attributes.Count}");
            foreach (var attribute in content.Attributes)
            {
                streamWriter.WriteLine($"{attribute.Name} {GetAttributeType(attribute)}");
            }
        }

        public void WriteDecisionValues(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"{fileFormat.DecisionValuesSectionHeader} {content.DecisionAttribute.AvailableValues.Count()}");
            foreach (var decisionClass in content.DecisionAttribute.AvailableValues)
            {
                streamWriter.WriteLine(decisionClass);
            }
        }

        public void WriteRules(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"{fileFormat.RulesSectionHeader} {content.Rules.Count}");
            foreach (var rule in content.Rules)
            {
                WriteRule(rule, streamWriter);
            }
        }

        public void WriteRule(Rule rule, StreamWriter streamWriter)
        {
            int conditionsSoFar = 0;
            foreach (var condition in rule.Conditions)
            {
                WriteCondition(condition, streamWriter);
                if(++conditionsSoFar < rule.Conditions.Count)
                {
                   streamWriter.Write(fileFormat.ConditionSeparatorChar);
                }
                else
                {
                    streamWriter.Write(fileFormat.DecisionStringStartChars.FirstOrDefault());
                }         
            }

            WriteDecision(rule.Decisions, streamWriter);
            streamWriter.Write(Environment.NewLine);
        }

        public void WriteDecision(ICollection<Decision> decisions, StreamWriter streamWriter)
        {
            if(decisions.Count > 1)
            {
                throw new NotSupportedException("Multiple decisions not supported");
            }

            var decision = decisions.FirstOrDefault();
            string decisionSupportString = $"[{decision.Support}]";
            string decisionString = $"({decision.DecisionAttribute.Name}{fileFormat.DecisionRelationChars.FirstOrDefault()}{decision.Value}{decisionSupportString}) {decision.Support}";
            streamWriter.Write(decisionString);
        }

        public void WriteCondition(Condition condition, StreamWriter streamWriter)
        {
            streamWriter.Write($"({condition.Attribute.Name}{Tools.GetRelationString(condition.RelationType)}{condition.Value})");
        }

        private string GetAttributeType(Model.Attribute attribute)
        {
            return attribute.Type.ToString().ToLowerInvariant();
        }
    }
}
