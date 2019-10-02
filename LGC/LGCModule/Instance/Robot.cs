using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using CommonData.HIRATA;
using KgsCommon;
using LGC.Comm;
using System.Linq;
using BaseAp;
namespace LGC
{
    class Robot : Obj
    {//RobotComm("192.168.1.1" , 48879);
        public bool cv_IsForce = false;
        public int cv_WaitRobotSpeed = 0;
        public int cv_WaitFfuSpeed = 0;
        public Queue<TowerCommand> cv_TowerJobQ = new Queue<TowerCommand>();
        public  Queue<bool> cv_BuzzerQ = new Queue<bool>();
        public bool cv_HadInit = false;
        public bool cv_Initilizing = false;
        private delegate void DeleProcessCommand(CommandData m_Command);
        private Dictionary<APIEnum.CommandType, DeleProcessCommand> cv_ProcessCommandPtr = new Dictionary<APIEnum.CommandType, DeleProcessCommand>();
        private RobotJob cv_CurJob = null;
        public RobotJob CurJob
        {
            get { return cv_CurJob; }
            set { cv_CurJob = value; }
        }
        public RobotComm cv_Comm = null;
        public RobotData cv_Data = null;
        private string cv_Ip;
        private int cv_Port;
        private KTimer cv_GifTimer = null;
        public bool IsBusy
        {
            get { return CurJob != null; }
        }

