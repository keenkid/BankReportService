namespace MonitorConfiguration
{
    partial class FrmConfig
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfig));
            this.gbWatchDetail = new System.Windows.Forms.GroupBox();
            this.panelBackup = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBackupDirectory = new System.Windows.Forms.TextBox();
            this.btnOpenBackup = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbProcessClass = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbAction = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenWatch = new System.Windows.Forms.Button();
            this.txtWatchDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvWatch = new System.Windows.Forms.DataGridView();
            this.WatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WatchPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcessType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BackupPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbWatchDetail.SuspendLayout();
            this.panelBackup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWatch)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWatchDetail
            // 
            this.gbWatchDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWatchDetail.Controls.Add(this.panelBackup);
            this.gbWatchDetail.Controls.Add(this.btnSave);
            this.gbWatchDetail.Controls.Add(this.cmbProcessClass);
            this.gbWatchDetail.Controls.Add(this.label5);
            this.gbWatchDetail.Controls.Add(this.cmbAction);
            this.gbWatchDetail.Controls.Add(this.label3);
            this.gbWatchDetail.Controls.Add(this.btnOpenWatch);
            this.gbWatchDetail.Controls.Add(this.txtWatchDirectory);
            this.gbWatchDetail.Controls.Add(this.label2);
            this.gbWatchDetail.Controls.Add(this.txtName);
            this.gbWatchDetail.Controls.Add(this.label1);
            this.gbWatchDetail.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbWatchDetail.Location = new System.Drawing.Point(13, 13);
            this.gbWatchDetail.Name = "gbWatchDetail";
            this.gbWatchDetail.Size = new System.Drawing.Size(759, 127);
            this.gbWatchDetail.TabIndex = 0;
            this.gbWatchDetail.TabStop = false;
            this.gbWatchDetail.Text = "Watch Detail";
            // 
            // panelBackup
            // 
            this.panelBackup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBackup.Controls.Add(this.label4);
            this.panelBackup.Controls.Add(this.txtBackupDirectory);
            this.panelBackup.Controls.Add(this.btnOpenBackup);
            this.panelBackup.Location = new System.Drawing.Point(262, 59);
            this.panelBackup.Name = "panelBackup";
            this.panelBackup.Size = new System.Drawing.Size(497, 30);
            this.panelBackup.TabIndex = 14;
            this.panelBackup.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(5, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Backup Path";
            // 
            // txtBackupDirectory
            // 
            this.txtBackupDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupDirectory.Location = new System.Drawing.Point(91, 4);
            this.txtBackupDirectory.MaxLength = 512;
            this.txtBackupDirectory.Name = "txtBackupDirectory";
            this.txtBackupDirectory.Size = new System.Drawing.Size(363, 21);
            this.txtBackupDirectory.TabIndex = 5;
            this.txtBackupDirectory.WordWrap = false;
            // 
            // btnOpenBackup
            // 
            this.btnOpenBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenBackup.Location = new System.Drawing.Point(460, 2);
            this.btnOpenBackup.Name = "btnOpenBackup";
            this.btnOpenBackup.Size = new System.Drawing.Size(31, 23);
            this.btnOpenBackup.TabIndex = 6;
            this.btnOpenBackup.Tag = "2";
            this.btnOpenBackup.Text = "...";
            this.btnOpenBackup.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(636, 97);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbProcessClass
            // 
            this.cmbProcessClass.DropDownHeight = 100;
            this.cmbProcessClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcessClass.DropDownWidth = 255;
            this.cmbProcessClass.FormattingEnabled = true;
            this.cmbProcessClass.IntegralHeight = false;
            this.cmbProcessClass.Location = new System.Drawing.Point(100, 100);
            this.cmbProcessClass.Name = "cmbProcessClass";
            this.cmbProcessClass.Size = new System.Drawing.Size(255, 20);
            this.cmbProcessClass.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(6, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "Process";
            // 
            // cmbAction
            // 
            this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Location = new System.Drawing.Point(100, 63);
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.Size = new System.Drawing.Size(140, 20);
            this.cmbAction.TabIndex = 4;
            this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.cmbAction_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Next Action";
            // 
            // btnOpenWatch
            // 
            this.btnOpenWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenWatch.Location = new System.Drawing.Point(722, 24);
            this.btnOpenWatch.Name = "btnOpenWatch";
            this.btnOpenWatch.Size = new System.Drawing.Size(31, 23);
            this.btnOpenWatch.TabIndex = 3;
            this.btnOpenWatch.Tag = "1";
            this.btnOpenWatch.Text = "...";
            this.btnOpenWatch.UseVisualStyleBackColor = true;
            // 
            // txtWatchDirectory
            // 
            this.txtWatchDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWatchDirectory.Location = new System.Drawing.Point(353, 26);
            this.txtWatchDirectory.MaxLength = 512;
            this.txtWatchDirectory.Name = "txtWatchDirectory";
            this.txtWatchDirectory.Size = new System.Drawing.Size(363, 21);
            this.txtWatchDirectory.TabIndex = 2;
            this.txtWatchDirectory.WordWrap = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(267, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Watch Path";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(100, 26);
            this.txtName.MaxLength = 120;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(140, 21);
            this.txtName.TabIndex = 1;
            this.txtName.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Watcher Name";
            // 
            // dgvWatch
            // 
            this.dgvWatch.AllowUserToAddRows = false;
            this.dgvWatch.AllowUserToDeleteRows = false;
            this.dgvWatch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LemonChiffon;
            this.dgvWatch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvWatch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWatch.ColumnHeadersHeight = 25;
            this.dgvWatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvWatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WatchName,
            this.WatchPath,
            this.ProcessType,
            this.NextAction,
            this.BackupPath});
            this.dgvWatch.ContextMenuStrip = this.contextMenuStrip;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvWatch.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvWatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWatch.GridColor = System.Drawing.SystemColors.Highlight;
            this.dgvWatch.Location = new System.Drawing.Point(0, 0);
            this.dgvWatch.MultiSelect = false;
            this.dgvWatch.Name = "dgvWatch";
            this.dgvWatch.ReadOnly = true;
            this.dgvWatch.RowHeadersVisible = false;
            this.dgvWatch.RowHeadersWidth = 20;
            this.dgvWatch.RowTemplate.Height = 23;
            this.dgvWatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWatch.Size = new System.Drawing.Size(759, 403);
            this.dgvWatch.TabIndex = 1;
            this.dgvWatch.TabStop = false;
            this.dgvWatch.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvWatch_CellMouseDown);
            // 
            // WatchName
            // 
            this.WatchName.DataPropertyName = "Name";
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.WatchName.DefaultCellStyle = dataGridViewCellStyle2;
            this.WatchName.FillWeight = 10F;
            this.WatchName.HeaderText = "Name";
            this.WatchName.Name = "WatchName";
            this.WatchName.ReadOnly = true;
            // 
            // WatchPath
            // 
            this.WatchPath.DataPropertyName = "WatchPath";
            this.WatchPath.FillWeight = 30F;
            this.WatchPath.HeaderText = "Watch Path";
            this.WatchPath.Name = "WatchPath";
            this.WatchPath.ReadOnly = true;
            // 
            // ProcessType
            // 
            this.ProcessType.DataPropertyName = "ProcessType";
            this.ProcessType.FillWeight = 15F;
            this.ProcessType.HeaderText = "Process Type";
            this.ProcessType.Name = "ProcessType";
            this.ProcessType.ReadOnly = true;
            // 
            // NextAction
            // 
            this.NextAction.DataPropertyName = "NextAction";
            this.NextAction.FillWeight = 15F;
            this.NextAction.HeaderText = "Next Action";
            this.NextAction.Name = "NextAction";
            this.NextAction.ReadOnly = true;
            // 
            // BackupPath
            // 
            this.BackupPath.DataPropertyName = "BackupPath";
            this.BackupPath.FillWeight = 30F;
            this.BackupPath.HeaderText = "Backup Path";
            this.BackupPath.Name = "BackupPath";
            this.BackupPath.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuUpdate,
            this.menuDelete});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // menuUpdate
            // 
            this.menuUpdate.Image = ((System.Drawing.Image)(resources.GetObject("menuUpdate.Image")));
            this.menuUpdate.Name = "menuUpdate";
            this.menuUpdate.Size = new System.Drawing.Size(152, 22);
            this.menuUpdate.Text = "Update";
            this.menuUpdate.Click += new System.EventHandler(this.menuUpdate_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Image = ((System.Drawing.Image)(resources.GetObject("menuDelete.Image")));
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(152, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dgvWatch);
            this.panel1.Location = new System.Drawing.Point(13, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 403);
            this.panel1.TabIndex = 2;
            // 
            // FrmConfig
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbWatchDetail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.gbWatchDetail.ResumeLayout(false);
            this.gbWatchDetail.PerformLayout();
            this.panelBackup.ResumeLayout(false);
            this.panelBackup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWatch)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbWatchDetail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtWatchDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOpenWatch;
        private System.Windows.Forms.Button btnOpenBackup;
        private System.Windows.Forms.TextBox txtBackupDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbProcessClass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbAction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvWatch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Panel panelBackup;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem menuUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn WatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn WatchPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessType;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn BackupPath;
    }
}

