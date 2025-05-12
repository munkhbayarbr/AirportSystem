namespace ClientApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            button2 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            button1 = new Button();
            textBox1 = new TextBox();
            order = new Button();
            status = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(12, 48);
            panel1.Name = "panel1";
            panel1.Size = new Size(1273, 679);
            panel1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(528, 636);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 4;
            button2.Text = "save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += click_Save;
            // 
            // panel2
            // 
            panel2.Location = new Point(637, 19);
            panel2.Name = "panel2";
            panel2.Size = new Size(620, 646);
            panel2.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 100);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 2;
            label1.Text = "label1";
            // 
            // button1
            // 
            button1.Location = new Point(164, 29);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "search";
            button1.UseVisualStyleBackColor = true;
            button1.Click += click_Search;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(33, 29);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 0;
            // 
            // order
            // 
            order.Location = new Point(12, 12);
            order.Name = "order";
            order.Size = new Size(94, 29);
            order.TabIndex = 1;
            order.Text = "order";
            order.UseVisualStyleBackColor = true;
            order.Click += click_Order;
            // 
            // status
            // 
            status.Location = new Point(112, 12);
            status.Name = "status";
            status.Size = new Size(94, 29);
            status.TabIndex = 2;
            status.Text = "status";
            status.UseVisualStyleBackColor = true;
            status.Click += click_Status;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1297, 739);
            Controls.Add(status);
            Controls.Add(order);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += form1_FormClosing;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button2;
        private Panel panel2;
        private Label label1;
        private Button button1;
        private TextBox textBox1;
        private Button order;
        private Button status;
    }
}
