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
            tableLayoutPanel2 = new TableLayoutPanel();
            panel1 = new Panel();
            txtPassport = new TextBox();
            button1 = new Button();
            btnSearch = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel2 = new Panel();
            label1 = new Label();
            lblBookedSeat = new Label();
            lblPhone = new Label();
            lblBday = new Label();
            lblName = new Label();
            panelSeats = new Panel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1024, 554);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel1, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 17.6470585F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 82.35294F));
            tableLayoutPanel2.Size = new Size(1018, 548);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.MenuHighlight;
            panel1.Controls.Add(txtPassport);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnSearch);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1012, 90);
            panel1.TabIndex = 0;
            // 
            // txtPassport
            // 
            txtPassport.Location = new Point(78, 32);
            txtPassport.MinimumSize = new Size(250, 40);
            txtPassport.Multiline = true;
            txtPassport.Name = "txtPassport";
            txtPassport.PlaceholderText = "Passport No";
            txtPassport.Size = new Size(250, 40);
            txtPassport.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(929, 19);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(77, 46);
            button1.TabIndex = 3;
            button1.Text = "Dashboard";
            button1.UseVisualStyleBackColor = true;
            button1.Click += click_Dashboard;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.Turquoise;
            btnSearch.Location = new Point(346, 32);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(46, 40);
            btnSearch.TabIndex = 0;
            btnSearch.Text = "Хайх";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel3.Controls.Add(panel2, 0, 0);
            tableLayoutPanel3.Controls.Add(panelSeats, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 99);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1012, 446);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Controls.Add(lblBookedSeat);
            panel2.Controls.Add(lblPhone);
            panel2.Controls.Add(lblBday);
            panel2.Controls.Add(lblName);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(626, 440);
            panel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(188, 21);
            label1.TabIndex = 11;
            label1.Text = "Зорчигчийн мэдээлэл:";
            // 
            // lblBookedSeat
            // 
            lblBookedSeat.AutoSize = true;
            lblBookedSeat.BackColor = SystemColors.ButtonFace;
            lblBookedSeat.Font = new Font("Segoe UI", 16F);
            lblBookedSeat.Location = new Point(40, 162);
            lblBookedSeat.MinimumSize = new Size(262, 38);
            lblBookedSeat.Name = "lblBookedSeat";
            lblBookedSeat.Size = new Size(262, 38);
            lblBookedSeat.TabIndex = 10;
            lblBookedSeat.Text = "Seat";
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.BackColor = SystemColors.ButtonFace;
            lblPhone.Font = new Font("Segoe UI", 16F);
            lblPhone.Location = new Point(40, 124);
            lblPhone.MinimumSize = new Size(262, 38);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(262, 38);
            lblPhone.TabIndex = 9;
            lblPhone.Text = "Phone";
            // 
            // lblBday
            // 
            lblBday.AutoSize = true;
            lblBday.BackColor = SystemColors.ButtonFace;
            lblBday.Font = new Font("Segoe UI", 16F);
            lblBday.Location = new Point(40, 86);
            lblBday.MinimumSize = new Size(262, 38);
            lblBday.Name = "lblBday";
            lblBday.Size = new Size(262, 38);
            lblBday.TabIndex = 8;
            lblBday.Text = "Birthday";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.BackColor = SystemColors.ButtonFace;
            lblName.Font = new Font("Segoe UI", 16F);
            lblName.Location = new Point(40, 48);
            lblName.MinimumSize = new Size(262, 38);
            lblName.Name = "lblName";
            lblName.Size = new Size(262, 38);
            lblName.TabIndex = 6;
            lblName.Text = "First Name";
            // 
            // panelSeats
            // 
            panelSeats.Dock = DockStyle.Fill;
            panelSeats.Location = new Point(635, 3);
            panelSeats.Name = "panelSeats";
            panelSeats.Size = new Size(374, 440);
            panelSeats.TabIndex = 1;
            // 
            // SeatSelectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 554);
            Controls.Add(tableLayoutPanel1);
            Name = "SeatSelectionForm";
            Text = "SeatSelectionForm";
            FormClosing += SeatSelection_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Drawing.Printing.PrintDocument printDocument;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private TextBox txtPassport;
        private Button btnSearch;
        private Button button1;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel2;
        private Label label1;
        private Label lblBookedSeat;
        private Label lblPhone;
        private Label lblBday;
        private Label lblLastName;
        private Label lblName;
        private Panel panelSeats;
    }
}