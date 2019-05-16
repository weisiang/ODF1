using System;
using CommonData.HIRATA;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using System.Text.RegularExpressions;

namespace UI.GUI
{
    public partial class BufferUI : UserControl , BufferReflash
    {
        int cv_SlotCount = 0;
        int cv_Id = 0;
        Match match = Match.Empty;
        public BufferUI(int m_Id, int m_SlotCount)
        {
            InitializeComponent();
            cv_SlotCount = m_SlotCount;
            cv_Id = m_Id;
            this.gb_Buffer.Text = "Buffer " + Convert.ToString(m_Id);
            cv_glassDataView.Rows.Add(m_SlotCount);
            cv_glassDataView.AutoGenerateColumns = false;
            cv_glassDataView.RowHeadersVisible = false;
            cv_glassDataView.Rows[0].Cells[0].Value = "";

            int i = 0;
            for (; i < m_SlotCount; i++)
            {
                cv_glassDataView.Rows[i + 1].Cells[0].Value = "";
                ToolStripMenuItem tmp = new ToolStripMenuItem();
                tmp.Text = (i + 1).ToString();
                tmp.Click += dELETEToolStripMenuItem_Click;
                (cv_menuDataEdit.Items[0] as ToolStripMenuItem).DropDownItems.Add(tmp);
            }
        }
        private void CheckUiSlotType()
        {
            Buffer buffer = UiForm.GetBuffer(1);
            if(cv_glassDataView.Rows[0].Cells[0].Value == null || string.IsNullOrEmpty(cv_glassDataView.Rows[0].Cells[0].Value.ToString()) ||
               cv_glassDataView.Rows[0].Cells[0].Value.ToString().Substring(0, 1) != buffer.cv_Data.PSlot1Type.ToString().Substring(0, 1))
            {
                cv_glassDataView.Rows[0].Cells[0].Value = buffer.cv_Data.PSlot1Type.ToString().Substring(0, 1) + ":1";
            }
            if (cv_glassDataView.Rows[1].Cells[0].Value == null || string.IsNullOrEmpty(cv_glassDataView.Rows[1].Cells[0].Value.ToString()) ||
               cv_glassDataView.Rows[1].Cells[0].Value.ToString().Substring(0, 1) != buffer.cv_Data.PSlot2Type.ToString().Substring(0, 1))
            {
                cv_glassDataView.Rows[1].Cells[0].Value = buffer.cv_Data.PSlot2Type.ToString().Substring(0, 1) + ":2";
            }
            if (cv_glassDataView.Rows[2].Cells[0].Value == null || string.IsNullOrEmpty(cv_glassDataView.Rows[2].Cells[0].Value.ToString()) ||
               cv_glassDataView.Rows[2].Cells[0].Value.ToString().Substring(0, 1) != buffer.cv_Data.PSlot3Type.ToString().Substring(0, 1))
            {
                cv_glassDataView.Rows[2].Cells[0].Value = buffer.cv_Data.PSlot3Type.ToString().Substring(0, 1) + ":3";
            }
            if (cv_glassDataView.Rows[3].Cells[0].Value == null || string.IsNullOrEmpty(cv_glassDataView.Rows[3].Cells[0].Value.ToString()) ||
               cv_glassDataView.Rows[3].Cells[0].Value.ToString().Substring(0, 1) != buffer.cv_Data.PSlot4Type.ToString().Substring(0, 1))
            {
                cv_glassDataView.Rows[3].Cells[0].Value = buffer.cv_Data.PSlot4Type.ToString().Substring(0, 1) + ":4";
            }
            if (cv_glassDataView.Rows[4].Cells[0].Value == null || string.IsNullOrEmpty(cv_glassDataView.Rows[4].Cells[0].Value.ToString()) ||
               cv_glassDataView.Rows[4].Cells[0].Value.ToString().Substring(0, 1) != buffer.cv_Data.PSlot5Type.ToString().Substring(0, 1))
            {
                cv_glassDataView.Rows[4].Cells[0].Value = buffer.cv_Data.PSlot5Type.ToString().Substring(0, 1) + ":5";
            }
            if (cv_glassDataView.Rows[5].Cells[0].Value == null || string.IsNullOrEmpty(cv_glassDataView.Rows[5].Cells[0].Value.ToString()) ||
               cv_glassDataView.Rows[5].Cells[0].Value.ToString().Substring(0, 1) != buffer.cv_Data.PSlot6Type.ToString().Substring(0, 1))
            {
                cv_glassDataView.Rows[5].Cells[0].Value = buffer.cv_Data.PSlot6Type.ToString().Substring(0, 1) + ":6";
            }
        }
        public void refresh(BufferData m_DataMap )
        {
            int slot = 0;
            CheckUiSlotType();
            if (m_DataMap != null)
            {
                for (int i = 0; i < cv_SlotCount ; i++)
                {
                    match = (Regex.Match(cv_glassDataView.Rows[i].Cells[0].Value.ToString(), @"[0-9]", RegexOptions.IgnoreCase));
                    if (match.Success)
                    {
                        slot = Convert.ToInt32(match.Value);
                    }
                    else
                    {
                        continue;
                    }

                    GlassData glass_data = null;
                    bool has_sensor = false;
                    if (m_DataMap.GetSlotData(slot, ref glass_data, ref has_sensor))
                    {
                        if (glass_data != null)
                        {
                            if(glass_data.PHasData)
                            cv_glassDataView.Rows[i].Cells[1].Value = glass_data.PId;
                            else
                            cv_glassDataView.Rows[i].Cells[1].Value = "";
                        }
                        else
                        {
                            cv_glassDataView.Rows[i].Cells[1].Value = "";
                        }
                        CommonStaticData.ChangeDataViewItemColor(ref cv_glassDataView, i, glass_data);
                    }
                    else
                    {
                        UiForm.WriteLog( LogLevelType.Error , "Buffer UI refresh ERROR. Slot Un-match");
                    }
                }
            }
            cv_glassDataView.ClearSelection();
        }

        private void cv_glassDataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cv_glassDataView.Rows[e.RowIndex].Selected = false;
        }
        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem tmp = sender as ToolStripItem;
            int slot = Convert.ToInt32(tmp.Text.Trim());
            UiForm.cv_GlassDataForm.Register(ActionTarget.Buffer, cv_Id, slot);
            UiForm.cv_GlassDataForm.Show();
        }

    }
}
