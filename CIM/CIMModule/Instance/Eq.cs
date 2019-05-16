using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIM.Comm;
using CommonData.HIRATA;

namespace CIM
{
    class Eq : Obj
    {
        public EqComm cv_Comm = null;
        public EqData cv_Data = null;
        string cv_Alias = "";
        public Eq(int m_Id, int m_SlotCount , string m_alias = "")
            : base(m_Id, m_SlotCount)
        {
            cv_Alias = m_alias;
            InitData();
            InitComm();
        }
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new EqComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new EqData(cv_Id, cv_SlotCount);
            }
        }
    }

}
