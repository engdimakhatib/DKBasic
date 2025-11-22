namespace DKBasic.Forms
{
    partial class InputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.integer = new System.Windows.Forms.RadioButton();
            this.Real = new System.Windows.Forms.RadioButton();
            this.String = new System.Windows.Forms.RadioButton();
            this.Boolean = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(40, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Value";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(166, 35);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(264, 30);
            this.textBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(40, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select Type";
            // 
            // integer
            // 
            this.integer.AutoSize = true;
            this.integer.Checked = true;
            this.integer.Location = new System.Drawing.Point(205, 122);
            this.integer.Margin = new System.Windows.Forms.Padding(4);
            this.integer.Name = "integer";
            this.integer.Size = new System.Drawing.Size(80, 28);
            this.integer.TabIndex = 3;
            this.integer.TabStop = true;
            this.integer.Text = "integer";
            this.integer.UseVisualStyleBackColor = true;
            // 
            // Real
            // 
            this.Real.AutoSize = true;
            this.Real.Location = new System.Drawing.Point(205, 169);
            this.Real.Margin = new System.Windows.Forms.Padding(4);
            this.Real.Name = "Real";
            this.Real.Size = new System.Drawing.Size(64, 28);
            this.Real.TabIndex = 4;
            this.Real.Text = "Real";
            this.Real.UseVisualStyleBackColor = true;
            // 
            // String
            // 
            this.String.AutoSize = true;
            this.String.Location = new System.Drawing.Point(205, 205);
            this.String.Margin = new System.Windows.Forms.Padding(4);
            this.String.Name = "String";
            this.String.Size = new System.Drawing.Size(73, 28);
            this.String.TabIndex = 5;
            this.String.Text = "String";
            this.String.UseVisualStyleBackColor = true;
            // 
            // Boolean
            // 
            this.Boolean.AutoSize = true;
            this.Boolean.Location = new System.Drawing.Point(205, 256);
            this.Boolean.Margin = new System.Windows.Forms.Padding(4);
            this.Boolean.Name = "Boolean";
            this.Boolean.Size = new System.Drawing.Size(90, 28);
            this.Boolean.TabIndex = 6;
            this.Boolean.Text = "Boolean";
            this.Boolean.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(181, 307);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 59);
            this.button1.TabIndex = 7;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 388);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Boolean);
            this.Controls.Add(this.String);
            this.Controls.Add(this.Real);
            this.Controls.Add(this.integer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private RadioButton integer;
        private RadioButton Real;
        private RadioButton String;
        private RadioButton Boolean;
        private Button button1;
        public TextBox textBox1;
    }
}