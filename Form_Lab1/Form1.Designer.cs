namespace Form_Lab1
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
            this.loadDatabase = new System.Windows.Forms.Button();
            this.insertButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.updateButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.currentVersion = new System.Windows.Forms.Label();
            this.yearLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableDropDown = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // loadDatabase
            // 
            this.loadDatabase.Location = new System.Drawing.Point(26, 12);
            this.loadDatabase.Name = "loadDatabase";
            this.loadDatabase.Size = new System.Drawing.Size(103, 23);
            this.loadDatabase.TabIndex = 0;
            this.loadDatabase.Text = "Load Database";
            this.loadDatabase.UseVisualStyleBackColor = true;
            this.loadDatabase.Click += new System.EventHandler(this.loadDatabaseClick);
            // 
            // insertButton
            // 
            this.insertButton.Location = new System.Drawing.Point(682, 337);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(184, 71);
            this.insertButton.TabIndex = 1;
            this.insertButton.Text = "Insert";
            this.insertButton.UseVisualStyleBackColor = true;
            this.insertButton.Click += new System.EventHandler(this.insertClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(26, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(500, 220);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(573, 44);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(500, 220);
            this.dataGridView2.TabIndex = 3;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(892, 338);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 4;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateClick);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(892, 385);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteClick);
            // 
            // currentVersion
            // 
            this.currentVersion.AutoSize = true;
            this.currentVersion.Location = new System.Drawing.Point(1058, 489);
            this.currentVersion.Name = "currentVersion";
            this.currentVersion.Size = new System.Drawing.Size(0, 13);
            this.currentVersion.TabIndex = 17;
            // 
            // yearLabel
            // 
            this.yearLabel.AutoSize = true;
            this.yearLabel.Location = new System.Drawing.Point(14, 489);
            this.yearLabel.Name = "yearLabel";
            this.yearLabel.Size = new System.Drawing.Size(0, 13);
            this.yearLabel.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(41, 302);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 157);
            this.panel1.TabIndex = 18;
            // 
            // tableDropDown
            // 
            this.tableDropDown.FormattingEnabled = true;
            this.tableDropDown.Location = new System.Drawing.Point(156, 12);
            this.tableDropDown.Name = "tableDropDown";
            this.tableDropDown.Size = new System.Drawing.Size(165, 21);
            this.tableDropDown.TabIndex = 19;
            this.tableDropDown.SelectedIndexChanged += new System.EventHandler(this.tableDropDown_SelectTable);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(394, 324);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(253, 95);
            this.listBox1.TabIndex = 20;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 511);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.tableDropDown);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.currentVersion);
            this.Controls.Add(this.yearLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.loadDatabase);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1120, 550);
            this.MinimumSize = new System.Drawing.Size(1120, 550);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadDatabase;
        private System.Windows.Forms.Button insertButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label currentVersion;
        private System.Windows.Forms.Label yearLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox tableDropDown;
        private System.Windows.Forms.ListBox listBox1;
    }
}

