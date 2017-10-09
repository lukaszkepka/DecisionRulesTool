using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DecisionRulesTool.UserInterface.Model.CustomControls
{
    public class ReorderableTabControl : TabControl
    {
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            a();
        }
        void a ()
        {
            //if (true)
            //{
            //    var tabItem = Items[1];
            //    Items.RemoveAt(1);
            //    Items.Insert(0, tabItem);
            //    ((TabItem)tabItem).IsSelected = true;
            //}
        }
    }
}
