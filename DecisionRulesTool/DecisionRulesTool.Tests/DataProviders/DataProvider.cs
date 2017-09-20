using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests.DataProviders
{
    using Model.Model;

    public static class DataProvider
    {
        public static DataSet GetDataset(AttributeType[] attributeTypes, string[] names, string[][] avaiableValues)
        {
            DataSet dataSet = new DataSet("D1");

            for (int i = 0; i < attributeTypes.Length; i++)
            {
                Attribute attribute = new Attribute(attributeTypes[i], names[i], avaiableValues == null ? null : avaiableValues[i]);
                dataSet.Attributes.Add(attribute);
            }

            return dataSet;
        }

        public static Object GetObject(DataSet dataSet, params object[] objectValues)
        {
            return new Object(dataSet, objectValues);
        }

        public static Condition GetCondition(object attributeValue, Relation relation, Attribute attribute)
        {
            return new Condition(relation, attribute, attributeValue);
        }
    }
}
