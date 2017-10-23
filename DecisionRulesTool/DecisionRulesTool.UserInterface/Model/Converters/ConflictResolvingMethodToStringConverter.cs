using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DecisionRulesTool.UserInterface.Model.Converters
{
    public class ConflictResolvingMethodToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string conflictResolvingMethodString = string.Empty;
            if (value is ConflictResolvingMethod conflictResolvingMethod)
            {
                switch (conflictResolvingMethod)
                {
                    case ConflictResolvingMethod.WeightedVoting:
                        conflictResolvingMethodString = "Weighted Voting";
                        break;
                    case ConflictResolvingMethod.MajorityVoting:
                        conflictResolvingMethodString = "Majority Voting";
                        break;
                    case ConflictResolvingMethod.RefuseConflicts:
                        conflictResolvingMethodString = "Refuse Conflicts";
                        break;
                    default:
                        break;
                }
            }
            return conflictResolvingMethodString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
