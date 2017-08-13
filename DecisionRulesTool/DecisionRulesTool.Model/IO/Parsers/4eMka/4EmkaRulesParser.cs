using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers._4eMka;
using DecisionRulesTool.Model.IO;
using System.Text.RegularExpressions;

namespace DecisionRulesTool.Model.Parsers
{
    using Model;
    using System.Globalization;

    public class _4eMkaRulesParser : _4eMkaFileParser<RuleSet>
    {
        public override string[] SupportedFormats => new[] { "rls" };

        private readonly Regex ruleBeginRegex = new Regex("\\bRule \\b\\d+. ");

        public override RuleSet ParseFile(StreamReader fileStream)
        {
            RuleSet ruleSet = default(RuleSet);
            using (fileStream)
            {
                ruleSet = new RuleSet();
                ParseAttributes(fileStream, ruleSet);
                ParsePreferences(fileStream, ruleSet);
                ParseRules(fileStream, ruleSet);
            }
            return ruleSet;
        }

        private void ParsePreferences(StreamReader fileStream, RuleSet ruleSet)
        {

        }

        private void ParseRuleParameters(string parametersString, _4eMkaRule rule)
        {
            string[] parameterParts = RemoveBrackets(parametersString).Split(',');
            parameterParts = parameterParts.Select(x => x.Trim(' ', '%')).ToArray();
            rule.SetSupportValue(Convert.ToInt32(parameterParts[0]));
            rule.RelativeStrength = Convert.ToDecimal(parameterParts[1], CultureInfo.InvariantCulture);
        }

        private IEnumerable<Decision> ParseDecisions(string decisionString, _4eMkaRule rule)
        {
            IList<Decision> decisions = new List<Decision>();
            string[] decisionStrings = decisionString.Split(';');
            string rawDecision = RemoveBrackets(decisionStrings[0].Trim(' ')); 
            string[] rawDecisionArray = rawDecision.Split(' ');

            DecisionType decisionType = fileFormat.GetDecisionType(rawDecision);

            string[] decisionValues;
            //There are multiple decisions
            if (decisionType == DecisionType.Equality && rawDecisionArray.Contains("OR"))
            {
                string rawDecisionValues = rawDecision.Split('=').Last();
                decisionValues = rawDecisionValues.Split(new[] { "OR" }, StringSplitOptions.RemoveEmptyEntries);
                decisionValues = decisionValues.Select(x => x.Trim(' ')).ToArray();
            }
            //There are only one decision
            else
            {
                decisionValues = new[] { rawDecisionArray.Last() };
            }

            foreach(var decisionValue in decisionValues)
            {
                Decision decision = new Decision(decisionType, rule, decisionValue);
                decisions.Add(decision);
            }

            string parametersString = decisionStrings[1].Trim(' ');
            ParseRuleParameters(parametersString, rule);
            return decisions;
        }

        private Condition ParseCondition(string conditionString, RuleSet rulesSet)
        {
            string rawCondition = RemoveBrackets(conditionString);
            string[] relationParts = rawCondition.Split(fileFormat.ConditionRelationChars, StringSplitOptions.RemoveEmptyEntries);

            string attributeName = relationParts[0];
            Attribute attribute = rulesSet.Attributes.Where(x => x.Name.Equals(attributeName)).FirstOrDefault();
            object attributeValue = fileFormat.GetAttributeValue(attribute, relationParts[1]);
            Relation relation = fileFormat.GetConditionRelation(conditionString);

            return new Condition(relation, attribute, attributeValue);
        }

        private void ParseRules(StreamReader fileStream, RuleSet ruleSet)
        {
            MoveStreamToSection(fileStream, fileFormat.RulesSectionHeader);
            string fileLine = fileStream.ReadLine();
            while (fileLine != null && ruleBeginRegex.Match(fileLine).Success)
            {
                fileLine = ruleBeginRegex.Replace(fileLine, string.Empty);
                string[] fileLineParts = fileLine.Split(fileFormat.DecisionStringStartChars, StringSplitOptions.RemoveEmptyEntries);
                string conditionsString = fileLineParts[0];
                string decisionString = fileLineParts[1];
                string[] conditionsArray = conditionsString.Replace(" ", string.Empty).Split(fileFormat.ConditionSeparatorChar);

                _4eMkaRule rule = new _4eMkaRule(ruleSet);
                //Add conditions
                foreach (string condition in conditionsArray)
                {
                    rule.Conditions.Add(ParseCondition(condition, ruleSet));
                }

                //Add decisions
                foreach (Decision decision in ParseDecisions(decisionString, rule))
                {
                    rule.Decisions.Add(decision);
                }

                ruleSet.Rules.Add(rule);
                fileLine = fileStream.ReadLine();
            }
        }

        private void ParseAttributes(StreamReader fileStream, RuleSet ruleSet)
        {
            foreach (var attribute in base.ParseAttributes(fileStream))
            {
                ruleSet.Attributes.Add(attribute);
            }
            ruleSet.DecisionAttribute = ruleSet.Attributes.Last();
        }
    }
}