        public Robot(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitComm();
        }
        public Robot(int m_Id, int m_SlotCount, string m_Ip, int m_Port)
            : base(m_Id, m_SlotCount)
        {
            cv_Ip = m_Ip;
            cv_Port = m_Port;
            InitComm();
            InitData();
            AssignFunciton();
            LinkEvent();
            InitTimer();
            cv_Comm.cv_Controller.Open();
        }

        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new RobotComm(cv_Ip, cv_Port);
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new RobotData(cv_Id, cv_SlotCount);
            }
        }
        private void AssignFunciton()
        {
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.API, ProcessAPICommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Common, ProcessCommonCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.RFID, ProcessRFIDCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.LoadPort, ProcessLoadPortCommand);
            //cv_ProcessCommandPtr.Add(APIEnum.CommandType.E84, ProcessE84Command);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Robot, ProcessRobotCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Aligner, ProcessAlignerCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.IO, ProcessIOCommand);
            //cv_ProcessCommandPtr.Add(APIEnum.CommandType.Alignment, ProcessAlignmentCommand);
            //cv_ProcessCommandPtr.Add(APIEnum.CommandType.Barcode, ProcessBarcodeCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.OCR, ProcessOCRCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Event, ProcessEventCommand);
        }
        private void LinkEvent()
        {
            cv_Comm.cv_Controller.OnRecvEvent += OnRecvCommandReply;
            cv_Comm.cv_Controller.OnRecvTimeOutEvent += OnRobotCommandTimeout;
            cv_Comm.cv_Controller.OnConnectEvent += OnConnect;
            cv_Comm.cv_Controller.OnSendErrorEvent += OnSendError;
            cv_Comm.cv_Controller.OnRecvParseError += OnReplyParseError;
        }
        private void InitTimer()
        {
            if (cv_GifTimer == null)
            {
                cv_GifTimer = new KTimer();
                cv_GifTimer.Interval = 200;
                cv_GifTimer.Open();
                cv_GifTimer.Enabled = true;
                cv_GifTimer.ThreadEventEnabled = true;
                cv_GifTimer.OnTimer += OnGIFTimer;
            }
        }

        #region On Event
        private void OnRecvCommandReply(CommandData m_Command)
        {
            if (m_Command.cv_ReturnCode != 0)
            {
                if (m_Command.PCommandType != APIEnum.CommandType.Event && m_Command.PEventCommand != APIEnum.EventCommand.ERROR)
                {
                    if (!cv_Initilizing)
                    {
                        ApiReplyAbnormal(m_Command);
                        return;
                    }
                    else
                    {
                        if ((m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.ResetError) ||
                            (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.GetStatus) ||
                            (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.Home) ||
                            (m_Command.PCommandType == APIEnum.CommandType.API && m_Command.PApiCommand == APIEnum.APICommand.Remote)
                            )
                        {
                            LgcForm.SendinitCompleteFail();
                            LgcForm.ShowMsg("At Initial , Command :" + m_Command.PCommonCommand.ToString() + "failure!!! Please check and re-initilize", false, false);
                            ApiReplyAbnormal(m_Command);
                            return;
                        }
                    }
                }
            }
            if (m_Command.PCommandType == APIEnum.CommandType.API)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.API](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Common)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Common](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.RFID)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.RFID](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.LoadPort)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.LoadPort](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.E84)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.E84](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Robot)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Robot](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Aligner)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Aligner](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.IO)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.IO](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Alignment)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Alignment](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Barcode)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Barcode](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.OCR)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.OCR](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Event)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Event](m_Command);
            }
        }
        private void OnSendError(string m_CommandTxt, string m_Msg)
        {
            CommonData.HIRATA.MDShowMsg obj = new MDShowMsg();
            CommonData.HIRATA.Msg msg_obj = new Msg();
            msg_obj.PAutoClean = true;
            msg_obj.PUserRep = false;
            msg_obj.TimeOut = 10000;
            msg_obj.Txt = "API Command Send ERROR : " + m_CommandTxt;
            obj.Msg = msg_obj;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDShowMsg).Name, obj, KParseObjToXmlPropertyType.Field);

            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
            alarm.PCode = CommonData.HIRATA.Alarmtable.SendApiComandError.ToString();
            alarm.PLevel = AlarmLevele.Serious;
            alarm.PMainDescription = "Send API Comand Error ( API disconnected maybe)";
            alarm.PStatus = AlarmStatus.Occur;
            alarm.PUnit = 0;
            LgcForm.EditAlarm(alarm);
        }
        private void OnConnect(bool m_Isconnect)
        {
            LgcForm.PSystemData.PRobotConnect = m_Isconnect;
            LgcForm.WriteLog(LogLevelType.General, "Exe OnConnect : " + m_Isconnect.ToString());
            if (m_Isconnect)
            {
                cv_Comm.SetApiCommonCommand(APIEnum.APICommand.CurrentMode);
            }
        }
        private void OnReplyParseError(string m_Txt)
        {
            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
            alarm.PCode = Alarmtable.RobotApiReplyParseError.ToString();
            alarm.PMainDescription = "API Event/Reply parse error : " + m_Txt;
            alarm.PSubDescription = m_Txt;
            alarm.PUnit = 0;
            alarm.PLevel = AlarmLevele.Serious;
            alarm.PStatus = AlarmStatus.Occur;
            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
            LgcForm.EditAlarm(alarm);
        }
        private void OnRobotCommandTimeout(CommandData m_Command)
        {
            AlarmItem alarm = new AlarmItem();
            alarm.PUnit = 0;
            alarm.PCode = CommonData.HIRATA.Alarmtable.SendApiComandT3TimeOut.ToString();
            alarm.PLevel = AlarmLevele.Serious;
            alarm.PMainDescription = "Send API Comand T3 TimeOut : " + m_Command.GetCommandStr();
            alarm.PStatus = AlarmStatus.Occur;
            LgcForm.EditAlarm(alarm);
            if (cv_Initilizing)
            {
                if ((m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.ResetError) ||
                    (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.GetStatus) ||
                    (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.Home) ||
                    (m_Command.PCommandType == APIEnum.CommandType.API && m_Command.PApiCommand == APIEnum.APICommand.Remote)
                    )
                {
                    LgcForm.SendinitCompleteFail();
                    LgcForm.ShowMsg("At Initial , Command :" + m_Command.PCommonCommand.ToString() + "failure!!! Please check and re-initilize", false, false);
                    ApiReplyAbnormal(m_Command);
                    return;
                }
            }

        }
        #endregion

        public void AddRobotJob(RobotJob m_Job)
        {
            CurJob = m_Job;
        }
        public bool IsHasAnyDataAndSensor()
        {
            bool rtn = false;
            rtn = cv_Data.TheSlotHasDataOrSensor(RobotArm.rbaUp);
            if(!rtn)
            {
                rtn = cv_Data.TheSlotHasDataOrSensor(RobotArm.rbaDown);
            }
            return rtn;
        }
        public bool SetRobotTransferAction(CommandData m_Command, RobotJob m_Job)
        {
            bool rtn = false;
            if (LgcForm.PSystemData.PSystemStatus != EquipmentStatus.Down)
            {
                if (cv_Comm.SetRobotTransferAction(m_Command))
                {
                    LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Run;
                    AddRobotJob(m_Job);
                    rtn = true;
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        public bool SetRobotCommonAction(CommandData m_Command)
        {
            bool rtn = false;
            if (LgcForm.PSystemData.PSystemStatus != EquipmentStatus.Down)
            {
                if (cv_Comm.cv_Controller.Connected)
                {
                    return cv_Comm.SetRobotCommonAction(m_Command);
                }
                return rtn;
            }
            else
            {
                return rtn;
            }
        }
        private void OnGIFTimer()
        {
            /*
            if (CurJob != null)
            {
                if (CurJob.cv_Target == ActionTarget.Eq)
                {
                    RobotJob job = CurJob;
                    if (!job.cv_UseHs) return;
                    Eq eq = Form1.GetEqById(job.cv_TargetId);
                    if (job.cv_Action == RobotAction.Put)
                    {
                        if (eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_LoadWaitRbCommand || eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_ExchangeWaitRbPutCommand)
                        {
                            List<string> para = new List<string>();
                            para.Add(((int)job.cv_PutArm).ToString());
                            if (job.cv_Target == CommonData.HIRATA.ActionTarget.Eq)
                            {
                                para.Add("Stage" + Form1.GetEqById(job.cv_TargetId).cv_Comm.cv_RobotPosition.ToString());
                            }
                            else if (job.cv_Target == CommonData.HIRATA.ActionTarget.Port)
                            {
                                para.Add("P" + job.cv_TargetId.ToString());
                            }
                            else if (job.cv_Target == CommonData.HIRATA.ActionTarget.Buffer)
                            {
                                para.Add("Buffer" + job.cv_TargetId.ToString());
                            }
                            else if (job.cv_Target == CommonData.HIRATA.ActionTarget.Aligner)
                            {
                                para.Add("Aligner" + job.cv_TargetId.ToString());
                            }
                            para.Add(job.cv_TargetSlot.ToString());
                            LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1).SetTrigger(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1);
                            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.WaferPut.ToString(), APIEnum.CommnadDevice.Robot,
                                            0, para);
                        }
                        else if (eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_LoadWaitRbFinish || eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_ExchangeWaitRbPutFinish)
                        {
                                LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1).SetTrigger(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1);
                        }
                        else if (eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_LoadWaitEqCompleteOFF || eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_ExchangeWaitEqCompleteOFF)
                        {
                                LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1).SetTrigger(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1);
                                eq.cv_Comm.cv_InJob = false;
                        }
                    }
                    else if (job.cv_Action == RobotAction.Get)
                    {
                        if (eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_UnloadWaitRbCommand || eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_ExchangeWaitGetRbCommand)
                        {
                            List<string> para = new List<string>();
                            para.Add(((int)job.cv_GetArm).ToString());
                            if (job.cv_Target == CommonData.HIRATA.ActionTarget.Eq)
                            {
                                para.Add("Stage" + Form1.GetEqById(job.cv_TargetId).cv_Comm.cv_RobotPosition.ToString());
                            }
                            else if (job.cv_Target == CommonData.HIRATA.ActionTarget.Port)
                            {
                                para.Add("P" + job.cv_TargetId.ToString());
                            }
                            else if (job.cv_Target == CommonData.HIRATA.ActionTarget.Buffer)
                            {
                                para.Add("Buffer" + job.cv_TargetId.ToString());
                            }
                            else if (job.cv_Target == CommonData.HIRATA.ActionTarget.Aligner)
                            {
                                para.Add("Aligner" + job.cv_TargetId.ToString());
                            }
                            para.Add(job.cv_TargetSlot.ToString());
                            LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1).SetTrigger(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1);
                            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.WaferGet.ToString(), APIEnum.CommnadDevice.Robot,
                                            0, para);
                        }
                        else if (eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_UnloadWaitRbFinish || eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_ExchangeWaitRbGetFinish)
                        {
                                LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1).SetTrigger(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1);
                        }
                        else if (eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_LoadWaitEqCompleteOFF || eq.GetTimeChatCurStep(job.cv_TargetSlot) == TimechartNormal.STEP_ID_ExchangeWaitRbGetFinish)
                        {
                                LGCController.g_Controller.cv_TimechartController.GetTimeChartInstance(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1).SetTrigger(eq.cv_Comm.cv_TimeChatId + job.cv_TargetSlot - 1);
                                eq.cv_Comm.cv_InJob = false;
                        }
                    }
                }
            }
            */
        }
        public override void SendDataViaMmf()
        {
            this.cv_Data.GlassDataMap = this.cv_Data.GlassDataMap;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.RobotData).Name, this.cv_Data, KgsCommon.KParseObjToXmlPropertyType.Field);
            cv_Data.SaveToFile();
        }
        private static void ApiReplyAbnormal(CommandData m_Command)
        {
            string log = "[Process API Abnormal Reply] " + m_Command.GetCommandStr() + "\n" ;
            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
            int rep_code = m_Command.cv_ReturnCode;
            string str_rep_code = "";
            List<AlarmItem> list = new List<AlarmItem>();
            bool is_find = false;
            if (rep_code < 10)
            {
                str_rep_code = rep_code.ToString().Trim().PadLeft(2, '0');
            }
            else
            {
                str_rep_code = rep_code.ToString().Trim();
            }
            if(LgcForm.cv_ApiAlarm.ContainsKey(str_rep_code))
            {
                list = LgcForm.cv_ApiAlarm[str_rep_code];
            }
            else
            {
                log += "API alarm table can't find the Big Code.\n";
                LgcForm.ShowMsg(log, true, false);
            }
            for(int i=0 ; i<list.Count ; i++)
            {
                AlarmItem tmp_alarm = list.ElementAt(i);
                if(tmp_alarm.PCommandDevice == m_Command.PCommandDevice)
                {
                    if(tmp_alarm.cv_ResCode.Trim() == m_Command.cv_ReplyParaList[0].Trim())
                    {
                        alarm = list.ElementAt(i);
                        is_find = true;
                        break;
                    }
                }
            }
            LgcForm.ShowMsg("Command Reply Abnormal : " + m_Command.GetCommandStr(), true, false);
            if(is_find)
            {
                log += "Fined the alarm in Alarm List and report";
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
            }
            else
            {
                log += "Can't fine the alarm in Alarm List";
                //LgcForm.ShowMsg(log, true, false);
            }
            log += "--------------------------------";
            LgcForm.WriteLog(LogLevelType.General, log, FunInOut.None);
            /*
            alarm.PCode = m_Command.GetAlarmCode().ToString();
            alarm.PLevel = AlarmLevele.Serious;
            //alarm.PMainDescription = "Command Reply Abnormal : " + m_Command.GetCommandStr();
            alarm.PMainDescription = "Reply Abnormal : " + m_Command.cv_ReturnCode + "," + m_Command.cv_ReplyParaList[0] + " , " + m_Command.cv_ReplyParaList[1];
            if(m_Command.cv_ReplyParaList.Count>2)
            {
                alarm.PMainDescription += "," + m_Command.cv_ReplyParaList[2];
            }
            alarm.PStatus = AlarmStatus.Occur;
            LgcForm.ShowMsg("Command Reply Abnormal : " + m_Command.GetCommandStr(), true, false);
            LgcForm.EditAlarm(alarm);
            */
        }
        public void SetInitilize( bool m_IsForce=false)
        {
            cv_IsForce = m_IsForce;
            cv_HadInit = false;
            LgcForm.PSystemData.PInitaiizeOk = false;
            LgcForm.PSystemData.PInitaiizing = true;
            cv_Initilizing = true;
            CurJob = null;
            LgcForm.cv_RobotManaulJobPath.Clear();

            LgcForm.GetRobotById(1).PIsStatus = false;
            LgcForm.GetRobotById(1).PIsHome = false;
            LgcForm.GetRobotById(1).PIsResetError = false;

            LgcForm.GetAlignerById(1).PIsStatus = false;
            LgcForm.GetAlignerById(1).PIsHome = false;
            LgcForm.GetAlignerById(1).PIsResetError = false;

            LgcForm.GetBufferById(1).PIsStatus = false;

            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                LgcForm.GetPortById(i).PIsStatus = false;
                LgcForm.GetPortById(i).PIsHome = false;
                LgcForm.GetPortById(i).PIsResetError = false;
                LgcForm.GetPortById(i).PIsRemapping = false;
            }
            if (LgcForm.PSystemData.PRobotInline == EquipmentInlineMode.Local)
            {
                cv_Comm.SetApiCommonCommand(APIEnum.APICommand.Remote);
            }
            else if (LgcForm.PSystemData.PRobotInline == EquipmentInlineMode.Remote)
            {
                cv_Comm.SetErrorReset(APIEnum.CommnadDevice.Robot);
            }
        }

        #region process robot action complete

        private void ProcessRobotGetStandbyArmExtend(CommandData m_Command, RobotJob job)
        {
            LgcForm.WriteLog(LogLevelType.General , "Recv Robot  : " + m_Command.GetCommandStr());
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandbyArmExtend)
            {
                if(job.PAction == RobotAction.GetStandbyArmExtend && job.PTarget == ActionTarget.Eq &&
                    job.PTargetId == (int)EqId.VAS && job.PTargetSlot == 1)
                {
                    eq_time_chart_cur_step = LgcForm.GetEqById(4).GetTimeChatCurStep(1);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                    time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetVasStandByEnd)
                    {
                        LgcForm.cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + 
                            (int)RobotSideBitAddressOffset.Robot_Delivery_Ready, 1);
                        //time_chart_instance.SetTrigger(time_chart_id);
                    }
                }
                if(job.PAction == RobotAction.GetStandbyArmExtend)
                {
                    CurJob = null;
                }
            }
        }
        private void ProcessRobotPutStandbyArmExtend(CommandData m_Command, RobotJob job)
        {
            LgcForm.WriteLog(LogLevelType.General, "Recv Robot  : " + m_Command.GetCommandStr());
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.PutStandbyArmExtend && job.PTarget == ActionTarget.Eq &&
                    job.PTargetId == (int)EqId.VAS)// && job.PTargetSlot == 1)
                {
                    if (job.PTargetSlot == 1)
                    {
                        eq_time_chart_cur_step = LgcForm.GetEqById((int)EqId.VAS).GetTimeChatCurStep(1);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                        time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByEnd)
                        {
                            LgcForm.cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Robot_Delivery_Ready, 1);
                        }
                    }
                    if(job.PAction == RobotAction.PutStandbyArmExtend)
                    {
                        CurJob = null;
                    }
                }
            }
        }
        private void ProcessRobotTopPutExtend(CommandData m_Command, RobotJob job)
        {
            LgcForm.WriteLog(LogLevelType.General, "Recv Robot  : " + m_Command.GetCommandStr());
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.TopPutStandbyArmExtend && job.PTarget == ActionTarget.Eq &&
                    job.PTargetId == (int)EqId.VAS)// && job.PTargetSlot == 1)
                {
                    if (job.PTargetSlot == 2)
                    {
                        eq_time_chart_cur_step = LgcForm.GetEqById((int)EqId.VAS).GetTimeChatCurStep(2);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                        time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByEnd)
                        {
                            LgcForm.cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Robot_Delivery_Ready, 1);
                        }
                    }
                }
                if (job.PAction == RobotAction.TopPutStandbyArmExtend)
                {
                    CurJob = null;
                }
            }
        }
        private void ProcessRobotTopPutWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandby)
            {
                if (job.PAction == RobotAction.TopPutWait)
                {
                    CurJob = null;
                }
            }
        }
        private void ProcessRobotTopGetWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopGetStandby)
            {
                if (job.PAction == RobotAction.TopGetWait)
                {
                    CurJob = null;
                }
            }
        }
        private void ProcessRobotTopPut(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            bool robot_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferPut)
            {
                if (job.PAction == RobotAction.TopPut)
                {
                    if (job.PTarget == ActionTarget.Eq && job.PTargetId == (int)EqId.VAS && 
                        job.PTargetSlot == 2)
                    {
                        LgcForm.cv_MmfController.SendBcTreansferReport(DataFlowAction.Send, robot.cv_Data.GlassDataMap[(int)job.PPutArm]);

                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                    }
                    CurJob = null;
                    if(LgcForm.PSystemData.PRobotStatus == EquipmentStatus.Run)
                    {
                        LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Idle;
                    }
                }
            }
        }
        private void ProcessRobotTopGet(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferGet)
            {
                if (job.PAction == RobotAction.TopGet)
                {
                    if (job.PTarget == ActionTarget.Eq)
                    {
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = this.cv_Comm.cv_GlassDataGetFromEq;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap = robot.cv_Data.GlassDataMap;
                        Global.Controller.SendMmfNotifyObject(typeof(RobotData).Name, robot.cv_Data, KgsCommon.KParseObjToXmlPropertyType.Field);
                    }
                    if (!job.PUseHs)
                    {
                        if (job.PAction != RobotAction.Exchange)
                        {
                            CurJob = null;
                        }
                    }
                }
            }
        }
        private void ProcessRobotPutWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandby)
            {
                if (job.PAction == RobotAction.PutWait)
                {
                    CurJob = null;
                }
            }
        }
        private void ProcessRobotGetWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandby)
            {
                if (job.PAction == RobotAction.GetWait)
                {
                    CurJob = null;
                }
            }
        }
        private void ProcessRobotPut(CommandData m_Command, RobotJob job)
        {
            LgcForm.WriteLog(LogLevelType.General , "Recv Robot put : " + m_Command.GetCommandStr());
            Robot robot = LgcForm.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferPut)
            {
                if (job.PAction == RobotAction.Put)
                {
                    bool robot_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
                    if (job.PTarget == ActionTarget.Port)
                    {
                        Port port = LgcForm.GetPortById(job.PTargetId);
                        port.cv_Data.GlassDataMap[job.PTargetSlot] = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                        //direct copy sensror.
                        port.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor = true;

                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;

                        if (port.cv_Data.PPortMode == PortMode.Unloader)
                        {
                            if (port.PLotStatus == LotStatus.Reserved) port.PLotStatus = LotStatus.Process;
                        }

                        robot.SendDataViaMmf();
                        port.SendDataViaMmf();
                        port.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                        LgcForm.cv_MmfController.SendBcTreansferReport(DataFlowAction.Store, port.cv_Data.GlassDataMap[job.PTargetSlot], (int)port.cv_Data.cv_Id,
                            (int)port.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq);
                           
                        if(LgcForm.PSystemData.PONT)
                        {
                            bool is_unlod = true;
                            for(int i=1 ; i<= port.cv_SlotCount ; i++)
                            {
                                if(port.cv_Data.GlassDataMap[i].POcrResult == OCRResult.None &&
                                    port.cv_Data.GlassDataMap[i].PHasSensor && port.cv_Data.GlassDataMap[i].PHasData) 
                                {
                                    is_unlod = false;
                                }
                            }
                            if(is_unlod)
                            {
                                cv_Comm.SetPortUnloadAction(port.cv_Id);
                            }
                        }
                    }
                    else if (job.PTarget == ActionTarget.Aligner)
                    {
                        Aligner aligner = LgcForm.GetAlignerById(job.PTargetId);
                        aligner.cv_Data.GlassDataMap[job.PTargetSlot] = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                        aligner.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        //aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor = true;
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        aligner.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        aligner.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                    }
                    else if (job.PTarget == ActionTarget.Buffer)
                    {
                        Buffer buffer = LgcForm.GetBufferById(job.PTargetId);
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot] = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].PEnterBufferTime = SysUtils.Now();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        //robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = false;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        buffer.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        //cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.Buffer);
                        robot.cv_Data.SaveToFile();
                        buffer.cv_Data.SaveToFile();
                    }
                    else if (job.PTarget == ActionTarget.Eq)
                    {
                        LgcForm.cv_MmfController.SendBcTreansferReport(DataFlowAction.Send, robot.cv_Data.GlassDataMap[(int)job.PPutArm]);

                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        //robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = false;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                        //cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                    }
                    if (job.PAction == RobotAction.Exchange && job.PTarget == ActionTarget.Aligner)
                    {
                        LgcForm.WriteLog(LogLevelType.General, "Set robot job null.");
                    }
                    else
                    {
                        CurJob = null;
                        if (LgcForm.PSystemData.PRobotStatus == EquipmentStatus.Run)
                        {
                            LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Idle;
                        }
                        LgcForm.WriteLog(LogLevelType.General, "Set robot job null.");
                    }
                }
            }
        }
        private void ProcessRobotGet(CommandData m_Command, RobotJob job)
        {
            LgcForm.WriteLog(LogLevelType.General , "Recv Robot Get : " + m_Command.GetCommandStr());
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferGet)
            {
                Robot robot = LgcForm.GetRobotById(job.PRobotId);
                bool robot_sensor = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor;
                if (job.PAction == RobotAction.Get)
                {
                    if (job.PTarget == ActionTarget.Port)
                    {
                        Port port = LgcForm.GetPortById(job.PTargetId);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = port.cv_Data.GlassDataMap[job.PTargetSlot];
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        port.cv_Data.GlassDataMap[job.PTargetSlot] = new GlassData();
                        port.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq =(uint)job.PTargetSlot;
                        port.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        port.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                        LgcForm.cv_MmfController.SendBcTreansferReport(DataFlowAction.Fetch, robot.cv_Data.GlassDataMap[(int)job.PGetArm] , (int)port.cv_Data.cv_Id,
                            (int)job.PTargetSlot);
                        if(!port.cv_Data.HasOtherJobHaveToDo())
                        {
                            LgcForm.cv_MmfController.SendBcLastSubstrateReport(robot.cv_Data.GlassDataMap[(int)job.PGetArm], (int)port.cv_Data.cv_Id, job.PTargetSlot);
                        }

                    }
                    else if (job.PTarget == ActionTarget.Buffer)
                    {
                        Buffer buffer = LgcForm.GetBufferById(job.PTargetId);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = buffer.cv_Data.GlassDataMap[job.PTargetSlot];
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot] = new GlassData();
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        robot.SendDataViaMmf();
                        buffer.SendDataViaMmf();
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.Buffer);
                        buffer.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                    }
                    else if (job.PTarget == ActionTarget.Aligner)
                    {
                        Aligner Aligner = LgcForm.GetAlignerById(job.PTargetId);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = Aligner.cv_Data.GlassDataMap[job.PTargetSlot];
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        Aligner.cv_Data.GlassDataMap[job.PTargetSlot] = new GlassData();
                        Aligner.cv_Data.GlassDataMap[job.PTargetSlot].PSlotInEq = (uint)job.PTargetSlot;
                        //Aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor = false;
                        Aligner.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                        Aligner.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                       // cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                       // cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                    }
                    else if (job.PTarget == ActionTarget.Eq)
                    {
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                    }
                    CurJob = null;
                    if (LgcForm.PSystemData.PRobotStatus == EquipmentStatus.Run)
                    {
                        LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Idle;
                    }
                    LgcForm.WriteLog(LogLevelType.General , "Set robot job null.");
                }
            }
        }
        private void ProcessRobotStop(CommandData m_Command)
        {
            Robot robot = LgcForm.GetRobotById(1);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.Stop)
            {
                LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Stop;
            }
        }
        #endregion

        #region process each Robot command reply
        private void ProcessAPICommand(CommandData m_Command)
        {
            if (m_Command.PApiCommand == APIEnum.APICommand.Remote)
            {
                cv_Comm.SetApiCommonCommand(APIEnum.APICommand.Version);
                LgcForm.PSystemData.PRobotInline = EquipmentInlineMode.Remote;
                if (!cv_HadInit && cv_Initilizing)
                {
                    cv_Comm.SetErrorReset(APIEnum.CommnadDevice.Robot);
                }
            }
            else if (m_Command.PApiCommand == APIEnum.APICommand.Local)
            {
                LgcForm.PSystemData.PRobotInline = EquipmentInlineMode.Local;
                LgcForm.CheckSystemStatus();
            }
            else if (m_Command.PApiCommand == APIEnum.APICommand.Version)
            {
                LgcForm.PSystemData.PRobotVersion = m_Command.cv_ReplyParaList[0];
            }
            else if (m_Command.PApiCommand == APIEnum.APICommand.CurrentMode)
            {
                if (Regex.Match(m_Command.cv_ReplyParaList[0], @"remote", RegexOptions.IgnoreCase).Success)
                {
                    LgcForm.PSystemData.PRobotInline = EquipmentInlineMode.Remote;
                }
                else
                {
                    LgcForm.PSystemData.PRobotInline = EquipmentInlineMode.Local;
                }
            }
        }
        private void ProcessCommonCommand(CommandData m_Command)
        {
            bool ok = true;
            if (m_Command.PCommonCommand == APIEnum.CommonCommand.Home)
            {
                if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Robot)
                {
                    ok = ProcessRobotHome(m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.P)
                {
                    ok = ProcessPortHome(m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Aligner)
                {
                    ok = ProcessAlignerHome(m_Command);
                }
            }
            else if (m_Command.PCommonCommand == APIEnum.CommonCommand.ResetError)
            {
                if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Robot)
                {
                    ok = ProcessResetError(m_Command.PCommandDevice, m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.P)
                {
                    ok = ProcessResetError(m_Command.PCommandDevice, m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Aligner)
                {
                    ok = ProcessResetError(m_Command.PCommandDevice, m_Command);
                }
            }
            else if (m_Command.PCommonCommand == APIEnum.CommonCommand.GetStatus)
            {
                if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.Robot)
                {
                        ok = ProcessRobotStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.Aligner)
                {
                    ok = ProcessAlignerStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.P)
                {
                    ok = ProcessPortStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.EFEM)
                {
                    ok = ProcessEventStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.Robot)
                {
                    ok = ProcessEventStatus(m_Command);
                }
                //ProcessRobotOutVas
            }
        }
        private void ProcessRFIDCommand(CommandData m_Command)
        {
            if (m_Command.PRfidCommand == APIEnum.RfidCommand.ReadFoupID)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                {
                    if (m_Command.cv_ReplyParaList[0].Trim() == "r")
                    {
                        LgcForm.ShowMsg("Command Reply Abnormal (RIFD read error ) : " + m_Command.GetCommandStr(), true, false);
                    }
                    else
                    {
                        if (job_port.PPortStatus == PortStaus.LDCM && job_port.PLotStatus == LotStatus.FoupSensorOn)
                        {
                            job_port.cv_Data.PLotId = m_Command.cv_ReplyParaList[0];
                            cv_Comm.SetGetMappingData(m_Command.cv_DeviceId);
                            job_port.SendDataViaMmf();
                        }
                    }
                }
            }
        }
        private void ProcessLoadPortCommand(CommandData m_Command)
        {
            if (m_Command.PLoadPortCommand == APIEnum.LoadPortCommand.Load)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                if (job_port.PPortStatus != PortStaus.LDCM)
                {
                    job_port.PPortStatus = PortStaus.LDCM;
                }
                if (!cv_HadInit && cv_Initilizing)
                {
                    cv_Comm.SetGetMappingData(m_Command.cv_DeviceId);
                }
                else
                {
                    if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                    {
                        job_port.cv_Data.Clear();
                        job_port.PClamp = PortClamp.Clamp;
                        cv_Comm.SetReadRFIDRead(m_Command.cv_DeviceId);
                    }
                }
            }
            else if (m_Command.PLoadPortCommand == APIEnum.LoadPortCommand.GetWaferSlot2)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                //if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                {
                    int work_count = 0;
                    foreach (int key in job_port.cv_Data.GlassDataMap.Keys)
                    {
                        if (key == 0) continue;
                        if (key <= job_port.cv_Data.cv_SlotCount)
                        {
                            if (m_Command.cv_ReplyParaList[key - 1] != "1" && m_Command.cv_ReplyParaList[key - 1] != "0")
                            {
                                LgcForm.ShowMsg("Command reply error (mapping data abnormal) : " +
                                    string.Join("," , m_Command.cv_ReplyParaList.ToString()), true, false);
                                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                                alarm.PCode = Alarmtable.MappingDataError.ToString();
                                alarm.PMainDescription = "Mapping Data Error";
                                alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                                alarm.PUnit = 0;
                                alarm.PLevel = AlarmLevele.Serious;
                                alarm.PStatus = AlarmStatus.Occur;
                                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                                LgcForm.EditAlarm(alarm);
                                return;
                            }
                            if (m_Command.cv_ReplyParaList[key - 1] == "1")
                            {
                                job_port.cv_Data.GlassDataMap[key].PHasSensor = true;
                                work_count++;
                            }
                            else
                            {
                                job_port.cv_Data.GlassDataMap[key].PHasSensor = false;
                            }
                        }
                        else
                        {
                            job_port.cv_Data.GlassDataMap[key].PHasSensor = false;
                        }
                    }
                    job_port.cv_Data.PWorkCount = (uint)work_count;
                    if (!string.IsNullOrEmpty(job_port.cv_Data.PLotId))
                    {
                        if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                        {
                            job_port.PLotStatus = LotStatus.MappingEnd;
                            job_port.PPortStatus = PortStaus.LDCM;
                            cv_Comm.SetLoadUnloadLed(true, SignalTowerControl.On, m_Command.cv_DeviceId);
                        }
                        else
                        {
                            if (!cv_HadInit && cv_Initilizing)
                            {
                                LgcForm.GetPortById(job_port.cv_Id).PIsRemapping = true;
                                cv_Comm.SetStatus(APIEnum.CommnadDevice.P, job_port.cv_Id);
                            }
                        }
                    }
                }
            }
            if (m_Command.PLoadPortCommand == APIEnum.LoadPortCommand.Unload)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                job_port.PClamp = CommonData.HIRATA.PortClamp.Unclamp;
                if (job_port.PPortStatus != PortStaus.UDRQ)
                {
                    job_port.PPortStatus = PortStaus.UDRQ;
                    cv_Comm.SetLoadUnloadLed(false, SignalTowerControl.On, m_Command.cv_DeviceId);
                }
                LgcForm.RemovePortToProcessList(m_Command.cv_DeviceId);
                if(LgcForm.PSystemData.PONT)
                {
                    job_port.PLotStatus = LotStatus.FoupSensorOn;
                    job_port.cv_Data.PPortStatus = PortStaus.LDCM;
                    cv_Comm.SetPortLoadAction(m_Command.cv_DeviceId);
                }
            }
        }
        private void ProcessE84Command(CommandData m_Command)
        {
        }
        private void ProcessRobotCommand(CommandData m_Command)
        {
            RobotJob job = CurJob;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferGet)
            {
                if (job.PAction == RobotAction.Get)
                {
                    ProcessRobotGet(m_Command, job);
                }
            }

            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferPut)
            {
                if (job.PAction == RobotAction.Put)
                {
                    ProcessRobotPut(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandby)
            {
                if (job.PAction == RobotAction.GetWait)
                {
                    ProcessRobotGetWait(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandby)
            {
                if (job.PAction == RobotAction.PutWait)
                {
                    ProcessRobotPutWait(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferGet)
            {
                if (job.PAction == RobotAction.TopGet)
                {
                    ProcessRobotTopGet(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferPut)
            {
                if (job.PAction == RobotAction.TopPut)
                {
                    ProcessRobotTopPut(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopGetStandby)
            {
                if (job.PAction == RobotAction.TopGetWait)
                {
                    ProcessRobotTopGetWait(m_Command, job);
                }
            }

            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandby)
            {
                if (job.PAction == RobotAction.TopPutWait)
                {
                    ProcessRobotTopPutWait(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.TopPutStandbyArmExtend)
                {
                    ProcessRobotTopPutExtend(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.PutStandbyArmExtend)
                {
                    ProcessRobotPutStandbyArmExtend(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandbyArmExtend)
            {
                if (job.PAction == RobotAction.GetStandbyArmExtend)
                {
                    ProcessRobotGetStandbyArmExtend(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.Stop)
            {
                ProcessRobotStop(m_Command);
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.ReStart)
            {
                if (!cv_HadInit && cv_Initilizing)
                {
                    cv_Comm.SetHome(APIEnum.CommnadDevice.Robot, 0);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.SetRobotSpeed)
            {
                LgcForm.PSystemData.PRobotSpeed = cv_WaitRobotSpeed;
                if (!cv_HadInit && cv_Initilizing)
                {
                    cv_Comm.SetSetFFUVoltage(LgcForm.PSystemData.PFFUSpeed);
                    cv_WaitFfuSpeed = LgcForm.PSystemData.PFFUSpeed;
                }
            }
            
            CommonData.HIRATA.MDRobotAction obj = new CommonData.HIRATA.MDRobotAction();
            obj.PAction = CommonData.HIRATA.RobotAction.ActionComplete;
            LgcForm.cv_MmfController.SendRobotAction(obj, MmfEventClientEventType.etNotify, false);
        }
        private void ProcessAlignerCommand(CommandData m_Command)
        {
            Aligner aligner = LgcForm.GetAlignerById(1);
            if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.AlignerVacuum)
            {
                if (!aligner.cv_Data.GlassDataMap[1].PHasData && !aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if(aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff1)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.PutAligner;
                    }
                }
                else if (aligner.cv_Data.GlassDataMap[1].PHasData && aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitVuccumOn)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.FindNotch;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitVuccomOff2)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.GetAligner;
                    }
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.Alignment)
            {
                if (aligner.cv_Data.GlassDataMap[1].PIsWaitOcr)
                {
                    cv_Comm.SetOcrRead();
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.SetAlignerDegree)
            {
                if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff1)
                {
                    cv_Comm.SetAlignerVaccum(false);
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.ToAngle)
            {
                if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitToAngle)
                {
                    aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff2;
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.FindNotch)
            {
                if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitFindNotch)
                {
                    aligner.cv_Data.PPreAction = AlignerPreAction.OcrConnect;
                }
            }
        }
        private void ProcessIOCommand(CommandData m_Command)
        {
            if (m_Command.PIoCommand == APIEnum.IoCommand.GetBufferStatus)
            {
                ProcessBufferStatus(m_Command);
            }
            else if (m_Command.PIoCommand == APIEnum.IoCommand.SignalTower)
            {
                Robot robot = LgcForm.GetRobotById(1);
                if (robot.cv_TowerJobQ.Count > 0)
                {
                    robot.cv_TowerJobQ.Dequeue();
                }
            }
            else if (m_Command.PIoCommand == APIEnum.IoCommand.SetFFUVoltage)
            {
                LgcForm.PSystemData.PFFUSpeed = cv_WaitFfuSpeed;
                if (!cv_HadInit && cv_Initilizing)
                {
                    LgcForm.SendinitComplete();
                }
            }

        }
        private void ProcessOCRCommand(CommandData m_Command)
        {
            Aligner aligner = LgcForm.GetAlignerById(1);
            if (m_Command.POcrCommand == CommonData.HIRATA.APIEnum.OcrCommand.Connect)
            {
                if (LgcForm.GetAlignerById(1).cv_Data.GlassDataMap[1].PHasData &&
                    LgcForm.GetAlignerById(1).cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if(aligner.cv_Data.PPreAction == AlignerPreAction.WaitConnect)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.ReadOcr;
                    }
                }
            }
            else if (m_Command.POcrCommand == CommonData.HIRATA.APIEnum.OcrCommand.Read)
            {
                if (aligner.cv_Data.GlassDataMap[1].PHasData &&
                    aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if (m_Command.cv_ReplyParaList[0] != "r")
                    {
                        if (aligner.cv_Data.GlassDataMap[1].PId != m_Command.cv_ReplyParaList[0])
                        {
                            LgcForm.cv_MmfController.SendBcOcrReport(aligner.cv_Data.GlassDataMap[1], m_Command.cv_ReplyParaList[0].Trim());
                            aligner.cv_Data.GlassDataMap[1].PId = m_Command.cv_ReplyParaList[0];
                            aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.Mismatch;
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = Alarmtable.OcrReadError.ToString();
                            alarm.PMainDescription = "Ocr Read Error";
                            alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Light;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            LgcForm.EditAlarm(alarm);
                            LgcForm.ShowMsg("OCR read Error!!!" , true , false);
                            //return;
                            //report BC ocr read.
                            CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                            report_bc.PGlass = aligner.cv_Data.GlassDataMap[1];
                            LgcForm.cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);

                            if ( LgcForm.PSystemData.POperationMode == OperationMode.Auto && LgcForm.PSystemData.POcrMode == OCRMode.ErrorHold)
                            {
                                LgcForm.cv_MmfController.SendShowOcrDecide();
                            }
                        }
                        else
                        {
                            aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.OK ;
                            LgcForm.cv_MmfController.SendBcOcrReport(aligner.cv_Data.GlassDataMap[1], m_Command.cv_ReplyParaList[0].Trim());
                        }


                        if(aligner.cv_Data.PPreAction == AlignerPreAction.WaitReadOct)
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                        }
                        LgcForm.GetAlignerById(1).SendDataViaMmf();
                    }
                    else
                    {
                        LgcForm.GetAlignerById(1).cv_Data.GlassDataMap[1].POcrResult = OCRResult.Fail;
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = Alarmtable.OcrReadError.ToString();
                        alarm.PMainDescription = "Ocr Read Error";
                        alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                        alarm.PUnit = 0;
                        alarm.PLevel = AlarmLevele.Light;
                        alarm.PStatus = AlarmStatus.Occur;
                        alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                        LgcForm.EditAlarm(alarm);
                    }
                }
            }
        }
        private void ProcessEventCommand(CommandData m_Command)
        {
            if (m_Command.PEventCommand == APIEnum.EventCommand.FoupPlace)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                if (job_port.PPortStatus != PortStaus.LDRQ)
                {
                    job_port.PPortStatus = PortStaus.LDRQ;
                }
                job_port.PLotStatus = LotStatus.FoupSensorOn;
                job_port.cv_Data.PPortHasCst = PortHasCst.Has ;
                job_port.SendDataViaMmf();
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.FoupRemove)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                job_port.cv_Data.PPortStatus = PortStaus.UDCM;
                job_port.cv_Data.PLotStatus = LotStatus.Empty;
                job_port.PLDRQTime = SysUtils.Now();
                job_port.cv_Data.PPortHasCst = PortHasCst.Empty;
                //SysUtils.Sleep(1000);
                job_port.cv_Data.Clear();
                //job_port.cv_Data.PPortStatus = PortStaus.LDRQ;
                job_port.SendDataViaMmf();
                cv_Comm.SetLoadUnloadLed(false, SignalTowerControl.Off, m_Command.cv_DeviceId);
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.ERROR)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = Alarmtable.RobotApiErrorEvent.ToString();
                alarm.PMainDescription = "Robot_API_ERROR_EVENT";
                alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                LgcForm.EditAlarm(alarm);
                LgcForm.PSystemData.POperationMode = OperationMode.Manual;
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.FoupPresence)
            {

            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.OperatorAccessButtonClick)
            {
                Port job_port = LgcForm.GetPortById(m_Command.cv_DeviceId);
                if (job_port.cv_Data.PPortHasCst == PortHasCst.Has)
                {
                    job_port.PLotStatus = LotStatus.FoupSensorOn;
                    job_port.PPortStatus = PortStaus.LDRQ;
                    job_port.cv_Data.PWaitUnload = false;
                    cv_Comm.SetPortLoadAction(m_Command.cv_DeviceId);
                }
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.OperatorAccessButton2Click)
            {

            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.VasTopPutEnd)
            {
                ProcessRobotOutVas(m_Command);
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.GetStatus)
            {
                Robot robot = LgcForm.GetRobotById(1);
                LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event S", FunInOut.None);
                if (m_Command.cv_ReplyParaList[1].Trim() == "1")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = true;
//                    LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }
                else if (m_Command.cv_ReplyParaList[1].Trim() == "0")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = false;
  //                  LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }
                else
                {
                    LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "RobotApi Robot Status Error";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                }

                if (m_Command.cv_ReplyParaList[2].Trim() == "1")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = true;
    //                LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }

                else if (m_Command.cv_ReplyParaList[2].Trim() == "0")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = false;
      //              LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }
                else
                {
                    LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "RobotApi Robot Status Error";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                }
                robot.SendDataViaMmf();
                robot.cv_Data.SaveToFile();
                LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event E", FunInOut.None);
            }
            else if ( (int)m_Command.PEventCommand >= (int)APIEnum.EventCommand.Pressure &&
                 (int)m_Command.PEventCommand <= (int)APIEnum.EventCommand.GetStatus)
            {
                CommonData.HIRATA.MDEfemStatusSingle obj = new MDEfemStatusSingle();
                obj.PStatusType = m_Command.PEventCommand;
                obj.PValue = Convert.ToInt16(m_Command.cv_ReplyParaList[1].Trim());
                if(obj.PStatusType != APIEnum.EventCommand.RobotEnable)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                    alarm.PMainDescription = "Robot API EFEM Status Error : " +  m_Command.PEventCommand.ToString();
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                    LgcForm.EditAlarm(alarm);
                }
                Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDEfemStatusSingle).Name, obj, KParseObjToXmlPropertyType.Field);
            }
        }
        private void ProcessAlignmentCommand(CommandData m_Command)
        {
        }
        private void ProcessBarcodeCommand(CommandData m_Command)
        {
        }
        #endregion

        #region HOME
        private bool ProcessRobotHome(CommandData m_Command)
        {
            bool rtn = false;
            LgcForm.GetRobotById(1).PIsHome = true;
            if (!cv_HadInit && cv_Initilizing)
            {
                //cv_Comm.SetAllPortHome();
                if(!LgcForm.GetRobotById(1).PIsSensorUnmatch)
                    cv_Comm.SetAllPortStatus();
                else
                {
                    LgcForm.SendinitCompleteFail();
                }
            }
            rtn = true;
            return rtn;
        }
        private bool ProcessAlignerHome(CommandData m_Command)
        {
            bool rtn = false;
            Aligner aligner = LgcForm.GetAlignerById(m_Command.cv_DeviceId);
            aligner.PIsHome = true;
            if (!cv_HadInit && cv_Initilizing)
            {
                cv_Comm.SetStatus(APIEnum.CommnadDevice.Buffer);
            }
            else if(aligner.cv_Data.PPreAction == AlignerPreAction.WaitHome)
            {
                aligner.cv_Data.PPreAction = AlignerPreAction.SetToAngle;
            }
            return rtn;
        }
        private bool ProcessPortHome(CommandData m_Command)
        {
            bool rtn = false;
            Port port = LgcForm.GetPortById(m_Command.cv_DeviceId);
            //port.cv_Data.Clear();
            port.PIsHome = true;
            if(port.PLotStatus == LotStatus.Process)
            {
                port.PLotStatus = LotStatus.Abort;
            }
            else if ((port.PLotStatus != LotStatus.Cancel) && (port.PLotStatus != LotStatus.Abort) && (port.PLotStatus != LotStatus.ProcessEnd))
            {
                port.PLotStatus = LotStatus.Cancel;
            }
            port.PPortStatus = PortStaus.UDRQ;
            port.PClamp = PortClamp.Unclamp;
            port.cv_Data.SaveToFile();
            if (!cv_HadInit && cv_Initilizing)
            {
                if (LgcForm.CheckAllPortHome())
                {
                    cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                }
            }
            return rtn;
        }
        #endregion

        #region Reset Error
        private bool ProcessResetError(APIEnum.CommnadDevice m_Device, CommandData m_Command)
        {
            bool rtn = true;
            switch (m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    LgcForm.GetRobotById(1).PIsResetError = true;
                    if (!cv_HadInit && cv_Initilizing)
                    {
                        for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
                        {
                            cv_Comm.SetErrorReset(APIEnum.CommnadDevice.P, i);
                        }
                    }
                    break;
                case APIEnum.CommnadDevice.P:
                    LgcForm.GetPortById(m_Command.cv_DeviceId).PIsResetError = true;
                    if (!cv_HadInit && cv_Initilizing)
                    {
                        if (LgcForm.CheckAllPortResetError())
                        {
                            cv_Comm.SetErrorReset(APIEnum.CommnadDevice.Aligner, 1);
                        }
                    }
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    LgcForm.GetAlignerById(1).PIsResetError = true;
                    if (!cv_HadInit && cv_Initilizing)
                    {
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                    }
                    else
                    {
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner  ,1);
                    }
                    break;
            };
            return rtn;
        }
        #endregion

        #region Status
        private bool ProcessRobotOutVas(CommandData m_Command)
        {
            bool ok = false;
            EqId eq_id = EqId.VAS;
            int slot = 2;
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;

            if (eq_id == EqId.VAS)
            {
                if (slot == 2)
                {
                    eq_time_chart_cur_step = LgcForm.GetEqById((int)eq_id).GetTimeChatCurStep(2);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                    time_chart_instance = (TimechartNormal)LgcForm.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);

                    if (eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitRobotCompleteOn)
                    {
                        time_chart_instance.SetSignal(RobotSideBitAddressOffset.Receipt_Complete, true);
                        time_chart_instance.SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
                        ok = true;
                    }
                    else
                    {
                        LgcForm.ShowMsg("VAS time chart not at STEP_ID_WaitRobotCommandFinish", true, false);
                    }
                }
            }
            return ok;
        }
        private bool ProcessRobotStatus(CommandData m_Command)
        {
            //00,GetStatus,Robot,StatusCode, Lower EE Wafer Presence, Upper EE Wafe Presence 
            bool ok = true;
            PIsSensorUnmatch = false;
            if (m_Command.cv_ReplyParaList[1].Trim() == "1")
            {
                cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = true;
            }
            else if (m_Command.cv_ReplyParaList[1].Trim() == "0")
            {
                cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = false;
            }
            else
            {
                LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Robot Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }

            if (m_Command.cv_ReplyParaList[2].Trim() == "1")
            {
                cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = true;
            }

            else if (m_Command.cv_ReplyParaList[2].Trim() == "0")
            {
                cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = false;
            }
            else
            {
                LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Robot Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }

            if (m_Command.cv_ReplyParaList[0].Trim() == "0601")
            {
                LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Idle;
            }
            else if (m_Command.cv_ReplyParaList[0].Trim() == "4401")
            {
                LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Run;
            }
            else if (m_Command.cv_ReplyParaList[0].Trim() == "0621")
            {
                LgcForm.PSystemData.PRobotStatus = EquipmentStatus.Stop;
            }
            else
            {
                LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Robot Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (ok)
            {
                PIsStatus = true;
                SendDataViaMmf();
                if (!cv_HadInit && cv_Initilizing)
                {
                    List<int> tmp = new List<int>();
                    if (!cv_Data.IsSensorDataMatch(out tmp))
                    {
                        LgcForm.ShowMsg("Robot unmatch slot : " + string.Join(",", tmp), true, false);
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializeRobotSensorUnmatch.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "At Initialize Robot Sensor Unmatch";
                        alarm.PUnit = 0;
                        alarm.PStatus = AlarmStatus.Occur;
                        LgcForm.EditAlarm(alarm);
                        //LgcForm.SendinitCompleteFail();
                        PIsSensorUnmatch = true;
                    }
                    //else
                    {
                        //if(m_Command.cv_ReplyParaList[0].Trim() == "0621")
                        if(LgcForm.PSystemData.PRobotStatus == EquipmentStatus.Stop)
                        {
                            cv_Comm.SetRobotRestart();
                        }
                        else
                        {
                            cv_Comm.SetHome(APIEnum.CommnadDevice.Robot, 0);
                        }
                    }
                }
            }
            else
            {
                if (cv_Initilizing)
                {
                    LgcForm.SendinitCompleteFail();
                }
            }
            return ok;
        }
        private bool ProcessAlignerStatus(CommandData m_Command)
        {
            bool ok = true;
            Aligner aligner = LgcForm.GetAlignerById(1);
            if (Regex.Match(m_Command.cv_ReplyParaList[1].Trim(), @"true", RegexOptions.IgnoreCase).Success)
            {
                aligner.cv_Data.GlassDataMap[1].PHasSensor = true;
            }

            else if (Regex.Match(m_Command.cv_ReplyParaList[1].Trim(), @"false", RegexOptions.IgnoreCase).Success)
            {
                aligner.cv_Data.GlassDataMap[1].PHasSensor = false;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiAlignerStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Aligner Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[0].Trim(), @"off", RegexOptions.IgnoreCase).Success)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiAlignerOffline.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Aligner Offline";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
            }
            if (ok)
            {
                List<int> tmp = new List<int>();
                if (aligner.cv_Data.IsSensorDataMatch(out tmp))
                {
                    LgcForm.GetAlignerById(1).PIsStatus = true;
                    if (cv_Initilizing)
                    {
                        cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                    }
                }
                else
                {
                    LgcForm.ShowMsg(" Aligner unmatch slot : " + string.Join(",", tmp), true, false);
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializeAlignerSensorUnmatch.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "At Initialize Aligner Sensor Unmatch";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                    if (cv_Initilizing)
                    {
                        LgcForm.SendinitCompleteFail();
                    }
                }
            }
            else
            {
                if (cv_Initilizing)
                {
                    LgcForm.SendinitCompleteFail();
                }
            }
            LgcForm.GetAlignerById(1).SendDataViaMmf();
            return ok;
        }
        private bool ProcessPortStatus(CommandData m_Command)
        {
            //：00,GetStatus,P*, LP mode, LP status, Foup status, Clamp status, Door status , Port type
            /*
             *  LP mode is mean “Online/ Teaching/ Maintain/ Unknow”. 
                LP status is mean “No error/ Error code/ Unknow”. 
                Foup status is mean “Present/ Absent/ Unknow”. 
                Clamp status is mean “Clamp/ Unclamp/ Unknow”. 
                Door status is mean “Open/ Close/ Unknow”. 
                Example：00,GetStatus, P*,Online,No error,Present,Clamp,Open 
             * port type : 0 type 1 / 1 type 2 , ... , 4 type 5.
             */
            bool ok = true;
            int port_id = m_Command.cv_DeviceId;
            Port port = LgcForm.GetPortById(port_id);
            PortHasCst has_cst = PortHasCst.None;
            PortClamp port_clamp = PortClamp.None;
            bool door_open = false;
            bool port_status_is_load = false;
            if (!Regex.Match(m_Command.cv_ReplyParaList[0].Trim(), @"Online", RegexOptions.IgnoreCase).Success)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortNotInOnline.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Not In Online";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (!Regex.Match(m_Command.cv_ReplyParaList[1].Trim(), @"No error", RegexOptions.IgnoreCase).Success)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[2].Trim(), @"Present", RegexOptions.IgnoreCase).Success)
            {
                has_cst = PortHasCst.Has;
            }
            else if (Regex.Match(m_Command.cv_ReplyParaList[2].Trim(), @"Absent", RegexOptions.IgnoreCase).Success)
            {
                has_cst = PortHasCst.Empty;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortFoupSensorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Foup sensor error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[3].Trim(), @"Unclamp", RegexOptions.IgnoreCase).Success)
            {
                port_clamp = PortClamp.Unclamp;
            }
            else if (Regex.Match(m_Command.cv_ReplyParaList[3].Trim(), @"clamp", RegexOptions.IgnoreCase).Success)
            {
                port_clamp = PortClamp.Clamp;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortFoupClampSensorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Foup clamp sensor error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[5].Trim(), @"\d", RegexOptions.IgnoreCase).Success)
            {
                int type = Convert.ToInt16(m_Command.cv_ReplyParaList[5].Trim());
                if (type < 0 && type > 4)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.PortTypeValueOverRange.ToString();
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PMainDescription = "Port type value over range , please re-set";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                    ok = false;
                }
                else if(port.cv_Data.PEfemPortType != type)
                {//EfemPortTypeError
                    /*
                    port.cv_Data.PEfemPortType = type;
                    if(type == 0)
                    {
                        port.cv_Data.cv_SlotCount = 25;
                        port.cv_SlotCount = 25;
                        port.SendDataViaMmf();
                    }
                    else if (type == 4)
                    {
                        port.cv_Data.cv_SlotCount = 13;
                        port.cv_SlotCount = 13;
                        port.SendDataViaMmf();
                    }
                    else
                    {
                    */
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.EfemPortTypeError.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "Efem Port Type Error";
                        alarm.PUnit = 0;
                        alarm.PStatus = AlarmStatus.Occur;
                        LgcForm.EditAlarm(alarm);
                        ok = false;
                    //}
                }
                else
                {
                    bool slot_error = false;
                    if(port.cv_Data.PEfemPortType == 0)
                    {
                        if(port.cv_Data.cv_SlotCount != 25 || port.cv_SlotCount != 25)
                        {
                            slot_error = true;
                        }
                    }
                    else if(port.cv_Data.PEfemPortType == 4)
                    {
                        if(port.cv_Data.cv_SlotCount != 13 || port.cv_SlotCount != 13)
                        {
                            slot_error = true;
                        }
                    }
                    if(slot_error)
                    {
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.PortTypeSlotNumberError.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "Port Type Slot Number Error , please contact vendor";
                        alarm.PUnit = 0;
                        alarm.PStatus = AlarmStatus.Occur;
                        LgcForm.EditAlarm(alarm);
                        ok = false;
                    }
                }
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortDoorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Door error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[4].Trim(), @"Open", RegexOptions.IgnoreCase).Success)
            {
                door_open = true;
            }
            else if (Regex.Match(m_Command.cv_ReplyParaList[4].Trim(), @"Close", RegexOptions.IgnoreCase).Success)
            {
                door_open = false; ;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortDoorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Door error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
                ok = false;
            }
            if (port_clamp == PortClamp.Clamp)
            {
                if ((!door_open) || (has_cst == PortHasCst.Empty))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.PortClampButDoorCloseOrNoFoup.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Port Clamp But Door Close Or No Foup";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                    ok = false;
                }
                else
                {
                    port_status_is_load = true;
                }
            }
            else if (port_clamp == PortClamp.Unclamp && door_open)
            {
                if (door_open)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.PortUnClampButDoorOpen.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Port UnClamp But Door Open";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                    ok = false;
                }
                else
                {
                    port_status_is_load = false;
                }
            }
            if (!ok)
            {
                if (cv_Initilizing)
                {
                    LgcForm.SendinitCompleteFail();
                }
            }
            if (ok)
            {
                if (port_status_is_load)
                {
                    if (has_cst == PortHasCst.Has)
                    {
                        port.cv_Data.PPortHasCst = PortHasCst.Has;
                        if (port.PPortStatus == PortStaus.LDCM)
                        {
                            if (!LgcForm.GetPortById(port.cv_Id).PIsRemapping)
                            {
                                if (!cv_IsForce)
                                {
                                    cv_Comm.SetPortLoadAction(port.cv_Id);
                                    ok = false;
                                }
                            }
                            else
                            {
                                List<int> tmp = new List<int>();
                                if (!port.cv_Data.IsSensorDataMatch(out tmp))
                                {
                                    LgcForm.ShowMsg("Port " +  port_id +" unmatch slot : " + string.Join(",", tmp), true, false);
                                    LgcForm.GetPortById(port.cv_Id).PIsRemapping = false;
                                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                                    alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializePortSensorUnmatch.ToString();
                                    alarm.PLevel = AlarmLevele.Serious;
                                    alarm.PMainDescription = "At Initialize Port Sensor Unmatch";
                                    alarm.PUnit = 0;
                                    alarm.PStatus = AlarmStatus.Occur;
                                    LgcForm.EditAlarm(alarm);
                                    LgcForm.SendinitCompleteFail();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (has_cst == PortHasCst.Has)
                    {
                        if (port.PPortStatus == PortStaus.UDRQ)
                        {
                            //port.PLotStatus = LotStatus.Cancel;
                            //port.PClamp = PortClamp.Unclamp;
                            //port.cv_Data.PPortHasCst = PortHasCst.Has;
                        }
                        else if (port.PPortStatus == PortStaus.LDRQ || port.PPortStatus == PortStaus.UDCM || port.PPortStatus == PortStaus.None)
                        {
                            port.PPortStatus = PortStaus.LDRQ;
                            port.PLotStatus = LotStatus.FoupSensorOn;
                            port.PClamp = PortClamp.Unclamp;
                            port.cv_Data.ClearAllGlassData();
                            port.cv_Data.PPortHasCst = PortHasCst.Has;
                            //cv_Comm.SetOperatorAccessButton(SignalTowerControl.Flash, port.cv_Id);
                        }
                        else
                        {
                            port.PPortStatus = PortStaus.UDRQ;
                            port.PClamp = PortClamp.Unclamp;
                            port.cv_Data.PPortHasCst = PortHasCst.Has;
                        }
                    }
                    else
                    {
                        port.cv_Data.Clear();
                        port.PPortStatus = PortStaus.LDRQ;
                        port.PLotStatus = LotStatus.Empty;
                        port.PClamp = PortClamp.Unclamp;
                        port.cv_Data.PPortHasCst = PortHasCst.Empty;
                    }
                }
            }
            if (ok)
            {
                LgcForm.GetPortById(m_Command.cv_DeviceId).PIsStatus = true;
                if (!cv_HadInit && cv_Initilizing)
                {
                    if (LgcForm.CheckAllPortStatus())
                    {
                        if(!cv_Comm.SetAllPortHome())
                        {
                            cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                        }
                    }
                }
            }
            LgcForm.GetPortById(port_id).SendDataViaMmf();
            return ok;
        }
        private bool ProcessBufferStatus(CommandData m_Command)
        {
            bool ok = true;
            Buffer buffer = LgcForm.GetBufferById(1);
            if (m_Command.cv_ReplyParaList.Count >= buffer.cv_SlotCount)
            {
                for (int i = 0; i < m_Command.cv_ReplyParaList.Count; i++)
                {
                    if (i < LgcForm.GetBufferById(1).cv_SlotCount)
                    {
                        if (Regex.Match(m_Command.cv_ReplyParaList[i], @"1", RegexOptions.IgnoreCase).Success)
                        {
                            buffer.cv_Data.GlassDataMap[i + 1].PHasSensor = true;
                        }
                        else if (Regex.Match(m_Command.cv_ReplyParaList[i], @"0", RegexOptions.IgnoreCase).Success)
                        {
                            buffer.cv_Data.GlassDataMap[i + 1].PHasSensor = false;
                        }
                        else
                        {
                            LgcForm.ShowMsg("Command reply error (mapping data abnormal) : " +
                                string.Join(",", m_Command.cv_ReplyParaList.ToString()), true, false);
                            ok = false;
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                            alarm.PMainDescription = "Robot API Buffer sensor Error";
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Serious;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            LgcForm.EditAlarm(alarm);
                        }
                    }
                }
            }
            else
            {
                ok = false;
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                alarm.PMainDescription = "Robot API Buffer Status Error";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                LgcForm.EditAlarm(alarm);
            }
            if (ok)
            {
                List<int> tmp = new List<int>();
                if (buffer.cv_Data.IsSensorDataMatch(out tmp))
                {
                    if (cv_Initilizing)
                    {
                        cv_Comm.SetStatus(APIEnum.CommnadDevice.EFEM);
                    }
                }
                else
                {
                    LgcForm.ShowMsg("Buffer unmatch slot : " + string.Join(",", tmp), true, false);
                    ok = false;
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializeBufferSensorUnmatch.ToString();
                    alarm.PMainDescription = "At Initialize Buffer Sensor Unmatch";
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                    LgcForm.EditAlarm(alarm);
                    if (cv_Initilizing)
                    {
                        LgcForm.SendinitCompleteFail();
                    }
                }
            }
            else
            {
                if (cv_Initilizing)
                {
                    LgcForm.SendinitCompleteFail();
                }
            }
            buffer.SendDataViaMmf();
            return ok;
        }
        private bool ProcessEventStatus(CommandData m_Command)
        {
            /*
             *  Normal Reply：00,GetStatus,EFEM, Pressure, Vacuum, Ionizer1, Ionizer2, Ionizer3, Ionizer4, 
             *  Ionizer5, Ionizer6, Ionizer7, Ionizer8, FFU1, 
FFU2, FFU3, FFU4, FFU5, FFU6, FFU7, FFU8, FFU9, FFU10, FFU11,Robot Mode, Robot Enable, Door, EMO, Power 
            */
            bool ok = false;
            CommonData.HIRATA.MDEfemStatus obj = new MDEfemStatus();
            //cb_ManualApi.Items.AddRange(Enum.GetNames(typeof(APIEnum.APICommand)).ToArray<string>());
            List<string> tmp = Enum.GetNames(typeof(CommonData.HIRATA.APIEnum.EventCommand)).ToList<string>();
            if (m_Command.cv_ReplyParaList.Count <= tmp.Count)
            {
                obj.cv_Pressure = Convert.ToInt16(m_Command.cv_ReplyParaList[0].Trim());
                obj.cv_Vacuum = Convert.ToInt16(m_Command.cv_ReplyParaList[1].Trim());
                obj.cv_Ionizer1 = Convert.ToInt16(m_Command.cv_ReplyParaList[2].Trim());
                obj.cv_Ionizer2 = Convert.ToInt16(m_Command.cv_ReplyParaList[3].Trim());
                obj.cv_Ionizer3 = Convert.ToInt16(m_Command.cv_ReplyParaList[4].Trim());
                obj.cv_Ionizer4 = Convert.ToInt16(m_Command.cv_ReplyParaList[5].Trim());
                obj.cv_Ionizer5 = Convert.ToInt16(m_Command.cv_ReplyParaList[6].Trim());
                obj.cv_Ionizer6 = Convert.ToInt16(m_Command.cv_ReplyParaList[7].Trim());
                obj.cv_Ionizer7 = Convert.ToInt16(m_Command.cv_ReplyParaList[8].Trim());
                obj.cv_Ionizer8 = Convert.ToInt16(m_Command.cv_ReplyParaList[9].Trim());
                obj.cv_FFU1 = Convert.ToInt16(m_Command.cv_ReplyParaList[10].Trim());
                obj.cv_FFU2 = Convert.ToInt16(m_Command.cv_ReplyParaList[11].Trim());
                obj.cv_FFU3 = Convert.ToInt16(m_Command.cv_ReplyParaList[12].Trim());
                obj.cv_FFU4 = Convert.ToInt16(m_Command.cv_ReplyParaList[13].Trim());
                obj.cv_FFU5 = Convert.ToInt16(m_Command.cv_ReplyParaList[14].Trim());
                obj.cv_FFU6 = Convert.ToInt16(m_Command.cv_ReplyParaList[15].Trim());
                obj.cv_FFU7 = Convert.ToInt16(m_Command.cv_ReplyParaList[16].Trim());
                obj.cv_FFU8 = Convert.ToInt16(m_Command.cv_ReplyParaList[17].Trim());
                obj.cv_FFU9 = Convert.ToInt16(m_Command.cv_ReplyParaList[18].Trim());
                obj.cv_FFU10 = Convert.ToInt16(m_Command.cv_ReplyParaList[19].Trim());
                obj.cv_FFU11 = Convert.ToInt16(m_Command.cv_ReplyParaList[20].Trim());
                obj.cv_RobotMode = Convert.ToInt16(m_Command.cv_ReplyParaList[21].Trim());
                obj.cv_RobotEnable = Convert.ToInt16(m_Command.cv_ReplyParaList[22].Trim());
                obj.cv_Door = Convert.ToInt16(m_Command.cv_ReplyParaList[23].Trim());
                obj.cv_EMO = Convert.ToInt16(m_Command.cv_ReplyParaList[24].Trim());
                obj.cv_Power = Convert.ToInt16(m_Command.cv_ReplyParaList[25].Trim());

                Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDEfemStatus).Name, obj, KParseObjToXmlPropertyType.Field);
                ok = true;

                for(int i = 0 ; i<=25 ; i++)
                {
                    if(m_Command.cv_ReplyParaList[i].Trim() == "0" )
                    {
                        if(i != 22)
                        {
                            ok = false;
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                            alarm.PMainDescription = "Robot API EFEM Status Error";
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Serious;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            LgcForm.EditAlarm(alarm);
                            if (cv_Initilizing)
                            {
                                LgcForm.SendinitCompleteFail();
                            }
                        }
                    }
                }
            }
            else
            {
                ok = false;
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                alarm.PMainDescription = "Robot API EFEM Status Error";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                LgcForm.EditAlarm(alarm);
                if (cv_Initilizing)
                {
                    LgcForm.SendinitCompleteFail();
                }
            }
            if (ok)
            {
                if (!cv_HadInit && cv_Initilizing)
                {
                    //LgcForm.SendinitComplete();
                    cv_Comm.SetRobotSpeed(LgcForm.PSystemData.PRobotSpeed);
                    cv_WaitRobotSpeed = LgcForm.PSystemData.PRobotSpeed;
                }
            }
            return ok;
        }
        #endregion
    }
}
