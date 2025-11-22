using DKBasic.Compiling_Steps;
using DKBasic.Compiling_Steps.Execution_Tree;
using DKBasic.helper;
using DKBasic.structure;

namespace DKBasic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Global.Read_Key_Word();
            Error.Read_Files_Error();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            Texp head = null;
            StreamReader reader = null;

            try
            {
                richTextBox1.SaveFile("test.txt", RichTextBoxStreamType.PlainText);
                reader = new StreamReader("test.txt");
                Global.G_Current_File_SR = reader;

                Texp last = null;
                Global.token = Lexical_Analysis.Lexical_Token();
                head = build_AST_Tree.Read_Condition(ref last);

                richTextBox2.AppendText("تحليل التعبير : \n\n");
                int index = 0;

                Texp current = head;
                while (current != null)
                {
                    string token = current.token.ToString();
                    string value = "";

                    if (current.Val_NB != 0)
                        value = current.Val_NB.ToString();
                    if (!string.IsNullOrEmpty(current.Val_STR))
                        value = "\"" + current.Val_STR + "\"";
                    if (current.Val_Var != null)
                        value = current.Val_Var.name.ToString();

                    richTextBox2.AppendText($"[{index}] المعرف : {token} \t القيمة : {value} \n");
                    current = current.next;
                    index++;
                }
                richTextBox2.AppendText("\n\n");
                richTextBox2.AppendText("تقييم التعبير : \n\n");

            // Texp evaluated =   Optimization.Evaluate_Exp(head);
            Boolean evaluated = Optimization.Exaluate_Cond(head);
                richTextBox2.AppendText($" القيمة المنطقية : \t {evaluated} \n");

                //richTextBox2.AppendText($" المعرف : \t {evaluated.token} \n");
                //richTextBox2.AppendText($" النص : \t {evaluated.Val_STR} \n");
                //richTextBox2.AppendText($" الرقم : \t {evaluated.Val_NB} \n");

                Texp.FreeList(head);
            //    head = evaluated;

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(Global.Message_Wrong))
                {
                    MessageBox.Show(Global.Message_Wrong);
                }
                else
                {
                    MessageBox.Show($"خطأ: {ex.Message}");
                }
            }
            finally
            {
                // تحرير الذاكرة في جميع الأحوال
                if (head != null)
                {
                    Texp.FreeList(head);
                    head = null;
                }

                if (reader != null)
                {
                    reader.Close();
                    Global.G_Current_File_SR = null;
                }
            }
        }

    }
}

