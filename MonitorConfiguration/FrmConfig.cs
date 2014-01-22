using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using BankReportMonitor;
using BankReport.Common;
using System.Xml.Linq;
using System.Threading;

namespace MonitorConfiguration
{
    public partial class FrmConfig : Form
    {
        //private SynchronizationContext synContext;

        private const string MONITOR_CONFIG_FILENAME = "BankReportMonitor.dll.config";

        private List<DropDownListData> processTypes = null;

        private string assemblyPath = string.Empty;

        private MonitorItemManager configFileManager = null;

        private ActionStatus formStatus = ActionStatus.New;

        private MonitorData beforeUpdateData = null;

        public FrmConfig()
        {
            //synContext = SynchronizationContext.Current;

            InitializeComponent();

            this.btnOpenWatch.Click += OpenFolder;
            this.btnOpenBackup.Click += OpenFolder;

            InitializeMember();
        }

        public void InitializeMember()
        {
#if DEBUG
            assemblyPath = @"C:\D\VS2010Projects\BankReportService\BankReportTest\bin\Debug\";
#else
            assemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
#endif
            string monitorConfigFilePath = Path.Combine(assemblyPath, MONITOR_CONFIG_FILENAME);

            if (File.Exists(monitorConfigFilePath))
            {
                configFileManager = new MonitorItemManager(monitorConfigFilePath);
            }
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            ThreadStart start = InitializeFormStatus;
            start.BeginInvoke(null, null);

            //InitializeFormStatus();
        }

