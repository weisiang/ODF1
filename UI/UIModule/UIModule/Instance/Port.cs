using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using UI.GUI;
using UI.Comm;

namespace UI
{
    class Port : Obj
    {
        public PortData cv_Data = null;
        public PortComm cv_Comm = null;
        private MgvForm cv_MgvForm = null;
        private CstDataConfirm cv_CstDataConfirm = null;
        public Port(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitUI();
            InitComm();
            cv_MgvForm = new MgvForm(cv_Id);
            cv_CstDataConfirm = new CstDataConfirm(m_Id);
        }
        protected override void InitUI()
        {
            if (cv_Ui == null)
            {
                cv_Ui = new PortUI(cv_Id, cv_SlotCount);
            }
        }
        public  void ChangeUISlotNumber(int m_Slots)
        {
            if (cv_Ui != null)
            {
                cv_Ui = null;
                cv_Ui = new PortUI(cv_Id, m_Slots);
            }
            else
            {
                cv_Ui = new PortUI(cv_Id, m_Slots);
            }
        }
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new PortComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new PortData(cv_Id, cv_SlotCount);
            }
        }
        public void ShowMgvForm()
        {
            if(!cv_MgvForm.Visible)
            {
                cv_MgvForm.Show();
            }
        }
        public void ShowCstDataConfirm()
        {
            if (!cv_CstDataConfirm.Visible)
            {
                cv_CstDataConfirm.Registe();
                cv_CstDataConfirm.Show();
            }
        }
        public override void reFresh()
        {
            (cv_Ui as PortUI).refresh(cv_Data);
        }
    }
}
