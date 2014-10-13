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

        public override string ToString()
        {
            return table.Name + ":" + column.Name + " (" + options.Count.ToString() + " option" + (options.Count != 1 ? "s" : "") + ")";
        }

        private const string XML_TEMPLATE = "        <options>" + "\n" +
                                            "            <table>%TABLE%</table>" + "\n" +
                                            "            <column>%COLUMN%</column>" + "\n" +
                                            "            %OPTIONS%" + "\n" +
                                            "        </options>";

        public string ToXML()
        {
            string xml = XML_TEMPLATE;

            xml = xml.Replace("%TABLE%", table.Name);
            xml = xml.Replace("%COLUMN%", column.Name);

            string opts = "";
            foreach (var i in options)
                opts += "\n" + "            " + "<option>" + i + "</option>";

            xml = xml.Replace("%OPTIONS%", opts);

            return xml;
        }

        public Table GetTable()
        {
            return table;
        }

        public Column GetColumn()
        {
            return column;
        }
    }
}
