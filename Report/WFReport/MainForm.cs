using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
