namespace ParserV2
{
    partial class Form1
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
            inputTextBox = new TextBox();
            outputTextBox = new TextBox();
            inputTxtBtn = new Button();
            btn_showForm2 = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // inputTextBox
            // 
            inputTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            inputTextBox.Location = new Point(98, 53);
            inputTextBox.Multiline = true;
            inputTextBox.Name = "inputTextBox";
            inputTextBox.ScrollBars = ScrollBars.Both;
            inputTextBox.Size = new Size(316, 514);
            inputTextBox.TabIndex = 0;
            inputTextBox.WordWrap = false;
            // 
            // outputTextBox
            // 
            outputTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            outputTextBox.Location = new Point(778, 53);
            outputTextBox.Multiline = true;
            outputTextBox.Name = "outputTextBox";
            outputTextBox.ReadOnly = true;
            outputTextBox.ScrollBars = ScrollBars.Both;
            outputTextBox.Size = new Size(321, 514);
            outputTextBox.TabIndex = 1;
            outputTextBox.WordWrap = false;
            // 
            // inputTxtBtn
            // 
            inputTxtBtn.Location = new Point(534, 397);
            inputTxtBtn.Name = "inputTxtBtn";
            inputTxtBtn.Size = new Size(157, 52);
            inputTxtBtn.TabIndex = 2;
            inputTxtBtn.Text = "Choose File";
            inputTxtBtn.UseVisualStyleBackColor = true;
            inputTxtBtn.Click += inputTxtBtn_Click;
            // 
            // btn_showForm2
            // 
            btn_showForm2.Location = new Point(514, 278);
            btn_showForm2.Name = "btn_showForm2";
            btn_showForm2.Size = new Size(198, 62);
            btn_showForm2.TabIndex = 3;
            btn_showForm2.Text = "Scanner Tokens to Syntax Tree";
            btn_showForm2.UseVisualStyleBackColor = true;
            btn_showForm2.Click += btn_showForm2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(514, 165);
            button1.Name = "button1";
            button1.Size = new Size(198, 62);
            button1.TabIndex = 4;
            button1.Text = "Code to Scanner";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1247, 671);
            Controls.Add(button1);
            Controls.Add(btn_showForm2);
            Controls.Add(inputTxtBtn);
            Controls.Add(outputTextBox);
            Controls.Add(inputTextBox);
            MaximumSize = new Size(1265, 718);
            MinimumSize = new Size(1265, 718);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox inputTextBox;
        private TextBox outputTextBox;
        private Button inputTxtBtn;
        private Button btn_showForm2;
        private Button button1;
    }
}