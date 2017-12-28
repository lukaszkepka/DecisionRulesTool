using DecisionRulesTool.Model.Exceptions;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.RuleTester.Result;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.UserInterface.ViewModel.Windows;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel.Dialog
{
    [AddINotifyPropertyChangedInterface]
    public class TestResultSaverViewModel : BaseTestResultIOViewModel
    {
        public override string Title => "Decision Rules Tool - Test Result Saving";
        public override string Action => "Saving";

        public TestResultSaverViewModel(ApplicationRepository applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
        }

        public string GetFolderPath()
        {
            string folderPath = servicesRepository.DialogService.BrowseFolderDialog(Environment.CurrentDirectory);
            folderPath = Path.Combine(folderPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
            return folderPath;
        }

        public string GetFilePath(string folderPath, TestObject testRequest)
        {
            return $"{folderPath}\\{testRequest.GetFileName()}";
        }

        public async void RunSaving()
        {
            try
            {
                IsInProgress = true;

                IEnumerable<TestObject> testRequestsToSave = applicationRepository.TestRequests.Where(x => x.Progress == 100);
                MaxIteration = testRequestsToSave.Count();
                string folderPath = GetFolderPath();
                int savedFilesCount = 0;

                if (!string.IsNullOrEmpty(folderPath))
                {
                    foreach (var testRequest in testRequestsToSave)
                    {
                        if (!cancelRequest)
                        {
                            TestResultViewModel testResultViewModel = new TestResultViewModel(testRequest, applicationRepository, servicesRepository);
                            await Task.Factory.StartNew(() => testResultViewModel.SaveResultToFile(GetFilePath(folderPath, testRequest)));
                            UpdateProgress(savedFilesCount++, MaxIteration);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                IsInProgress = false;
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during saving test results : {ex.Message}");
                IsInProgress = false;
            }
        }
    }
}
