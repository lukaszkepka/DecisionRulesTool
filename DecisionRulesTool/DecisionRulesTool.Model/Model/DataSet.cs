using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class DataSet
    {
        public bool IsSelected { get; set; }

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
            Attributes = new List<Attribute>();
            Objects = new List<Object>();
        }

        public string Name { get; set; }
        public ICollection<Attribute> Attributes { get; private set; }
        public ICollection<Object> Objects { get; private set; }

        public bool IsCompatibleWith(DataSet testSet)
        {
            return Attributes.SequenceEqual(testSet.Attributes);
        }

        public object GetShortenName()
        {
            return Name;
        }
    }
}
