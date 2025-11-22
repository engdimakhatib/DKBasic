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
    public partial class AboutDKBasicForm : Form
    {
        public AboutDKBasicForm()
        {
            InitializeComponent();
            SetUpForm();
        }

        private void AboutDKBasicForm_Load(object sender, EventArgs e)
        {

        }

        private void SetUpForm()
        {
            this.Text = "About DKBasic Language";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Create main panel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.White;

            // Create title label
            Label titleLabel = new Label();
            titleLabel.Text = "DKBasic Programming Language";
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.ForeColor = Color.DarkBlue;
            titleLabel.AutoSize = true;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Padding = new Padding(0, 0, 0, 10);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Create subtitle label
            Label subtitleLabel = new Label();
            subtitleLabel.Text = "Designed by Engineer Dima Khatib";
            subtitleLabel.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            subtitleLabel.ForeColor = Color.Gray;
            subtitleLabel.AutoSize = true;
            subtitleLabel.Dock = DockStyle.Top;
            subtitleLabel.Padding = new Padding(0, 0, 0, 20);
            subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Create RichTextBox for content
            RichTextBox infoRichTextBox = new RichTextBox();
            infoRichTextBox.Dock = DockStyle.Fill;
            infoRichTextBox.ReadOnly = true;
            infoRichTextBox.BackColor = Color.White;
            infoRichTextBox.BorderStyle = BorderStyle.None;
            infoRichTextBox.Font = new Font("Consolas", 10);
            infoRichTextBox.Margin = new Padding(10);
            infoRichTextBox.Text = GetDKBasicInfo();

            // Apply formatting to RichTextBox
            ApplyTextFormatting(infoRichTextBox);

            // Add controls to main panel
            mainPanel.Controls.Add(infoRichTextBox);
            mainPanel.Controls.Add(subtitleLabel);
            mainPanel.Controls.Add(titleLabel);

            this.Controls.Add(mainPanel);
        }

        private void ApplyTextFormatting(RichTextBox rtb)
        {
            rtb.SelectAll();
            rtb.SelectionColor = Color.Black;
            rtb.SelectionFont = new Font("Segoe UI", 10);

            // Format titles and sections
            FormatSection(rtb, "DKBasic is a simple programming language", Color.DarkBlue, FontStyle.Bold, 11);
            FormatSection(rtb, "The language contains the following instructions:", Color.DarkBlue, FontStyle.Bold, 11);

            // Format instruction numbers
            for (int i = 1; i <= 11; i++)
            {
                FormatSection(rtb, i + ". ", Color.DarkRed, FontStyle.Bold, 10);
            }

            // Format Arabic comments
            FormatSection(rtb, "// ", Color.Green, FontStyle.Regular, 9);

            // Format keywords
            string[] keywords = {
                "General Form:", "Example:", "if", "else", "then", "while", "DO",
                "procedure", "call", "switch", "default", "for", "to", "down to",
                "step", "break", "continue", "writeln", "write", "Read"
            };

            foreach (string keyword in keywords)
            {
                FormatKeyword(rtb, keyword);
            }

            // Format Additional Syntax Rules
            FormatSection(rtb, "Additional Syntax Rules:", Color.DarkRed, FontStyle.Bold, 11);

            rtb.Select(0, 0); // Reset selection
            rtb.ScrollToCaret();
        }

        private void FormatSection(RichTextBox rtb, string text, Color color, FontStyle style, float size)
        {
            int start = 0;
            while (start < rtb.TextLength)
            {
                int index = rtb.Find(text, start, rtb.TextLength, RichTextBoxFinds.None);
                if (index == -1) break;

                rtb.Select(index, text.Length);
                rtb.SelectionColor = color;
                rtb.SelectionFont = new Font("Segoe UI", size, style);

                start = index + text.Length;
            }
        }

        private void FormatKeyword(RichTextBox rtb, string keyword)
        {
            int start = 0;
            while (start < rtb.TextLength)
            {
                int index = rtb.Find(keyword, start, rtb.TextLength, RichTextBoxFinds.WholeWord);
                if (index == -1) break;

                rtb.Select(index, keyword.Length);
                rtb.SelectionColor = Color.Blue;
                rtb.SelectionFont = new Font("Consolas", 10, FontStyle.Bold);

                start = index + keyword.Length;
            }
        }

        private string GetDKBasicInfo()
        {
            return @"DKBasic is a simple programming language designed by Engineer Dima Khatib

The language contains the following instructions:

1. Assignment Statement: // تعليمة الإسناد
   General Form:
   [identifier] := [expression] ;

   Example:
   current_Num := 0 ;
   x := y + 5 * 2 ;

2. if/else Statement: // تعليمة الشرط
   General Form:
   if ( [condition] ) then {
       [commands if true]
   }
   else {
       [commands if false]
   }

   Example:
   if (x > 10) then {
       writeln('Greater than 10');
   }
   else {
       writeln('Less or equal to 10');
   }

3. Print Statements: // تعليمات الطباعة
   General Form:
   writeln([expression1], [expression2], ...) ;  ← with new line
   write([expression1], [expression2], ...) ;    ← without new line

   Example:
   writeln('The result is:', result);
   write('Hello World');

4. Input Statement: // تعليمة قراءة المدخلات
   General Form:
   Read( variable1, variable2, ... ) ;

   Example:
   Read(x, y, z);

5. while Loop: // حلقة التكرار while
   General Form:
   while ( [condition] ) DO {
       [execution commands]
   }

   Example:
   while (i < 10) DO {
       writeln(i);
       i := i + 1;
   }

6. Procedure Definition: // تعريف الإجرائيات
   General Form:
   procedure [procedure_name] ( [input_params], output [output_params] ) {
       [procedure commands]
   }

7. Procedure Call: // استدعاء الإجرائية
   General Form:
   call [procedure_name] ( [input_params] output [output_params] ) ;

   Example:
   call Calculate(x, y output result);

8. Switch Statement: // تعليمة الاختيار
   General Form:
   switch [variable] of: {
       [value1]: { [commands1] }
       [value2]: { [commands2] }
       default: { [default_commands] }
   }

   Example:
   switch choice of: {
       1: { writeln('Option 1'); }
       2: { writeln('Option 2'); }
       default: { writeln('Invalid option'); }
   }

9. Increment/Decrement Operations: // عمليات الزيادة والنقصان
   General Forms:
   variable++ ;    // post-increment
   variable-- ;    // post-decrement  
   ++variable ;    // pre-increment
   --variable ;    // pre-decrement

   Example:
   counter++;
   --index;

10. For Loop: // حلقة التكرار for
    General Forms:
    for [variable] := [start] to [end] DO { [commands] }
    for [variable] := [start] down to [end] DO { [commands] }
    for [variable] := [start] to [end] step [step_value] DO { [commands] }

    Example:
    for i := 1 to 10 DO {
        writeln(i);
    }
    for j := 10 down to 1 DO {
        writeln(j);
    }
    for k := 0 to 100 step 5 DO {
        writeln(k);
    }

11. Break and Continue Statements: // تعليمات الخروج والمتابعة
    General Forms:
    break;     // exit loop
    continue;  // skip to next iteration

    Example:
    while (true) DO {
        if (x > 100) then {
            break;
        }
        if (x % 2 == 0) then {
            continue;
        }
        writeln(x);
    }

Additional Syntax Rules:
- All statements end with semicolon (;)
- Code blocks are enclosed in curly braces { }
- Conditions are enclosed in parentheses ( )
- Multiple parameters are separated by commas
- Variables must be declared before use
- Procedures can have both input and output parameters

Language Features:
✓ Structured programming constructs
✓ Procedure support with parameters
✓ Multiple loop types
✓ Conditional statements
✓ Input/output operations
✓ Variable operations and expressions
";
        }
    }
}
//namespace DKBasic.Forms
//{
//    public partial class AboutDKBasicForm : Form
//    {
//        public AboutDKBasicForm()
//        {
//            InitializeComponent();
//            SetUpForm();
//        }

