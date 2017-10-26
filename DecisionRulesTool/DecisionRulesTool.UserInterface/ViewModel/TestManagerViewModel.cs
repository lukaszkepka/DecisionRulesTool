using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class TestManagerViewModel : BaseWindowViewModel
    {
        private RuleTesterManager ruleTesterManager;
        private TestRequest selectedTestRequest;
        private ICollection<TestRequest> testRequests;

        #region Properties
        public ICollection<TestRequest> TestRequests
        {
            get
            {
                return testRequests;
            }
            set
            {
                testRequests = value.ToList();
                OnPropertyChanged("TestRequests");
            }
        }
        #endregion

        public ICommand Run { get; set; }

        public TestManagerViewModel(ICollection<TestRequest> testRequests)
        {
            this.ruleTesterManager = new RuleTesterManager();
            this.testRequests = testRequests;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            Run = new RelayCommand(f);
        }

        public void f()
        {
            foreach (var testRequest in testRequests)
            {
                ruleTesterManager.AddTestRequest(testRequest);
            }

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += (sender, e) => { ruleTesterManager.RunTesting(new RuleTester(new ConditionChecker(), new ProgressNotifier())); };
            bgw.RunWorkerAsync();
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
