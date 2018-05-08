namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    partial class WebConfigEncryptForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebConfigEncryptForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.configFilePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sectionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isEncryptedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Encryption = new System.Windows.Forms.DataGridViewButtonColumn();
            this.configSectionListBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.configSectionListBindingSource1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 490);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.configFilePathDataGridViewTextBoxColumn,
            this.sectionNameDataGridViewTextBoxColumn,
            this.isEncryptedDataGridViewCheckBoxColumn,
            this.Encryption});
            this.dataGridView1.DataSource = this.configSectionListBindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(818, 466);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // configFilePathDataGridViewTextBoxColumn
            // 
            this.configFilePathDataGridViewTextBoxColumn.DataPropertyName = "ConfigFilePath";
            this.configFilePathDataGridViewTextBoxColumn.HeaderText = "Config File Path";
            this.configFilePathDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.configFilePathDataGridViewTextBoxColumn.Name = "configFilePathDataGridViewTextBoxColumn";
            this.configFilePathDataGridViewTextBoxColumn.ReadOnly = true;
            this.configFilePathDataGridViewTextBoxColumn.Width = 400;
            // 
            // sectionNameDataGridViewTextBoxColumn
            // 
            this.sectionNameDataGridViewTextBoxColumn.DataPropertyName = "SectionName";
            this.sectionNameDataGridViewTextBoxColumn.HeaderText = "Section Name";
            this.sectionNameDataGridViewTextBoxColumn.MinimumWidth = 75;
            this.sectionNameDataGridViewTextBoxColumn.Name = "sectionNameDataGridViewTextBoxColumn";
            this.sectionNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.sectionNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // isEncryptedDataGridViewCheckBoxColumn
            // 
            this.isEncryptedDataGridViewCheckBoxColumn.DataPropertyName = "IsEncrypted";
            this.isEncryptedDataGridViewCheckBoxColumn.HeaderText = "Encrypted?";
            this.isEncryptedDataGridViewCheckBoxColumn.Name = "isEncryptedDataGridViewCheckBoxColumn";
            this.isEncryptedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isEncryptedDataGridViewCheckBoxColumn.Width = 75;
            // 
            // Encryption
            // 
            this.Encryption.DataPropertyName = "ToggleButtonText";
            this.Encryption.HeaderText = "Toggle Encryption";
            this.Encryption.Name = "Encryption";
            this.Encryption.ReadOnly = true;
            this.Encryption.Width = 150;
            // 
            // configSectionListBindingSource1
            // 
            this.configSectionListBindingSource1.DataSource = typeof(edu.cwru.weatherhead.WebConfigEncrypt.ConfigSectionList);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.cancelScanToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(818, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // cancelScanToolStripMenuItem
            // 
            this.cancelScanToolStripMenuItem.Name = "cancelScanToolStripMenuItem";
            this.cancelScanToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.cancelScanToolStripMenuItem.Text = "Cancel Scan";
            this.cancelScanToolStripMenuItem.Click += new System.EventHandler(this.cancelScanToolStripMenuItem_Click);
            // 
            // WebConfigEncryptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 512);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WebConfigEncryptForm";
            this.Text = "Web.config Encrypt/Decrypt";
            this.Shown += new System.EventHandler(this.WebConfigEncryptForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.configSectionListBindingSource1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource configSectionListBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn configFilePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isEncryptedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn Encryption;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelScanToolStripMenuItem;
    }
}