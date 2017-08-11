using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using System.Diagnostics;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.IO;

namespace DecisionRulesTool.Model.Parsers
{
    using Model;
    using System.Globalization;

    public class RsesRulesParser : RsesFileParser<RuleSet>
    {
        public override string[] SupportedFormats => new[] { "rul" };

        public RsesRulesParser() : base()
        {

        }

        private void ParseHeader(StreamReader fileStream, RuleSet rulesSet)
        {
            rulesSet.Name = GetSectionValue(fileStream, RsesFileFormat.RulesFileHeader);
        }

        private void ParseAttributes(StreamReader fileStream, RuleSet rulesSet)
        {
            foreach (var attribute in base.ParseAttributes(fileStream))
            {
                rulesSet.Attributes.Add(attribute);
            }
            rulesSet.DecisionAttribute = rulesSet.Attributes.Last();
        }

        private string[] ParseDecisionValues(StreamReader fileStream, RuleSet rulesSet)
        {
            int valuesCount = Convert.ToInt32(GetSectionValue(fileStream, RsesFileFormat.DecisionValuesSectionHeader));
            string[] decisionValues = new string[valuesCount];
            for (int i = 0; i < valuesCount; i++)
            {
                decisionValues[i] = fileStream.ReadLine();
            }
            return decisionValues;
        }

        private void ParseRules(StreamReader fileStream, RuleSet rulesSet)
        {
            int rulesCount = Convert.ToInt32(GetSectionValue(fileStream, RsesFileFormat.RulesSectionHeader));
            for (int i = 0; i < rulesCount; i++)
            {
                string ruleString = fileStream.ReadLine();
                Rule rule = ParseRule(ruleString, rulesSet);
                rulesSet.Rules.Add(rule);
            }
        }

        private Condition ParseCondition(string conditionString, RuleSet rulesSet)
        {
            //Remove brackets from condition string
            string rawCondition = RemoveBrackets(conditionString);
            string[] relationParts = rawCondition.Split(RsesFileFormat.ConditionRelationChars, StringSplitOptions.RemoveEmptyEntries);
            var attributeName = relationParts[0];

            Attribute attribute = rulesSet.Attributes.Where(x => x.Name.Equals(attributeName)).FirstOrDefault();
            object attributeValue = GetAttributeValue(attribute, relationParts[1]);
            //Relation is set to equality, because we assume that RSES don't have another types available
            return new Condition(Relation.Equality, attribute, attributeValue);
        }

        private ICollection<Decision> ParseDecision(string decisionString, Rule rule)
        {
            ICollection<Decision> decisions = new List<Decision>();
            var decisionStringParts = decisionString.Split(' ');
            string decisionStringWithoutSupport = decisionStringParts.Take(decisionStringParts.Length - 1).Aggregate((x, y) => x + y);
            //Remove brackets from decision string
            string rawDecision = RemoveBrackets(decisionStringWithoutSupport);
            string[] rawDecisionParts = rawDecision.Split(RsesFileFormat.DecisionRelationChars, StringSplitOptions.RemoveEmptyEntries);
            string decisionValuesString = rawDecisionParts[1];

            string[] decisionValues;
            //There are mutliple decisions for rule
            if (decisionValuesString.StartsWith("{"))
            {
                //Remove brackets from decision string
                string rawDecisionValues = RemoveBrackets(decisionValuesString);
                decisionValues = rawDecisionValues.Split(',');
            }
            //There are one decision for rule
            else
            {
                decisionValues = new[] { decisionValuesString };
            }

            foreach (var decisionValue in decisionValues)
            {
                var decisionValueParts = decisionValue.Split('[');
                string supportValueString = new string(decisionValueParts[1].TakeWhile(x => x != ']').ToArray());
                int support = Convert.ToInt32(supportValueString);
                Decision decision = new Decision(DecisionType.Equality, rule, decisionValueParts[0], support);
                decisions.Add(decision);
            }
            return decisions;
        }

        private Rule ParseRule(string ruleString, RuleSet ruleSet)
        {
            Rule rule = new Rule(ruleSet);

            string[] ruleParts = ruleString.Split(RsesFileFormat.DecisionStringStartChars, StringSplitOptions.RemoveEmptyEntries);
            var conditionsString = ruleParts[0];
            var decisionString = ruleParts[1];
            var conditionsArray = conditionsString.Replace(" ", String.Empty).Split(RsesFileFormat.ConditionSeparatorChar);
            //Add conditions
            foreach (string condition in conditionsArray)
            {
                rule.Conditions.Add(ParseCondition(condition, ruleSet));
            }
            //Add decisions
            foreach (Decision decision in ParseDecision(decisionString, rule))
            {
                rule.Decisions.Add(decision);
            }

            return rule;
        }

        public override RuleSet ParseFile(StreamReader fileStream)
        {
            RuleSet ruleSet = default(RuleSet);
            using (fileStream)
            {
                ruleSet = new RuleSet();
                ParseHeader(fileStream, ruleSet);
                ParseAttributes(fileStream, ruleSet);
                ParseDecisionValues(fileStream, ruleSet);
                ParseRules(fileStream, ruleSet);
            }
            return ruleSet;
        }
    }
}
