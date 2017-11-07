using DecisionRulesTool.Model;
using DecisionRulesTool.Model.Exceptions;
using DecisionRulesTool.Model.IO;
using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.UserInterface.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionRulesTool.UserInterface.Services
{
    public class TestSetLoaderService : ITestSetLoaderService
    {

        private static readonly string ruleSetsFilter =
            $"RSES filter (*{BaseFileFormat.FileExtensions.RSESDataset})|*{BaseFileFormat.FileExtensions.RSESDataset}|" +
            $"4EMKA filter (*{BaseFileFormat.FileExtensions._4emkaDataset})|*{BaseFileFormat.FileExtensions._4emkaDataset}|" +
            $"All files (*.*)|*.*";

        private IFileParserFactory<DataSet> fileParserFactory;
        private DialogService dialogService;

        public TestSetLoaderService(IFileParserFactory<DataSet> fileParserFactory, DialogService dialogService)
        {
            this.dialogService = dialogService;
            this.fileParserFactory = fileParserFactory;
        }

        private OpenFileDialogSettings CreateOpenFileDialogOptions()
        {
            return new OpenFileDialogSettings
            {
                Multiselect = true,
                Filter = ruleSetsFilter,
                InitialDirectory = Path.GetFullPath(Globals.RsesFilesDirectory)
            };
        }

        public ICollection<DataSet> LoadDataSets()
        {
            List<DataSet> testSets = new List<DataSet>();
            OpenFileDialogSettings options = CreateOpenFileDialogOptions();

            foreach (string filePath in dialogService.OpenFileDialog(options))
            {
                string fileExtension = Path.GetExtension(filePath);

                try
                {
                    IFileParser<DataSet> testSetParser = fileParserFactory.Create(fileExtension);
                    DataSet testSet = testSetParser.ParseFile(filePath);
                    testSets.Add(testSet);
                }
                catch (FileFormatNotSupportedException fileFormatException)
                {
                    Debug.WriteLine($"Exception thrown : {fileFormatException.Message}");
                    dialogService.ShowInformationMessage($"File extension not supported : {fileFormatException.FileExtension}");
                }
                catch (InvalidFileBodyException invalidFileBodyException)
                {
                    Debug.WriteLine($"Exception thrown : {invalidFileBodyException.Message}");
                    dialogService.ShowInformationMessage($"File body has invalid format: {invalidFileBodyException.FilePath}");
                }
            }

            return testSets;
        }
    }
}

