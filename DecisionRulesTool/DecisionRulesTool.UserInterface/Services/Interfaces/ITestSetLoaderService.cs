using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System;
using System.Collections.Generic;


namespace DecisionRulesTool.UserInterface.Services.Interfaces
{
    public interface ITestSetLoaderService
    {
        ICollection<DataSet> LoadDataSets();
    }
}
