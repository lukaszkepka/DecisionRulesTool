using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class ApplicationViewModel : BaseViewModel
    {
        public ICollection<RuleSetSubset> RuleSets { get; private set; }

        public ToolbarViewModel ToolbarViewModel { get; set; }
        public RuleSetManagerViewModel RuleSetManagerViewModel { get; set; }
        public TestConfiguratorViewModel TestConfiguratorViewModel { get; set; }
        public TestResultViewerViewModel TestResultViewerViewModel { get; set; }

        public ApplicationViewModel(IUnityContainer container) : base(container)
        {
            RuleSets = new ObservableCollection<RuleSetSubset>();

            //RuleSetManagerViewModel = new RuleSetManagerViewModel();
            //TestConfiguratorViewModel = new TestConfiguratorViewModel(RuleSets);
            //TestResultViewerViewModel = new TestResultViewerViewModel(new ObservableCollection<TestRequest>());

            //ToolbarViewModel ToolbarViewModel = new ToolbarViewModel()
            //{
            //    RuleSetManagerViewModel = this.RuleSetManagerViewModel,
            //    TestConfiguratorViewModel = this.TestConfiguratorViewModel,
            //    TestResultViewerViewModel = this.TestResultViewerViewModel
            //};
        }
    }
}
