
namespace UI
{
    partial class user_SamplingSlot
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_byPeriod = new System.Windows.Forms.RadioButton();
            this.radioButton_byslot = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton_back = new System.Windows.Forms.RadioButton();
            this.radioButton_front = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_n = new System.Windows.Forms.TextBox();
            this.textBox_m = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_description = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_id = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.col_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_slot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_desrciption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_description);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_id);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.DarkRed;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1089, 820);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Slot Setting";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_byPeriod);
            this.panel1.Controls.Add(this.radioButton_byslot);
            this.panel1.Location = new System.Drawing.Point(454, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(124, 60);
            this.panel1.TabIndex = 15;
            // 
            // radioButton_byPeriod
            // 
            this.radioButton_byPeriod.AutoSize = true;
            this.radioButton_byPeriod.Location = new System.Drawing.Point(22, 35);
            this.radioButton_byPeriod.Name = "radioButton_byPeriod";
            this.radioButton_byPeriod.Size = new System.Drawing.Size(67, 16);
            this.radioButton_byPeriod.TabIndex = 14;
            this.radioButton_byPeriod.TabStop = true;
            this.radioButton_byPeriod.Text = "ByPeriod";
            this.radioButton_byPeriod.UseVisualStyleBackColor = true;
            // 
            // radioButton_byslot
            // 
            this.radioButton_byslot.AutoSize = true;
            this.radioButton_byslot.Location = new System.Drawing.Point(22, 13);
            this.radioButton_byslot.Name = "radioButton_byslot";
            this.radioButton_byslot.Size = new System.Drawing.Size(55, 16);
            this.radioButton_byslot.TabIndex = 13;
            this.radioButton_byslot.TabStop = true;
            this.radioButton_byslot.Text = "BySlot";
            this.radioButton_byslot.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox_n);
            this.groupBox3.Controls.Add(this.textBox_m);
            this.groupBox3.Location = new System.Drawing.Point(231, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 455);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Method 2";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton_back);
            this.panel2.Controls.Add(this.radioButton_front);
            this.panel2.Location = new System.Drawing.Point(18, 115);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(124, 60);
            this.panel2.TabIndex = 16;
            // 
            // radioButton_back
            // 
            this.radioButton_back.AutoSize = true;
            this.radioButton_back.Location = new System.Drawing.Point(22, 35);
            this.radioButton_back.Name = "radioButton_back";
            this.radioButton_back.Size = new System.Drawing.Size(73, 16);
            this.radioButton_back.TabIndex = 14;
            this.radioButton_back.TabStop = true;
            this.radioButton_back.Text = "From back";
            this.radioButton_back.UseVisualStyleBackColor = true;
            // 
            // radioButton_front
            // 
            this.radioButton_front.AutoSize = true;
            this.radioButton_front.Location = new System.Drawing.Point(22, 13);
            this.radioButton_front.Name = "radioButton_front";
            this.radioButton_front.Size = new System.Drawing.Size(74, 16);
            this.radioButton_front.TabIndex = 13;
            this.radioButton_front.TabStop = true;
            this.radioButton_front.Text = "From front";
            this.radioButton_front.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(92, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "/";
            // 
            // textBox_n
            // 
            this.textBox_n.Location = new System.Drawing.Point(116, 58);
            this.textBox_n.Name = "textBox_n";
            this.textBox_n.Size = new System.Drawing.Size(68, 22);
            this.textBox_n.TabIndex = 1;
            // 
            // textBox_m
            // 
            this.textBox_m.Location = new System.Drawing.Point(18, 58);
            this.textBox_m.Name = "textBox_m";
            this.textBox_m.Size = new System.Drawing.Size(68, 22);
            this.textBox_m.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkedListBox1);
            this.groupBox2.Location = new System.Drawing.Point(18, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 455);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Method 1";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkedListBox1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Slot  01",
            "Slot  02",
            "Slot  03",
            "Slot  04",
            "Slot  05",
            "Slot  06",
            "Slot  07",
            "Slot  08",
            "Slot  09",
            "Slot  10",
            "Slot  11",
            "Slot  12",
            "Slot  13",
            "Slot  14",
            "Slot  15",
            "Slot  16",
            "Slot  17",
            "Slot  18",
            "Slot  19",
            "Slot  20",
            "Slot  21",
            "Slot  22",
            "Slot  23",
            "Slot  24",
            "Slot  25"});
            this.checkedListBox1.Location = new System.Drawing.Point(57, 21);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(92, 429);
            this.checkedListBox1.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(508, 295);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(94, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "Even Slot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(508, 240);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(94, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Odd Slot";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(444, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Description";
            // 
            // textBox_description
            // 
            this.textBox_description.Location = new System.Drawing.Point(508, 189);
            this.textBox_description.Name = "textBox_description";
            this.textBox_description.Size = new System.Drawing.Size(280, 22);
            this.textBox_description.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(508, 350);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Reset Checked";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(713, 406);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(609, 406);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Modify";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(508, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(485, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID";
            // 
            // textBox_id
            // 
            this.textBox_id.Location = new System.Drawing.Point(508, 126);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.Size = new System.Drawing.Size(280, 22);
            this.textBox_id.TabIndex = 1;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_id,
            this.Method,
            this.Time,
            this.col_slot,
            this.col_desrciption});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView2.Location = new System.Drawing.Point(0, 522);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1089, 298);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellDoubleClick);
            // 
            // col_id
            // 
            this.col_id.DataPropertyName = "PNo";
            this.col_id.HeaderText = "No";
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            this.col_id.Width = 50;
            // 
            // Method
            // 
            this.Method.DataPropertyName = "PMethod";
            this.Method.HeaderText = "Method";
            this.Method.Name = "Method";
            this.Method.ReadOnly = true;
            this.Method.Width = 50;
            // 
            // Time
            // 
            this.Time.DataPropertyName = "PTime";
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // col_slot
            // 
            this.col_slot.DataPropertyName = "PlistStr";
            this.col_slot.FillWeight = 250F;
            this.col_slot.HeaderText = "Slot";
            this.col_slot.Name = "col_slot";
            this.col_slot.ReadOnly = true;
            this.col_slot.Width = 300;
            // 
            // col_desrciption
            // 
            this.col_desrciption.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_desrciption.DataPropertyName = "PDesription";
            this.col_desrciption.HeaderText = "Description";
            this.col_desrciption.Name = "col_desrciption";
            this.col_desrciption.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(16, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "M/N";
            // 
            // user_SamplingSlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.groupBox1);
            this.Name = "user_SamplingSlot";
            this.Size = new System.Drawing.Size(1089, 820);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_id;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_description;
        public System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_byPeriod;
        private System.Windows.Forms.RadioButton radioButton_byslot;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton_back;
        private System.Windows.Forms.RadioButton radioButton_front;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_n;
        private System.Windows.Forms.TextBox textBox_m;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Method;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_slot;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_desrciption;
        private System.Windows.Forms.Label label4;
    }
}
