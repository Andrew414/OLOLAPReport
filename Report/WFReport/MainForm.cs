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
        public WFReport.Metadata.DataBase currentDB = null;
        public WFReport.DataReport.Report currentReport = null;
        public string currentPathMetadata = null;
        public string currentPathReport = null;

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
            MessageBox.Show("OLOLO");
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            
        }

        private void openMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdMetadata.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LoadMetadataFromFile(ofdMetadata.FileName);

                    UpdateUIwithMetadata();
                    MessageBox.Show("The description of the database \"" + currentDB.Name + "\"" + " was loaded successfully!",
                    "Database metadata loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while opening the file \"" + ofdMetadata.FileName + "\":\n" + ex.Message,
                        "Unable to open the file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    currentDB = null;
                    currentReport = null;
                    currentPathMetadata = null;
                    currentPathReport = null;
                }
            }
        }

        private Dictionary<string, string> ParseRowsOrColumns(XmlElement node, string mode)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (XmlElement i in node)
            {
                dict.Add(mode + i.Name, i.InnerText);
            }

            return dict;
        }

        private Dictionary<string, string> ParseVisibleData(XmlElement node)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (XmlElement i in node)
            {
                if (i.Name == "rows" || i.Name == "columns")
                {
                    foreach (var j in ParseRowsOrColumns(i, i.Name[0].ToString()))
                        dict.Add(j.Key, j.Value);
                }

                if (i.Name == "aggregate")
                    dict.Add("aggrname", ParseRowsOrColumns(i, "aggr")["aggrname"]);
            }

            return dict;
        }

        private WFReport.DataReport.OptionsRestriction ParseOptions(XmlElement node)
        {
            WFReport.DataReport.OptionsRestriction rest = new DataReport.OptionsRestriction();
            
            foreach(XmlElement i in node)
            {
                if (i.Name == "table")
                    rest.table = currentDB.Tables[i.InnerText];
                if (i.Name == "column")
                    rest.column = rest.table.Columns[i.InnerText];
                if (i.Name == "option")
                    rest.options.Add(i.InnerText);
            }

            return rest;
        }

        private WFReport.DataReport.FromToRestriction ParseFromTo(XmlElement node)
        {
            WFReport.DataReport.FromToRestriction rest = new DataReport.FromToRestriction();

            foreach (XmlElement i in node)
            {
                if (i.Name == "table")
                    rest.table = currentDB.Tables[i.InnerText];
                if (i.Name == "column")
                    rest.column = rest.table.Columns[i.InnerText];
                if (i.Name == "from")
                    rest.fromValue = i.InnerText;
                if (i.Name == "to")
                    rest.toValue = i.InnerText;
            }

            return rest;
        }

        private List<WFReport.DataReport.Restriction> ParseFixedData(XmlElement node)
        {
            List<WFReport.DataReport.Restriction> fixedData = new List<DataReport.Restriction>();
            foreach (XmlElement i in node)
            {
                if (i.Name == "options")
                    fixedData.Add(ParseOptions(i));

                if (i.Name == "fromto")
                    fixedData.Add(ParseFromTo(i));
            }

            return fixedData;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdReport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var xmlfile = new FileStream(ofdReport.FileName, FileMode.Open, FileAccess.Read))
                    {
                        XmlDocument xmlreport = new XmlDocument();
                        xmlreport.Load(xmlfile);

                        //TODO: Validate scheme against XSD:
                        //metadata.Validate(...)

                        WFReport.DataReport.Report report = new DataReport.Report();

                        XmlNode node = xmlreport.ChildNodes[0];
                        while (node.Name != "report")
                        {
                            node = node.NextSibling;
                        }

                        string cTable = null, cColumn = null;
                        string rTable = null, rColumn = null;
                        string                aColumn = null;
                        string cGroup = null, rGroup  = null;

                        string metadatapath = null;
                        string dbname = null;

                        currentReport = new DataReport.Report();

                        foreach (XmlElement i in node)
                        {
                            if (i.Name == "dbname")
                            {
                                dbname = i.InnerText;
                                metadatapath = i.Attributes["metadata"].Value;

                                if (currentPathMetadata == null)
                                {
                                    try
                                    {
                                        LoadMetadataFromFile(metadatapath);
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show("Error while loading metadata from the report. Please try to load the metadata manually.\nThe error description:\n" + ex.Message,
                                            "Error while loading metadata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            if (i.Name == "visible")
                            {
                                var dict = ParseVisibleData(i);

                                if (dict.Keys.Contains("ctable"))  cTable  = dict["ctable" ];
                                if (dict.Keys.Contains("ccolumn")) cColumn = dict["ccolumn"];
                                if (dict.Keys.Contains("cgroup"))  cGroup  = dict["cgroup"];

                                if (dict.Keys.Contains("rtable"))  rTable  = dict["rtable"];
                                if (dict.Keys.Contains("rcolumn")) rColumn = dict["rcolumn"];
                                if (dict.Keys.Contains("rgroup"))  rGroup  = dict["rgroup"];

                                if (dict.Keys.Contains("aggrname")) aColumn = dict["aggrname"];

                                currentReport.cTable = currentDB.Tables[cTable];
                                currentReport.cColumn = currentReport.cTable.Columns[cColumn];
                                currentReport.cGroup = cGroup ?? "";

                                currentReport.rTable = currentDB.Tables[rTable];
                                currentReport.rColumn = currentReport.rTable.Columns[rColumn];
                                currentReport.rGroup = rGroup ?? "";

                                currentReport.aTable = currentDB.Operations;
                                currentReport.aColumn = currentReport.aTable.Columns[aColumn];
                            }
                            if (i.Name == "fixeddata")
                            {
                                currentReport.restrictions = ParseFixedData(i);
                            }
                        }

                        currentPathReport = ofdReport.FileName;
                        UpdateUIwithReportData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while opening the file \"" + ofdReport.FileName + "\":\n" + ex.Message,
                        "Unable to read the report", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    currentReport = null; 
                    currentPathReport = null;
                }
            }
        }

        

        private void UpdateUIwithMetadata()
        {
            lbxImportantFields.Items.Clear();
            cboAggregate.Items.Clear();
            cboColumns.Items.Clear();
            cboRows.Items.Clear();

            cboAggregate.Items.Add(" ");
            cboColumns.Items.Add(" ");
            cboRows.Items.Add(" ");

            foreach (var i in currentDB.Tables.Values)
            {
                foreach (var j in i.Columns.Values)
                {
                    if (!j.Aggregate)
                    {
                        lbxImportantFields.Items.Add(i.Name + ": " + j.Name, true);
                        cboColumns.Items.Add(i.Name + ": " + j.Name);
                        cboRows.Items.Add(i.Name + ": " + j.Name);
                        
                    } else
                    {
                        cboAggregate.Items.Add(i.Name + ": " + j.Name);
                    }
                }
            }

            

            
        }

        private void UpdateUIwithReportData()
        {
            UpdateUIwithMetadata();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

    }
}
