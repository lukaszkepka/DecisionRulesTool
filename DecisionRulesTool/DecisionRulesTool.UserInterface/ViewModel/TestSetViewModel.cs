using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;
    using DecisionRulesTool.UserInterface.Services;
    using Unity;

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
        #endregion

        public List<Attribute> Attributes { get; private set; }

        public TestSetViewModel(DataSet testSet, ServicesRepository servicesRepository)
            : base(servicesRepository)
        {
            this.testSet = testSet;
            Attributes = testSet.Attributes.ToList();
        }
    }
}
