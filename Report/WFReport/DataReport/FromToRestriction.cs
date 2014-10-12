using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFReport.Metadata;

namespace WFReport.DataReport
{
    public class FromToRestriction : Restriction
    {
        public Table table = null;
        public Column column = null;

        public string fromValue = null;
        public string toValue = null;

        public override string ToString()
        {
            return table.Name + ":" + column.Name + " (" + fromValue + "..." + toValue + ")";
        }
    }
}
