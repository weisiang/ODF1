namespace UI
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cv_tcBar = new MaterialSkin.Controls.MaterialTabControl();
            this.cv_tpLayout = new System.Windows.Forms.TabPage();
            this.cv_tpAlarm = new System.Windows.Forms.TabPage();
            this.cv_AlarmDataView = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pan_alarm = new System.Windows.Forms.Panel();
            this.btn_resetAllAlarm = new System.Windows.Forms.Button();
            this.btn_buzzerOff = new System.Windows.Forms.Button();
            this.cv_tpRecipe = new System.Windows.Forms.TabPage();
            this.cv_tpManual = new System.Windows.Forms.TabPage();
            this.gpb_AlignerAction = new System.Windows.Forms.GroupBox();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.bt_AlignerExe = new System.Windows.Forms.Button();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.gpb_PortAction = new System.Windows.Forms.GroupBox();
            this.cb_PortActionName = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.bt_PortExe = new System.Windows.Forms.Button();
            this.cb_PortActionPortId = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.gpb_RobotAction = new System.Windows.Forms.GroupBox();
            this.cb_RobotActionArm = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cb_RobotActionTargetId = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cb_RobotActionName = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bt_RobotExe = new System.Windows.Forms.Button();
            this.cb_RobotActionTargetSlot = new System.Windows.Forms.ComboBox();
            this.cb_RobotActionTarget = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_RobotActionRobotId = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cv_tpLog = new System.Windows.Forms.TabPage();
            this.cv_tpIo = new System.Windows.Forms.TabPage();
            this.cv_dataViewIo = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_tpAccount = new System.Windows.Forms.TabPage();
            this.cv_dataViewAccount = new System.Windows.Forms.DataGridView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.lbl_time = new System.Windows.Forms.Label();
            this.pan_module = new System.Windows.Forms.Panel();
            this.lbl_date = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.lbl_warningStatus = new MaterialSkin.Controls.MaterialLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel16 = new System.Windows.Forms.Panel();
            this.lbl_alarmStatus = new MaterialSkin.Controls.MaterialLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel15 = new System.Windows.Forms.Panel();
            this.lbl_hsmsStatus = new MaterialSkin.Controls.MaterialLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.lbl_plcStatus = new MaterialSkin.Controls.MaterialLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.lbl_systemStatus = new MaterialSkin.Controls.MaterialLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.lbl_disSystemMode = new MaterialSkin.Controls.MaterialLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.materialDivider9 = new MaterialSkin.Controls.MaterialDivider();
            this.button2 = new System.Windows.Forms.Button();
            this.materialContextMenuStrip1 = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialDivider8 = new MaterialSkin.Controls.MaterialDivider();
            this.lbl_ReIni = new System.Windows.Forms.Button();
            this.gb_RobotAction = new System.Windows.Forms.GroupBox();
            this.lbl_ActionName = new System.Windows.Forms.Label();
            this.lbl_ActionArm = new System.Windows.Forms.Label();
            this.lbl_ActionTarget = new System.Windows.Forms.Label();
            this.lbl_ActionId = new System.Windows.Forms.Label();
            this.lbl_ActionSlot = new System.Windows.Forms.Label();
            this.materialDivider7 = new MaterialSkin.Controls.MaterialDivider();
            this.btn_ShowManualOut = new System.Windows.Forms.Button();
            this.materialDivider6 = new MaterialSkin.Controls.MaterialDivider();
            this.btn_ShowManualIn = new System.Windows.Forms.Button();
            this.pan_version = new System.Windows.Forms.Panel();
            this.lbl_per = new System.Windows.Forms.Label();
            this.lbl_id = new System.Windows.Forms.Label();
            this.lbl_version = new System.Windows.Forms.Label();
            this.materialDivider4 = new MaterialSkin.Controls.MaterialDivider();
            this.button1 = new System.Windows.Forms.Button();
            this.materialDivider3 = new MaterialSkin.Controls.MaterialDivider();
            this.cv_btnSelectMode = new System.Windows.Forms.Button();
            this.menu_systemMode = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.oFFLINEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNLINELOCALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNLINEREMOTEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialDivider5 = new MaterialSkin.Controls.MaterialDivider();
            this.cv_btnLogout = new System.Windows.Forms.Button();
            this.materialDivider2 = new MaterialSkin.Controls.MaterialDivider();
            this.cv_btnLogin = new System.Windows.Forms.Button();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.panel8 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.cv_tcBar.SuspendLayout();
            this.cv_tpAlarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_AlarmDataView)).BeginInit();
            this.pan_alarm.SuspendLayout();
            this.cv_tpManual.SuspendLayout();
            this.gpb_AlignerAction.SuspendLayout();
            this.gpb_PortAction.SuspendLayout();
            this.gpb_RobotAction.SuspendLayout();
            this.cv_tpIo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_dataViewIo)).BeginInit();
            this.cv_tpAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_dataViewAccount)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel2.SuspendLayout();
            this.materialContextMenuStrip1.SuspendLayout();
            this.gb_RobotAction.SuspendLayout();
            this.pan_version.SuspendLayout();
            this.menu_systemMode.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1356, 946);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(136, 142);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1220, 804);
            this.panel4.TabIndex = 2;
            // 
            // panel7
            // 
            this.panel7.AutoScroll = true;
            this.panel7.Controls.Add(this.cv_tcBar);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 25);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1210, 779);
            this.panel7.TabIndex = 2;
            // 
            // cv_tcBar
            // 
            this.cv_tcBar.Controls.Add(this.cv_tpLayout);
            this.cv_tcBar.Controls.Add(this.cv_tpAlarm);
            this.cv_tcBar.Controls.Add(this.cv_tpRecipe);
            this.cv_tcBar.Controls.Add(this.cv_tpManual);
            this.cv_tcBar.Controls.Add(this.cv_tpLog);
            this.cv_tcBar.Controls.Add(this.cv_tpIo);
            this.cv_tcBar.Controls.Add(this.cv_tpAccount);
            this.cv_tcBar.Depth = 0;
            this.cv_tcBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_tcBar.Location = new System.Drawing.Point(0, 0);
            this.cv_tcBar.MouseState = MaterialSkin.MouseState.HOVER;
            this.cv_tcBar.Name = "cv_tcBar";
            this.cv_tcBar.SelectedIndex = 0;
            this.cv_tcBar.Size = new System.Drawing.Size(1210, 779);
            this.cv_tcBar.TabIndex = 1;
            // 
            // cv_tpLayout
            // 
            this.cv_tpLayout.AutoScroll = true;
            this.cv_tpLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.cv_tpLayout.Location = new System.Drawing.Point(4, 22);
            this.cv_tpLayout.Name = "cv_tpLayout";
            this.cv_tpLayout.Padding = new System.Windows.Forms.Padding(3);
            this.cv_tpLayout.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpLayout.TabIndex = 0;
            this.cv_tpLayout.Text = "Layout";
            // 
            // cv_tpAlarm
            // 
            this.cv_tpAlarm.AutoScroll = true;
            this.cv_tpAlarm.Controls.Add(this.cv_AlarmDataView);
            this.cv_tpAlarm.Controls.Add(this.pan_alarm);
            this.cv_tpAlarm.Location = new System.Drawing.Point(4, 22);
            this.cv_tpAlarm.Name = "cv_tpAlarm";
            this.cv_tpAlarm.Padding = new System.Windows.Forms.Padding(3);
            this.cv_tpAlarm.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpAlarm.TabIndex = 1;
            this.cv_tpAlarm.Text = "Alarm";
            this.cv_tpAlarm.UseVisualStyleBackColor = true;
            // 
            // cv_AlarmDataView
            // 
            this.cv_AlarmDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_AlarmDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.cv_AlarmDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_AlarmDataView.Location = new System.Drawing.Point(3, 3);
            this.cv_AlarmDataView.Name = "cv_AlarmDataView";
            this.cv_AlarmDataView.RowTemplate.Height = 24;
            this.cv_AlarmDataView.Size = new System.Drawing.Size(1196, 703);
            this.cv_AlarmDataView.TabIndex = 0;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.DataPropertyName = "PTime";
            this.Column3.HeaderText = "Time";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.DataPropertyName = "PCode";
            this.Column4.HeaderText = "Code";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.DataPropertyName = "PLevel";
            this.Column5.HeaderText = "Level";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.DataPropertyName = "PEqId";
            this.Column6.HeaderText = "Unit";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column7.DataPropertyName = "PMsg";
            this.Column7.HeaderText = "Description";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // pan_alarm
            // 
            this.pan_alarm.BackColor = System.Drawing.Color.Blue;
            this.pan_alarm.Controls.Add(this.btn_resetAllAlarm);
            this.pan_alarm.Controls.Add(this.btn_buzzerOff);
            this.pan_alarm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_alarm.Location = new System.Drawing.Point(3, 706);
            this.pan_alarm.Name = "pan_alarm";
            this.pan_alarm.Size = new System.Drawing.Size(1196, 44);
            this.pan_alarm.TabIndex = 1;
            // 
            // btn_resetAllAlarm
            // 
            this.btn_resetAllAlarm.Location = new System.Drawing.Point(1054, 6);
            this.btn_resetAllAlarm.Name = "btn_resetAllAlarm";
            this.btn_resetAllAlarm.Size = new System.Drawing.Size(75, 31);
            this.btn_resetAllAlarm.TabIndex = 3;
            this.btn_resetAllAlarm.Text = "Reset";
            this.btn_resetAllAlarm.UseVisualStyleBackColor = true;
            this.btn_resetAllAlarm.Click += new System.EventHandler(this.btn_resetAllAlarm_Click);
            // 
            // btn_buzzerOff
            // 
            this.btn_buzzerOff.Location = new System.Drawing.Point(1150, 6);
            this.btn_buzzerOff.Name = "btn_buzzerOff";
            this.btn_buzzerOff.Size = new System.Drawing.Size(75, 31);
            this.btn_buzzerOff.TabIndex = 0;
            this.btn_buzzerOff.Text = "Buzzer OFF";
            this.btn_buzzerOff.UseVisualStyleBackColor = true;
            this.btn_buzzerOff.Click += new System.EventHandler(this.btn_buzzerOff_Click);
            // 
            // cv_tpRecipe
            // 
            this.cv_tpRecipe.AutoScroll = true;
            this.cv_tpRecipe.Location = new System.Drawing.Point(4, 22);
            this.cv_tpRecipe.Name = "cv_tpRecipe";
            this.cv_tpRecipe.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpRecipe.TabIndex = 2;
            this.cv_tpRecipe.Text = "Recipe";
            this.cv_tpRecipe.UseVisualStyleBackColor = true;
            // 
            // cv_tpManual
            // 
            this.cv_tpManual.AutoScroll = true;
            this.cv_tpManual.Controls.Add(this.gpb_AlignerAction);
            this.cv_tpManual.Controls.Add(this.gpb_PortAction);
            this.cv_tpManual.Controls.Add(this.gpb_RobotAction);
            this.cv_tpManual.Location = new System.Drawing.Point(4, 22);
            this.cv_tpManual.Name = "cv_tpManual";
            this.cv_tpManual.Padding = new System.Windows.Forms.Padding(3);
            this.cv_tpManual.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpManual.TabIndex = 3;
            this.cv_tpManual.Text = "Manual";
            this.cv_tpManual.UseVisualStyleBackColor = true;
            // 
            // gpb_AlignerAction
            // 
            this.gpb_AlignerAction.Controls.Add(this.comboBox9);
            this.gpb_AlignerAction.Controls.Add(this.label15);
            this.gpb_AlignerAction.Controls.Add(this.comboBox6);
            this.gpb_AlignerAction.Controls.Add(this.label12);
            this.gpb_AlignerAction.Controls.Add(this.bt_AlignerExe);
            this.gpb_AlignerAction.Controls.Add(this.comboBox7);
            this.gpb_AlignerAction.Controls.Add(this.label13);
            this.gpb_AlignerAction.Dock = System.Windows.Forms.DockStyle.Left;
            this.gpb_AlignerAction.Location = new System.Drawing.Point(575, 3);
            this.gpb_AlignerAction.Name = "gpb_AlignerAction";
            this.gpb_AlignerAction.Size = new System.Drawing.Size(286, 747);
            this.gpb_AlignerAction.TabIndex = 2;
            this.gpb_AlignerAction.TabStop = false;
            this.gpb_AlignerAction.Text = "Aligner";
            // 
            // comboBox9
            // 
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Location = new System.Drawing.Point(109, 125);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(107, 20);
            this.comboBox9.TabIndex = 10;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(6, 122);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 23);
            this.label15.TabIndex = 9;
            this.label15.Text = "Action";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(109, 86);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(107, 20);
            this.comboBox6.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(6, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 7;
            this.label12.Text = "Degree";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_AlignerExe
            // 
            this.bt_AlignerExe.Location = new System.Drawing.Point(6, 243);
            this.bt_AlignerExe.Name = "bt_AlignerExe";
            this.bt_AlignerExe.Size = new System.Drawing.Size(75, 23);
            this.bt_AlignerExe.TabIndex = 6;
            this.bt_AlignerExe.Text = "Execute";
            this.bt_AlignerExe.UseVisualStyleBackColor = true;
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(109, 49);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(107, 20);
            this.comboBox7.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(6, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 0;
            this.label13.Text = "Aligner";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gpb_PortAction
            // 
            this.gpb_PortAction.Controls.Add(this.cb_PortActionName);
            this.gpb_PortAction.Controls.Add(this.label11);
            this.gpb_PortAction.Controls.Add(this.bt_PortExe);
            this.gpb_PortAction.Controls.Add(this.cb_PortActionPortId);
            this.gpb_PortAction.Controls.Add(this.label14);
            this.gpb_PortAction.Dock = System.Windows.Forms.DockStyle.Left;
            this.gpb_PortAction.Location = new System.Drawing.Point(289, 3);
            this.gpb_PortAction.Name = "gpb_PortAction";
            this.gpb_PortAction.Size = new System.Drawing.Size(286, 747);
            this.gpb_PortAction.TabIndex = 1;
            this.gpb_PortAction.TabStop = false;
            this.gpb_PortAction.Text = "PORT";
            // 
            // cb_PortActionName
            // 
            this.cb_PortActionName.FormattingEnabled = true;
            this.cb_PortActionName.Location = new System.Drawing.Point(109, 86);
            this.cb_PortActionName.Name = "cb_PortActionName";
            this.cb_PortActionName.Size = new System.Drawing.Size(107, 20);
            this.cb_PortActionName.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(6, 83);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 7;
            this.label11.Text = "Action";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_PortExe
            // 
            this.bt_PortExe.Location = new System.Drawing.Point(6, 243);
            this.bt_PortExe.Name = "bt_PortExe";
            this.bt_PortExe.Size = new System.Drawing.Size(75, 23);
            this.bt_PortExe.TabIndex = 6;
            this.bt_PortExe.Text = "Execute";
            this.bt_PortExe.UseVisualStyleBackColor = true;
            this.bt_PortExe.Click += new System.EventHandler(this.bt_PortExe_Click);
            // 
            // cb_PortActionPortId
            // 
            this.cb_PortActionPortId.FormattingEnabled = true;
            this.cb_PortActionPortId.Location = new System.Drawing.Point(109, 49);
            this.cb_PortActionPortId.Name = "cb_PortActionPortId";
            this.cb_PortActionPortId.Size = new System.Drawing.Size(107, 20);
            this.cb_PortActionPortId.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(6, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 23);
            this.label14.TabIndex = 0;
            this.label14.Text = "Port";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gpb_RobotAction
            // 
            this.gpb_RobotAction.BackColor = System.Drawing.Color.Transparent;
            this.gpb_RobotAction.Controls.Add(this.cb_RobotActionArm);
            this.gpb_RobotAction.Controls.Add(this.label17);
            this.gpb_RobotAction.Controls.Add(this.cb_RobotActionTargetId);
            this.gpb_RobotAction.Controls.Add(this.label16);
            this.gpb_RobotAction.Controls.Add(this.cb_RobotActionName);
            this.gpb_RobotAction.Controls.Add(this.label10);
            this.gpb_RobotAction.Controls.Add(this.bt_RobotExe);
            this.gpb_RobotAction.Controls.Add(this.cb_RobotActionTargetSlot);
            this.gpb_RobotAction.Controls.Add(this.cb_RobotActionTarget);
            this.gpb_RobotAction.Controls.Add(this.label9);
            this.gpb_RobotAction.Controls.Add(this.label8);
            this.gpb_RobotAction.Controls.Add(this.cb_RobotActionRobotId);
            this.gpb_RobotAction.Controls.Add(this.label7);
            this.gpb_RobotAction.Dock = System.Windows.Forms.DockStyle.Left;
            this.gpb_RobotAction.Location = new System.Drawing.Point(3, 3);
            this.gpb_RobotAction.Name = "gpb_RobotAction";
            this.gpb_RobotAction.Size = new System.Drawing.Size(286, 747);
            this.gpb_RobotAction.TabIndex = 0;
            this.gpb_RobotAction.TabStop = false;
            this.gpb_RobotAction.Text = "ROBOT";
            // 
            // cb_RobotActionArm
            // 
            this.cb_RobotActionArm.FormattingEnabled = true;
            this.cb_RobotActionArm.Location = new System.Drawing.Point(109, 82);
            this.cb_RobotActionArm.Name = "cb_RobotActionArm";
            this.cb_RobotActionArm.Size = new System.Drawing.Size(107, 20);
            this.cb_RobotActionArm.TabIndex = 12;
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(6, 79);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 23);
            this.label17.TabIndex = 11;
            this.label17.Text = "Arm";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_RobotActionTargetId
            // 
            this.cb_RobotActionTargetId.FormattingEnabled = true;
            this.cb_RobotActionTargetId.Location = new System.Drawing.Point(108, 148);
            this.cb_RobotActionTargetId.Name = "cb_RobotActionTargetId";
            this.cb_RobotActionTargetId.Size = new System.Drawing.Size(107, 20);
            this.cb_RobotActionTargetId.TabIndex = 10;
            this.cb_RobotActionTargetId.SelectedValueChanged += new System.EventHandler(this.cb_RobotActionTargetId_SelectedValueChanged);
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(5, 145);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
            this.label16.TabIndex = 9;
            this.label16.Text = "Id";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_RobotActionName
            // 
            this.cb_RobotActionName.FormattingEnabled = true;
            this.cb_RobotActionName.Location = new System.Drawing.Point(108, 214);
            this.cb_RobotActionName.Name = "cb_RobotActionName";
            this.cb_RobotActionName.Size = new System.Drawing.Size(107, 20);
            this.cb_RobotActionName.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(5, 211);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 23);
            this.label10.TabIndex = 7;
            this.label10.Text = "Action";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_RobotExe
            // 
            this.bt_RobotExe.Location = new System.Drawing.Point(6, 243);
            this.bt_RobotExe.Name = "bt_RobotExe";
            this.bt_RobotExe.Size = new System.Drawing.Size(75, 23);
            this.bt_RobotExe.TabIndex = 6;
            this.bt_RobotExe.Text = "Execute";
            this.bt_RobotExe.UseVisualStyleBackColor = true;
            this.bt_RobotExe.Click += new System.EventHandler(this.bt_RobotExe_Click);
            // 
            // cb_RobotActionTargetSlot
            // 
            this.cb_RobotActionTargetSlot.FormattingEnabled = true;
            this.cb_RobotActionTargetSlot.Location = new System.Drawing.Point(107, 181);
            this.cb_RobotActionTargetSlot.Name = "cb_RobotActionTargetSlot";
            this.cb_RobotActionTargetSlot.Size = new System.Drawing.Size(107, 20);
            this.cb_RobotActionTargetSlot.TabIndex = 5;
            // 
            // cb_RobotActionTarget
            // 
            this.cb_RobotActionTarget.FormattingEnabled = true;
            this.cb_RobotActionTarget.Location = new System.Drawing.Point(108, 115);
            this.cb_RobotActionTarget.Name = "cb_RobotActionTarget";
            this.cb_RobotActionTarget.Size = new System.Drawing.Size(107, 20);
            this.cb_RobotActionTarget.TabIndex = 4;
            this.cb_RobotActionTarget.SelectedIndexChanged += new System.EventHandler(this.cb_RobotActionTarget_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(4, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 23);
            this.label9.TabIndex = 3;
            this.label9.Text = "Slot";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(4, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 2;
            this.label8.Text = "Target";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_RobotActionRobotId
            // 
            this.cb_RobotActionRobotId.FormattingEnabled = true;
            this.cb_RobotActionRobotId.Location = new System.Drawing.Point(109, 49);
            this.cb_RobotActionRobotId.Name = "cb_RobotActionRobotId";
            this.cb_RobotActionRobotId.Size = new System.Drawing.Size(107, 20);
            this.cb_RobotActionRobotId.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(6, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = "Robot";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_tpLog
            // 
            this.cv_tpLog.AutoScroll = true;
            this.cv_tpLog.Location = new System.Drawing.Point(4, 22);
            this.cv_tpLog.Name = "cv_tpLog";
            this.cv_tpLog.Padding = new System.Windows.Forms.Padding(3);
            this.cv_tpLog.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpLog.TabIndex = 4;
            this.cv_tpLog.Text = "Log";
            this.cv_tpLog.UseVisualStyleBackColor = true;
            // 
            // cv_tpIo
            // 
            this.cv_tpIo.Controls.Add(this.cv_dataViewIo);
            this.cv_tpIo.Location = new System.Drawing.Point(4, 22);
            this.cv_tpIo.Name = "cv_tpIo";
            this.cv_tpIo.Padding = new System.Windows.Forms.Padding(3);
            this.cv_tpIo.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpIo.TabIndex = 5;
            this.cv_tpIo.Text = "IO";
            this.cv_tpIo.UseVisualStyleBackColor = true;
            // 
            // cv_dataViewIo
            // 
            this.cv_dataViewIo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_dataViewIo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.cv_dataViewIo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_dataViewIo.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.cv_dataViewIo.Location = new System.Drawing.Point(3, 3);
            this.cv_dataViewIo.Name = "cv_dataViewIo";
            this.cv_dataViewIo.RowTemplate.Height = 24;
            this.cv_dataViewIo.Size = new System.Drawing.Size(1196, 747);
            this.cv_dataViewIo.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "NAME";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "VALUE";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "VALUE";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // cv_tpAccount
            // 
            this.cv_tpAccount.AutoScroll = true;
            this.cv_tpAccount.Controls.Add(this.cv_dataViewAccount);
            this.cv_tpAccount.Location = new System.Drawing.Point(4, 22);
            this.cv_tpAccount.Name = "cv_tpAccount";
            this.cv_tpAccount.Size = new System.Drawing.Size(1202, 753);
            this.cv_tpAccount.TabIndex = 6;
            this.cv_tpAccount.Text = "Account";
            this.cv_tpAccount.UseVisualStyleBackColor = true;
            // 
            // cv_dataViewAccount
            // 
            this.cv_dataViewAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_dataViewAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_dataViewAccount.Location = new System.Drawing.Point(0, 0);
            this.cv_dataViewAccount.Name = "cv_dataViewAccount";
            this.cv_dataViewAccount.RowTemplate.Height = 24;
            this.cv_dataViewAccount.Size = new System.Drawing.Size(1202, 753);
            this.cv_dataViewAccount.TabIndex = 0;
            this.cv_dataViewAccount.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.cv_dataViewAccount_RowPrePaint);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.materialTabSelector1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1210, 25);
            this.panel6.TabIndex = 1;
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.cv_tcBar;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(1210, 25);
            this.materialTabSelector1.TabIndex = 0;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel11);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1210, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 804);
            this.panel5.TabIndex = 0;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(71)))), ((int)(((byte)(37)))));
            this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(10, 804);
            this.panel11.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.panel18);
            this.panel3.Controls.Add(this.panel17);
            this.panel3.Controls.Add(this.panel16);
            this.panel3.Controls.Add(this.panel15);
            this.panel3.Controls.Add(this.panel14);
            this.panel3.Controls.Add(this.panel13);
            this.panel3.Controls.Add(this.panel12);
            this.panel3.Controls.Add(this.panel10);
            this.panel3.Controls.Add(this.panel9);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(136, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1220, 142);
            this.panel3.TabIndex = 1;
            // 
            // panel18
            // 
            this.panel18.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel18.Controls.Add(this.lbl_time);
            this.panel18.Controls.Add(this.pan_module);
            this.panel18.Controls.Add(this.lbl_date);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel18.Location = new System.Drawing.Point(1032, 10);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(178, 132);
            this.panel18.TabIndex = 11;
            // 
            // lbl_time
            // 
            this.lbl_time.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(231)))), ((int)(((byte)(247)))));
            this.lbl_time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_time.Font = new System.Drawing.Font("NI7SEG", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_time.Location = new System.Drawing.Point(0, 55);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(178, 52);
            this.lbl_time.TabIndex = 1;
            this.lbl_time.Text = "21:12:13";
            this.lbl_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pan_module
            // 
            this.pan_module.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pan_module.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_module.Location = new System.Drawing.Point(0, 107);
            this.pan_module.Name = "pan_module";
            this.pan_module.Size = new System.Drawing.Size(178, 25);
            this.pan_module.TabIndex = 2;
            // 
            // lbl_date
            // 
            this.lbl_date.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lbl_date.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_date.Font = new System.Drawing.Font("NI7SEG", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_date.Location = new System.Drawing.Point(0, 0);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Size = new System.Drawing.Size(178, 55);
            this.lbl_date.TabIndex = 0;
            this.lbl_date.Text = "2018/01/01";
            this.lbl_date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel17
            // 
            this.panel17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel17.Controls.Add(this.lbl_warningStatus);
            this.panel17.Controls.Add(this.label6);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel17.Location = new System.Drawing.Point(560, 10);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(109, 132);
            this.panel17.TabIndex = 8;
            // 
            // lbl_warningStatus
            // 
            this.lbl_warningStatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_warningStatus.Depth = 0;
            this.lbl_warningStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_warningStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_warningStatus.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_warningStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_warningStatus.Location = new System.Drawing.Point(0, 0);
            this.lbl_warningStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_warningStatus.Name = "lbl_warningStatus";
            this.lbl_warningStatus.Size = new System.Drawing.Size(107, 118);
            this.lbl_warningStatus.TabIndex = 11;
            this.lbl_warningStatus.Text = "WARNING";
            this.lbl_warningStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Num";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.Visible = false;
            // 
            // panel16
            // 
            this.panel16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel16.Controls.Add(this.lbl_alarmStatus);
            this.panel16.Controls.Add(this.label5);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel16.Location = new System.Drawing.Point(448, 10);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(112, 132);
            this.panel16.TabIndex = 7;
            // 
            // lbl_alarmStatus
            // 
            this.lbl_alarmStatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_alarmStatus.Depth = 0;
            this.lbl_alarmStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_alarmStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_alarmStatus.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_alarmStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_alarmStatus.Location = new System.Drawing.Point(0, 0);
            this.lbl_alarmStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_alarmStatus.Name = "lbl_alarmStatus";
            this.lbl_alarmStatus.Size = new System.Drawing.Size(110, 118);
            this.lbl_alarmStatus.TabIndex = 8;
            this.lbl_alarmStatus.Text = "ALARM";
            this.lbl_alarmStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Location = new System.Drawing.Point(0, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Num";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Visible = false;
            // 
            // panel15
            // 
            this.panel15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel15.Controls.Add(this.lbl_hsmsStatus);
            this.panel15.Controls.Add(this.label4);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel15.Location = new System.Drawing.Point(336, 10);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(112, 132);
            this.panel15.TabIndex = 6;
            // 
            // lbl_hsmsStatus
            // 
            this.lbl_hsmsStatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_hsmsStatus.Depth = 0;
            this.lbl_hsmsStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_hsmsStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_hsmsStatus.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_hsmsStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_hsmsStatus.Location = new System.Drawing.Point(0, 0);
            this.lbl_hsmsStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_hsmsStatus.Name = "lbl_hsmsStatus";
            this.lbl_hsmsStatus.Size = new System.Drawing.Size(110, 118);
            this.lbl_hsmsStatus.TabIndex = 7;
            this.lbl_hsmsStatus.Text = "CONNECTED";
            this.lbl_hsmsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(0, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 12);
            this.label4.TabIndex = 0;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Visible = false;
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.lbl_plcStatus);
            this.panel14.Controls.Add(this.label3);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel14.Location = new System.Drawing.Point(224, 10);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(112, 132);
            this.panel14.TabIndex = 5;
            // 
            // lbl_plcStatus
            // 
            this.lbl_plcStatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_plcStatus.Depth = 0;
            this.lbl_plcStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_plcStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_plcStatus.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_plcStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_plcStatus.Location = new System.Drawing.Point(0, 0);
            this.lbl_plcStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_plcStatus.Name = "lbl_plcStatus";
            this.lbl_plcStatus.Size = new System.Drawing.Size(110, 118);
            this.lbl_plcStatus.TabIndex = 6;
            this.lbl_plcStatus.Text = "DISCONNECT";
            this.lbl_plcStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(0, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 12);
            this.label3.TabIndex = 0;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Visible = false;
            // 
            // panel13
            // 
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.lbl_systemStatus);
            this.panel13.Controls.Add(this.label2);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel13.Location = new System.Drawing.Point(112, 10);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(112, 132);
            this.panel13.TabIndex = 4;
            // 
            // lbl_systemStatus
            // 
            this.lbl_systemStatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_systemStatus.Depth = 0;
            this.lbl_systemStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_systemStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_systemStatus.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_systemStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_systemStatus.Location = new System.Drawing.Point(0, 0);
            this.lbl_systemStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_systemStatus.Name = "lbl_systemStatus";
            this.lbl_systemStatus.Size = new System.Drawing.Size(110, 118);
            this.lbl_systemStatus.TabIndex = 5;
            this.lbl_systemStatus.Text = "DOWN";
            this.lbl_systemStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 12);
            this.label2.TabIndex = 0;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            // 
            // panel12
            // 
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel12.Controls.Add(this.lbl_disSystemMode);
            this.panel12.Controls.Add(this.label1);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel12.Location = new System.Drawing.Point(0, 10);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(112, 132);
            this.panel12.TabIndex = 2;
            // 
            // lbl_disSystemMode
            // 
            this.lbl_disSystemMode.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_disSystemMode.Depth = 0;
            this.lbl_disSystemMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_disSystemMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_disSystemMode.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_disSystemMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_disSystemMode.Location = new System.Drawing.Point(0, 0);
            this.lbl_disSystemMode.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_disSystemMode.Name = "lbl_disSystemMode";
            this.lbl_disSystemMode.Size = new System.Drawing.Size(110, 118);
            this.lbl_disSystemMode.TabIndex = 10;
            this.lbl_disSystemMode.Text = "OFFLINE";
            this.lbl_disSystemMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 12);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(71)))), ((int)(((byte)(37)))));
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(1210, 10);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(10, 132);
            this.panel10.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(71)))), ((int)(((byte)(37)))));
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1220, 10);
            this.panel9.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.materialDivider9);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.materialDivider8);
            this.panel2.Controls.Add(this.lbl_ReIni);
            this.panel2.Controls.Add(this.gb_RobotAction);
            this.panel2.Controls.Add(this.materialDivider7);
            this.panel2.Controls.Add(this.btn_ShowManualOut);
            this.panel2.Controls.Add(this.materialDivider6);
            this.panel2.Controls.Add(this.btn_ShowManualIn);
            this.panel2.Controls.Add(this.pan_version);
            this.panel2.Controls.Add(this.materialDivider4);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.materialDivider3);
            this.panel2.Controls.Add(this.cv_btnSelectMode);
            this.panel2.Controls.Add(this.materialDivider5);
            this.panel2.Controls.Add(this.cv_btnLogout);
            this.panel2.Controls.Add(this.materialDivider2);
            this.panel2.Controls.Add(this.cv_btnLogin);
            this.panel2.Controls.Add(this.materialDivider1);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(136, 946);
            this.panel2.TabIndex = 0;
            // 
            // materialDivider9
            // 
            this.materialDivider9.BackColor = System.Drawing.Color.White;
            this.materialDivider9.Depth = 0;
            this.materialDivider9.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider9.Location = new System.Drawing.Point(0, 732);
            this.materialDivider9.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider9.Name = "materialDivider9";
            this.materialDivider9.Size = new System.Drawing.Size(134, 3);
            this.materialDivider9.TabIndex = 24;
            this.materialDivider9.Text = "materialDivider9";
            // 
            // button2
            // 
            this.button2.ContextMenuStrip = this.materialContextMenuStrip1;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.button2.FlatAppearance.BorderSize = 3;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button2.Location = new System.Drawing.Point(0, 656);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 76);
            this.button2.TabIndex = 23;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Exit_Click);
            // 
            // materialContextMenuStrip1
            // 
            this.materialContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialContextMenuStrip1.Depth = 0;
            this.materialContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem,
            this.cancelToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.materialContextMenuStrip1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialContextMenuStrip1.Name = "materialContextMenuStrip1";
            this.materialContextMenuStrip1.Size = new System.Drawing.Size(115, 70);
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.setToolStripMenuItem.Text = "Edit";
            this.setToolStripMenuItem.Click += new System.EventHandler(this.setToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // materialDivider8
            // 
            this.materialDivider8.BackColor = System.Drawing.Color.White;
            this.materialDivider8.Depth = 0;
            this.materialDivider8.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider8.Location = new System.Drawing.Point(0, 653);
            this.materialDivider8.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider8.Name = "materialDivider8";
            this.materialDivider8.Size = new System.Drawing.Size(134, 3);
            this.materialDivider8.TabIndex = 22;
            this.materialDivider8.Text = "materialDivider8";
            // 
            // lbl_ReIni
            // 
            this.lbl_ReIni.ContextMenuStrip = this.materialContextMenuStrip1;
            this.lbl_ReIni.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_ReIni.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.lbl_ReIni.FlatAppearance.BorderSize = 3;
            this.lbl_ReIni.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_ReIni.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.lbl_ReIni.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_ReIni.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ReIni.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_ReIni.Location = new System.Drawing.Point(0, 577);
            this.lbl_ReIni.Name = "lbl_ReIni";
            this.lbl_ReIni.Size = new System.Drawing.Size(134, 76);
            this.lbl_ReIni.TabIndex = 21;
            this.lbl_ReIni.Text = "Initial";
            this.lbl_ReIni.UseVisualStyleBackColor = true;
            this.lbl_ReIni.Click += new System.EventHandler(this.ReInit_Click);
            // 
            // gb_RobotAction
            // 
            this.gb_RobotAction.Controls.Add(this.lbl_ActionName);
            this.gb_RobotAction.Controls.Add(this.lbl_ActionArm);
            this.gb_RobotAction.Controls.Add(this.lbl_ActionTarget);
            this.gb_RobotAction.Controls.Add(this.lbl_ActionId);
            this.gb_RobotAction.Controls.Add(this.lbl_ActionSlot);
            this.gb_RobotAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_RobotAction.ForeColor = System.Drawing.SystemColors.Control;
            this.gb_RobotAction.Location = new System.Drawing.Point(0, 747);
            this.gb_RobotAction.Name = "gb_RobotAction";
            this.gb_RobotAction.Size = new System.Drawing.Size(134, 129);
            this.gb_RobotAction.TabIndex = 20;
            this.gb_RobotAction.TabStop = false;
            this.gb_RobotAction.Text = "Robot Action";
            // 
            // lbl_ActionName
            // 
            this.lbl_ActionName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.lbl_ActionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ActionName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_ActionName.Location = new System.Drawing.Point(3, 16);
            this.lbl_ActionName.Name = "lbl_ActionName";
            this.lbl_ActionName.Size = new System.Drawing.Size(128, 22);
            this.lbl_ActionName.TabIndex = 5;
            this.lbl_ActionName.Text = "Arm";
            this.lbl_ActionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_ActionArm
            // 
            this.lbl_ActionArm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.lbl_ActionArm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ActionArm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_ActionArm.Location = new System.Drawing.Point(3, 38);
            this.lbl_ActionArm.Name = "lbl_ActionArm";
            this.lbl_ActionArm.Size = new System.Drawing.Size(128, 22);
            this.lbl_ActionArm.TabIndex = 4;
            this.lbl_ActionArm.Text = "Action ";
            this.lbl_ActionArm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_ActionTarget
            // 
            this.lbl_ActionTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ActionTarget.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_ActionTarget.Location = new System.Drawing.Point(3, 60);
            this.lbl_ActionTarget.Name = "lbl_ActionTarget";
            this.lbl_ActionTarget.Size = new System.Drawing.Size(128, 22);
            this.lbl_ActionTarget.TabIndex = 2;
            this.lbl_ActionTarget.Text = "Target";
            this.lbl_ActionTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_ActionId
            // 
            this.lbl_ActionId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ActionId.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_ActionId.Location = new System.Drawing.Point(3, 82);
            this.lbl_ActionId.Name = "lbl_ActionId";
            this.lbl_ActionId.Size = new System.Drawing.Size(128, 22);
            this.lbl_ActionId.TabIndex = 1;
            this.lbl_ActionId.Text = "Id";
            this.lbl_ActionId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_ActionSlot
            // 
            this.lbl_ActionSlot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ActionSlot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_ActionSlot.Location = new System.Drawing.Point(3, 104);
            this.lbl_ActionSlot.Name = "lbl_ActionSlot";
            this.lbl_ActionSlot.Size = new System.Drawing.Size(128, 22);
            this.lbl_ActionSlot.TabIndex = 0;
            this.lbl_ActionSlot.Text = "Slot";
            this.lbl_ActionSlot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialDivider7
            // 
            this.materialDivider7.BackColor = System.Drawing.Color.White;
            this.materialDivider7.Depth = 0;
            this.materialDivider7.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider7.Location = new System.Drawing.Point(0, 574);
            this.materialDivider7.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider7.Name = "materialDivider7";
            this.materialDivider7.Size = new System.Drawing.Size(134, 3);
            this.materialDivider7.TabIndex = 19;
            this.materialDivider7.Text = "materialDivider7";
            // 
            // btn_ShowManualOut
            // 
            this.btn_ShowManualOut.ContextMenuStrip = this.materialContextMenuStrip1;
            this.btn_ShowManualOut.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_ShowManualOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.btn_ShowManualOut.FlatAppearance.BorderSize = 3;
            this.btn_ShowManualOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_ShowManualOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.btn_ShowManualOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ShowManualOut.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ShowManualOut.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_ShowManualOut.Location = new System.Drawing.Point(0, 498);
            this.btn_ShowManualOut.Name = "btn_ShowManualOut";
            this.btn_ShowManualOut.Size = new System.Drawing.Size(134, 76);
            this.btn_ShowManualOut.TabIndex = 18;
            this.btn_ShowManualOut.Text = "Manual_Out";
            this.btn_ShowManualOut.UseVisualStyleBackColor = true;
            this.btn_ShowManualOut.Click += new System.EventHandler(this.btn_ShowManualOut_Click);
            // 
            // materialDivider6
            // 
            this.materialDivider6.BackColor = System.Drawing.Color.White;
            this.materialDivider6.Depth = 0;
            this.materialDivider6.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider6.Location = new System.Drawing.Point(0, 495);
            this.materialDivider6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider6.Name = "materialDivider6";
            this.materialDivider6.Size = new System.Drawing.Size(134, 3);
            this.materialDivider6.TabIndex = 17;
            this.materialDivider6.Text = "materialDivider6";
            // 
            // btn_ShowManualIn
            // 
            this.btn_ShowManualIn.ContextMenuStrip = this.materialContextMenuStrip1;
            this.btn_ShowManualIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_ShowManualIn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.btn_ShowManualIn.FlatAppearance.BorderSize = 3;
            this.btn_ShowManualIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_ShowManualIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.btn_ShowManualIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ShowManualIn.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ShowManualIn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_ShowManualIn.Location = new System.Drawing.Point(0, 419);
            this.btn_ShowManualIn.Name = "btn_ShowManualIn";
            this.btn_ShowManualIn.Size = new System.Drawing.Size(134, 76);
            this.btn_ShowManualIn.TabIndex = 16;
            this.btn_ShowManualIn.Text = "Manual_In";
            this.btn_ShowManualIn.UseVisualStyleBackColor = true;
            this.btn_ShowManualIn.Click += new System.EventHandler(this.btn_ShowManualIn_Click);
            // 
            // pan_version
            // 
            this.pan_version.BackColor = System.Drawing.Color.Transparent;
            this.pan_version.Controls.Add(this.lbl_per);
            this.pan_version.Controls.Add(this.lbl_id);
            this.pan_version.Controls.Add(this.lbl_version);
            this.pan_version.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_version.Location = new System.Drawing.Point(0, 876);
            this.pan_version.Name = "pan_version";
            this.pan_version.Size = new System.Drawing.Size(134, 68);
            this.pan_version.TabIndex = 13;
            // 
            // lbl_per
            // 
            this.lbl_per.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_per.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_per.ForeColor = System.Drawing.Color.White;
            this.lbl_per.Location = new System.Drawing.Point(0, 46);
            this.lbl_per.Name = "lbl_per";
            this.lbl_per.Size = new System.Drawing.Size(134, 23);
            this.lbl_per.TabIndex = 2;
            this.lbl_per.Text = "Permission";
            // 
            // lbl_id
            // 
            this.lbl_id.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_id.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_id.ForeColor = System.Drawing.Color.White;
            this.lbl_id.Location = new System.Drawing.Point(0, 23);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(134, 23);
            this.lbl_id.TabIndex = 1;
            this.lbl_id.Text = "Id";
            // 
            // lbl_version
            // 
            this.lbl_version.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_version.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_version.ForeColor = System.Drawing.Color.White;
            this.lbl_version.Location = new System.Drawing.Point(0, 0);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(134, 23);
            this.lbl_version.TabIndex = 0;
            this.lbl_version.Text = "version";
            // 
            // materialDivider4
            // 
            this.materialDivider4.BackColor = System.Drawing.Color.White;
            this.materialDivider4.Depth = 0;
            this.materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider4.Location = new System.Drawing.Point(0, 416);
            this.materialDivider4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider4.Name = "materialDivider4";
            this.materialDivider4.Size = new System.Drawing.Size(134, 3);
            this.materialDivider4.TabIndex = 12;
            this.materialDivider4.Text = "materialDivider4";
            // 
            // button1
            // 
            this.button1.ContextMenuStrip = this.materialContextMenuStrip1;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.button1.FlatAppearance.BorderSize = 3;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(0, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 76);
            this.button1.TabIndex = 11;
            this.button1.Text = "Pos";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // materialDivider3
            // 
            this.materialDivider3.BackColor = System.Drawing.Color.White;
            this.materialDivider3.Depth = 0;
            this.materialDivider3.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider3.ForeColor = System.Drawing.Color.Transparent;
            this.materialDivider3.Location = new System.Drawing.Point(0, 337);
            this.materialDivider3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider3.Name = "materialDivider3";
            this.materialDivider3.Size = new System.Drawing.Size(134, 3);
            this.materialDivider3.TabIndex = 10;
            this.materialDivider3.Text = "materialDivider3";
            // 
            // cv_btnSelectMode
            // 
            this.cv_btnSelectMode.ContextMenuStrip = this.menu_systemMode;
            this.cv_btnSelectMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_btnSelectMode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.cv_btnSelectMode.FlatAppearance.BorderSize = 3;
            this.cv_btnSelectMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.cv_btnSelectMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.cv_btnSelectMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cv_btnSelectMode.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cv_btnSelectMode.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cv_btnSelectMode.Location = new System.Drawing.Point(0, 261);
            this.cv_btnSelectMode.Name = "cv_btnSelectMode";
            this.cv_btnSelectMode.Size = new System.Drawing.Size(134, 76);
            this.cv_btnSelectMode.TabIndex = 9;
            this.cv_btnSelectMode.Text = "Select Mode";
            this.cv_btnSelectMode.UseVisualStyleBackColor = true;
            // 
            // menu_systemMode
            // 
            this.menu_systemMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(80)))), ((int)(((byte)(129)))));
            this.menu_systemMode.Depth = 0;
            this.menu_systemMode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oFFLINEToolStripMenuItem,
            this.oNLINELOCALToolStripMenuItem,
            this.oNLINEREMOTEToolStripMenuItem});
            this.menu_systemMode.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.menu_systemMode.MouseState = MaterialSkin.MouseState.HOVER;
            this.menu_systemMode.Name = "materialContextMenuStrip1";
            this.menu_systemMode.Size = new System.Drawing.Size(137, 70);
            // 
            // oFFLINEToolStripMenuItem
            // 
            this.oFFLINEToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.oFFLINEToolStripMenuItem.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.oFFLINEToolStripMenuItem.Name = "oFFLINEToolStripMenuItem";
            this.oFFLINEToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.oFFLINEToolStripMenuItem.Text = "OFFLINE";
            this.oFFLINEToolStripMenuItem.Click += new System.EventHandler(this.oFFLINEToolStripMenuItem_Click);
            // 
            // oNLINELOCALToolStripMenuItem
            // 
            this.oNLINELOCALToolStripMenuItem.Name = "oNLINELOCALToolStripMenuItem";
            this.oNLINELOCALToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.oNLINELOCALToolStripMenuItem.Text = "MONITOR";
            this.oNLINELOCALToolStripMenuItem.Click += new System.EventHandler(this.oNLINELOCALToolStripMenuItem_Click);
            // 
            // oNLINEREMOTEToolStripMenuItem
            // 
            this.oNLINEREMOTEToolStripMenuItem.Name = "oNLINEREMOTEToolStripMenuItem";
            this.oNLINEREMOTEToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.oNLINEREMOTEToolStripMenuItem.Text = "CONTROL";
            this.oNLINEREMOTEToolStripMenuItem.Click += new System.EventHandler(this.oNLINEREMOTEToolStripMenuItem_Click);
            // 
            // materialDivider5
            // 
            this.materialDivider5.BackColor = System.Drawing.Color.White;
            this.materialDivider5.Depth = 0;
            this.materialDivider5.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider5.ForeColor = System.Drawing.Color.White;
            this.materialDivider5.Location = new System.Drawing.Point(0, 258);
            this.materialDivider5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider5.Name = "materialDivider5";
            this.materialDivider5.Size = new System.Drawing.Size(134, 3);
            this.materialDivider5.TabIndex = 15;
            this.materialDivider5.Text = "materialDivider5";
            // 
            // cv_btnLogout
            // 
            this.cv_btnLogout.ContextMenuStrip = this.materialContextMenuStrip1;
            this.cv_btnLogout.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.cv_btnLogout.FlatAppearance.BorderSize = 3;
            this.cv_btnLogout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.cv_btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.cv_btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cv_btnLogout.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cv_btnLogout.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cv_btnLogout.Location = new System.Drawing.Point(0, 182);
            this.cv_btnLogout.Name = "cv_btnLogout";
            this.cv_btnLogout.Size = new System.Drawing.Size(134, 76);
            this.cv_btnLogout.TabIndex = 14;
            this.cv_btnLogout.Text = "LogOut";
            this.cv_btnLogout.UseVisualStyleBackColor = true;
            this.cv_btnLogout.Click += new System.EventHandler(this.cv_btnLogout_Click);
            // 
            // materialDivider2
            // 
            this.materialDivider2.BackColor = System.Drawing.Color.White;
            this.materialDivider2.Depth = 0;
            this.materialDivider2.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider2.Location = new System.Drawing.Point(0, 179);
            this.materialDivider2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider2.Name = "materialDivider2";
            this.materialDivider2.Size = new System.Drawing.Size(134, 3);
            this.materialDivider2.TabIndex = 8;
            this.materialDivider2.Text = "materialDivider2";
            // 
            // cv_btnLogin
            // 
            this.cv_btnLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(75)))), ((int)(((byte)(81)))));
            this.cv_btnLogin.FlatAppearance.BorderSize = 3;
            this.cv_btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.cv_btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.cv_btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cv_btnLogin.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cv_btnLogin.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cv_btnLogin.Location = new System.Drawing.Point(0, 103);
            this.cv_btnLogin.Name = "cv_btnLogin";
            this.cv_btnLogin.Size = new System.Drawing.Size(134, 76);
            this.cv_btnLogin.TabIndex = 6;
            this.cv_btnLogin.Text = "Login";
            this.cv_btnLogin.UseVisualStyleBackColor = true;
            this.cv_btnLogin.Click += new System.EventHandler(this.cv_btnLogin_Click);
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.White;
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider1.Location = new System.Drawing.Point(0, 100);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(134, 3);
            this.materialDivider1.TabIndex = 3;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.pictureBox1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(134, 100);
            this.panel8.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 100);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1356, 946);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HIRATA Controller";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.cv_tcBar.ResumeLayout(false);
            this.cv_tpAlarm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_AlarmDataView)).EndInit();
            this.pan_alarm.ResumeLayout(false);
            this.cv_tpManual.ResumeLayout(false);
            this.gpb_AlignerAction.ResumeLayout(false);
            this.gpb_PortAction.ResumeLayout(false);
            this.gpb_RobotAction.ResumeLayout(false);
            this.cv_tpIo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_dataViewIo)).EndInit();
            this.cv_tpAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_dataViewAccount)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.materialContextMenuStrip1.ResumeLayout(false);
            this.gb_RobotAction.ResumeLayout(false);
            this.pan_version.ResumeLayout(false);
            this.menu_systemMode.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private MaterialSkin.Controls.MaterialTabControl cv_tcBar;
        private System.Windows.Forms.TabPage cv_tpLayout;
        private System.Windows.Forms.TabPage cv_tpManual;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TabPage cv_tpLog;
        private System.Windows.Forms.TabPage cv_tpAlarm;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.TabPage cv_tpIo;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialDivider materialDivider3;
        private System.Windows.Forms.Button cv_btnSelectMode;
        private MaterialSkin.Controls.MaterialDivider materialDivider2;
        private System.Windows.Forms.Button cv_btnLogin;
        private MaterialSkin.Controls.MaterialContextMenuStrip menu_systemMode;
        private System.Windows.Forms.ToolStripMenuItem oFFLINEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oNLINELOCALToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oNLINEREMOTEToolStripMenuItem;
        private System.Windows.Forms.DataGridView cv_AlarmDataView;
        private System.Windows.Forms.DataGridView cv_dataViewIo;
        private System.Windows.Forms.TabPage cv_tpRecipe;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private System.Windows.Forms.TabPage cv_tpAccount;
        private System.Windows.Forms.DataGridView cv_dataViewAccount;
        private System.Windows.Forms.Panel panel12;
        private MaterialSkin.Controls.MaterialLabel lbl_disSystemMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel16;
        private MaterialSkin.Controls.MaterialLabel lbl_alarmStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel15;
        private MaterialSkin.Controls.MaterialLabel lbl_hsmsStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel14;
        private MaterialSkin.Controls.MaterialLabel lbl_plcStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel13;
        private MaterialSkin.Controls.MaterialLabel lbl_systemStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private MaterialSkin.Controls.MaterialDivider materialDivider4;
        private System.Windows.Forms.Button button1;
        private MaterialSkin.Controls.MaterialContextMenuStrip materialContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Panel pan_version;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.GroupBox gpb_RobotAction;
        private System.Windows.Forms.ComboBox cb_RobotActionName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button bt_RobotExe;
        private System.Windows.Forms.ComboBox cb_RobotActionTargetSlot;
        private System.Windows.Forms.ComboBox cb_RobotActionTarget;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_RobotActionRobotId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gpb_AlignerAction;
        private System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button bt_AlignerExe;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox gpb_PortAction;
        private System.Windows.Forms.ComboBox cb_PortActionName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bt_PortExe;
        private System.Windows.Forms.ComboBox cb_PortActionPortId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pan_alarm;
        private System.Windows.Forms.Button btn_resetAllAlarm;
        private System.Windows.Forms.Button btn_buzzerOff;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.Label lbl_per;
        private System.Windows.Forms.Label lbl_id;
        private MaterialSkin.Controls.MaterialDivider materialDivider5;
        private System.Windows.Forms.Button cv_btnLogout;
        private System.Windows.Forms.Button btn_ShowManualIn;
        private MaterialSkin.Controls.MaterialDivider materialDivider6;
        private MaterialSkin.Controls.MaterialDivider materialDivider7;
        private System.Windows.Forms.Button btn_ShowManualOut;
        private System.Windows.Forms.GroupBox gb_RobotAction;
        private System.Windows.Forms.Label lbl_ActionArm;
        private System.Windows.Forms.Label lbl_ActionTarget;
        private System.Windows.Forms.Label lbl_ActionId;
        private System.Windows.Forms.Label lbl_ActionSlot;
        private System.Windows.Forms.Label lbl_ActionName;
        private MaterialSkin.Controls.MaterialDivider materialDivider8;
        private System.Windows.Forms.Button lbl_ReIni;
        private System.Windows.Forms.ComboBox cb_RobotActionTargetId;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.ComboBox cb_RobotActionArm;
        private System.Windows.Forms.Label label17;
        private MaterialSkin.Controls.MaterialDivider materialDivider9;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Label lbl_time;
        private System.Windows.Forms.Label lbl_date;
        private System.Windows.Forms.Panel pan_module;
        private MaterialSkin.Controls.MaterialLabel lbl_warningStatus;
    }
}

