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
    class Robot : Obj
    {
        public RobotComm cv_Comm = null;
        public RobotData cv_Data = null;
        public Robot(int m_Id, int m_SlotCount , RobotUI.RobotGlassShape m_UpShage, RobotUI.RobotGlassShape m_DownShape)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitUI(m_Id, m_SlotCount, m_UpShage, m_DownShape);
            InitComm();
        }
        private void InitUI(int m_Id, int m_SlotCount, RobotUI.RobotGlassShape m_UpShage, RobotUI.RobotGlassShape m_DownShape)
        {
            if (cv_Ui == null)
            {
                cv_Ui = new RobotUI(cv_Id, cv_SlotCount, m_UpShage, m_DownShape);
            }
        }
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new RobotComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new RobotData(cv_Id, cv_SlotCount);
            }
        }
        public override void reFresh()
        {
            (cv_Ui as RobotUI).refresh(cv_Data);
        }

    }
}
