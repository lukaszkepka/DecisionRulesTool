using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecisionRulesTool.Model.Parsers.RSES
{
    using IO;
    using Model;
    using System.Globalization;

    public class RsesDataSetParser : RsesFileParser<DataSet>
    {
        public override string[] SupportedFormats => new[] { "tab" };

        private void ParseHeader(StreamReader fileStream, DataSet dataSet)
        {
            dataSet.Name = GetSectionValue(fileStream, fileFormat.DatasetFileHeader);
        }

        private void ParseAttributes(StreamReader fileStream, DataSet dataSet)
        {
            foreach (var attribute in base.ParseAttributes(fileStream))
            {
                dataSet.Attributes.Add(attribute);
            }
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
            int objectsCount = Convert.ToInt32(GetSectionValue(fileStream, fileFormat.ObjectsSectionHeader));
            int objectIndex = 0;
            while (objectIndex < objectsCount)
            {
                string fileLine = fileStream.ReadLine();
                if (!string.IsNullOrEmpty(fileLine))
                {
                    string[] values = fileLine.Split(',');
                    Object dataSetObject = new Object(dataSet, new object[values.Length]);
                    SetObjectValues(values, dataSetObject);
                    dataSet.Objects.Add(dataSetObject);
                    objectIndex++;
                }
            }
        }

        public override DataSet ParseFile(StreamReader fileStream)
        {
            DataSet dataSet = default(DataSet);
            using (fileStream)
            {
                dataSet = new DataSet();
                ParseHeader(fileStream, dataSet);
                ParseAttributes(fileStream, dataSet);
                ParseObjects(fileStream, dataSet);
            }
            return dataSet;
        }
    }
}
