using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model
{
    public static class SelectableItemExtension
    {
        public static IEnumerable<T> GetSelectedItems<T>(this IEnumerable<SelectableItem<T>> selectableItems)
        {
            return selectableItems.Where(x => x.IsSelected).Select(x => x.Item);
        }
    }
}
