﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using LGC.Comm;
using KgsCommon;
using BaseAp;

namespace LGC 
{
    class Port : Obj
    {
        public PortData cv_Data = null;
        public PortComm cv_Comm = null;
        private KDateTime cv_LDRQTime = SysUtils.Now();
        public Port(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount ) 
        {
            InitData();
            InitComm();
        }
        public CommonData.HIRATA.PortStaus PPortStatus
        {
            get { return (CommonData.HIRATA.PortStaus)cv_Data.cv_PortStatus; }
            set 
            { 
                if(cv_Data.cv_PortStatus != (UInt16)value )
                {
                    cv_Data.cv_PortStatus = (UInt16)value;
                    SendDataViaMmf();
                    if (value == PortStaus.UDRQ && this.cv_Data.PPortMode == PortMode.Unloader && LgcForm.PSystemData.PSystemOnlineMode == OnlineMode.Control)
                    {
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = Alarmtable.UnloadPortIsUDRQWhenOnlineMode.ToString();
                        alarm.PMainDescription = "Unload Port Is UDRQ When OnlineMode";
                        alarm.PUnit = 0;
                        alarm.PLevel = AlarmLevele.Light;
                        alarm.PStatus = AlarmStatus.Occur;
                        alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                        LgcForm.EditAlarm(alarm);
                    }
                }
            }
        }
        public LotStatus PLotStatus
        {
            get { return (CommonData.HIRATA.LotStatus)cv_Data.cv_LotStatus; }
            set
            {
                if ((UInt16)value != cv_Data.cv_LotStatus)
                {
                    cv_Data.cv_LotStatus = (UInt16)value;
                    SendDataViaMmf();
                    if(value == LotStatus.MappingEnd)
                    {
                        SysUtils.Sleep(500);
                        if(LgcForm.PSystemData.PSystemOnlineMode == OnlineMode.Offline)
                        {
                            if(LgcForm.PSystemData.PONT)
                            {
                                GetOntModeData();
                            }
                            else
                            {
                                SendShowMonitor();
                            }
                        }
                        else
                        {
                            //TODO ask bc data.
                        }
                    }
                }
            }
        }
        public PortClamp PClamp
        {
            get { return cv_Data.PPortClamp; }
            set 
            {
                if(cv_Data.PPortClamp != value)
                {
                    cv_Data.PPortClamp = value; 
                    SendDataViaMmf();
                }
            }
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
        public override void SendDataViaMmf()
        {
            this.cv_Data.GlassDataMap = this.cv_Data.GlassDataMap;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.PortData).Name, this.cv_Data , KgsCommon.KParseObjToXmlPropertyType.Field);
            LgcForm.WritePortToPlc(this.cv_Id);
            cv_Data.SaveToFile();
        }
        public void SendShowMonitor()
        {
            CommonData.HIRATA.MDPopMonitorForm obj = new MDPopMonitorForm();
            obj.PortId = this.cv_Id;
            obj.PType = MmfEventClientEventType.etRequest;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDPopMonitorForm).Name, obj , KgsCommon.KParseObjToXmlPropertyType.Field);
        }
        public KDateTime PLDRQTime
        {
            get { return cv_LDRQTime; }
            set { cv_LDRQTime = value; }
        }
        public override bool CanAccess(bool m_IsLoad, int m_Slot, bool m_IsExchange = false)
        {
            bool rtn = false;
            if (IsSlotValid(m_Slot))
            {
                if (m_IsLoad)
                {
                    if (!cv_Data.GlassDataMap[m_Slot].PHasData && !cv_Data.GlassDataMap[m_Slot].PHasSensor)
                    {
                        rtn = true;
                    }
                }
                else
                {
                    if (cv_Data.GlassDataMap[m_Slot].PHasData && cv_Data.GlassDataMap[m_Slot].PHasSensor)
                    {
                        rtn = true;
                    }
                }
            }
            return rtn;
        }
        public void GetOntModeData()
        {
            for (int i = 1; i <= this.cv_Data.cv_SlotCount; i++)
            {
                if (this.cv_Data.GlassDataMap[i].PHasSensor)
                {
                    this.cv_Data.GlassDataMap[i] = new GlassData();
                    this.cv_Data.GlassDataMap[i].cv_SlotInEq = (uint)i;// = new GlassData();
                    this.cv_Data.GlassDataMap[i].PHasSensor = true;// = (uint)i;
                    this.cv_Data.GlassDataMap[i].PWorkSlot = (uint)i;
                    this.cv_Data.GlassDataMap[i].PCimMode = LgcForm.PSystemData.PSystemOnlineMode;
                    this.cv_Data.GlassDataMap[i].PWorkType = CommonData.HIRATA.WorkType.Test;
                    this.cv_Data.GlassDataMap[i].PFoupSeq = 100;
                    this.cv_Data.GlassDataMap[i].PId = "ONT" + cv_Id.ToString() + "Glass" + i.ToString();
                    this.cv_Data.GlassDataMap[i].PProcessFlag = CommonData.HIRATA.ProcessFlag.Need;
                    //this.cv_Data.GlassDataMap[i].PProductionCategory = CommonData.HIRATA.ProductCategory.Glass;
                    this.cv_Data.GlassDataMap[i].PProductionCategory = this.cv_Data.PProductionType;
                    for (int j = 0; j < 15; j++)
                    {
                        this.cv_Data.GlassDataMap[i].cv_Nods[j].PProcessAbnormal = false;
                        this.cv_Data.GlassDataMap[i].cv_Nods[j].PProcessHistory = 0;
                        this.cv_Data.GlassDataMap[i].cv_Nods[j].PRecipe = 0;
                    }
                }
                else
                {
                    this.cv_Data.GlassDataMap[i] = new GlassData();
                    this.cv_Data.GlassDataMap[i].cv_SlotInEq = (uint)i;// = new GlassData();
                }
            }
            LgcForm.cv_InProcessPort.Add(cv_Id);
            this.PLotStatus = LotStatus.Process;
            this.cv_Data.SaveToFile();
        }
        public bool IsHasAnyDataAndSensor()
        {
            bool rtn = false;
            for (int i = 1; i <= cv_SlotCount; i++)
            {
                if (cv_Data.GlassDataMap[1].PHasData || cv_Data.GlassDataMap[1].PHasSensor)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
    }
}
