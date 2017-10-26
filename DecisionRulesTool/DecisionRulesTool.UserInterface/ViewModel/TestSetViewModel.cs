using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;

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
                OnPropertyChanged("TestSet");
            }
        }
        #endregion

        public List<Attribute> Attributes { get; private set; }

        public TestSetViewModel(DataSet testSet)
        {
            this.testSet = testSet;
            Attributes = testSet.Attributes.ToList();
        }
    }
}
