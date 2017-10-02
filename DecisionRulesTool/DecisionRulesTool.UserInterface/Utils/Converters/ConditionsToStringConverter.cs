using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace DecisionRulesTool.UserInterface.Utils.Converters
{
    [ValueConversion(typeof(ICollection<Condition>), typeof(string))]
    public class ConditionsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder result = new StringBuilder(); ;
            ICollection<Condition> conditions = value as ICollection<Condition>;
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    result.Append(condition.Attribute.Name);
                    result.Append(" ");
                    result.Append(condition.RelationType.ToString());
                    result.Append(" ");
                    result.Append(condition.Value == null ? string.Empty : condition.Value);
                    result.Append(" AND ");
                }
            }
            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
