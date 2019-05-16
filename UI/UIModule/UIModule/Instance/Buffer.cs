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
    class Buffer : Obj
    {
        public BufferComm cv_Comm = null;
        public BufferData cv_Data = null;
        public Buffer(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitUI();
            InitComm();
        }
        protected override void InitUI()
        {
            if (cv_Ui == null)
            {
                cv_Ui = new BufferUI(cv_Id, cv_SlotCount);
            }
        }
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new BufferComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new BufferData(cv_Id, cv_SlotCount);
            }
        }
        public override void reFresh()
        {
            (cv_Ui as BufferUI).refresh(cv_Data);
        }
    }
}
