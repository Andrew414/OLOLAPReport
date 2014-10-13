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

        string filterstate = "";
        List<int> lastImportant = new List<int>();

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

            //btnFiltersAggregate.Visible = btnFiltersColumn.Visible = btnFiltersRows.Visible = false;
            cboRows.Width = cboAggregate.Width = cboColumns.Width;

            btnSwap.Top = this.Height - 122;
            btnGenerate.Top = btnSwap.Top;

            gbxAdditional.Height = this.Height - 483;

            btnMainFiltersAdd.Top = gbxAdditional.Height - 27;
            btnMainFiltersDelete.Top = btnMainFiltersEdit.Top = btnMainFiltersAdd.Top;

            lbxAdditionalFilters.Height = gbxAdditional.Height - 48;
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

            List<int> newSelected = new List<int>();

            foreach (var i in lbxImportantFields.CheckedIndices)
            {
                newSelected.Add((int)i);
            }

            if (newSelected.Count >= lastImportant.Count)
            {
                lastImportant = newSelected;

                cboColumns.Items.Clear();
                cboRows.Items.Clear();

                cboColumns.Items.Add(" ");
                cboRows.Items.Add(" ");

                foreach(var i in lastImportant)
                {
                    cboColumns.Items.Add(lbxImportantFields.Items[i]);
                    cboRows.Items.Add(lbxImportantFields.Items[i]);
                }

                UpdateUIwithReportData();

                return;
            }

            foreach (var i in lbxImportantFields.Items)
            {
                if (!lbxImportantFields.CheckedItems.Contains(i))
                {
                    if (cboAggregate.SelectedItem.ToString().Contains(i.ToString()))
                    {
                        cboAggregate.SelectedIndex = 0;
                    }

                    cboAggregate.Items.Remove(i);

                    if (cboColumns.SelectedItem.ToString().Contains(i.ToString()))
                    {
                        cboColumns.SelectedIndex = 0;
                    }

                    cboColumns.Items.Remove(i);

                    if (cboRows.SelectedItem.ToString().Contains(i.ToString()))
                    {
                        cboRows.SelectedIndex = 0;
                    }

                    cboRows.Items.Remove(i);

                    List<WFReport.DataReport.Restriction> toBeDeleted = new List<DataReport.Restriction>();
                    foreach (var j in currentReport.restrictions)
                    {
                        if (j.ToString().Contains(i.ToString()))
                            toBeDeleted.Add(j);
                    }
                    foreach (var j in toBeDeleted)
                    {
                        currentReport.restrictions.Remove(j);
                    }
                }
            }

            UpdateUIwithReportData();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            mimOptionsAbout_Click(sender, e);
        }

        private void openMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdMetadata.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LoadMetadataFromFile(ofdMetadata.FileName);

                    UpdateUIwithMetadata();

                    for (int i = 0; i < lbxImportantFields.Items.Count; i++)
                        lbxImportantFields.SetItemChecked(i, true);

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

                        UpdateUIwithMetadata();
                        for (int k = 0; k < lbxImportantFields.Items.Count; k++)
                            lbxImportantFields.SetItemChecked(k, true);

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
                        lbxImportantFields.Items.Add(i.Name + ": " + j.Name);
                        cboColumns.Items.Add(i.Name + ": " + j.Name);
                        cboRows.Items.Add(i.Name + ": " + j.Name);
                        
                    } else
                    {
                        cboAggregate.Items.Add(i.Name + ": " + j.Name);
                    }
                }
            }

        }

        private void UIProcessGroupBy(WFReport.Metadata.Column col, ComboBox combo, string group)
        {
            if (col != null && col.Type == Metadata.ColumnType.DateColumn)
            {
                combo.Enabled = true;

                // day = 1; month = 2; year = 3; another = 0

                switch (group)
                {
                    case "Month":
                        combo.SelectedItem = combo.Items[2];
                        break;
                    case "Year":
                        combo.SelectedItem = combo.Items[3];
                        break;
                    case "Day":
                        combo.SelectedItem = combo.Items[1];
                        break;
                    default:
                        combo.SelectedItem = combo.Items[0];
                        break;
                }
            }
            else
            {
                combo.Enabled = false;
                combo.SelectedItem = combo.Items[0];
            }
        }

        private void UpdateUIwithReportData()
        {
            //UpdateUIwithMetadata();

            UIProcessGroupBy(currentReport.cColumn, cboColumnGroup, currentReport.cGroup);
            UIProcessGroupBy(currentReport.rColumn, cboRowsGroup, currentReport.rGroup);

            cboAggregate.SelectedIndex = (currentReport.aColumn != null && currentReport.aTable != null) ? cboAggregate.Items.IndexOf(currentReport.aTable.Name + ": " + currentReport.aColumn.Name) : 0;
            cboColumns  .SelectedIndex = (currentReport.cColumn != null && currentReport.cTable != null) ? cboColumns  .Items.IndexOf(currentReport.cTable.Name + ": " + currentReport.cColumn.Name) : 0;
            cboRows     .SelectedIndex = (currentReport.rColumn != null && currentReport.rTable != null) ? cboRows     .Items.IndexOf(currentReport.rTable.Name + ": " + currentReport.rColumn.Name) : 0;

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
                    default:
                        return "strftime(\"%Y-%m-%d\", " +
                           table.Name + "." + column.Name + ")";

                    // Sqlite plugin doesn't recognize the unformatted dates. 
                    // So default action is changed to be the same as for "day" 
                    //default:
                    //    return table.Name + "." + column.Name;
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

            if (currentReport.aColumn == null || currentReport.aTable == null || 
                currentReport.cColumn == null || currentReport.cTable == null || 
                currentReport.rColumn == null || currentReport.rTable == null)
            {
                MessageBox.Show("At lease one of the important fields is not chosen (rows/columns/aggregate).\nPlease choose them and then try again", "Fields for report are not chosen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //try
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
                            columns.Add(record[btnSwap.Checked ? 1 : 0].ToString());
                            rows.Add(record[btnSwap.Checked ? 0 : 1].ToString());
                            values.Add(record[2].ToString());
                        }

                        LoadDataToGrid(columns, rows, values);
                    }

                }
            }/*
            catch (Exception ex)
            {
                MessageBox.Show("An error occured during fetching the data for the report:\n" + ex.Message,
                    "Cannot build the report graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            

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

                for(var i = 0; i < lbxOptions.Items.Count; i++)
                {
                    lbxOptions.SetItemChecked(i, true);
                }

                tbxFrom.Text = "";
                tbxTo.Text = "";
            }
            catch (Exception) { }
        }

        private void btnFilterDone_Click(object sender, EventArgs e)
        {
            if (filterstate != "add" && filterstate != "edit")
            {
                return;
            }

            if (rbnOptions.Checked)
            {
                WFReport.DataReport.OptionsRestriction rest = new DataReport.OptionsRestriction();

                rest.table = currentDB.Tables[cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[0]];
                rest.column = rest.table.Columns[cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[1].Split(new char[] { ' ' })[1]];

                foreach (var i in lbxOptions.CheckedItems)
                    rest.options.Add(i.ToString());

                if (filterstate == "add")
                    currentReport.restrictions.Add(rest);

                if (filterstate == "edit")
                    currentReport.restrictions[lbxAdditionalFilters.SelectedIndex] = rest;

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

                if (filterstate == "add")
                    currentReport.restrictions.Add(rest);

                if (filterstate == "edit")
                    currentReport.restrictions[lbxAdditionalFilters.SelectedIndex] = rest;

                UpdateUIwithReportData();

                btnFilterCancel_Click(sender, e);
            }

            filterstate = "";
        }

        private void btnMainFiltersEdit_Click(object sender, EventArgs e)
        {
            if (!UpdateDeleteCheck())
            {
                return;
            }

            filterstate = "edit";

            pnlLeft.Enabled = false;

            pnlFilterTableColumnType.Left = btnMainFiltersAdd.Left + 50;
            pnlFilterTableColumnType.Top = btnMainFiltersAdd.Top + 300;

            pnlFilterTableColumnType.Visible = true;

            pnlFromTo.Top = pnlOptions.Top = pnlFilterTableColumnType.Top;

            cboFilter.Items.Clear();

            foreach (var i in cboColumns.Items)
                cboFilter.Items.Add(i);

            WFReport.Metadata.Column col = currentReport.restrictions[lbxAdditionalFilters.SelectedIndex].GetColumn();
            WFReport.Metadata.Table  tab = currentReport.restrictions[lbxAdditionalFilters.SelectedIndex].GetTable();

            for (int i = 0; i < cboFilter.Items.Count; i++)
                if (cboFilter.Items[i].ToString().Contains(tab.Name + ": " + col.Name))
                {
                    cboFilter.SelectedIndex = i;
                    break;
                }

            //cboFilter.Select()

            WFReport.DataReport.Restriction restriction = currentReport.restrictions[lbxAdditionalFilters.SelectedIndex];

            if (restriction is WFReport.DataReport.OptionsRestriction) rbnOptions.Select();
            if (restriction is WFReport.DataReport. FromToRestriction) rbnFromTo .Select();

            if (restriction is WFReport.DataReport.OptionsRestriction)
            {
                WFReport.DataReport.OptionsRestriction rest = restriction as WFReport.DataReport.OptionsRestriction;

                pnlOptions.Visible = true;
                pnlFromTo.Visible = false;
                pnlOptions.Left = pnlFilterTableColumnType.Left + pnlFilterTableColumnType.Width;

                lbxOptions.Items.Clear();

                string table = cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[0];
                string column = cboFilter.Items[cboFilter.SelectedIndex].ToString().Split(new char[] { ':' })[1].Split(new char[] { ' ' })[1];

                foreach (var i in GetAllEntriesForTableColumn(table, column))
                {
                    lbxOptions.Items.Add(i, rest.options.Contains(i));
                }
            }

            if (restriction is WFReport.DataReport.FromToRestriction)
            {
                pnlFromTo.Visible = true;
                pnlOptions.Visible = false;
                pnlFromTo.Left = pnlFilterTableColumnType.Left + pnlFilterTableColumnType.Width;

                WFReport.DataReport.FromToRestriction rest = restriction as WFReport.DataReport.FromToRestriction;

                tbxFrom.Text = rest.fromValue;
                tbxTo.Text = rest.toValue;
            }

        }

        private void mimFileNew_Click(object sender, EventArgs e)
        {
            if (currentDB == null)
            {
                MessageBox.Show("Database is not loaded. Please use \"Open metadata\" command (Ctrl+M)", "No database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            currentReport = new DataReport.Report();
            currentPathReport = null;

            pnlLeft.Enabled = true;
        }

        private void tbxNew_Click(object sender, EventArgs e)
        {
            mimFileNew_Click(sender, e);
        }

        private void SaveReportToFile(WFReport.DataReport.Report report, string filename)
        {
            try
            {
                string data = currentReport.ToXML(currentDB, currentPathMetadata);

                StreamWriter file = new StreamWriter(filename);
                file.WriteLine(data);
                file.Close();

                currentPathReport = filename;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error while saving the file \"" + filename + "\": " + e.Message, "File cannot be saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckSaveReport()
        {
            if (currentDB == null)
            {
                MessageBox.Show("Database is not loaded.\nPlease load it by using the command \"Load metadata\" (Ctrl + M)", "No database loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (currentReport == null)
            {
                MessageBox.Show("Report is not loaded.\nPlease load it by using the command \"Load report\" (Ctrl + O), or create the new one (Ctrl + M)", "No report loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void mimFileSaveAs_Click(object sender, EventArgs e)
        {
            if (!CheckSaveReport())
                return;

            if (sfdReport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveReportToFile(currentReport, sfdReport.FileName);
            }
        }

        private void mimFileSave_Click(object sender, EventArgs e)
        {
            if (!CheckSaveReport())
                return;

            if (currentPathReport != null)
            {
                SaveReportToFile(currentReport, currentPathReport);
            }
            else
            {
                mimFileSaveAs_Click(sender, e);
            }
        }

        private void tbxSave_Click(object sender, EventArgs e)
        {
            mimFileSave_Click(sender, e);
        }

        private void mimFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mimOptionsAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OLOLAP v1.0.", "OLOLAP info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool UpdateDeleteCheck()
        {
            if (lbxAdditionalFilters.SelectedIndex >= 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Nothing is selected", "Selection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnMainFiltersDelete_Click(object sender, EventArgs e)
        {
            if (!UpdateDeleteCheck())
            {
                return;
            }

            currentReport.restrictions.RemoveAt(lbxAdditionalFilters.SelectedIndex);

            UpdateUIwithReportData();
        }

        private void lbxAdditionalFilters_DoubleClick(object sender, EventArgs e)
        {
            if (lbxAdditionalFilters.SelectedIndex < 0)
                return;

            btnMainFiltersEdit_Click(sender, e);
        }

        private void cboColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboColumns.SelectedIndex == 0)
            {
                currentReport.cTable = null;
                currentReport.cColumn = null;
                currentReport.cGroup = "";

                cboColumnGroup.Enabled = false;

                return;
            }

            string stable = cboColumns.SelectedItem.ToString().Split(new char[] { ':' })[0];
            string scolumn = cboColumns.SelectedItem.ToString().Split(new char[] { ':' })[1].Split(new char[] {' '})[1];

            Metadata.Table table = currentDB.Tables[stable];
            Metadata.Column column = table.Columns[scolumn];

            if (column.Type == Metadata.ColumnType.DateColumn)
            {
                cboColumnGroup.Enabled = true;

                if (cboColumnGroup.SelectedIndex < 0)
                    cboColumnGroup.SelectedIndex = 0;

                currentReport.cGroup = cboColumnGroup.SelectedItem.ToString();
            }
            else
            {
                cboColumnGroup.Enabled = false;
            }

            currentReport.cTable = table;
            currentReport.cColumn = column;
        }

        private void cboRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRows.SelectedIndex == 0)
            {
                currentReport.rTable = null;
                currentReport.rColumn = null;
                currentReport.rGroup = "";

                cboRowsGroup.Enabled = false;

                return;
            }

            string stable = cboRows.SelectedItem.ToString().Split(new char[] { ':' })[0];
            string scolumn = cboRows.SelectedItem.ToString().Split(new char[] { ':' })[1].Split(new char[] { ' ' })[1];

            Metadata.Table table = currentDB.Tables[stable];
            Metadata.Column column = table.Columns[scolumn];

            if (column.Type == Metadata.ColumnType.DateColumn)
            {
                cboRowsGroup.Enabled = true;

                if (cboRowsGroup.SelectedIndex < 0)
                    cboRowsGroup.SelectedIndex = 0;

                currentReport.rGroup = cboRowsGroup.SelectedItem.ToString();
            }
            else
            {
                cboRowsGroup.Enabled = false;
            }

            currentReport.rTable = table;
            currentReport.rColumn = column;
        }

        private void btnSwap_CheckedChanged(object sender, EventArgs e)
        {
            btnGenerate_Click(sender, e);
        }

        private void cboColumnGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentReport.cGroup = cboColumnGroup.SelectedItem.ToString();
        }

        private void cboRowsGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentReport.rGroup = cboRowsGroup.SelectedItem.ToString();
        }

        private void cboAggregate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAggregate.SelectedIndex == 0)
            {
                currentReport.aTable = null;
                currentReport.aColumn = null;

                return;
            }

            string stable = cboAggregate.SelectedItem.ToString().Split(new char[] { ':' })[0];
            string scolumn = cboAggregate.SelectedItem.ToString().Split(new char[] { ':' })[1].Split(new char[] { ' ' })[1];

            Metadata.Table table = currentDB.Tables[stable];
            Metadata.Column column = table.Columns[scolumn];

            currentReport.aTable = table;
            currentReport.aColumn = column;
        }

    }
}
