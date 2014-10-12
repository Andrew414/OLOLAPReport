using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
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
            //this.Text = this.Width.ToString() + "x" + this.Height.ToString();
            grdReport.Height = this.Height - 92;
            grdReport.Width = this.Width - 294;

            tbxSqlQueryDevMode.Height = grdReport.Height;
            tbxSqlQueryDevMode.Width = grdReport.Width;

            btnFiltersAggregate.Visible = btnFiltersColumn.Visible = btnFiltersRows.Visible = false;
            cboRows.Width = cboAggregate.Width = cboColumns.Width;
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
            //MessageBox.Show("OLOLO");
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
                        pnlLeft.Enabled = true;
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

        private void RefreshData()
        {

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

            cboAggregate.SelectedIndex = cboAggregate.Items.IndexOf(currentReport.aTable.Name + ": " + currentReport.aColumn.Name);
            cboColumns  .SelectedIndex = cboColumns  .Items.IndexOf(currentReport.cTable.Name + ": " + currentReport.cColumn.Name);
            cboRows     .SelectedIndex = cboRows     .Items.IndexOf(currentReport.rTable.Name + ": " + currentReport.rColumn.Name);

            lbxAdditionalFilters.Items.Clear();
            foreach (var i in currentReport.restrictions)
            {
                lbxAdditionalFilters.Items.Add(i.ToString());
            }

            RefreshData();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private string ConvertGroupBy(WFReport.Metadata.Table table, WFReport.Metadata.Column column, string group)
        {
            if (!String.IsNullOrWhiteSpace(group) && column.Type == Metadata.ColumnType.DateColumn)
            {
                switch (group)
                {
                    case "Month":
                        return "strftime(\"%Y-%m\", " +
                            table.Name + "." + column.Name + ")";
                    case "Year":
                        return "strftime(\"%Y\", " +
                            table.Name + "." + column.Name + ")";
                    case "Day":
                        return "strftime(\"%Y-%m-%d\", " +
                           table.Name + "." + column.Name + ")";
                    default:
                        return table.Name + "." + column.Name;
                }
            }

            return table.Name + "." + column.Name; 
        }

        string ConvertRestrictionToWhereString(WFReport.DataReport.Restriction rest)
        {
            if (rest is WFReport.DataReport.FromToRestriction)
            {
                WFReport.DataReport.FromToRestriction fromto = rest as WFReport.DataReport.FromToRestriction;
                return "(" + fromto.table.Name + "." + fromto.column.Name + " >= '" + fromto.fromValue +
                    "' AND " +      fromto.table.Name + "." + fromto.column.Name + " <= '" + fromto.toValue + "')";
            }

            if (rest is WFReport.DataReport.OptionsRestriction)
            {
                WFReport.DataReport.OptionsRestriction opts = rest as WFReport.DataReport.OptionsRestriction;
                string res = "";

                foreach (var i in opts.options)
                    res += " OR " + opts.table.Name + "." + opts.column.Name + " = '" + i + "'";
                return "(" + res.Substring(4) + ")";
            }

            return "TRUE";
        }

        private string BuildSQLQuery()
        {
            string sqlQuery = WFReport.DataReport.Report.SQLTEMPLATEREPORT;

            sqlQuery = sqlQuery.Replace("<COLUMNS>", ConvertGroupBy(currentReport.cTable, currentReport.cColumn, currentReport.cGroup));
            sqlQuery = sqlQuery.Replace("<ROWS>", ConvertGroupBy(currentReport.rTable, currentReport.rColumn, currentReport.rGroup));
            sqlQuery = sqlQuery.Replace("<AGGREGATE>", currentReport.aTable.Name + "." + currentReport.aColumn.Name);

            string whereQuery = "WHERE\r\n\t";
            if (currentReport.restrictions.Count == 0)
            {
                whereQuery = "";
            }
            else
            {
                var container = currentReport.restrictions.Select(x => ConvertRestrictionToWhereString(x));

                foreach (var i in container)
                {
                    if (i != container.First())
                        whereQuery += " AND ";

                    whereQuery += i;
                }

            }

            sqlQuery = sqlQuery.Replace("<WHERESTATEMENT>", whereQuery);

            return sqlQuery;
        }

        private void LoadDataToGrid(List<string> col, List<string> row, List<string> val)
        {
            grdReport.Rows.Clear();
            grdReport.Columns.Clear();

            Dictionary<int, string> cols = new Dictionary<int, string>();
            Dictionary<int, string> rows = new Dictionary<int, string>();
            foreach (var i in col)
                if (!cols.Values.Contains(i))
                    cols.Add(cols.Count, i);

            foreach (var i in row)
                if (!rows.Values.Contains(i))
                    rows.Add(rows.Count, i);

            grdReport.Columns.Add(" ", " ");

            foreach (var i in cols)
                grdReport.Columns.Add(cols[i.Key], cols[i.Key]);

            foreach (var i in rows)
            {
                string[] rowdata = new string[grdReport.Columns.Count];

                rowdata[0] = i.Value;

                for (int j = 1; j < grdReport.Columns.Count; j++)
                {
                    string rname = i.Value;
                    string cname = cols[j-1];

                    rowdata[j] = "0";

                    for (int k = 0; k < col.Count; k++)
                    {
                        if (row[k] == rname && col[k] == cname)
                            rowdata[j] = val[k];
                    }
                }

                grdReport.Rows.Add(rowdata);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (currentDB == null)
            {
                MessageBox.Show("No database loaded!\nPlease load it by loading metadata (Ctrl+M) or by loading report (Ctrl+O)", "Database is not loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentReport == null)
            {
                MessageBox.Show("No report is being changed!\nPlease load it (Ctrl+O), or create the new one (Ctrl+N)", "The report is not loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query = BuildSQLQuery();
                tbxSqlQueryDevMode.Text = query;

                string databaseName = currentDB.Path;
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName)))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        SQLiteDataReader reader = command.ExecuteReader();

                        List<string> columns = new List<string>();
                        List<string> rows = new List<string>();
                        List<string> values = new List<string>();

                        foreach (DbDataRecord record in reader)
                        {
                            columns.Add(record[0].ToString());
                            rows.Add(record[1].ToString());
                            values.Add(record[2].ToString());
                        }

                        LoadDataToGrid(columns, rows, values);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured during fetching the data for the report:\n" + ex.Message,
                    "Cannot build the report graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            //MessageBox.Show(sqlQuery);
        }

        private void mimOptionsDev_Click(object sender, EventArgs e)
        {
            if (tbxSqlQueryDevMode.Visible)
            {
                tbxSqlQueryDevMode.Visible = false;
                mimOptionsDev.Text = "Enable raw &sql mode";
                lblModeInfo.Text = "Raw Sql mode disabled. Press Ctrl+D to enable it";
            }
            else
            {
                tbxSqlQueryDevMode.Visible = true;
                mimOptionsDev.Text = "Disable raw &sql mode";
                lblModeInfo.Text = "Raw Sql mode enabled. Press Ctrl+D to disable it";
            }
        }

        string filterstate = "";

        IEnumerable<String> GetAllEntriesForTableColumn(string table, string column)
        {
            string databaseName = currentDB.Path;
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName)))
            {
                connection.Open();

                string sqlstr = "SELECT " + table + "." + column + " FROM " + table + " GROUP BY " + table + "." + column + ";";

                using (SQLiteCommand command = new SQLiteCommand(sqlstr , connection))
                {
                    SQLiteDataReader reader = command.ExecuteReader();

                    List<string> columns = new List<string>();
                    List<string> rows = new List<string>();
                    List<string> values = new List<string>();

                    foreach (DbDataRecord record in reader)
                    {
                        yield return record[0].ToString();
                    }
                }
            }
        }

        private void btnMainFiltersAdd_Click(object sender, EventArgs e)
        {
            filterstate = "add";

            pnlLeft.Enabled = false;

            pnlFilterTableColumnType.Left = btnMainFiltersAdd.Left + 50;
            pnlFilterTableColumnType.Top = btnMainFiltersAdd.Top + 300;

            pnlFilterTableColumnType.Visible = true;
            if (rbnOptions.Checked)
            {
                pnlOptions.Visible = true;
                pnlFromTo.Visible = false;
                pnlOptions.Left = pnlFilterTableColumnType.Left + pnlFilterTableColumnType.Width;
            }
            else
            {
                pnlFromTo.Visible = true;
                pnlOptions.Visible = false;
                pnlFromTo.Left = pnlFilterTableColumnType.Left + pnlFilterTableColumnType.Width;
            }

            pnlFromTo.Top = pnlOptions.Top = pnlFilterTableColumnType.Top;

            cboFilter.Items.Clear();

            foreach (var i in cboColumns.Items)
                cboFilter.Items.Add(i);
        }

        private void btnFilterCancel_Click(object sender, EventArgs e)
        {
            filterstate = "";

            pnlFilterTableColumnType.Visible = pnlFromTo.Visible = pnlOptions.Visible = false;
            pnlLeft.Enabled = true;
        }

        private void rbnOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (filterstate != "")
            {
                if (rbnOptions.Checked)
                {
                    pnlOptions.Visible = true;
                    pnlFromTo.Visible = false;
                    pnlOptions.Left = pnlFilterTableColumnType.Left + pnlFilterTableColumnType.Width;
                }
                else
                {
                    pnlFromTo.Visible = true;
                    pnlOptions.Visible = false;
                    pnlFromTo.Left = pnlFilterTableColumnType.Left + pnlFilterTableColumnType.Width;
                }

                pnlFromTo.Top = pnlOptions.Top = pnlFilterTableColumnType.Top;
            }
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilter.SelectedIndex < 1)
                return;

            try
            {
                lbxOptions.Items.Clear();

                string table  = cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[0];
                string column = cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[1].Split(new char[] {' '})[1];

                foreach (var i in GetAllEntriesForTableColumn(table, column))
                {
                    lbxOptions.Items.Add(i);
                }
            }
            catch (Exception) { }
        }

        private void btnFilterDone_Click(object sender, EventArgs e)
        {
            if (rbnOptions.Checked)
            {
                WFReport.DataReport.OptionsRestriction rest = new DataReport.OptionsRestriction();

                rest.table = currentDB.Tables[cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[0]];
                rest.column = rest.table.Columns[cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[1].Split(new char[] { ' ' })[1]];

                foreach (var i in lbxOptions.CheckedItems)
                    rest.options.Add(i.ToString());

                currentReport.restrictions.Add(rest);

                UpdateUIwithReportData();

                btnFilterCancel_Click(sender, e);
            }

            if (rbnFromTo.Checked)
            {
                WFReport.DataReport.FromToRestriction rest = new DataReport.FromToRestriction();

                rest.table = currentDB.Tables[cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[0]];
                rest.column = rest.table.Columns[cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[1].Split(new char[] { ' ' })[1]];

                rest.fromValue = tbxFrom.Text;
                rest.  toValue = tbxTo  .Text;

                currentReport.restrictions.Add(rest);

                UpdateUIwithReportData();

                btnFilterCancel_Click(sender, e);
            }


        }

        private void btnMainFiltersEdit_Click(object sender, EventArgs e)
        {
            if (lbxAdditionalFilters.SelectedIndex >= 0)
            {
 
            }
        }

    }
}
