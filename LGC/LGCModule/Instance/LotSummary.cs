using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UI
{
    class LotSummary : Obj
    {
        public LotSummary(int m_Id, int m_SlotCount, BindingList<CommonData.PortData> m_list)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitUI(m_list);
            InitComm();
        }
        private void InitUI(BindingList<CommonData.PortData> m_list)
        {
            if (cv_Ui == null)
            {
                cv_Ui = new LotSummaryUI(m_list);
            }
        }
        protected override void InitComm()
        {
        }
        protected override void InitData()
        {
        }
        public override void reFresh()
        {
            cv_Ui.Refresh();
        }

    }
}