        private void InitializeFormStatus()
        {
            BindAction();

            BindProcess();

            BindMonitorItem();
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.cmbAction.SelectedValue as string) == AfterProcessAction.Backup.ToString())
            {
                this.panelBackup.Visible = true;
            }
            else
            {
                this.panelBackup.Visible = false;
            }
        }

        private void OpenFolder(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog.ShowDialog(this))
            {
                if (((sender as Button).Tag as string) == "1")
                {
                    this.txtWatchDirectory.Text = this.folderBrowserDialog.SelectedPath;
                }
                else if (((sender as Button).Tag as string) == "2")
                {
                    this.txtBackupDirectory.Text = this.folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void BindAction()
        {
            List<DropDownListData> datas = new List<DropDownListData>()
            {
                new DropDownListData(){DisplayMember=Enum.GetName(typeof(AfterProcessAction),AfterProcessAction.None),ValueMember=AfterProcessAction.None.ToString()},
                new DropDownListData(){DisplayMember=Enum.GetName(typeof(AfterProcessAction),AfterProcessAction.Delete),ValueMember=AfterProcessAction.Delete.ToString()},
                new DropDownListData(){DisplayMember=Enum.GetName(typeof(AfterProcessAction),AfterProcessAction.Backup),ValueMember=AfterProcessAction.Backup.ToString()}
            };

            //synContext.Send(new SendOrPostCallback((obj) =>
            //    {
            //        this.cmbAction.DisplayMember = "DisplayMember";
            //        this.cmbAction.ValueMember = "ValueMemeber";
            //        this.cmbAction.DataSource = datas;
            //        this.cmbAction.SelectedValue = AfterProcessAction.Backup.ToString();
            //    }), null);

            this.cmbAction.UIThread(() =>
                {
                    this.cmbAction.DisplayMember = DropDownListData.DISPLAYMEMBER_NAME;
                    this.cmbAction.ValueMember = DropDownListData.VALUEMEMBER_NAME;
                    this.cmbAction.DataSource = datas;
                    this.cmbAction.SelectedValue = AfterProcessAction.Backup.ToString();
                });
        }

        private void BindProcess()
        {
            if (null == processTypes)
            {
                string[] assemblyFiles = Directory.GetFiles(Path.Combine(assemblyPath, "BankReport"), "*.dll", SearchOption.TopDirectoryOnly);

                processTypes = new List<DropDownListData>();

                processTypes.Add(new DropDownListData() { ValueMember = string.Empty, DisplayMember = "Please Select" });

                assemblyFiles.ToList().ForEach(file =>
                    {
                        Assembly.LoadFrom(file).GetTypes().ToList().ForEach(tp =>
                        {
                            if ((!tp.IsAbstract) && (null != tp.GetInterface(typeof(IProcess).FullName)))
                            {
                                processTypes.Add(new DropDownListData() { DisplayMember = tp.GetDescription(), ValueMember = tp.AssemblyQualifiedName });
                            }
                        });
                    });
            }
            this.cmbProcessClass.UIThread(() =>
                {
                    this.cmbProcessClass.DisplayMember = DropDownListData.DISPLAYMEMBER_NAME;
                    this.cmbProcessClass.ValueMember = DropDownListData.VALUEMEMBER_NAME;
                    this.cmbProcessClass.DataSource = processTypes;
                });
        }

        private void BindMonitorItem()
        {
            var datas = configFileManager.GetMonitorItems();

            foreach (var data in datas)
            {
                foreach (var pt in processTypes)
                {
                    if (string.Equals(pt.ValueMember.ContentTrim(), data.ProcessType.ContentTrim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        data.ProcessType = pt.DisplayMember;
                        break;
                    }
                }
            }

            this.dgvWatch.UIThread(() =>
                {
                    this.dgvWatch.DataSource = datas;
                    this.dgvWatch.ClearSelection();
                });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (formStatus == ActionStatus.New)
            {
                if (CheckInputComplete() && CheckInputCorrect())
                {
                    configFileManager.AddMonitorItem(CollectWatchDetailData());
                    BindMonitorItem();
                    EmptyWatchDetail();
                    formStatus = ActionStatus.New;
                }
            }
            else if (formStatus == ActionStatus.Update)
            {
                if (CheckInputComplete())
                {
                    MonitorData nowData = CollectWatchDetailData();

                    configFileManager.DeleteMonitorItem(beforeUpdateData.Name);
                    configFileManager.AddMonitorItem(nowData);
                    BindMonitorItem();
                    EmptyWatchDetail();
                    formStatus = ActionStatus.New;
                }
            }
        }

        private void EmptyWatchDetail()
        {
            this.txtName.Text = string.Empty;
            this.txtWatchDirectory.Text = string.Empty;
            this.cmbAction.SelectedValue = AfterProcessAction.Backup.ToString();
            this.txtBackupDirectory.Text = string.Empty;
            this.cmbProcessClass.SelectedIndex = 0;
        }

        private bool CheckInputComplete()
        {
            if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                MessageBox.Show(this, "Please input the watcher name", "Warning", MessageBoxButtons.OK);
                this.txtName.Focus();
                return false;
            }
            if (!Directory.Exists(this.txtWatchDirectory.Text.Trim()))
            {
                MessageBox.Show(this, "Please input an existing folder path", "Warning", MessageBoxButtons.OK);
                this.txtWatchDirectory.Focus();
                return false;
            }
            if (this.panelBackup.Visible && !Directory.Exists(this.txtBackupDirectory.Text.Trim()))
            {
                MessageBox.Show(this, "Please input an existing folder path", "Warning", MessageBoxButtons.OK);
                this.txtBackupDirectory.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbAction.SelectedValue as string))
            {
                MessageBox.Show(this, "Please select the process class", "Warning", MessageBoxButtons.OK);
                this.cmbProcessClass.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbProcessClass.SelectedValue.ToString()))
            {
                MessageBox.Show(this, "Please select the process class", "Warning", MessageBoxButtons.OK);
                this.cmbProcessClass.Focus();
                return false;
            }
            if (this.panelBackup.Visible && string.Equals(this.txtWatchDirectory.Text.Trim(), this.txtBackupDirectory.Text.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(this, "Watch path and backup path should be difference.", "Warning", MessageBoxButtons.OK);
                this.txtBackupDirectory.Focus();
                return false;
            }
            return true;
        }

        private bool CheckInputCorrect()
        {
            if (configFileManager.CheckMonitorItemExistByName(this.txtName.Text.Trim()))
            {
                MessageBox.Show(this, "This watcher name already exist.", "Warning", MessageBoxButtons.OK);
                this.txtName.Focus();
                return false;
            }
            if (configFileManager.CheckMonitorItemExistByPath(this.txtWatchDirectory.Text.Trim()))
            {
                MessageBox.Show(this, "This path already under watch.", "Warning", MessageBoxButtons.OK);
                this.txtWatchDirectory.Focus();
                return false;
            }
            return true;
        }

        private MonitorData CollectWatchDetailData()
        {
            MonitorData data = new MonitorData();

            data.Name = this.txtName.Text.Trim();
            data.WatchPath = this.txtWatchDirectory.Text.Trim();
            data.NextAction = this.cmbAction.SelectedValue as string;
            if (data.NextAction == AfterProcessAction.Backup.ToString())
            {
                data.BackupPath = this.txtBackupDirectory.Text.Trim();
            }
            else
            {
                data.BackupPath = string.Empty;
            }
            data.ProcessType = this.cmbProcessClass.SelectedValue as string;

            return data;
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            var rows = this.dgvWatch.SelectedRows;
            if (0 == rows.Count)
            {
                return;
            }
            if (DialogResult.OK == MessageBox.Show(this, "Are you intend to delete it?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
            {
                try
                {
                    var watchName = rows[0].Cells[0].Value as string;
                    configFileManager.DeleteMonitorItem(watchName);
                    BindMonitorItem();
                    EmptyWatchDetail();
                    formStatus = ActionStatus.New;
                }
                catch
                {
                    MessageBox.Show(this, "Delete configuration item error, please try it again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dgvWatch_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.dgvWatch.ClearSelection();
                this.dgvWatch.Rows[e.RowIndex].Selected = true;
            }
        }

        private void menuUpdate_Click(object sender, EventArgs e)
        {
            if (this.dgvWatch.SelectedRows.Count == 0)
            {
                return;
            }
            var cells = this.dgvWatch.SelectedRows[0].Cells;
            this.txtName.Text = cells[0].Value as string;
            this.txtWatchDirectory.Text = cells[1].Value as string;

            string action = cells[2].Value as string;
            this.cmbAction.SelectedValue = action;
            if (action == AfterProcessAction.Backup.ToString())
            {
                this.txtBackupDirectory.Text = cells[3].Value as string;
                this.panelBackup.Visible = true;
            }
            else
            {
                this.panelBackup.Visible = false;
            }
            this.cmbProcessClass.Text = cells[4].Value as string;

            beforeUpdateData = CollectWatchDetailData();
            formStatus = ActionStatus.Update;
        }
    }
}
