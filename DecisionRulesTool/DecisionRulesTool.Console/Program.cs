using DecisionRulesTool.Model;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.Parsers.RSES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DecisionRulesTool.Console
{
    using DecisionRulesTool.Model.Model;
    class Program
    {
        static void Main(string[] args)
        {
            using (var t = new DatabaseContext())
            {
                Attribute a = new Attribute()
                {
                    Name = "asd"
                };

                Object o = new Object()
                {
                    //Values = new[] { "a", "d", "l" },
                    DataSet = new Model.Model.DataSet { Name = "sadasd" }
                };

                t.Objects.Add(o);
                t.SaveChanges();
            }
        }
    }
}
