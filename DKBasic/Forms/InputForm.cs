using DKBasic.helper;
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
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (integer.Checked)
            {
                Global.Is_Input_Integer = true;
                Global.Is_Input_Real = false;
                Global.Is_Input_String = false;
                Global.Is_Input_Boolean = false;
            }
            else if (Real.Checked)
            {
                Global.Is_Input_Integer = false;
                Global.Is_Input_Real = true;
                Global.Is_Input_String = false;
                Global.Is_Input_Boolean = false;
            }
            else if (String.Checked)
            {
                Global.Is_Input_Integer = false;
                Global.Is_Input_Real = false;
                Global.Is_Input_String = true;
                Global.Is_Input_Boolean = false;
            }
            else if (Boolean.Checked)
            {
                Global.Is_Input_Integer = false;
                Global.Is_Input_Real = false;
                Global.Is_Input_String = false;
                Global.Is_Input_Boolean = true;
            }
            this.Close();
        }
    }
}
