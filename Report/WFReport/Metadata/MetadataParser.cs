using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WFReport
{
    partial class MainForm
    {


        private string FindDbName(XmlNode node)
        {
            foreach (XmlElement i in node.ChildNodes)
                if (i.Name == "dbname")
                    return i.InnerText;
            return "";
        }

        private string FindDbPath(XmlNode node)
        {
            foreach (XmlElement i in node.ChildNodes)
                if (i.Name == "dbpath")
                    return i.InnerText;
            return "";
        }

        private WFReport.Metadata.Column ParseColumn(XmlElement node)
        {
            WFReport.Metadata.Column col = new Metadata.Column();

            col.Type = WFReport.Metadata.Column.GetColumnTypeForString(node.Attributes["type"].Value);
            col.Name = node.InnerText;
            col.Aggregate = false;

            return col;
        }

        private WFReport.Metadata.Table ParseTable(XmlElement node)
        {
            WFReport.Metadata.Table table = new Metadata.Table();

            table.Type = Metadata.TableType.DataTable;

            foreach (XmlElement i in node.ChildNodes)
            {
                if (i.Name == "name")
                    table.Name = i.InnerText;

                if (i.Name == "column")
                    table.Columns.Add(i.InnerText, ParseColumn(i));
            }

            return table;
        }

        private IEnumerable<WFReport.Metadata.Table> ParseDataTables(XmlElement node)
        {
            foreach (XmlElement i in node.ChildNodes)
            {
                if (i.Name == "table")
                    yield return ParseTable(i);
            }
        }

        private IEnumerable<WFReport.Metadata.Table> FindDataTables(XmlNode node)
        {
            foreach (XmlElement i in node.ChildNodes)
            {
                if (i.Name == "datatables")
                {
                    var tables = ParseDataTables(i);
                    return tables.ToArray();
                }
            }
            return null;
        }

        private IEnumerable<WFReport.Metadata.Column> ParseAggregatesAndInformation(XmlElement node)
        {
            foreach (XmlElement i in node.ChildNodes)
            {
                if (i.Name == "column")
                    yield return ParseColumn(i);
            }
        }

        private WFReport.Metadata.Table ParseFactTable(XmlElement node)
        {
            WFReport.Metadata.Table table = new Metadata.Table();

            foreach (XmlElement i in node.ChildNodes)
            {
                if (i.Name == "name")
                    table.Name = i.InnerText;

                if (i.Name == "aggregate")
                {
                    var aggregates = ParseAggregatesAndInformation(i).ToArray();
                    foreach (var j in aggregates)
                    {
                        j.Aggregate = true;
                        table.Columns.Add(j.Name, j);
                    }
                }

                if (i.Name == "information")
                {
                    var infos = ParseAggregatesAndInformation(i).ToArray();
                    foreach (var j in infos)
                    {
                        table.Columns.Add(j.Name, j);
                    }
                }
            }

            return table;
        }

        private WFReport.Metadata.Table FindFactTable(XmlNode node)
        {
            foreach (XmlElement i in node.ChildNodes)
            {
                if (i.Name == "operationtable")
                {
                    return ParseFactTable(i);
                }
            }
            return null;
        }

        private void LoadMetadataFromFile(string filename)
        {
            using (var xmlfile = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                XmlDocument metadata = new XmlDocument();
                metadata.Load(xmlfile);

                //TODO: Validate scheme against XSD:
                //metadata.Validate(...)

                WFReport.Metadata.DataBase db = new Metadata.DataBase();

                XmlNode node = metadata.ChildNodes[0];
                while (node.Name != "metadata")
                {
                    node = node.NextSibling;
                }

                db.Name = FindDbName(node);
                db.Path = FindDbPath(node);

                var tables = FindDataTables(node);
                foreach (var i in tables)
                    db.Tables.Add(i.Name, i);

                var table = FindFactTable(node);

                db.Operations = table;
                db.Tables.Add(table.Name, table);

                currentDB = db;
                currentPathMetadata = filename;
            }
        }
    }
}
