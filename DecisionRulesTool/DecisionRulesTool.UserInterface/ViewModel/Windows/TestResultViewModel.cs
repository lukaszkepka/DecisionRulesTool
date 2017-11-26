using ClosedXML.Excel;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
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
        private TestRequest testRequest;
        #endregion

        #region Properties
        public DataTable TestResultDataTable { get; private set; }
        public DataTable ConfusionMatrix { get; private set; }
        #endregion

        #region Commands
        public ICommand SaveToFile { get; private set; }
        #endregion

        #region Constructor
        public TestResultViewModel(TestRequest testRequest, ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.testRequest = testRequest;
            InitializeCommands();
            FillTestResultDataTable();
            FillConfusionMatrixDataTable();
        }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            SaveToFile = new RelayCommand(OnSaveToFile);
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
                ConfusionMatrix = servicesRepository.TestResultConverter.ConvertConfusionMatrix(testRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                if (filePath != null)
                {
                    //TODO : Remove to test result saver class
                    XLWorkbook wb = new XLWorkbook();
                    DataTable dt = TestResultDataTable;
                    wb.Worksheets.Add(dt, "Labels");

                    DataTable cm = ConfusionMatrix;
                    wb.Worksheets.Add(cm, "Confusion Matrix");

                    wb.SaveAs(filePath);
                }

            }
        }
        #endregion
    }
}
