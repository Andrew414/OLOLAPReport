using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFReport.Metadata
{
    public enum ColumnType
    {
        StringColumn, NumberColumn, DateColumn 
    }

    public class Column
    {
        public ColumnType Type;
        public String Name;

        public Boolean Aggregate { get; set; }

        public static ColumnType GetColumnTypeForString(string type)
        {
            if (type.ToLower() == "string")
                return ColumnType.StringColumn;
            if (type.ToLower() == "int")
                return ColumnType.NumberColumn;
            if (type.ToLower() == "float")
                return ColumnType.NumberColumn;
            if (type.ToLower() == "double")
                return ColumnType.NumberColumn;
            if (type.ToLower() == "date")
                return ColumnType.DateColumn;

            return ColumnType.StringColumn;
        }
    }
}
