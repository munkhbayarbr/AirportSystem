namespace ClientApp
{
    partial class SeatSelectionForm
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
            printDocument = new System.Drawing.Printing.PrintDocument();
            tableLayoutPanel1 = new TableLayoutPanel();
            panelSeats = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel1 = new Panel();
            lblPassport = new Label();
            txtPassport = new TextBox();
            btnSearch = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panelSeats, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1024, 554);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panelSeats
            // 
            panelSeats.Dock = DockStyle.Fill;
            panelSeats.Location = new Point(515, 3);
            panelSeats.Name = "panelSeats";
            panelSeats.Size = new Size(506, 548);
            panelSeats.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel2.Size = new Size(506, 548);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblPassport);
            panel1.Controls.Add(txtPassport);
            panel1.Controls.Add(btnSearch);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(500, 158);
            panel1.TabIndex = 0;
            // 
            // lblPassport
            // 
            lblPassport.AutoSize = true;
            lblPassport.Location = new Point(55, 72);
            lblPassport.Name = "lblPassport";
            lblPassport.Size = new Size(38, 15);
            lblPassport.TabIndex = 2;
            lblPassport.Text = "label1";
            // 
            // txtPassport
            // 
            txtPassport.Location = new Point(138, 69);
            txtPassport.Name = "txtPassport";
            txtPassport.Size = new Size(100, 23);
            txtPassport.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(283, 69);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 0;
            btnSearch.Text = "button1";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += BtnSearch_Click;
            // 
            // SeatSelectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 554);
            Controls.Add(tableLayoutPanel1);
            Name = "SeatSelectionForm";
            Text = "SeatSelectionForm";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Drawing.Printing.PrintDocument printDocument;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panelSeats;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private Label lblPassport;
        private TextBox txtPassport;
        private Button btnSearch;
    }
}