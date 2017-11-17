using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public abstract class TestRequestGeneratorViewModel : BaseDialogViewModel
    {
        #region Properties
        public ICollection<SelectableItem<ConflictResolvingMethod>> ConflictResolvingMethods { get; private set; }
        public TestRequestGeneratorOptionsViewModel SettingsViewModel { get; }
        #endregion

        #region Commands
        public ICommand ShowSettings { get; private set; }
        #endregion

        public TestRequestGeneratorViewModel(ServicesRepository servicesRepository) 
            : base(servicesRepository)
        {
            this.SettingsViewModel = new TestRequestGeneratorOptionsViewModel(servicesRepository);
            InitializeCommands();
            InitializeConflictResolvingMethods();         
        }

        private void InitializeCommands()
        {
            ShowSettings = new RelayCommand(OnShowSettings);
        }

        private void InitializeConflictResolvingMethods()
        {
            var conflictResolvingMethodsArray = (ConflictResolvingMethod[])Enum.GetValues(typeof(ConflictResolvingMethod));
            this.ConflictResolvingMethods = new ObservableCollection<SelectableItem<ConflictResolvingMethod>>(conflictResolvingMethodsArray.Select(x => new SelectableItem<ConflictResolvingMethod>(x)));
        }

        private void OnShowSettings()
        {
            try
            {
                servicesRepository.DialogService.ShowDialog(SettingsViewModel);
            }
            catch(Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        protected List<ConflictResolvingMethod> GetSelectedConflictResolvingStrategies()
        {
            return ConflictResolvingMethods.Where(x => x.IsSelected).Select(x => x.Item).ToList();
        }

        public abstract IEnumerable<TestRequest> GenerateTestRequests();

    }
}