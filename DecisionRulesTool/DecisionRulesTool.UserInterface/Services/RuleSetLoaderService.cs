﻿using DecisionRulesTool.Model;
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
    public class RuleSetLoaderService : IRuleSetLoaderService
    {
        private readonly string ruleSetsFilter =
            $"RSES filter (*{BaseFileFormat.FileExtensions.RSESRuleSet})|*{BaseFileFormat.FileExtensions.RSESRuleSet}|" +
            $"4EMKA filter (*{BaseFileFormat.FileExtensions._4emkaRuleSet})|*{BaseFileFormat.FileExtensions._4emkaRuleSet}|" +
            $"All files (*.*)|*.*";

        private IFileParserFactory<RuleSet> fileParserFactory;
        private IDialogService dialogService;

        public RuleSetLoaderService(IFileParserFactory<RuleSet> fileParserFactory, IDialogService dialogService)
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

        public ICollection<RuleSet> LoadRuleSets()
        {
            List<RuleSet> ruleSets = new List<RuleSet>();
            OpenFileDialogSettings options = CreateOpenFileDialogOptions();

            foreach (string filePath in dialogService.OpenFileDialog(options))
            {
                string fileExtension = Path.GetExtension(filePath);

                try
                {
                    IFileParser<RuleSet> ruleSetParser = fileParserFactory.Create(fileExtension);
                    RuleSet ruleSet = ruleSetParser.ParseFile(filePath);
                    ruleSets.Add(ruleSet);
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

            return ruleSets;
        }
    }
}




