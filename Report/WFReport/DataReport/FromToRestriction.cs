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

        private const string XML_TEMPLATE = "        <fromto>" + "\n" +
                                            "            <table>%TABLE%</table>" + "\n" +
                                            "            <column>%COLUMN%</column>" + "\n" +
                                            "            <from>%FROM%</from>" + "\n" +
                                            "            <to>%TO%</to>" + "\n" +
                                            "        </fromto>";

        public string ToXML()
        {
            string xml = XML_TEMPLATE;

            xml = xml.Replace("%TABLE%", this.table.Name);
            xml = xml.Replace("%COLUMN%", this.column.Name);
            xml = xml.Replace("%FROM%", this.fromValue);
            xml = xml.Replace("%TO%", this.toValue);

            return xml;
        }
    }
}
