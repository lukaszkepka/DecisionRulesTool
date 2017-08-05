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
    public class RsesRulesParser : RsesFileParser<RuleSet>
    {
        public override string[] SupportedFormats => new[] { "rul" };

        public RsesRulesParser() : base()
        {

        }

        private void ParseHeader(StreamReader fileStream, RuleSet rulesSet)
        {
            rulesSet.Name = GetSectionValue(fileStream, RsesFileFormat.RULES_FILE_HEADER);
        }

        private void ParseAttributes(StreamReader fileStream, RuleSet rulesSet)
        {
            //rulesSet.Attributes = base.ParseAttributes(fileStream);
        }

        private void ParseDecisionValues(StreamReader fileStream, RuleSet rulesSet)
        {
            ICollection<string> decisionValues = new List<string>();
            int valuesCount = Convert.ToInt32(GetSectionValue(fileStream, RsesFileFormat.DECISION_VALUES_SECTION_HEADER));        
            for (int i = 0; i < valuesCount; i++)
            {
                string value = fileStream.ReadLine();
                decisionValues.Add(value);
            }
            //rulesSet.DecisionValues = decisionValues;
        }

        private void ParseRules(StreamReader fileStream, RuleSet rulesSet)
        {
            ICollection<Rule> rules = new List<Rule>();
            int rulesCount = Convert.ToInt32(GetSectionValue(fileStream, RsesFileFormat.RULES_SECTION_HEADER));
            for (int i = 0; i < rulesCount; i++)
            {
                string ruleString = fileStream.ReadLine();
                Rule rule = ParseRule(ruleString, rulesSet);
                rules.Add(rule);
            }
            rulesSet.Rules = rules;
        }

        private Premise ParseRelation(string relationString, RuleSet rulesSet)
        {
            string rawRelation = relationString.Substring(1, relationString.Length - 2);
            string[] relationParts = rawRelation.Split('=');
            var attributeName = relationParts[0];
            var attributeValue = relationParts[1];

            Premise relation = new Premise();
            relation.Attribute = rulesSet.Attributes.Where(x => x.Name.Equals(attributeName)).FirstOrDefault();
            //relation.Type = Premise.Category.Equality;
            //relation.Value = attributeValue;

            return relation;
        }

        private IEnumerable<Conclusion> ParseDecision(string decisionString, RuleSet rulesSet)
        {
            ICollection<Conclusion> decisions = new List<Conclusion>();
            var t = decisionString.Split(' ')[0];
            string rawDecision = t.Substring(1, t.Length - 2);
            string[] rawDecisionParts = rawDecision.Split('=');
            string decisionAttribute = rawDecisionParts[0];
            string[] decisionValues;
            string decisionValuesString = rawDecisionParts[1];
            if(decisionValuesString[0] == '{')
            {
                string q = decisionValuesString.Substring(1, decisionValuesString.Length - 2);
                decisionValues = q.Split(',');
            }
            else
            {
                decisionValues = new[] { decisionValuesString };
            }

            foreach(var decisionValue in decisionValues)
            {
                var decisionParts = decisionValue.Split('[');
                string supportValueString = new string(decisionParts[1].TakeWhile(x => x != ']').ToArray());
                Conclusion decision = new Conclusion();
                //decision.SupportValue = Convert.ToDouble(supportValueString);
                //decision.Relation = new Premise()
                //{
                //    Attribute = rulesSet.DecisionAttribute,
                //    Type = Premise.Category.Equality,
                //    Value = decisionParts[0]
                //};
                //decisions.Add(decision);

            }
            return decisions;
        }

        private Rule ParseRule(string ruleString, RuleSet rulesSet)
        {
            ICollection<Premise> relations = new List<Premise>();

            string[] ruleParts = ruleString.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
            var relationsString = ruleParts[0];
            var decision = ruleParts[1];
            var conditions = relationsString.Split('&');
            foreach(string condition in conditions)
            {
                var relation = ParseRelation(condition, rulesSet);
                relations.Add(relation);
            }

            Rule rule = new Rule();
            //rule.Decision = ParseDecision(decision, rulesSet);
            //rule.Relations = relations;
            //rule.RulesSet = rulesSet;
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
