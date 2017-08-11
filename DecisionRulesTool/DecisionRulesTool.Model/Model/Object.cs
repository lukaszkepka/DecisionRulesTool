using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Object
    {
        public Object(DataSet dataSet, params object[] values)
        {
            DataSet = dataSet;
            Values = values;
        }

        public object[] Values { get; }
        public DataSet DataSet { get; set; }
        public ICollection<Attribute> Attributes
        {
            get
            {
                return DataSet.Attributes;
            }
        }
        public object this[Attribute attribute]
        {
            get
            {
                int index = Attributes.TakeWhile(x => !x.Equals(attribute)).Count();
                return Values[index];
            }
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            Object dataObject = obj as Object;
            if(dataObject != null)
            {
                result = Values.SequenceEqual(dataObject.Values) &&
                         Attributes.SequenceEqual(dataObject.Attributes);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
