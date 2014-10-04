using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFReport.Metadata
{
    public enum TableType
    {
        DataTable, OperationTable
    }

    public class Table
    {
        public TableType Type { get; set; }

        public Dictionary<string, Column> Columns = new Dictionary<string,Column>();

        public String Name;
    }
}
