using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFReport.Metadata;

namespace WFReport.DataReport
{
    public class OptionsRestriction : Restriction
    {
        public Table table = null;
        public Column column = null;

        public List<string> options = new List<string>();
    }
}
