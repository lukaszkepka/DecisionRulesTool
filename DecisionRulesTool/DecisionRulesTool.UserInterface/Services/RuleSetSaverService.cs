using DecisionRulesTool.Model.FileSavers;
using DecisionRulesTool.Model.IO;
using DecisionRulesTool.Model.IO.FileSavers.Factory;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Services
{
    public class RuleSetSaverService : IRuleSetSaverService
    {
        private IFileSaverFactory<RuleSet> fileSaverFactory;
        private IDialogService dialogService;

        public RuleSetSaverService(IFileSaverFactory<RuleSet> fileSaverFactory, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.fileSaverFactory = fileSaverFactory;
        }

        private string GetExtensionFilter(string fileExtension)
        {
            switch (fileExtension)
            {
                case BaseFileFormat.FileExtensions.RSESRuleSet:
                    return $"RSES filter (*{BaseFileFormat.FileExtensions.RSESRuleSet})|*{BaseFileFormat.FileExtensions.RSESRuleSet}";
                case BaseFileFormat.FileExtensions._4emkaRuleSet:
                    return  $"4EMKA filter (*{BaseFileFormat.FileExtensions._4emkaRuleSet})|*{BaseFileFormat.FileExtensions._4emkaRuleSet}";
                default:
                    throw new NotSupportedException($"Extension {fileExtension} not supported)");
            }
        }

        private SaveFileDialogSettings CreateSaveFileDialogOptions(string fileExtension)
        {
            return new SaveFileDialogSettings()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                ExtensionFilter = GetExtensionFilter(fileExtension)
            };
        }

        public void SaveRuleSet(RuleSet ruleSet)
        {
            SaveFileDialogSettings options = CreateSaveFileDialogOptions(ruleSet.FileExtension);
            string filePath = dialogService.SaveFileDialog(options);

            IFileSaver<RuleSet> fileSaver = fileSaverFactory.Create(ruleSet.FileExtension);
            fileSaver.Save(ruleSet, filePath);
        }
    }
}
