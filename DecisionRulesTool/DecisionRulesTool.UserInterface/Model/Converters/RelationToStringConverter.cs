using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace DecisionRulesTool.UserInterface.Model.Converters
{
    [ValueConversion(typeof(Relation), typeof(string))]
    public class RelationToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string relationString = string.Empty;
            if (value is Relation relation)
            {
                switch (relation)
                {
                    case Relation.Equality:
                        relationString = "=";
                        break;
                    case Relation.Greather:
                        relationString = ">";
                        break;
                    case Relation.GreatherOrEqual:
                        relationString = ">=";
                        break;
                    case Relation.Less:
                        relationString = "<";
                        break;
                    case Relation.LessOrEqual:
                        relationString = "<=";
                        break;
                    case Relation.Undefined:
                        relationString = "?";
                        break;
                    default:
                        break;
                }
            }
            return relationString;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Relation relation = Relation.Undefined;
            Enum.TryParse(value.ToString(), out relation);
            return relation;
        }
    }
}
