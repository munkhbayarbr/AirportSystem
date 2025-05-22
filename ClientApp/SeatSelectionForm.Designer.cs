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
            button1 = new Button();
            lblPassport = new Label();
            txtPassport = new TextBox();
            btnSearch = new Button();
            panel2 = new Panel();
            lblBookedSeat = new Label();
            lblPhone = new Label();
            lblBday = new Label();
            lblLastName = new Label();
            lblFname = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
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
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.Size = new Size(1170, 739);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panelSeats
            // 
            panelSeats.Dock = DockStyle.Fill;
            panelSeats.Location = new Point(588, 4);
            panelSeats.Margin = new Padding(3, 4, 3, 4);
            panelSeats.Name = "panelSeats";
            panelSeats.Size = new Size(579, 731);
            panelSeats.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel1, 0, 0);
            tableLayoutPanel2.Controls.Add(panel2, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 4);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel2.Size = new Size(579, 731);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(lblPassport);
            panel1.Controls.Add(txtPassport);
            panel1.Controls.Add(btnSearch);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 4);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(573, 211);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(465, 14);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 3;
            button1.Text = "Dashboard";
            button1.UseVisualStyleBackColor = true;
            button1.Click += click_Dashboard;
            // 
            // lblPassport
            // 
            lblPassport.AutoSize = true;
            lblPassport.Location = new Point(63, 96);
            lblPassport.Name = "lblPassport";
            lblPassport.Size = new Size(88, 20);
            lblPassport.TabIndex = 2;
            lblPassport.Text = "Passport No";
            // 
            // txtPassport
            // 
            txtPassport.Location = new Point(158, 92);
            txtPassport.Margin = new Padding(3, 4, 3, 4);
            txtPassport.Name = "txtPassport";
            txtPassport.Size = new Size(114, 27);
            txtPassport.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(323, 92);
            btnSearch.Margin = new Padding(3, 4, 3, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(86, 31);
            btnSearch.TabIndex = 0;
            btnSearch.Text = "Хайх";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += BtnSearch_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblBookedSeat);
            panel2.Controls.Add(lblPhone);
            panel2.Controls.Add(lblBday);
            panel2.Controls.Add(lblLastName);
            panel2.Controls.Add(lblFname);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 222);
            panel2.Name = "panel2";
            panel2.Size = new Size(573, 506);
            panel2.TabIndex = 1;
            // 
            // lblBookedSeat
            // 
            lblBookedSeat.AutoSize = true;
            lblBookedSeat.BackColor = SystemColors.ActiveBorder;
            lblBookedSeat.Font = new Font("Segoe UI", 16F);
            lblBookedSeat.Location = new Point(90, 373);
            lblBookedSeat.MinimumSize = new Size(300, 50);
            lblBookedSeat.Name = "lblBookedSeat";
            lblBookedSeat.Size = new Size(300, 50);
            lblBookedSeat.TabIndex = 4;
            lblBookedSeat.Text = "Seat";
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.BackColor = SystemColors.ActiveBorder;
            lblPhone.Font = new Font("Segoe UI", 16F);
            lblPhone.Location = new Point(90, 291);
            lblPhone.MinimumSize = new Size(300, 50);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(300, 50);
            lblPhone.TabIndex = 3;
            lblPhone.Text = "Phone";
            // 
            // lblBday
            // 
            lblBday.AutoSize = true;
            lblBday.BackColor = SystemColors.ActiveBorder;
            lblBday.Font = new Font("Segoe UI", 16F);
            lblBday.Location = new Point(90, 210);
            lblBday.MinimumSize = new Size(300, 50);
            lblBday.Name = "lblBday";
            lblBday.Size = new Size(300, 50);
            lblBday.TabIndex = 2;
            lblBday.Text = "Birthday";
            lblBday.Click += label3_Click;
            // 
            // lblLastName
            // 
            lblLastName.AutoSize = true;
            lblLastName.BackColor = SystemColors.ActiveBorder;
            lblLastName.Font = new Font("Segoe UI", 16F);
            lblLastName.Location = new Point(90, 132);
            lblLastName.MinimumSize = new Size(300, 50);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(300, 50);
            lblLastName.TabIndex = 1;
            lblLastName.Text = "Last Name";
            // 
            // lblFname
            // 
            lblFname.AutoSize = true;
            lblFname.BackColor = SystemColors.ActiveBorder;
            lblFname.Font = new Font("Segoe UI", 16F);
            lblFname.Location = new Point(90, 66);
            lblFname.MinimumSize = new Size(300, 50);
            lblFname.Name = "lblFname";
            lblFname.Size = new Size(300, 50);
            lblFname.TabIndex = 0;
            lblFname.Text = "First Name";
            lblFname.Click += label1_Click;
            // 
            // SeatSelectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1170, 739);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "SeatSelectionForm";
            Text = "SeatSelectionForm";
            FormClosing += SeatSelection_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private Panel panel2;
        private Label lblFname;
        private Label lblBday;
        private Label lblLastName;
        private Label lblPhone;
        private Label lblBookedSeat;
        private Button button1;
    }
}