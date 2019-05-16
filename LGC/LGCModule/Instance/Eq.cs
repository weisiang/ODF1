﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGC.Comm;
using CommonData.HIRATA;
using KgsCommon;
using BaseAp;

namespace LGC 
{
    class Eq : Obj 
    {
        public EqComm cv_Comm = null;
        public EqData cv_Data = null;
        public int cv_Node = 0;
        public string cv_Alias = "";
        public int cv_WriteToEqAddup = 0;
        public int cv_WriteToEqAdddown = 0;
        public int cv_ReadFromEqAddup = 0;
        public int cv_ReadFromEqAdddown = 0;
        RobotArm cv_GetArm = RobotArm.rabNone;
        RobotArm cv_PutArm = RobotArm.rabNone;
        public Eq(int m_Id, int m_Node, int m_SlotCount, RobotArm m_GetArm, RobotArm m_PutArm, string m_alias = "")
            : base(m_Id, m_SlotCount )
        {
            cv_GetArm = m_GetArm;
            cv_PutArm = m_PutArm;
            cv_Alias = m_alias;
            cv_Node = m_Node;
            InitData();
            InitComm();
            if(m_Id == (int)EqId.SDP1)
            {
                cv_WriteToEqAddup = 0x3200;
                cv_ReadFromEqAddup = 0x3A00;
            }
            else if (m_Id == (int)EqId.SDP2)
            {
                cv_WriteToEqAddup = 0x3240;
                cv_ReadFromEqAddup = 0x4200;
            }
            else if (m_Id == (int)EqId.IJP)
            {
                cv_WriteToEqAddup = 0x3280;
                cv_ReadFromEqAddup = 0x4A00;
            }
            else if (m_Id == (int)EqId.VAS)
            {
                cv_WriteToEqAddup = 0x32c0;
                cv_ReadFromEqAddup = 0x5200;
                cv_WriteToEqAdddown = 0x3300;
                cv_ReadFromEqAdddown = 0x5240;
            }
            else if (m_Id == (int)EqId.UV_1)
            {
                cv_WriteToEqAddup = 0x3340;
                cv_ReadFromEqAddup = 0x5A00;
            }
            else if (m_Id == (int)EqId.SDP3)
            {
                cv_WriteToEqAddup = 0x3380;
                cv_ReadFromEqAddup = 0x6A00;
            }
            else if (m_Id == (int)EqId.AOI)
            {
                cv_WriteToEqAddup = 0x33c0;
                cv_ReadFromEqAddup = 0x7200;
            }
            else if (m_Id == (int)EqId.UV_2)
            {
                cv_WriteToEqAddup = 0x3400;
                cv_ReadFromEqAddup = 0x7A00;
            }
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
        public int GetTimeChatCurStep(int m_Slot)
        {
            int step = 0;
            if (cv_Id == 4)
            {
                if (m_Slot == 1)
                {
                    step = LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(cv_Comm.cv_TimeChatId + m_Slot).GetCurrentStep(cv_Comm.cv_TimeChatId + m_Slot);
                }
                else if (m_Slot == 2)
                {
                    step = LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(cv_Comm.cv_TimeChatId).GetCurrentStep(cv_Comm.cv_TimeChatId);
                }
            }
            else
            {
                step = LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(cv_Comm.cv_TimeChatId).GetCurrentStep(cv_Comm.cv_TimeChatId);
            }
            return step;
        }
        public override void SendDataViaMmf()
        {
            this.cv_Data.GlassDataMap = this.cv_Data.GlassDataMap;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.EqData).Name, this.cv_Data, KgsCommon.KParseObjToXmlPropertyType.Field);
        }
        public int  PNode
        {
            get { return cv_Node; }
            set { cv_Node = value; }
        }
        public RobotArm PGetArm
        {
            get { return cv_GetArm; }
            set { cv_GetArm = value; }
        }
        public RobotArm PPutArm
        {
            get { return cv_PutArm; }
            set { cv_PutArm = value; }
        }

        public override bool CanAccess(bool m_IsLoad , int m_Slot , bool m_IsExchange=false)
        {
            bool rtn = false;
            EqInterFaceType gif_type = EqInterFaceType.None;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            //
            int step = GetTimeChatCurStep(m_Slot);
            if (step == TimechartNormal.STEP_ID_ActionReady)
            {
                if (cv_Id == (int)EqId.VAS)
                {
                    if (m_Slot == 1)
                    {
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                        time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    }
                    else if (m_Slot == 2)
                    {
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                        time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    }
                }
                else
                {
                    time_chart_id =LgcForm.GetEqById(cv_Id).cv_Comm.cv_TimeChatId;
                    time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                }
                if (!m_IsExchange)
                {
                    if (m_IsLoad)
                    {
                        gif_type = time_chart_instance.cv_ActionType;
                        if (gif_type == EqInterFaceType.Load) rtn = true;
                    }
                    else
                    {
                        gif_type = time_chart_instance.cv_ActionType;
                        if ((gif_type == EqInterFaceType.Unload) || (gif_type == EqInterFaceType.Exchange)) 
                            rtn = true;
                    }
                }
                else
                {
                    gif_type = time_chart_instance.cv_ActionType;
                    if (gif_type == EqInterFaceType.Exchange)
                    {
                        if (cv_Id != (int)EqId.VAS)
                        {
                            rtn = true;
                        }
                    }
                }
            }
            return rtn;
        }
        public override EquipmentStatus GetStatus()
        {
            int value = 0; 
            EquipmentStatus rtn = EquipmentStatus.None;

            if(cv_Id != (int)EqId.VAS)
            {
                value = LgcForm.cv_Mio.GetPortValue(cv_ReadFromEqAddup + 0x4F);
            }
            else
            {
                value = LgcForm.cv_Mio.GetPortValue(cv_ReadFromEqAddup + 0x8F);
            }
            if(Enum.IsDefined(typeof(EquipmentStatus) , value))
            {
                rtn = (EquipmentStatus)value;
            }
            return rtn;
        }
    }
}
