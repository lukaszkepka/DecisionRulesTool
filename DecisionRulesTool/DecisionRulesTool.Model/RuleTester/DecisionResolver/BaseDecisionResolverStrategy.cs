using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleTester.DecisionResolver
{
    using Model;
    using Utils;

    public abstract class BaseDecisionResolverStrategy : IDecisionResolverStrategy
    {
        protected readonly IDictionary<Object, int[]> decisionMatrix;
        protected readonly string[] decisionValues;
        protected readonly int decisionAttributeIndex;

        public abstract ConflictResolvingMethod ResolvingMethod { get; }
        protected abstract string ResolveConflict(KeyValuePair<Object, int[]> decisionItem);

        public BaseDecisionResolverStrategy(DataSet testSet, Attribute decisionAttribute)
        {
            decisionValues = decisionAttribute.AvailableValues;
            decisionAttributeIndex = testSet.Attributes.TakeWhile(x => !x.Equals(decisionAttribute)).Count();
            decisionMatrix = new Dictionary<Object, int[]>(ReferenceEqualityComparer.Default);
            foreach (var dataObject in testSet.Objects)
            {
                decisionMatrix.Add(dataObject, new int[decisionValues.Length]);
            }
        }

        public BaseDecisionResolverStrategy(DataSet testSet, Attribute decisionAttribute, string[] decisionValues)
        {
            this.decisionValues = decisionValues;
            decisionAttributeIndex = testSet.Attributes.TakeWhile(x => !x.Equals(decisionAttribute)).Count();
            decisionMatrix = new Dictionary<Object, int[]>(ReferenceEqualityComparer.Default);
            foreach (var dataObject in testSet.Objects)
            {
                decisionMatrix.Add(dataObject, new int[decisionValues.Length]);
            }
        }

        public int GetDecisionValueIndex(Decision decision)
        {
            for (int i = 0; i < decisionValues.Length; i++)
            {
                if (decision.Value.Equals(decisionValues[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual void AddDecision(Object dataObject, Rule rule)
        {
            foreach (Decision decision in rule.Decisions)
            {
                AddDecision(dataObject, decision);
            }
        }

        public virtual void AddDecision(Object dataObject, Decision decision)
        {
            int index = GetDecisionValueIndex(decision);
            if (index >= 0)
            {
                decisionMatrix[dataObject][index]++;
            }
        }

        public virtual ClassificationResult[] RunClassification()
        {
            ClassificationResult[] classificationResults = new ClassificationResult[decisionMatrix.Count];
            for (int i = 0; i < decisionMatrix.Count; i++)
            {
                var decisionItem = decisionMatrix.ElementAt(i);
                string decisionValue = string.Empty;

                if (HasConflicts(decisionItem))
                {
                    decisionValue = ResolveConflict(decisionItem);
                }
                else
                {
                    decisionValue = GetDecision(decisionItem);
                }

                classificationResults[i].Result = GetClassificationResult(decisionItem.Key, decisionValue);
                classificationResults[i].DecisionValue = decisionValue;
            }
            return classificationResults;
        }

        private bool HasConflicts(KeyValuePair<Object, int[]> decisionItem)
        {
            return decisionItem.Value.Max() != decisionItem.Value.Sum();
        }

        private string GetDecision(KeyValuePair<Object, int[]> decisionItem)
        {
            string decision = string.Empty;

            if (decisionItem.Value.Sum() == 0)
            {
                decision = ClassificationResult.NoCoverage;
            }
            else
            {
                int maxIndex = decisionItem.Value.GetIndexOfMax();
                decision = decisionValues[maxIndex];
            }
            return decision;
        }

        protected string GetClassificationResult(Object dataObject, string decisionValue)
        {
            string result = string.Empty;
            object actualDecisionValue = dataObject.Values[decisionAttributeIndex];
            if (decisionValues.Contains(decisionValue))
            {
                if (actualDecisionValue.Equals(decisionValue))
                {
                    result = ClassificationResult.PositiveClassification;
                }
                else
                {
                    result = ClassificationResult.NegativeClassification;
                }
            }
            else
            {
                result = decisionValue;
            }
            return result;
        }
    }
}
