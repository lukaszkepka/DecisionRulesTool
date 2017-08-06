using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class DataSet
    {
        public DataSet()
        {
            this.Attributes = new HashSet<Attribute>();
            this.Objects = new HashSet<Object>();
        }

        public string Name { get; set; }

        public virtual ICollection<Attribute> Attributes { get; private set; }
        public virtual ICollection<Object> Objects { get; private set; }
    }
}
