using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WFReport
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            MainResize(null, null);
        }

        private void MainResize(object sender, EventArgs e)
        {
            pnlLeft.Height = this.Height;
            this.Text = this.Width.ToString() + "x" + this.Height.ToString();
        }

        private void splitter1_Move(object sender, EventArgs e)
        {
            
        }

        private void splitter1_SplitterMoving(object sender, SplitterEventArgs e)
        {
            MessageBox.Show("!");
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            
        }

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

        private void openMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdMetadata.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var xmlfile = new FileStream(ofdMetadata.FileName, FileMode.Open, FileAccess.Read))
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

                        MessageBox.Show("The description of the database \"" + db.Name + "\"" + " was loaded successfully!", 
                            "Database metadata loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while opening the file \"" + ofdMetadata.FileName + "\":\n" + ex.Message,
                        "Unable to open the file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
