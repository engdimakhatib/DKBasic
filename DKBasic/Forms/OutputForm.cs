using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DKBasic.Forms
{
    public partial class OutputForm : Form
    {
        public OutputForm()
        {
            InitializeComponent();
        }

        private void OutputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
         richTextBox1.Clear();
            e.Cancel = true; // يمنع الإغلاق الفعلي
            this.Hide();     // فقط يخفي الفورم بدل أن يدمره

        }

        private void OutputForm_Load(object sender, EventArgs e)
        {

        }
    }
}
