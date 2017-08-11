using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class DataSet
    {
        public DataSet(string name, ICollection<Attribute> attributes, ICollection<Object> objects) : this(name)
        {
            Attributes = attributes;
            Objects = objects;
        }

        public DataSet(string name) : this()
        {
            Name = name;
        }

        public DataSet()
        {
            Attributes = new HashSet<Attribute>();
            Objects = new HashSet<Object>();
        }

        public string Name { get; set; }
        public virtual ICollection<Attribute> Attributes { get; private set; }
        public virtual ICollection<Object> Objects { get; private set; }
    }
}
