using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace DecisionRulesTool.UserInterface.View
{
    /// <summary>
    /// Interaction logic for GroupedTestResultWindow.xaml
    /// </summary>
    public partial class GroupedTestResultWindow : Window
    {
        private Style style1;
        private Style style2;

        public GroupedTestResultWindow()
        {
            InitializeComponent();
            style1 = new Style()
            {
                TargetType = typeof(DataGridCell)
            };

            style1.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Color.FromRgb(134, 240, 72))));


            style2 = new Style()
            {
                TargetType = typeof(DataGridCell)
            };

            style2.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Colors.Yellow)));

        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        void A()
        {
            //var rows = GetDataGridRows(this.G1DataGrid);

            //foreach (DataGridRow r in rows)
            //{
            //    DataRowView rv = (DataRowView)r.Item;
            //    foreach (DataGridColumn column in G1DataGrid.Columns)
            //    {
            //        if (column.GetCellContent(r) is TextBlock)
            //        {
            //            TextBlock cellContent = column.GetCellContent(r) as TextBlock;
            //            cellContent.Background = new SolidColorBrush(Color.FromRgb(134, 240, 72));
            //            StyleSelector 

            //        }
            //    }
            //}
        }





        private void G1DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void G1DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            A();
        }
    }
}
