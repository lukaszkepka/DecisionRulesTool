using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester.Result;
using DecisionRulesTool.Model.RuleTester.Result.Interfaces;
using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.UserInterface.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Services
{
    public class ServicesRepository
    {
        private IServiceLocator serviceLocator;

        public ServicesRepository(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IFileParserFactory<RuleSet> RuleSetParserFactory
        {
            get
            {
                return serviceLocator.GetInstance<IFileParserFactory<RuleSet>>();
            }
        }

        public IFileParserFactory<DataSet> TestSetParserFactory
        {
            get
            {
                return serviceLocator.GetInstance<IFileParserFactory<DataSet>>();
            }
        }

        public ITestRequestService TestRequestService
        {
            get
            {
                return serviceLocator.GetInstance<ITestRequestService>();
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return serviceLocator.GetInstance<IDialogService>();
            }
        }

        public IWindowNavigatorService WindowNavigatorService
        {
            get
            {
                return serviceLocator.GetInstance<IWindowNavigatorService>();
            }
        }

        public ITestSetLoaderService DataSetLoaderService
        {
            get
            {
                return serviceLocator.GetInstance<ITestSetLoaderService>();
            }
        }

        public IRuleSetLoaderService RuleSetLoaderService
        {
            get
            {
                return serviceLocator.GetInstance<IRuleSetLoaderService>();
            }
        }

        public IRuleSetSubsetService RuleSetSubsetService
        {
            get
            {
                return serviceLocator.GetInstance<IRuleSetSubsetService>();
            }
        }

        public ITestResultConverter<System.Data.DataTable> TestResultConverter
        {
            get
            {
                return serviceLocator.GetInstance<ITestResultConverter<System.Data.DataTable>>();
            }
        }
    }
}
