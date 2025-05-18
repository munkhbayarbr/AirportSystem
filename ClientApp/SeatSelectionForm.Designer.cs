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
            lblPassport = new Label();
            txtPassport = new TextBox();
            btnSearch = new Button();
            panelSeats = new Panel();
            printDocument = new System.Drawing.Printing.PrintDocument();
            SuspendLayout();
            // 
            // lblPassport
            // 
            lblPassport.AutoSize = true;
            lblPassport.Location = new Point(16, 12);
            lblPassport.Name = "lblPassport";
            lblPassport.Size = new Size(66, 15);
            lblPassport.TabIndex = 0;
            lblPassport.Text = "Passport ID";
            // 
            // txtPassport
            // 
            txtPassport.Location = new Point(108, 12);
            txtPassport.Name = "txtPassport";
            txtPassport.Size = new Size(120, 23);
            txtPassport.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(263, 12);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += BtnSearch_Click;
            // 
            // panelSeats
            // 
            panelSeats.AutoScroll = true;
            panelSeats.BorderStyle = BorderStyle.FixedSingle;
            panelSeats.Dock = DockStyle.Right;
            panelSeats.Location = new Point(458, 0);
            panelSeats.Name = "panelSeats";
            panelSeats.Size = new Size(342, 450);
            panelSeats.TabIndex = 3;
            // 
            // SeatSelectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelSeats);
            Controls.Add(btnSearch);
            Controls.Add(txtPassport);
            Controls.Add(lblPassport);
            Name = "SeatSelectionForm";
            Text = "SeatSelectionForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPassport;
        private TextBox txtPassport;
        private Button btnSearch;
        private Panel panelSeats;
        private System.Drawing.Printing.PrintDocument printDocument;
    }
}