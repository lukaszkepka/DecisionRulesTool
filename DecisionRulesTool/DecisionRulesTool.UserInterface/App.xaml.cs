
#define TEST

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
    using DecisionRulesTool.Model.IO.FileSavers.Factory;
    using DecisionRulesTool.Model.Model;
    using DecisionRulesTool.Model.Parsers;
    using DecisionRulesTool.Model.RuleTester.Result;
    using DecisionRulesTool.Model.RuleTester.Result.Interfaces;
    using DecisionRulesTool.UserInterface.Model;
    using DecisionRulesTool.UserInterface.View;
    using DecisionRulesTool.UserInterface.ViewModel.Dialog;
    using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
    using DecisionRulesTool.UserInterface.ViewModel.Results;
    using System;
    using System.Linq;
    using Unity;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected ApplicationCache InitializeApplicationCache()
        {
            //Rule sets
            var ruleSets = new ThreadSafeObservableCollection<RuleSetSubset>(Enumerable.Empty<RuleSetSubset>());
#if TEST
            IFileParserFactory<RuleSet> fileParserFactory = new RuleSetParserFactory();
            IFileParser<RuleSet> ruleSetParser = fileParserFactory.Create(BaseFileFormat.FileExtensions.RSESRuleSet);
            RuleSetSubset ruleSet1 = new RuleSetViewModel(ruleSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Rules/male.rul"));
            RuleSetSubset ruleSet2 = new RuleSetViewModel(ruleSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Rules/female.rul"));

            ruleSetParser = fileParserFactory.Create(BaseFileFormat.FileExtensions._4emkaRuleSet);
            RuleSetSubset ruleSet3 = new RuleSetViewModel(ruleSetParser.ParseFile($"{Globals._4eMkaFilesDirectory}/Rules/female_4K.rls"));

            ruleSets.Add(ruleSet1);
            ruleSets.Add(ruleSet2);
            ruleSets.Add(ruleSet3);
#endif

            //Test sets
            var testSets = new ThreadSafeObservableCollection<DataSet>(Enumerable.Empty<DataSet>());
#if TEST
            IFileParserFactory<DataSet> fileParserFactory1 = new DataSetParserFactory();
            IFileParser<DataSet> dataSetParser = fileParserFactory1.Create(BaseFileFormat.FileExtensions.RSESDataset);
            DataSet dataSet1 = dataSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Sets/mts.tab");
            DataSet dataSet2 = dataSetParser.ParseFile($"{Globals.RsesFilesDirectory}/Sets/fts.tab");

            dataSetParser = fileParserFactory1.Create(BaseFileFormat.FileExtensions._4emkaDataset);
            DataSet dataSet3 = dataSetParser.ParseFile($"{Globals._4eMkaFilesDirectory}/Sets/FIn.isf");
            DataSet dataSet4 = dataSetParser.ParseFile($"{Globals._4eMkaFilesDirectory}/Sets/FTst.isf");

            testSets.Add(dataSet1);
            testSets.Add(dataSet2);
            testSets.Add(dataSet3);
            testSets.Add(dataSet4);
#endif

            //Test requests
            var testRequests = new ThreadSafeObservableCollection<TestRequest>(Enumerable.Empty<TestRequest>());
#if TEST
            foreach (ConflictResolvingMethod conflictResolvingMethod in Enum.GetValues(typeof(ConflictResolvingMethod)))
            {
                testRequests.Add(new TestRequest(ruleSet1, testSets[0], conflictResolvingMethod));
                testRequests.Add(new TestRequest(ruleSet2, testSets[1], conflictResolvingMethod));
                testRequests.Add(new TestRequest(ruleSet3, testSets[2], conflictResolvingMethod));
                testRequests.Add(new TestRequest(ruleSet3, testSets[3], conflictResolvingMethod));
            }
#endif



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
            SimpleIoc.Default.Register<IFileSaverFactory<RuleSet>, RuleSetFileSaverFactory>();
            SimpleIoc.Default.Register<IFileParserFactory<RuleSet>, RuleSetParserFactory>();
            SimpleIoc.Default.Register<IFileParserFactory<DataSet>, DataSetParserFactory>();
            SimpleIoc.Default.Register<ITestRequestService, TestRequestService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IWindowNavigatorService, WindowNavigatorService>();
            SimpleIoc.Default.Register<ITestSetLoaderService, TestSetLoaderService>();
            SimpleIoc.Default.Register<IRuleSetLoaderService, RuleSetLoaderService>();
            SimpleIoc.Default.Register<IRuleSetSubsetService, RuleSetSubsetService>();
            SimpleIoc.Default.Register<IRuleSetSaverService, RuleSetSaverService>();
            
            SimpleIoc.Default.Register<TestResultComparisionViewModel>();
            SimpleIoc.Default.Register<TestRequestGeneratorViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<RuleSetManagerViewModel>();
            SimpleIoc.Default.Register<TestManagerViewModel>();

            SimpleIoc.Default.Register<ServicesRepository>();
            SimpleIoc.Default.Register(() => InitializeApplicationCache());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeContainer();

            MainWindow mainWindow = new MainWindow();

            mainWindow.DataContext = SimpleIoc.Default.GetInstance<MainWindowViewModel>();
            mainWindow.Show();
            SimpleIoc.Default.GetInstance<MainWindowViewModel>().CloseRequest += (sender, ee) =>
            {
                mainWindow.Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };

        }

    }
}
