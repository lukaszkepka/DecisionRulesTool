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


        public object[] Values { get; set; }

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
    }
}
