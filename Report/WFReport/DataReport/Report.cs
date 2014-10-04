using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFReport.Metadata;

namespace WFReport.DataReport
{
    public class Report
    {
        public Table rTable = null;
        public Table cTable = null;
        public Table aTable = null;

        public Column rColumn = null;
        public Column cColumn = null;
        public Column aColumn = null;

        public String rGroup = null;
        public String cGroup = null;

        public List<Restriction> restrictions = new List<Restriction>(); 
    }
}
