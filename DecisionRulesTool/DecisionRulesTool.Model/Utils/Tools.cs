using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Utils
{
    public static class Tools
    {
        public static string GetRelationString(Relation relation)
        {
            string result = string.Empty;
            switch (relation)
            {
                case Relation.Equality:
                    result = "=";
                    break;
                case Relation.Greather:
                    result = ">";
                    break;
                case Relation.GreatherOrEqual:
                    result = ">=";
                    break;
                case Relation.Less:
                    result = "<";
                    break;
                case Relation.LessOrEqual:
                    result = "<=";
                    break;
                case Relation.Undefined:
                    result = "?";
                    break;
                default:
                    break;
            }
            return result;
        }
        public static Relation ParseRelationString(string relationString)
        {
            Relation relation = Relation.Undefined;
            switch (relationString)
            {
                case "=":
                    relation = Relation.Equality;
                    break;
                case ">":
                    relation = Relation.Greather;
                    break;
                case ">=":
                    relation = Relation.GreatherOrEqual;
                    break;
                case "<":
                    relation = Relation.Less;
                    break;
                case "<=":
                    relation = Relation.LessOrEqual;
                    break;
            }
            return relation;
        }
        public static string GetDecisionTypeString(DecisionType decisionType)
        {
            string result = string.Empty;
            switch (decisionType)
            {
                case DecisionType.Equality:
                    result = "=";
                    break;
                case DecisionType.AtMost:
                    result = "At Most";
                    break;
                case DecisionType.AtLeast:
                    result = "At Least";
                    break;
                case DecisionType.Undefined:
                    result = "?";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
