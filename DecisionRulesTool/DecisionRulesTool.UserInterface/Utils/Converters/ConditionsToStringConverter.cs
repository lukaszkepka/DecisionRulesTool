using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.UserInterface.Utils.Converters
{
    using DecisionRulesTool.Model.Model;

    [ValueConversion(typeof(ICollection<Condition>), typeof(string))]
    public class ConditionsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder result = new StringBuilder();
            if (value is ICollection<Condition> conditions)
            {
                foreach (var condition in conditions)
                {
                    result.Append(condition.Attribute.Name);
                    result.Append($" {Tools.GetRelationString(condition.RelationType)} ");
                    result.Append(condition.Value ?? Attribute.MissingValue);
                    result.Append(" & ");
                }
                result.Remove(result.Length - 3, 2);
            }
            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
