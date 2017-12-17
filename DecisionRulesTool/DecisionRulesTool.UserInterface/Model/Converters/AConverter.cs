using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DecisionRulesTool.UserInterface.Model.Converters
{
    class AConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Colors.White;
            if (parameter is TextBlock textBlock && value is DataGridCell cell)
            {
                var dataTable = (DataTable)textBlock.DataContext;
                var dataColumn = dataTable.Columns[cell.Column.Header.ToString()];

                object resultObject = dataTable.Rows[dataTable.Rows.Count - 1][dataColumn];
                color = GetColorForColumn(resultObject.ToString());
            }

            return new SolidColorBrush(color);
        }

        private Color GetColorForColumn(string result)
        {
            Color color = Colors.White;
            switch (result)
            {
                case ClassificationResult.PositiveClassification:
                    color = Color.FromRgb(134, 240, 72);
                    break;
                case ClassificationResult.NegativeClassification:
                    color = Color.FromRgb(242, 30, 26);
                    break;
                case ClassificationResult.NoCoverage:
                case ClassificationResult.Ambigious:
                    color = Colors.Silver;
                    break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
