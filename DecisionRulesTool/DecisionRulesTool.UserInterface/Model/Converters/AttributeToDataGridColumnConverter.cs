using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace DecisionRulesTool.UserInterface.Model.Converters
{
    using DecisionRulesTool.Model.Model;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    public class AttributeToDataGridColumnConverter : IValueConverter
    {
        #region Constructors
        /// <summary>
        /// The default constructor
        /// </summary>
        public AttributeToDataGridColumnConverter() { }
        #endregion

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<DataGridColumn> dataGridColumns = new ObservableCollection<DataGridColumn>();
            if (value is IEnumerable<Attribute> attributes)
            {
                int i = 0;
                foreach (var attribute in attributes)
                {
                    var binding = new Binding($"Values[{i++}]");

                    dataGridColumns.Add(new DataGridTextColumn()
                    {
                        Header = attribute.Name,
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                        Binding = binding
                    });
                }
            }
            return dataGridColumns;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
