using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;

namespace Common
{
    public class PortData
    {
        public Dictionary<int, Common.GlassData> cv_GlassDataList = new Dictionary<int, Common.GlassData>();
        MgvForm cv_MgvFom = null;
        Form_CstDataConfirm cv_MoitorForm = null;
        public string cv_LotIt = "";
        public CommonData.PortHasCst PortHasCst
        {
            get;
            set;
        }
        public CommonData.PortClamp PortClamp
        {
            get;
            set;
        }
        public int Id
        {
            get;
            set;
        }
        public CommonData.PortAgv PortAgv
        {
            get;
            set;
        }
        public CommonData.PortEnable PortEnable
        {
            get;
            set;
        }
        public CommonData.PortType PortType
        {
            get;
            set;
        }
        public CommonData.PortMode PortMode
        {
            get;
            set;
        }
        public CommonData.LotStatus LortStatus
        {
            get;
            set;
        }
        public CommonData.PortStaus PortStatus
        {
            get;
            set;
        }
        public PortData(int m_id, CommonData.PortStaus m_portStatus, CommonData.LotStatus m_lotStatus, CommonData.PortMode m_portMode, CommonData.PortType m_portType, CommonData.PortEnable m_enable, CommonData.PortAgv m_agv)
        {
            Id = m_id;
            PortAgv = m_agv;
            PortEnable = m_enable;
            PortMode = m_portMode;
            PortStatus = m_portStatus;
            PortType = m_portType;
            cv_MgvFom = new MgvForm(Id);
            cv_MoitorForm = new Form_CstDataConfirm();
        }
        public PortData(int m_id , int m_MaxSlot )
        {
            Id = m_id;
            PortAgv = CommonData.PortAgv.None;
            PortEnable = CommonData.PortEnable.None;
            PortMode = CommonData.PortMode.None;
            PortStatus = CommonData.PortStaus.None;
            PortType = CommonData.PortType.None;
            cv_MgvFom = new MgvForm(Id);
            cv_MoitorForm = new Form_CstDataConfirm();
            for (int i = 1; i <= CommonData.CommonStaticData.g_CstSize; i++)
            {
                cv_GlassDataList.Add(i, null);
            }
        }
        public void InitializeGlassList()
        {
            if (cv_GlassDataList != null)
            {
                cv_GlassDataList.Clear();
            }
            for (int i = 1; i <= CommonData.CommonStaticData.g_CstSize; i++)
            {
                cv_GlassDataList.Add(i, null);
            }
        }
        public void ShowMgvForm()
        {
            if(cv_MgvFom != null)
            {
                cv_MgvFom.Show();
            }
        }
        public void ShowMonitorForm()
        {
            if(cv_MoitorForm != null)
            {
                cv_MoitorForm.Show();
            }
        }
    }
}
