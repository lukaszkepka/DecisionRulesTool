using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model
{
    [DebuggerDisplay("{ToString()}")]
    public class SelectableItem<T> : ISelectable
    {
        public bool IsSelected { get; set; }
        public T Item { get; set; }

        public SelectableItem(T item)
        {
            Item = item;
            IsSelected = false;
        }

        public static explicit operator T(SelectableItem<T> selectableItem)
        {
            return selectableItem.Item;
        }

        public static explicit operator SelectableItem<T>(T item)
        {
            return new SelectableItem<T>(item);
        }

        public override string ToString()
        {
            return $"Selected = {IsSelected}, Item = {Item.ToString()}";
        }
    }
}
