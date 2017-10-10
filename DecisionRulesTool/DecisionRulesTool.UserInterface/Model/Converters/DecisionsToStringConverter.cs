using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.UserInterface.Model.Converters
{
    [ValueConversion(typeof(ICollection<Decision>), typeof(string))]
    public class DecisionsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder result = new StringBuilder(); ;
            ICollection<Decision> decisions = value as ICollection< Decision>;
            if (decisions != null)
            {
                foreach (var decision in decisions)
                {
                    result.Append(decision.DecisionAttribute.Name);
                    result.Append($" {Tools.GetDecisionTypeString(decision.Type)} ");
                    result.Append(decision.Value.ToString());
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
