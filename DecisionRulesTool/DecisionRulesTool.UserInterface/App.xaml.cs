using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Interfaces;
using DecisionRulesTool.UserInterface.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace DecisionRulesTool.UserInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void InitializeContainer(IUnityContainer container)
        {
            ICollection<RuleSetSubset> ruleSets = new ObservableCollection<RuleSetSubset>();


            container.RegisterType<IRuleSetLoaderService, RuleSetLoaderService>();
            container.RegisterType<IRuleSetSubsetService, RuleSetSubsetService>();
            container.RegisterType<ITestSetLoaderService, TestSetLoaderService>();
            container.RegisterType<IWindowNavigatorService, WindowNavigatorService>();

            //container.RegisterInstance(new ToolbarViewModel(container));
            //container.RegisterInstance(new RuleSetManagerViewModel(ruleSets, container));
            //container.RegisterInstance(new TestConfiguratorViewModel(ruleSets, container));
            //container.RegisterInstance(new TestResultViewerViewModel(new ObservableCollection<TestRequest>(), container));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            InitializeContainer(container);

            IWindowNavigatorService windowNavigatorService = container.Resolve<IWindowNavigatorService>();
            windowNavigatorService.SwitchContext(new RuleSetManagerViewModel(new ObservableCollection<RuleSetSubset>(), container));
        }

    }
}
