using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CommonData.HIRATA;
using KgsCommon;
using BaseAp;

namespace UI
{
    public partial class LotSummaryUI : UserControl
    {
        public LotSummaryUI(BindingList<CommonData.HIRATA.PortData> m_list)
        {
            InitializeComponent();
            //SetLotSummaryGrid();
            dataGrid_LotSummary.AutoGenerateColumns = false;
            GridBinding(m_list);
        }
        internal void GridBinding(BindingList<CommonData.HIRATA.PortData> m_list)
        {
            dataGrid_LotSummary.DataSource = m_list;
        }
        public void reFresh()
        {
            dataGrid_LotSummary.Refresh();
        }
        private void SetLotSummaryGrid()
        {
            DataGridViewTextBoxColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.DataPropertyName = "Id";
            IdColumn.HeaderText = "Id";
            IdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            IdColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(IdColumn);

            DataGridViewTextBoxColumn AgvColumn = new DataGridViewTextBoxColumn();
            AgvColumn.DataPropertyName = "PortAgv";
            AgvColumn.HeaderText = "AGV";
            AgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AgvColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(AgvColumn);

            DataGridViewTextBoxColumn PortEnableColumn = new DataGridViewTextBoxColumn();
            PortEnableColumn.DataPropertyName = "PortEnable";
            PortEnableColumn.HeaderText = "Enable";
            PortEnableColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortEnableColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortEnableColumn);

            DataGridViewTextBoxColumn PortTypeColumn = new DataGridViewTextBoxColumn();
            PortTypeColumn.DataPropertyName = "PortType";
            PortTypeColumn.HeaderText = "Type";
            PortTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortTypeColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortTypeColumn);

            DataGridViewTextBoxColumn PortModeColumn = new DataGridViewTextBoxColumn();
            PortModeColumn.DataPropertyName = "PortMode";
            PortModeColumn.HeaderText = "PortMode";
            PortModeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortModeColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortModeColumn);

            DataGridViewTextBoxColumn LortStatusColumn = new DataGridViewTextBoxColumn();
            LortStatusColumn.DataPropertyName = "LortStatus";
            LortStatusColumn.HeaderText = "LortStatus";
            LortStatusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            LortStatusColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(LortStatusColumn);

            DataGridViewTextBoxColumn PortStatusColumn = new DataGridViewTextBoxColumn();
            PortStatusColumn.DataPropertyName = "PortStatus";
            PortStatusColumn.HeaderText = "PortStatus";
            PortStatusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortStatusColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortStatusColumn);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int portid = getSeletPort();
            if (portid != 0)
            {
                if (canChangeType(portid))
                {
                    //send change port type msg to LCG.
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int portid = getSeletPort();
            if (portid != 0)
            {
                if (canChangeType(portid))
                {
                    //send change port type msg to LCG.
                }
            }
        }
        private int getSeletPort()
        {
            int portid = 0;
            if(dataGrid_LotSummary.CurrentRow != null)
            {
                string port_id = dataGrid_LotSummary.CurrentRow.Cells[0].Value.ToString();
                if(int.TryParse(port_id , out portid))
                {
                }
            }
            return portid;
        }
        private bool canChangeType(int portId)
        {
            bool rtn = false;
            Port port = UiForm.GetPort(portId);
            if( (port.cv_Data.PPortHasCst == PortHasCst.Empty) ||
                (port.cv_Data.PPortHasCst == PortHasCst.Has && (port.cv_Data.PPortStatus == PortStaus.LDRQ || port.cv_Data.PPortStatus == PortStaus.UDCM || port.cv_Data.PPortStatus == PortStaus.UDRQ))
                )
            {
                rtn = true;
            }
            return rtn;
        }


        private void dataGrid_LotSummary_CellClick(object sender, DataGridViewCellEventArgs e)
        {//dataGrid_LotSummary
            /*
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (e.RowIndex == -1) return;
            string select_str = dataGrid_LotSummary.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            int port_id = 0;
            if(int.TryParse(select_str , out port_id))
            {
                Port port = UiForm.GetPort(port_id);
                if(port != null)
                {
                    
                }
            }
            if (-1 != index)
            {
                textBox1.Text = UiForm.cv_SamplingData.cv_SamplingList[index].PNo.ToString();
                textBox2.Text = UiForm.cv_SamplingData.cv_SamplingList[index].PDesription;
                //clean all.
                for(int i=0; i< checkedListBox1.Items.Count;i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                //set true.
                for(int i=0; i< checkedListBox1.Items.Count;i++)
                {
                    string tmp = checkedListBox1.Items[i].ToString();
                    Match match = Regex.Match(tmp, @"\d+");
                    if(match.Success)
                    {
                        string str_slot = match.Value;
                        int slot = -1;
                        if(int.TryParse(str_slot , out slot))
                        {
                            if(UiForm.cv_SamplingData.cv_SamplingList[index].PHitList.Contains(slot))
                            {
                                checkedListBox1.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            */
        }
    }
}
