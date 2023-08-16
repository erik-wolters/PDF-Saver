namespace PDFSaverTestUI
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
            listView1 = new ListView();
            chPath = new ColumnHeader();
            chName = new ColumnHeader();
            btnTest = new Button();
            txtLog = new TextBox();
            txtOutputDir = new TextBox();
            btnBrowse = new Button();
            btnAddFile = new Button();
            btnRemoveFile = new Button();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.AllowDrop = true;
            listView1.Columns.AddRange(new ColumnHeader[] { chPath, chName });
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.Location = new Point(12, 70);
            listView1.Name = "listView1";
            listView1.Size = new Size(704, 212);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // chPath
            // 
            chPath.Text = "Bestandslocatie";
            chPath.Width = 500;
            // 
            // chName
            // 
            chName.Text = "Bestandsnaam";
            chName.Width = 200;
            // 
            // btnTest
            // 
            btnTest.Location = new Point(641, 515);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(75, 23);
            btnTest.TabIndex = 2;
            btnTest.Text = "Start";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 305);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(704, 198);
            txtLog.TabIndex = 3;
            // 
            // txtOutputDir
            // 
            txtOutputDir.Location = new Point(12, 41);
            txtOutputDir.Name = "txtOutputDir";
            txtOutputDir.PlaceholderText = "Uitvoermap";
            txtOutputDir.ReadOnly = true;
            txtOutputDir.Size = new Size(640, 23);
            txtOutputDir.TabIndex = 4;
            txtOutputDir.TextChanged += txtOutputDir_TextChanged;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(658, 41);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(58, 23);
            btnBrowse.TabIndex = 5;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnAddFile
            // 
            btnAddFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAddFile.ForeColor = Color.DarkGreen;
            btnAddFile.Location = new Point(12, 12);
            btnAddFile.Name = "btnAddFile";
            btnAddFile.Size = new Size(31, 23);
            btnAddFile.TabIndex = 6;
            btnAddFile.Text = "+";
            btnAddFile.UseVisualStyleBackColor = true;
            btnAddFile.Click += btnAddFile_Click;
            // 
            // btnRemoveFile
            // 
            btnRemoveFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnRemoveFile.ForeColor = Color.Red;
            btnRemoveFile.Location = new Point(49, 12);
            btnRemoveFile.Name = "btnRemoveFile";
            btnRemoveFile.Size = new Size(31, 23);
            btnRemoveFile.TabIndex = 7;
            btnRemoveFile.Text = "-";
            btnRemoveFile.UseVisualStyleBackColor = true;
            btnRemoveFile.Click += btnRemoveFile_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(728, 550);
            Controls.Add(btnRemoveFile);
            Controls.Add(btnAddFile);
            Controls.Add(btnBrowse);
            Controls.Add(txtOutputDir);
            Controls.Add(txtLog);
            Controls.Add(btnTest);
            Controls.Add(listView1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView1;
        private ColumnHeader chPath;
        private ColumnHeader chName;
        private Button btnTest;
        private TextBox txtLog;
        private TextBox txtOutputDir;
        private Button btnBrowse;
        private Button btnAddFile;
        private Button btnRemoveFile;
    }
}