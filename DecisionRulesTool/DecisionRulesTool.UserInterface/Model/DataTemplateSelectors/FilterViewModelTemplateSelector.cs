using DecisionRulesTool.UserInterface.ViewModel.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DecisionRulesTool.UserInterface.Model.DataTemplateSelectors
{
    public class FilterViewModelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LengthFilterViewModelTemplate { get; set; }
        public DataTemplate SupportValueViewModelTemplate { get; set; }
        public DataTemplate AttributePresenceViewModelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tabItem = item as FilterViewModel;
            switch (item)
            {
                case LengthFilterViewModel lengthFilterViewModel:
                    return LengthFilterViewModelTemplate;
                case SupportValueFilterViewModel supportValueFilterViewModel:
                    return SupportValueViewModelTemplate;
                case AttributePresenceFilterViewModel sttributePresenceFilterViewModel:
                    return AttributePresenceViewModelTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}
