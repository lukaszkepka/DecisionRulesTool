using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.ViewModel.Results;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class GroupedTestResultViewModel : BaseDialogViewModel
    {
        public TestRequestGroup TestRequestsAggregate { get; private set; }

        public AlgorithmsToTestSetsResultViewModel G1 { get; private set; }

        public ManyToManyResultViewModel G2 { get; private set; }


        public GroupedTestResultViewModel(TestRequestGroup aggregatedTestResult, ApplicationCache applicationCache,  ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            TestRequestsAggregate = aggregatedTestResult;
            G1 = new AlgorithmsToTestSetsResultViewModel(aggregatedTestResult, applicationCache, servicesRepository);
            G2 = new ManyToManyResultViewModel(aggregatedTestResult, applicationCache, servicesRepository);
        }

    }
}

