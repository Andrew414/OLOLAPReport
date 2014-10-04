using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFReport.Metadata
{
    public class DataBase
    {
        public Dictionary<string,Table> Tables = new Dictionary<string,Table>();
        public Table Operations;
        public String Name;
        public String Path;
    }
}
