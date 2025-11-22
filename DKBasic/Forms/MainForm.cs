using DKBasic.Compiling_Steps;
using DKBasic.helper;
using DKBasic.structure;
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
    public partial class MainForm : Form
    {
        public static string File_Name = "Untitled.dkb";
        private string Original_Content = "";
        private bool Is_Modified = false;
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages[0].Text = File_Name;
            Global.Read_Key_Word();
            Error.Read_Files_Error();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Is_Modified = (richTextBox1.Text != Original_Content);
            Update_Tab_Title();
        }
        private void Update_Tab_Title()
        {
            string title = Path.GetFileName(File_Name);
            if (string.IsNullOrEmpty(title))
                title = "Untitled.dkb";
            if (Is_Modified)
                title += "*";
            tabControl1.TabPages[0].Text = title;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File_Name == "Untitled.dkb" || !File.Exists(File_Name))
                {
                    using (SaveFileDialog Save_File_Dialog = new SaveFileDialog())
                    {
                        Save_File_Dialog.Filter = "DKB Files (*.dkb)|*.dkb|All Files (*.*)|*.*";
                        if (Save_File_Dialog.ShowDialog() == DialogResult.OK)
                        {
                            File_Name = Save_File_Dialog.FileName;
                            richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);
                            Original_Content = richTextBox1.Text;
                            Is_Modified = false;
                            Update_Tab_Title();
                            MessageBox.Show("File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);
                    Original_Content = richTextBox1.Text;
                    Is_Modified = false;
                    Update_Tab_Title();
                    MessageBox.Show("File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Saving File :{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Is_Modified)
                {
                    var result = MessageBox.Show("Do You Want To Save Changes " + Path.GetFileName(File_Name) + "?",
                        "",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        if (Is_Modified) return;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                richTextBox1.Clear();
                File_Name = "Untitled.dkb";
                Original_Content = "";
                Is_Modified = false;
                Update_Tab_Title();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Creating File :{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void Print_Compilation_Info(RichTextBox richTextBox)
        {
            richTextBox.Text += "\n----- معلومات التحليل النحوي -----\n";
            // طباعة معلومات الإجراءات
            richTextBox.Text += "[الإجراءات]:\n";
            richTextBox.RightToLeft = RightToLeft.Yes;
            TProcedure currentProc = Global.G_Procedure;
            while (currentProc != null)
            {
                richTextBox.Text += $"- الإجراء: {currentProc.name}\n";
                // متحولات الدخل
                if (currentProc.Params_In1 != null)
                {
                    richTextBox.Text += $"  مدخلات: من {currentProc.Params_In1.name} إلى {currentProc.Params_In2?.name ?? currentProc.Params_In1.name}\n";
                }
                // متحولات الخرج
                if (currentProc.Params_Out1 != null)
                {
                    richTextBox.Text += $"  مخرجات: من {currentProc.Params_Out1.name} إلى {currentProc.Params_Out2?.name ?? currentProc.Params_Out1.name}\n";
                }
                currentProc = (TProcedure)currentProc.next;
            }

            richTextBox.Text += "--------------------------------\n";
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (Is_Modified)
                {
                    var result = MessageBox.Show("Do You Want To Save Changes " + Path.GetFileName(File_Name) + "?",
                        "",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        if (Is_Modified) return;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                using (OpenFileDialog Open_File_Dialog = new OpenFileDialog())
                {
                    Open_File_Dialog.Filter = "DKB Files (*.dkb)|*.dkb|All Files (*.*)|*.*";
                    if (Open_File_Dialog.ShowDialog() == DialogResult.OK)
                    {
                        File_Name = Open_File_Dialog.FileName;
                        richTextBox1.LoadFile(File_Name, RichTextBoxStreamType.PlainText);
                        Original_Content = richTextBox1.Text;
                        Is_Modified = false;
                        Update_Tab_Title();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Opening File :{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Is_Modified)
            {
                var result = MessageBox.Show("Do You Want To Save Changes " + Path.GetFileName(File_Name) + "?",
                    "",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    if (Is_Modified) e.Cancel = true;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void aboutDKBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDKBasicForm About_Form = new AboutDKBasicForm();
            About_Form.ShowDialog();
        }

        private void compileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
               Free_Class.Free_ALL();
                Global.Compilation_Successful = true;
                Global.Message_Wrong = "";

                richTextBox2.Clear();
                Syntax_Analysis.Compile_Main_Program(Path.GetFullPath(File_Name));
                if (Global.Compilation_Successful)
                {
                    richTextBox2.Text += "Build Successed"; 
                }
                else
                {
                    richTextBox2.Text += Global.Message_Wrong.ToString();
                }
            }
            catch
            {
                richTextBox2.Text += Global.Message_Wrong.ToString();
            }
            finally
            {
                Global.G_Current_File_SR.Close();     
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Execute.Execute_List_Of_Instruction(Global.G_Main_Instruction);
            }
           catch (Exception ex)
            {
                MessageBox.Show(Global.Message_Wrong +"\n"+ ex.Message );
            }
        }
    }
}
