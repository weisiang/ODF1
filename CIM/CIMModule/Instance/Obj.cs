using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData;
using KgsCommon;
using CIM.Comm;
using System.Drawing;

namespace CIM
{
    class Obj
    {
        public int cv_Id = 0;
        public int cv_SlotCount = 0;
        public Obj(int m_Id, int m_slotCount)
        {
            cv_Id = m_Id;
            cv_SlotCount = m_slotCount;
        }
        protected virtual void InitUI()
        {
        }
        protected virtual void InitComm()
        {
        }
        protected virtual void InitData()
        {
        } 
    }
}