//        private void AboutDKBasicForm_Load(object sender, EventArgs e)
//        {

//        }
//        private void SetUpForm()
//        {
//            this.Text = "About DKBasic Language";
//            this.Size = new Size(600, 500);
//            this.StartPosition = FormStartPosition.CenterParent;
//            this.FormBorderStyle = FormBorderStyle.FixedDialog;
//            this.MaximizeBox = false;
//            this.MinimizeBox = false;
//            RichTextBox info_Rich_Text_Box = new RichTextBox();
//            info_Rich_Text_Box.Dock = DockStyle.Fill;
//            info_Rich_Text_Box.ReadOnly = true;
//            info_Rich_Text_Box.BackColor = Color.White;
//            info_Rich_Text_Box.Font = new Font("Cairo" , 10);
//            info_Rich_Text_Box.Text = GetDKBasicInfo();
//            this.Controls.Add(info_Rich_Text_Box);
//        }
//        private string GetDKBasicInfo()
//        {
//            return @"DKBasic is a simple programming language designed by Engineer Dima Khatib

//The language contains the following instructions:

//1. Assignment Statement: // تعليمة الإسناد
//   General Form:
//   [identifier] := [expression] ;

//   Example:
//   current_Num := 0 ;
//   x := y + 5 * 2 ;

