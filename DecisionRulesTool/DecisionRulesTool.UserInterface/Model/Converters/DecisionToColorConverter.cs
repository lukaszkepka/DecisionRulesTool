using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DecisionRulesTool.UserInterface.Model.Converters
{
    public class DecisionToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.White);

            if (value is DataRowView dataRowView)
            {
                if (dataRowView.Row.ItemArray.Length > 0)
                {
                    switch (dataRowView.Row[1])
                    {
                        case ClassificationResult.PositiveClassification:
                            solidColorBrush.Color = Color.FromRgb(134, 240, 72);
                            break;
                        case ClassificationResult.NegativeClassification:
                            solidColorBrush.Color = Color.FromRgb(242, 30, 26);
                            break;
                        case ClassificationResult.NoCoverage:
                        case ClassificationResult.Ambigious:
                            solidColorBrush.Color = Colors.Silver;
                            break;
                    }
                }
            }

            return solidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
