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
    using Model;
    using NLog;

    public class _4eMkaDataSetParser : _4eMkaFileParser<DataSet>
    {
        public override string SupportedFormat => BaseFileFormat.FileExtensions._4emkaDataset;

        public override DataSet ParseFile(StreamReader fileStream)
        {
            LogManager.GetCurrentClassLogger().Info("Testowa informacja!");

            DataSet dataSet = default(DataSet);
            using (fileStream)
            {
                dataSet = new DataSet();
                ParseAttributes(fileStream, dataSet);
                ParseObjects(fileStream, dataSet);
            }
            return dataSet;
        }


        private void SetObjectValues(string[] stringValues, Object dataObject)
        {
            var attributeEnumerator = dataObject.Attributes.GetEnumerator();
            attributeEnumerator.MoveNext();
            for (int i = 0; i < stringValues.Length; i++)
            {
                object value = fileFormat.GetAttributeValue(attributeEnumerator.Current, stringValues[i]);
                dataObject.Values[i] = value;
                attributeEnumerator.MoveNext();
            }
        }

        private void ParseObjects(StreamReader fileStream, DataSet dataSet)
        {
            MoveStreamToSection(fileStream, fileFormat.ObjectsSectionHeader);
            string fileLine = fileStream.ReadLine();
            while (!fileLine.Equals(string.Empty) && !fileLine.Equals(fileFormat.EndFileHeader))
            {
                string[] values = fileLine.Split(',');
                values = values.Select(x => x.Trim(' ', '\t')).ToArray();
                Object dataSetObject = new Object(dataSet, new object[values.Length]);
                SetObjectValues(values, dataSetObject);
                dataSet.Objects.Add(dataSetObject);
                fileLine = fileStream.ReadLine();
            }
        }

        private void ParseAttributes(StreamReader fileStream, DataSet dataSet)
        {
            foreach (var attribute in base.ParseAttributes(fileStream))
            {
                dataSet.Attributes.Add(attribute);
            }
        }
    }
}
