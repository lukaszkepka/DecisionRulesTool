using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecisionRulesTool.Model.Parsers.RSES
{
    using Model;
    using IO;

    public class RsesDataSetParser : RsesFileParser<DataSet>
    {
        public override string[] SupportedFormats => new[] { "tab" };

        private void ParseHeader(StreamReader fileStream, DataSet dataSet)
        {
            dataSet.Name = GetSectionValue(fileStream, RsesFileFormat.DATASET_FILE_HEADER);
        }

        private void ParseAttributes(StreamReader fileStream, DataSet dataSet)
        {
            //dataSet.Attributes = base.ParseAttributes(fileStream);
        }

        private void ParseObjects(StreamReader fileStream, DataSet dataSet)
        {
            ICollection<Object> objects = new List<Object>();
            int objectsCount = Convert.ToInt32(GetSectionValue(fileStream, RsesFileFormat.OBJECTS_SECTION_HEADER));
            int objectIndex = 0;
            while (objectIndex < objectsCount)
            {
                string fileLine = fileStream.ReadLine();
                if (!string.IsNullOrEmpty(fileLine))
                {
                    string[] values = fileLine.Split(',');
                    Object dataSetObject = new Object();
                    //dataSetObject.Attributes = dataSet.Attributes;
                    //dataSetObject.Values = values;
                    objects.Add(dataSetObject);
                    objectIndex++;
                }
            }
            //dataSet.Objects = objects;
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
