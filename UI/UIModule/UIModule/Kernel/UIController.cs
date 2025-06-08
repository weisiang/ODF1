using System;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;
using System.Text.RegularExpressions;
using System.Collections;
using CommonData.HIRATA;
using BaseAp;

namespace UI
{
    class UIController : BaseMmfController
    {
        public delegate void DeleAppEvent(string m_MessageId, object m_Object);
        public static event DeleAppEvent EventAppEvent;

        public static UIController g_Controller = null;

        public UIController() : base ("CommonData.HIRATA")
        {
            this.Open();
            AssignProcessFunctions();
            g_Controller = this;
        }
        ~UIController()
        {
        }
        protected override void AssignProcessFunctions()
        {
            base.AssignProcessFunctions();
            //common
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.PortData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.RobotData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.BufferData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.AlignerData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.EqData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction( typeof(CommonData.HIRATA.MDShowMsg).Name , ProcessMsg);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDPopMonitorForm).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDPopOpidForm).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotjobPath).Name, ProcessMmfEvent);

            //by case , please remove following define. And Add by csae code.
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCMsg).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDEfemStatus).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDEfemStatusSingle).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDTimeChartChange).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotjobType).Name, ProcessMmfEvent);
        }

        #region base
        protected override void ProcessAccountChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAccountChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessLogInOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessLogInOut(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        
        protected override void ProcessSamplingDataChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSamplingDataChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAccountReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAccountReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessOnlineReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessOnlineReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessInitialize(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessInitialize(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSetTimeOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSetTimeOut(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSystemData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSystemData(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessTimeoutData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessTimeoutData(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessOperatorModeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessOperatorModeChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessGlassCountData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessGlassCountData(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRobotAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRobotAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessChangePortSlotType(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRobotAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessShowOcrDecide(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRobotAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            ProcessMmfEvent(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        #endregion

        void ProcessMmfEvent(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "Recv : " + m_MessageId + "From : " + m_SourceModule + Environment.NewLine;
            if (EventAppEvent != null)
            {
                log += "Exe UI Event";
                EventAppEvent(m_MessageId, m_Object);
            }
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessMsg(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDShowMsg obj = m_Object as CommonData.HIRATA.MDShowMsg;
            CommonData.HIRATA.Msg msg_item = obj.Msg;
            CommonStaticData.PopForm(msg_item.Txt, msg_item.PAutoClean, msg_item.PUserRep, m_Ticket, msg_item.TimeOut);
            /*
            for (int i = 0; i < obj.Msgs.Count; ++i)
            {
                CommonData.HIRATA.Msg msg_item = obj.Msgs[i];

                msg_item.TimeOut = (msg_item.TimeOut == 0 ? msg_item.TimeOut * 1000 : 10 * msg_item.TimeOut);

                CommonStaticData.PopForm(msg_item.Txt, msg_item.PAutoClean, msg_item.PUserRep, m_Ticket, msg_item.TimeOut);

                log += "PopForm : " + msg_item.Txt + " AutoClean :" + msg_item.PAutoClean.ToString() + " Time : " + msg_item.TimeOut.ToString() + Environment.NewLine;
            }
            */
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        public void SendEqDataReq(params Int32[] m_Id)
        {
            //WriteIn
            CommonData.HIRATA.MDEqDataReq obj = new CommonData.HIRATA.MDEqDataReq();
            CommonData.HIRATA.Eq tmp = new CommonData.HIRATA.Eq();
            for (int i = 0; i < m_Id.Length; i++)
            {
                tmp.Id = m_Id[i];
                obj.Eqs.Add(tmp);
            }

            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            string rtn;
            object rtn_tmp = null;
            uint ticket;
            string log = "";
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDEqDataReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
            {
                log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDEqDataReq).Name;
            }
            if (!string.IsNullOrEmpty(log))
                WriteLog(LogLevelType.General, log);

            //WriteOut
        }
        public void SendRobotDataReq(params Int32[] m_Id)
        {
            //WriteIn
            string log = "Send : " + typeof(CommonData.HIRATA.MDRobotDataReq).Name + Environment.NewLine;
            CommonData.HIRATA.MDRobotDataReq obj = new CommonData.HIRATA.MDRobotDataReq();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            for (int i = 0; i < m_Id.Length; i++)
            {
                log += "Robot : " + m_Id[i].ToString() + Environment.NewLine;
                obj.Robots.Add(Convert.ToUInt16(m_Id[i]));
            }
            string rtn;
            object rtn_tmp = null;
            uint ticket;
            //Global.Controller.SendMmfRequestObject(typeof(CommonData.MDRobotStatus).Name, obj);

            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDRobotDataReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
            {
                log += "[Time out] Wait : " + typeof(CommonData.HIRATA.MDRobotDataReq).Name;
            }
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        public void SendPortDataReq(params Int32[] m_Id)
        {
            //WriteIn
            string log = "Send : " + typeof(CommonData.HIRATA.MDPortDataReq).Name + Environment.NewLine;
            CommonData.HIRATA.MDPortDataReq obj = new CommonData.HIRATA.MDPortDataReq();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            for (int i = 0; i < m_Id.Length; i++)
            {
                log += "Port : " + m_Id[i].ToString() + Environment.NewLine;
                obj.Ports.Add(Convert.ToUInt16(m_Id[i]));
            }
            string rtn;
            object rtn_tmp = null;
            uint ticket;

            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDPortDataReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
            {
                log += "[Time out] Wait : " + typeof(CommonData.HIRATA.MDPortDataReq).Name;
            }
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        public static void SendOpidReply(CommonData.HIRATA.Result m_Result, int m_PortId, string m_Opid, string m_CstSeq, uint m_ticket)
        {
            //WriteIn
            CommonData.HIRATA.MDPopOpidForm obj = new CommonData.HIRATA.MDPopOpidForm();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
            obj.PortId = m_PortId;
            CommonData.HIRATA.ReplyData rtn = new CommonData.HIRATA.ReplyData();
            rtn.CstSeq = m_CstSeq;
            rtn.OpId = m_Opid;
            obj.Reply = rtn;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDPopOpidForm).Name, obj, 100, typeof(CommonData.HIRATA.MDPopOpidForm).Name, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        public static void SendMonitorReply(int m_PortId, CommonData.HIRATA.Result m_Result)
        {
            //WriteIn
            CommonData.HIRATA.MDPopMonitorFormRep obj = new CommonData.HIRATA.MDPopMonitorFormRep();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
            obj.PortId = m_PortId;
            obj.PResult = m_Result;
            //obj.Port.Add(Form1.GetPort(m_PortId).cv_Data);
            obj.PortData = UiForm.GetPort(m_PortId).cv_Data;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDPopMonitorFormRep).Name, obj, 100, typeof(CommonData.HIRATA.MDPopMonitorFormRep).Name, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        public void SendRobotActionReq(int m_RobotId, CommonData.HIRATA.RobotAction m_Action, CommonData.HIRATA.RobotArm m_Arm, CommonData.HIRATA.ActionTarget m_Target, int m_TargetId, int m_TargetSlot, bool m_UseHS = false , bool m_IsAlignerExch=false , string m_AlignerDeg="")
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string log = "";
            CommonData.HIRATA.MDRobotAction obj = new CommonData.HIRATA.MDRobotAction();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            obj.PAction = m_Action;
            obj.RobotId = m_RobotId;
            obj.Source.PArm = m_Arm;
            obj.Source.PTarget = m_Target;
            obj.Source.Id = m_TargetId;
            obj.Source.Slot = m_TargetSlot;
            obj.PUseHS = m_UseHS;
            obj.cv_AlignerDeg = m_AlignerDeg.Trim();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            log += "Robot id : " + m_RobotId + " Action : " + m_Action.ToString() + " Arm : " + m_Arm.ToString() + " Target : " + m_Target.ToString() + " TargetId : " + m_TargetId + " Slot : " + m_TargetSlot + Environment.NewLine;

            bool result = SendRobotAction(obj, MmfEventClientEventType.etRequest, true);
            if (!result)
            {
                CommonStaticData.PopForm("Robot Action command Time out", false, false);
                WriteLog(LogLevelType.Error, "Robot Action command Time out");
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public void SendProgramStart()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendSystemDataReq(MmfEventClientEventType.etRequest, true);
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_EqNumber; i++)
            {
                SendEqDataReq(i);
            }
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_RobotNumber; i++)
            {
                SendRobotDataReq(i);
            }

            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                SendPortDataReq(i);
            }
            if(!SendRecipeReq(MmfEventClientEventType.etRequest, true))
            {
                WriteLog(LogLevelType.Warning, "SendRecipeReq time out");
            }
            if(!SendSamplingDataReq(MmfEventClientEventType.etRequest, true))
            {
                WriteLog(LogLevelType.Warning, "SendSamplingDataReq time out");
            }
            if (!SendAlarmReq(MmfEventClientEventType.etRequest, true))
            {
                WriteLog(LogLevelType.Warning, "SendAlarmReq time out");
            }
            if(!SendTimeOutReq(MmfEventClientEventType.etRequest, true))
            {
                WriteLog(LogLevelType.Warning, "SendTimeOutReq time out");
            }
            if (!SendGlassCountReq(MmfEventClientEventType.etRequest, true))
            {
                WriteLog(LogLevelType.Warning, "SendGlassCountReq time out");
            }
            SendAccountData();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}
