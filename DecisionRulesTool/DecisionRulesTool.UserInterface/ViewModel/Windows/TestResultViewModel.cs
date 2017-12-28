using ClosedXML.Excel;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.RuleTester.Result;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Model.Converters;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel.Windows
{
    public class TestResultViewModel : BaseWindowViewModel
    {
        #region Fields
        private TestRequestToExcelSaver fileSaver;
        private TestObject testRequest;
        #endregion

        #region Properties
        public decimal Coverage { get; private set; }
        public decimal Accuracy { get; private set; }
        public decimal TotalAccuracy { get; private set; }
        public DataTable TestResultDataTable { get; private set; }
        public DataTable ConfusionMatrix { get; private set; }
        #endregion

        #region Commands
        public ICommand SaveToFile { get; private set; }
        #endregion

        #region Constructor
        public TestResultViewModel(TestObject testRequest, ApplicationRepository applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.fileSaver = new TestRequestToExcelSaver();
            this.testRequest = testRequest;
            InitializeCommands();
            InitializeSummary();
            FillTestResultDataTable();
            FillConfusionMatrixDataTable();
        }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            SaveToFile = new RelayCommand(OnSaveToFile);
        }

        private void InitializeSummary()
        {
            this.Coverage = testRequest.TestResult.Coverage;
            this.Accuracy = testRequest.TestResult.Accuracy;
            this.TotalAccuracy = testRequest.TestResult.TotalAccuracy;
        }

        private void FillTestResultDataTable()
        {
            try
            {
                TestResultDataTable = servicesRepository.TestResultConverter.ConvertClassificationTable(testRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillConfusionMatrixDataTable()
        {
            try
            {
                ConfusionMatrix = servicesRepository.TestResultConverter.ConvertConfusionMatrix(testRequest?.TestResult?.ConfusionMatrix);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveResultToFile(string filePath)
        {
            fileSaver.SaveResultToFile(filePath, ConfusionMatrix, TestResultDataTable, testRequest);
        }

        private void OnSaveToFile()
        {
            if (TestResultDataTable.Rows.Count > 0)
            {
                SaveFileDialogSettings settings = new SaveFileDialogSettings()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    ExtensionFilter = $"Excel(*.xlsx)|*.xlsx"
                };

                string filePath = servicesRepository.DialogService.SaveFileDialog(settings);
                SaveResultToFile(filePath);
            }
        }
        #endregion
    }
}
