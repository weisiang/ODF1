using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using CIM.Comm;

namespace CIM
{
    class Robot : Obj
    {
        public RobotComm cv_Comm = null;
        public RobotData cv_Data = null;
        public Robot(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitComm();
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
    }
}
