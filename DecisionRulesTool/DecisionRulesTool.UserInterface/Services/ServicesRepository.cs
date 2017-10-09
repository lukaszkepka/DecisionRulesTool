using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Services
{
    public class ServicesRepository
    {
        private static ServicesRepository instance;

        public DialogService DialogService { get; }
        public WindowNavigatorService WindowNavigatorService { get; }
        public RuleSetLoaderService RuleSetLoaderService { get; }

        public static ServicesRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new ServicesRepository();
            }

            return instance;
        }

        private ServicesRepository()
        {
            this.WindowNavigatorService = new WindowNavigatorService();
            this.DialogService = new DialogService();
            this.RuleSetLoaderService = new RuleSetLoaderService(new RuleSetParserFactory(), this.DialogService);
        }
    }
}
