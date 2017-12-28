using DecisionRulesTool.Model.Exceptions;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.RuleTester.Result;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.UserInterface.View.Dialogs;
using DecisionRulesTool.UserInterface.ViewModel.Dialog;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionRulesTool.UserInterface.ViewModel.Windows
{
    [AddINotifyPropertyChangedInterface]
    public class TestResultLoaderViewModel : BaseTestResultIOViewModel
    {
        protected IFileParser<TestObject> testResultLoader;
        protected Action<TestObject, int> testRequestInserter;

        public override string Title => "Decision Rules Tool - Test Result Loading";
        public override string Action => "Loading";

        public TestResultLoaderViewModel(Action<TestObject, int> testRequestInserter, ApplicationRepository applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.testResultLoader = new TestRequestParser();
            this.testRequestInserter = testRequestInserter;
        }

        public string[] GetFilePaths()
        {
            OpenFileDialogSettings settings = new OpenFileDialogSettings()
            {
                Multiselect = true
            };

            return servicesRepository.DialogService.OpenFileDialog(settings);
        }

        public async void RunLoading(int serieNumber)
        {           
            string[] filePaths = GetFilePaths();
            MaxIteration = filePaths.Length;

            IsInProgress = true;
            for (int i = 0; i < MaxIteration && !cancelRequest; i++)
            {
                try
                {
                    TestObject testRequest = await Task.Factory.StartNew(() => testResultLoader.ParseFile(filePaths[i]));
                    testRequestInserter.Invoke(testRequest, serieNumber);
                    UpdateProgress(i, MaxIteration);
                }
                catch (FileFormatNotSupportedException ex)
                {
                    servicesRepository.DialogService.ShowErrorMessage($"File format : {ex.FileExtension} not supported");
                }
                catch (FormatException ex)
                {
                    servicesRepository.DialogService.ShowErrorMessage($"File has invalid structure");
                }
                catch (Exception ex)
                {
                    //;
                }
            }
            IsInProgress = false;
        }
    }
}