//2. if/else Statement: // تعليمة الشرط
//   General Form:
//   if ( [condition] ) then {
//       [commands if true]
//   }
//   else {
//       [commands if false]
//   }

//   Example:
//   if (x > 10) then {
//       writeln('Greater than 10');
//   }
//   else {
//       writeln('Less or equal to 10');
//   }

//3. Print Statements: // تعليمات الطباعة
//   General Form:
//   writeln([expression1], [expression2], ...) ;  ← with new line
//   write([expression1], [expression2], ...) ;    ← without new line

//   Example:
//   writeln('The result is:', result);
//   write('Hello World');

//4. Input Statement: // تعليمة قراءة المدخلات
//   General Form:
//   Read( variable1, variable2, ... ) ;

//   Example:
//   Read(x, y, z);

//5. while Loop: // حلقة التكرار while
//   General Form:
//   while ( [condition] ) DO {
//       [execution commands]
//   }

//   Example:
//   while (i < 10) DO {
//       writeln(i);
//       i := i + 1;
//   }

//6. Procedure Definition: // تعريف الإجرائيات
//   General Form:
//   procedure [procedure_name] ( [input_params], output [output_params] ) {
//       [procedure commands]
//   }

//7. Procedure Call: // استدعاء الإجرائية
//   General Form:
//   call [procedure_name] ( [input_params] output [output_params] ) ;

//   Example:
//   call Calculate(x, y output result);

//8. Switch Statement: // تعليمة الاختيار
//   General Form:
//   switch [variable] of: {
//       [value1]: { [commands1] }
//       [value2]: { [commands2] }
//       default: { [default_commands] }
//   }

//   Example:
//   switch choice of: {
//       1: { writeln('Option 1'); }
//       2: { writeln('Option 2'); }
//       default: { writeln('Invalid option'); }
//   }

//9. Increment/Decrement Operations: // عمليات الزيادة والنقصان
//   General Forms:
//   variable++ ;    // post-increment
//   variable-- ;    // post-decrement  
//   ++variable ;    // pre-increment
//   --variable ;    // pre-decrement

//   Example:
//   counter++;
//   --index;

//10. For Loop: // حلقة التكرار for
//    General Forms:
//    for [variable] := [start] to [end] DO { [commands] }
//    for [variable] := [start] down to [end] DO { [commands] }
//    for [variable] := [start] to [end] step [step_value] DO { [commands] }

//    Example:
//    for i := 1 to 10 DO {
//        writeln(i);
//    }
//    for j := 10 down to 1 DO {
//        writeln(j);
//    }
//    for k := 0 to 100 step 5 DO {
//        writeln(k);
//    }

//11. Break and Continue Statements: // تعليمات الخروج والمتابعة
//    General Forms:
//    break;     // exit loop
//    continue;  // skip to next iteration

//    Example:
//    while (true) DO {
//        if (x > 100) then {
//            break;
//        }
//        if (x % 2 == 0) then {
//            continue;
//        }
//        writeln(x);
//    }

//Additional Syntax Rules:
//- All statements end with semicolon (;)
//- Code blocks are enclosed in curly braces { }
//- Conditions are enclosed in parentheses ( )
//- Multiple parameters are separated by commas
//- Variables must be declared before use
//- Procedures can have both input and output parameters
//";
//        }
//    }
//}
