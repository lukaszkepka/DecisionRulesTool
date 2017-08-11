using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers._4eMka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecisionRulesTool.Model.IO.Parsers._4eMka
{
    public class _4EmkaDataSetParser : _4EmkaFileParser<DataSet>
    {
        public override string[] SupportedFormats => new[] { "isf" };

        public override DataSet ParseFile(StreamReader fileStream)
        {
            DataSet dataSet = default(DataSet);
            using (fileStream)
            {
                dataSet = new DataSet();
                ParseAttributes(fileStream, dataSet);
                ParseObjects(fileStream, dataSet);
            }
            return dataSet;
        }

        private void ParseObjects(StreamReader fileStream, DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        private void ParseAttributes(StreamReader fileStream, DataSet dataSet)
        {
            throw new NotImplementedException();
        }
    }
}
