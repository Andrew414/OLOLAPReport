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

        public static String SQLTEMPLATEREPORT = 
@"SELECT 
	<COLUMNS>,
	<ROWS>,
	sum(<AGGREGATE>)
FROM
	Operation
	INNER JOIN Buyer ON Buyer.Id = Operation.BuyerId
	INNER JOIN Store ON Store.Id = Operation.StoreId
	INNER JOIN Item  ON Item.Id  = Operation.ItemId

<WHERESTATEMENT>

GROUP BY
	<COLUMNS>,
	<ROWS>;";

        public List<Restriction> restrictions = new List<Restriction>();

        private const String XML_TEMPLATE = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\n" +
                                            "<report>" + "\n" +
                                            "    <dbname metadata=\"%META_FILENAME%\">%DB_NAME%</dbname>" + "\n" +
                                            "    <visible>" + "\n" +
                                            "        <rows>" + "\n" +
                                            "            <table>%R_TABLE%</table>" + "\n" +
                                            "            <column>%R_COLUMN%</column>%R_GROUP%" + "\n" +
                                            "        </rows>" + "\n" +
                                            "        <columns>" + "\n" +
                                            "            <table>%C_TABLE%</table>" + "\n" +
                                            "            <column>%C_COLUMN%</column>%C_GROUP%" + "\n" +
                                            "        </columns>" + "\n" +
                                            "        <aggregate>" + "\n" +
                                            "            <name>%AGGREGATE_NAME%</name>" + "\n" +
                                            "        </aggregate>" + "\n" +
                                            "    </visible>" + "\n" +
                                            "    <fixeddata>" + "\n" +
                                            "%RESTRICTIONS%" +
                                            "    </fixeddata>" + "\n" +
                                            "</report>";

        public string ToXML(WFReport.Metadata.DataBase db, string metadatapath)
        {
            string xml = XML_TEMPLATE;

            xml = xml.Replace("%META_FILENAME%", metadatapath);
            xml = xml.Replace("%DB_NAME%", db.Name);

            xml = xml.Replace("%R_TABLE%", this.rTable.Name);
            xml = xml.Replace("%R_COLUMN%", this.rColumn.Name);
            xml = xml.Replace("%R_GROUP%", string.IsNullOrWhiteSpace(this.rGroup) ? "" : 
                ("\n            " + "<group>" + this.rGroup + "</group>"));

            xml = xml.Replace("%C_TABLE%", this.cTable.Name);
            xml = xml.Replace("%C_COLUMN%", this.cColumn.Name);
            xml = xml.Replace("%C_GROUP%", string.IsNullOrWhiteSpace(this.cGroup) ? "" :
                ("\n            " + "<group>" + this.cGroup + "</group>"));

            xml = xml.Replace("%AGGREGATE_NAME%", this.aColumn.Name);

            string rests = "";

            foreach (var i in this.restrictions)
                rests += i.ToXML() + "\n";

            xml = xml.Replace("%RESTRICTIONS%", rests);

            return xml;
        }
    }
}
