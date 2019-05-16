using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using CIM.Comm;

namespace CIM
{
    class Port : Obj
    {
        public PortData cv_Data = null;
        public PortComm cv_Comm = null;
        public Port(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitComm();
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
    }
}
