using DecisionRulesTool.Model.RuleFilters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Utils
{
    class _FiltersToStringConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (value is IEnumerable<IRuleFilter> filters && filters.Any())
            {
                result = filters.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}");
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
