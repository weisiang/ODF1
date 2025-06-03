namespace UI.GUI
{
    partial class RobotJobPathUI
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_DropTop = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_preVirw = new System.Windows.Forms.Button();
            this.gdv_PathData = new System.Windows.Forms.DataGridView();
            this.Slot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Put = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Get = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdv_PathData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.btn_DropTop);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btn_preVirw);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 63);
            this.panel1.TabIndex = 1;
            // 
            // btn_DropTop
            // 
            this.btn_DropTop.Location = new System.Drawing.Point(84, 32);
            this.btn_DropTop.Name = "btn_DropTop";
            this.btn_DropTop.Size = new System.Drawing.Size(75, 23);
            this.btn_DropTop.TabIndex = 2;
            this.btn_DropTop.Text = "Drop Top";
            this.btn_DropTop.UseVisualStyleBackColor = true;
            this.btn_DropTop.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Clean";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_preVirw
            // 
            this.btn_preVirw.Location = new System.Drawing.Point(3, 3);
            this.btn_preVirw.Name = "btn_preVirw";
            this.btn_preVirw.Size = new System.Drawing.Size(75, 23);
            this.btn_preVirw.TabIndex = 0;
            this.btn_preVirw.Text = "PreView";
            this.btn_preVirw.UseVisualStyleBackColor = true;
            this.btn_preVirw.Click += new System.EventHandler(this.btn_preVirw_Click);
            // 
            // gdv_PathData
            // 
            this.gdv_PathData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdv_PathData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Slot,
            this.TargetId,
            this.Target,
            this.Action,
            this.Put,
            this.Get});
            this.gdv_PathData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdv_PathData.Location = new System.Drawing.Point(0, 63);
            this.gdv_PathData.Name = "gdv_PathData";
            this.gdv_PathData.RowTemplate.Height = 24;
            this.gdv_PathData.Size = new System.Drawing.Size(475, 597);
            this.gdv_PathData.TabIndex = 2;
            // 
            // Slot
            // 
            this.Slot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Slot.DataPropertyName = "PTargetSlot";
            this.Slot.HeaderText = "Slot";
            this.Slot.Name = "Slot";
            // 
            // TargetId
            // 
            this.TargetId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TargetId.DataPropertyName = "PTargetId";
            this.TargetId.HeaderText = "TargetId";
            this.TargetId.Name = "TargetId";
            this.TargetId.ReadOnly = true;
            // 
            // Target
            // 
            this.Target.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Target.DataPropertyName = "PTarget";
            this.Target.HeaderText = "Target";
            this.Target.Name = "Target";
            this.Target.ReadOnly = true;
            // 
            // Action
            // 
            this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Action.DataPropertyName = "PAction";
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            // 
            // Put
            // 
            this.Put.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Put.DataPropertyName = "PPutArm";
            this.Put.HeaderText = "Put";
            this.Put.Name = "Put";
            this.Put.ReadOnly = true;
            // 
            // Get
            // 
            this.Get.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Get.DataPropertyName = "PGetArm";
            this.Get.HeaderText = "Get";
            this.Get.Name = "Get";
            this.Get.ReadOnly = true;
            // 
            // RobotJobPathUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gdv_PathData);
            this.Controls.Add(this.panel1);
            this.Name = "RobotJobPathUI";
            this.Size = new System.Drawing.Size(475, 660);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdv_PathData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gdv_PathData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Slot;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn Put;
        private System.Windows.Forms.DataGridViewTextBoxColumn Get;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_preVirw;
        private System.Windows.Forms.Button btn_DropTop;

    }
}
