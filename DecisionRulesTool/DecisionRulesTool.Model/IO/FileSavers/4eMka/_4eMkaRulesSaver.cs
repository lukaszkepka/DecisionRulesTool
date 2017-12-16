using DecisionRulesTool.Model.FileSavers;
using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DecisionRulesTool.Model.Utils;
using System.Globalization;

namespace DecisionRulesTool.Model.IO.FileSavers._4eMka
{
    public class _4eMkaRulesSaver : _4eMkaFileSaver<RuleSet>
    {

        public _4eMkaRulesSaver() : base()
        {
        }

        public override void Save(RuleSet content, StreamWriter streamWriter)
        {
            WriteHeader(content, streamWriter);
            WriteAtributes(content, streamWriter);
            WriteDecisionInfo(content, streamWriter);
            WritePreferences(content, streamWriter);
            WriteRules(content, streamWriter);
        }

        private void WriteHeader(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"# Learning File: {cachedFileName}");
            streamWriter.WriteLine();
        }

        public void WriteAtributes(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"{fileFormat.AttributesSectionHeader}");
            foreach (var attribute in content.Attributes)
            {
                streamWriter.WriteLine($"+ {attribute.Name}: {GetAttributeType(attribute)}");
            }
        }

        public void WritePreferences(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"{fileFormat.PreferencesSectionHeader}");
            foreach (var attribute in content.Attributes)
            {
                streamWriter.WriteLine($"{attribute.Name}: none");
            }

            streamWriter.WriteLine();
        }

        public void WriteDecisionInfo(RuleSet content, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"decision: {content.DecisionAttribute.Name}");
            streamWriter.WriteLine();
        }

        public void WriteRules(RuleSet content, StreamWriter streamWriter)
        {
            int ruleNumber = 1;
            streamWriter.WriteLine($"{fileFormat.RulesSectionHeader}");
            foreach (var rule in content.Rules)
            {
                streamWriter.Write($"Rule {ruleNumber++}. ");
                WriteRule(rule, streamWriter);
            }

            streamWriter.WriteLine($"\n{fileFormat.EndFileHeader}");
        }

        public void WriteRule(Rule rule, StreamWriter streamWriter)
        {
            int conditionsSoFar = 0;
            foreach (var condition in rule.Conditions)
            {
                WriteCondition(condition, streamWriter);
                if (++conditionsSoFar < rule.Conditions.Count)
                {
                    streamWriter.Write($" {fileFormat.ConditionSeparatorChar} ");
                }
                else
                {
                    streamWriter.Write($" {fileFormat.DecisionStringStartChars.FirstOrDefault()} ");
                }
            }

            WriteDecision((_4eMkaRule)rule, streamWriter);
            streamWriter.Write(Environment.NewLine);
        }

        public void WriteDecision(_4eMkaRule rule, StreamWriter streamWriter)
        {
            if (rule.Decisions.Count > 1)
            {
                throw new NotSupportedException("Multiple decisions not supported");
            }

            var decision = rule.Decisions.FirstOrDefault();
            string decisionSupportString = $"[{rule.SupportValue}, {string.Format("{0:P2}", rule.RelativeStrength/100)}]";
            string decisionString = $"({decision.DecisionAttribute.Name} {GetDecisionOperatorString(decision.Type)} {decision.Value}); {decisionSupportString}";
            streamWriter.Write(decisionString);
        }

        private string GetDecisionOperatorString(DecisionType type)
        {
            switch (type)
            {
                case DecisionType.AtMost:
                    return "at most";
                case DecisionType.AtLeast:
                    return "at least";
                default:
                    throw new NotSupportedException();
            }
        }

        public void WriteCondition(Condition condition, StreamWriter streamWriter)
        {
            streamWriter.Write($"({condition.Attribute.Name}{Tools.GetRelationString(condition.RelationType)}{GetConditionValue(condition)})");
        }

        private string GetConditionValue(Condition condition)
        {

            if (condition.Attribute.Type == AttributeType.Numeric)
            {
                return (Convert.ToDecimal(condition.Value)).ToString(CultureInfo.InvariantCulture);
            }
            else if (condition.Attribute.Type == AttributeType.Symbolic)
            {
                return condition.Value.ToString();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private string GetAttributeType(Model.Attribute attribute)
        {

            if (attribute.Type == AttributeType.Numeric)
            {
                return "(continuous)";
            }
            else if (attribute.Type == AttributeType.Symbolic)
            {
                return $"[{attribute.AvailableValues.Aggregate((x, y) => $"{x}, {y}")}]";
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
