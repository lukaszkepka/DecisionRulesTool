using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DecisionRulesTool.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for ConfusionMatrix.xaml
    /// </summary>
    public partial class ConfusionMatrix : UserControl
    {
        public ConfusionMatrix()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ActualWidth > 0 && dataGrid.ActualHeight > 0 && dataGrid.Columns.Any())
            {
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    double width = (dataGrid.ActualWidth - 75) / (dataGrid.Columns.Count - 1);
                    if (i == 0)
                    {
                        width = 75;
                    }

                    dataGrid.Columns[i].Width = width;
                }


                dataGrid.RowHeight = dataGrid.ActualHeight / dataGrid.Columns.Count;
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.ActualWidth > 0 && dataGrid.ActualHeight > 0)
            {
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    double width = (dataGrid.ActualWidth - 75) / (dataGrid.Columns.Count - 1);
                    if (i == 0)
                    {
                        width = 75;
                    }

                    dataGrid.Columns[i].Width = width;
                }


                dataGrid.RowHeight = dataGrid.ActualHeight / dataGrid.Columns.Count;
            }
        }
    }
}
