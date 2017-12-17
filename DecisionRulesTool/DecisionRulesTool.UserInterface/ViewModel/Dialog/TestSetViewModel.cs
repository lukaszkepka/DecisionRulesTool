using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;
    using DecisionRulesTool.UserInterface.Model;
    using DecisionRulesTool.UserInterface.Services;

    public class TestSetViewModel : BaseDialogViewModel
    {
        private DataSet testSet;

        #region Properies
        public DataSet TestSet
        {
            get
            {
                return testSet;
            }
            set
            {
                testSet = value;
                RaisePropertyChanged("TestSet");
            }
        }
        public List<Attribute> Attributes { get; private set; }
        #endregion


        public TestSetViewModel(DataSet testSet, ApplicationCache applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            this.testSet = testSet;
            Attributes = testSet.Attributes.ToList();
        }
    }
}
