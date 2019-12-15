using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData.HIRATA;
using UI;

namespace UI.GUI
{
    public partial class RobotJobPathUI : UserControl
    {
        public RobotJobPathUI()
        {
            InitializeComponent();
            this.gdv_PathData.AutoGenerateColumns = false;
            UiForm.AddUiObjToEnableList(button2, UiForm.enumGroup.Group2);
            UiForm.AddUiObjToEnableList(btn_DropTop, UiForm.enumGroup.Group2);
        }
        public void Refresh(List<RobotJob> m_DataQueue)
        {
            this.gdv_PathData.DataSource = m_DataQueue.ToList();
            this.gdv_PathData.Refresh();
        }

        private void btn_preVirw_Click(object sender, EventArgs e)
        {
            UiForm.cv_MmfController.SendRobotJobAction(RobotJobAction.preView);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(UiForm.PSystemData.POperationMode == OperationMode.Manual)
            {
                UiForm.cv_MmfController.SendRobotJobAction(RobotJobAction.Clean);
            }
            else
            {
                CommonStaticData.PopForm("Please change to manual mode", true, false);
            }
        }
        public void SetButtonDisable()
        {
            btn_preVirw.Visible = false;
            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UiForm.PSystemData.POperationMode == OperationMode.Manual)
            {
                UiForm.cv_MmfController.SendRobotJobAction(RobotJobAction.DropTop);
            }
            else
            {
                CommonStaticData.PopForm("Please change to manual mode", true, false);
            }
        }
    }
}
