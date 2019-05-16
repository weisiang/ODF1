using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.GUI;
using UI.Comm;
using CommonData.HIRATA;

namespace UI
{
    class RobotJobPath : Obj
    {
        public RobotJobPath(int m_Id, int m_SlotCount)
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
                cv_Ui = new RobotJobPathUI();
            }
        }
        public RobotJobPathUI GetUI()
        {
            return cv_Ui as RobotJobPathUI;
        }
        public void Refresh(List<RobotJob> m_DataQueue)
        {
        }
    }
}
