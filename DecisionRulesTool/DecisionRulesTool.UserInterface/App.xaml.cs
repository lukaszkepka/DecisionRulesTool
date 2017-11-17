using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Interfaces;
using DecisionRulesTool.UserInterface.ViewModel;
using System.Collections.ObjectModel;
using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.UserInterface.Services.Dialog;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System.Windows;

namespace DecisionRulesTool.UserInterface
{
    using DecisionRulesTool.Model;
    using DecisionRulesTool.Model.IO;
    using DecisionRulesTool.Model.Model;
    using DecisionRulesTool.Model.Parsers;
    using DecisionRulesTool.Model.RuleTester.Result;
    using DecisionRulesTool.Model.RuleTester.Result.Interfaces;
    using DecisionRulesTool.UserInterface.Model;
    using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
    using System;
    using Unity;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected ApplicationCache InitializeApplicationCache()
        {
            //Rule sets
            var ruleSets = new ObservableCollection<RuleSetSubset>();
            IFileParserFactory<RuleSet> fileParserFactory = new RuleSetParserFactory();
            IFileParser<RuleSet> ruleSetParser = fileParserFactory.Create(BaseFileFormat.FileExtensions.RSESRuleSet);
            RuleSetSubset ruleSet1 = new RuleSetSubsetViewItem(ruleSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Rules/male.rul"));
            RuleSetSubset ruleSet2 = new RuleSetSubsetViewItem(ruleSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Rules/female.rul"));
            ruleSets.Add(ruleSet1);
            ruleSets.Add(ruleSet2);

            //Test sets
            var testSets = new ObservableCollection<DataSet>();
            IFileParserFactory<DataSet> fileParserFactory1 = new DataSetParserFactory();
            IFileParser<DataSet> dataSetParser = fileParserFactory1.Create(BaseFileFormat.FileExtensions.RSESDataset);
            DataSet dataSet1 = dataSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Sets/mts.tab");
            DataSet dataSet2 = dataSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Sets/fts.tab");
            testSets.Add(dataSet1);
            testSets.Add(dataSet2);

            //Test requests
            var testRequests = new ObservableCollection<TestRequest>();
            foreach (ConflictResolvingMethod conflictResolvingMethod in Enum.GetValues(typeof(ConflictResolvingMethod)))
            {
                testRequests.Add(new TestRequest(ruleSet1, testSets[0], conflictResolvingMethod));
                testRequests.Add(new TestRequest(ruleSet2, testSets[1], conflictResolvingMethod));
            }

            return new ApplicationCache()
            {
                RuleSets = ruleSets,
                TestSets = testSets,
                TestRequests = testRequests
            };
        }


        protected void InitializeContainer()
        {
            SimpleIoc.Default.Register<IServiceLocator>(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ITestResultConverter<System.Data.DataTable>, TestResultToDataTableConverter>();
            SimpleIoc.Default.Register<IFileParserFactory<RuleSet>, RuleSetParserFactory>();
            SimpleIoc.Default.Register<IFileParserFactory<DataSet>, DataSetParserFactory>();
            SimpleIoc.Default.Register<ITestRequestService, TestRequestService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IWindowNavigatorService, WindowNavigatorService>();
            SimpleIoc.Default.Register<ITestSetLoaderService, TestSetLoaderService>();
            SimpleIoc.Default.Register<IRuleSetLoaderService, RuleSetLoaderService>();
            SimpleIoc.Default.Register<IRuleSetSubsetService, RuleSetSubsetService>();

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<RuleSetManagerViewModel>();
            SimpleIoc.Default.Register<TestConfiguratorViewModel>();
            SimpleIoc.Default.Register<TestResultViewerViewModel>();

            SimpleIoc.Default.Register<ServicesRepository>();
            SimpleIoc.Default.Register(() => InitializeApplicationCache());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeContainer();

            var windowNavigatorService = SimpleIoc.Default.GetInstance<IWindowNavigatorService>();
            windowNavigatorService.SwitchContext(SimpleIoc.Default.GetInstance<MainWindowViewModel>());
        }

    }
}
