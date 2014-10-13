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

        private const string XML_TEMPLATE = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\n" +
"<metadata>" + "\n" +
"    <dbname>%DB_NAME%</dbname>" + "\n" +
"    <dbpath>%DB_PATH%</dbpath>" + "\n" +
"    <datatables>" + "\n" +
"        %DATA_TABLES%" +
"    </datatables>" + "\n" +
"    <operationtable>" + "\n" +
"        <name>%OPERATION_TABLE_NAME%</name>" + "\n" +
"        <aggregate>" + "\n" +
"            %AGGREGATE_COLUMNS%" +
"        </aggregate>" + "\n" +
"        <information>" + "\n" +
"            %INFORMATION_COLUMNS%" +
"        </information>" + "\n" +
"    </operationtable>" + "\n" +
"</metadata>";

        public string ToXml()
        {
            return XML_TEMPLATE;
        }
    }
}
