﻿using System;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KgsCommon;
using System.Text.RegularExpressions;
using System.Collections;
using System.Security.Permissions;
using CommonData.HIRATA;
using BaseAp;


namespace LGC
{
    class LGCController : BaseMmfController
    {
        Dictionary<EqGifTimeChartId, int> cv_TimeChartStep = new Dictionary<EqGifTimeChartId, int>();
        public KTimeCharts cv_TimeChart;
        public TimechartController cv_TimechartController;
        public TimeChartParser cv_TimeChartParser;
        KMemoryIOClient cv_Driver;

        public static LGCController g_Controller = null;

        KTimer cv_TimeChartTImer = null;

        public LGCController()
            : base("CommonData.HIRATA")
        {
            iniStatus();
            AssignProcessFunctions();
            g_Controller = this;
            InitTimeChart();
        }
        ~LGCController()
        {
        }
        public void initTimer()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_TimeChartTImer == null)
            {
                cv_TimeChartTImer = new KTimer();
                cv_TimeChartTImer.Interval = 500;
                cv_TimeChartTImer.ThreadEventEnabled = false;
                cv_TimeChartTImer.Enabled = true;
                cv_TimeChartTImer.OnTimer += OnTimer;
            }
        }
        private void OnTimer()
        {
            //WriteTimerIn
            bool change = false;
            for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
            {
                int step = cv_TimechartController.GetTimeChart().GetCurrentStep(i);
                if (step != cv_TimeChartStep[(EqGifTimeChartId)i])
                {
                    cv_TimeChartStep[(EqGifTimeChartId)i] = step;
                    change = true;
                }
            }

            if (change)
            {
                SendTimeChartStepMsg();
            }
            //WriteTimerOut
        }
        private void iniStatus()
        {
            LgcForm.CheckSystemStatus();
        }

        void InitTimeChart()
        {
            for (int i = 1; i < 10; i++)
            {
                cv_TimeChartStep[(EqGifTimeChartId)i] = 0;
            }
            KXmlItem generator_time_chart = new KXmlItem();
            generator_time_chart.LoadFromFile(CommonStaticData.g_TimeChartTemplate);
            KXmlItem single_time_chart = new KXmlItem(generator_time_chart.ItemsByLevelName["TimeChartSequence"]);
            for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP2; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
            {
                single_time_chart.Attributes["Id"] = i.ToString();
                single_time_chart.Attributes["Name"] = ((EqGifTimeChartId)i).ToString().Substring(13);
                generator_time_chart.ItemsByName["TimeCharts"].AddItem(single_time_chart);
            }
            generator_time_chart.SaveToFile(CommonStaticData.g_TimeChart, true);

            string time_chart_contents = generator_time_chart.Text;

            cv_TimechartController = new TimechartController(CommonStaticData.g_TimeChart);
            cv_TimeChartParser = new TimeChartParser(CommonStaticData.g_TimeChart);
            cv_TimeChart = cv_TimechartController.GetTimeChart();
            cv_Driver = cv_TimechartController.GetmemoryIoClient();

        }
        public void SetTimeChartTimeOut()
        {
            TimechartControllerBase.TimechartInstanceBase.cv_T0 = LgcForm.cv_TimeoutData.cv_T0Time;
            TimechartControllerBase.TimechartInstanceBase.cv_T1 = LgcForm.cv_TimeoutData.cv_T1Time;
            TimechartControllerBase.TimechartInstanceBase.cv_T3 = LgcForm.cv_TimeoutData.cv_T3Time;
        }

        protected override void AssignProcessFunctions()
        {
            base.AssignProcessFunctions();
            //online 
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDEqDataReq).Name, ProcessEqStatusNoti);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotDataReq).Name, ProcessRobotStatusNoti);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDPortDataReq).Name, ProcessPortStatusNoti);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.PortData).Name, ProcessPortData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.EqData).Name, ProcessEqData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.BufferData).Name, ProcessBufferData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.RobotData).Name, ProcessRobotData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDPopMonitorFormRep).Name, ProcessCstDataReply);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotInlineChange).Name, ProcessRobotInlineChange);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDApiCommand).Name, ProcessApiCommand);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDDataAction).Name, ProcessDataEditRep);
            //by casse
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name, ProcessBcAliveAndPlcConnect);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDGlassDataCheck).Name, ProcessGlassCheck);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDOcrMode).Name, ProcessOcrMode);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCTimeAdjust).Name, ProcessBcTimeAdjust);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCIdleDelayTime).Name, ProcessBcIdleTime);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCIndexInterval).Name, ProcessBcIntervalTime);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCPortCommand).Name, ProcessBcPortCommand);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCRecipeBodyQuery).Name, ProcessRecipeBodyReq);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCRecipeExist).Name, ProcessRecipeExist);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCAlarmReportToLGC).Name, ProcessBcAlarm);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDApiErrorReset).Name, ProcessApiErrorReset);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDForceCom).Name, ProcessForceCom);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDForceIni).Name, ProcessForceIni);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDResetTimeChart).Name, ProcessRsetTImechart);

        }

        #region base

        protected override void ProcessRobotJobAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDRobotJobAction obj = m_Object as MDRobotJobAction;
            if (obj.PAction == RobotJobAction.preView)
            {
                if (!LgcForm.cv_IsPreView && LgcForm.PSystemData.POperationMode == OperationMode.Manual
                    && LgcForm.cv_RobotJobPath != null && LgcForm.cv_RobotJobPath.Count == 0)
                {
                    LgcForm.cv_IsPreView = true;
                }
            }
            else if (obj.PAction == RobotJobAction.Clean)
            {
                if (LgcForm.PSystemData.POperationMode == OperationMode.Manual)
                {
                    LgcForm.cv_RobotJobPath.Clear();
                    LgcForm.SendRobotJobPath();
                }
            }
            else if (obj.PAction == RobotJobAction.DropTop)
            {
                if (LgcForm.PSystemData.POperationMode == OperationMode.Manual)
                {
                    LgcForm.cv_RobotJobPath.Dequeue();
                    LgcForm.SendRobotJobPath();
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessGlassCountDataReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendGlassCountData();
            SendGlassCountReq(MmfEventClientEventType.etReply, false, m_Ticket);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAccountChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAccountChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessLogInOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessLogInOut(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            //base.ProcessAlarmChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            AlarmData obj = m_Object as AlarmData;
            for (int i = 0; i < obj.cv_AlarmList.Count; i++)
            {
                if (obj.cv_AlarmList[i].PStatus == AlarmStatus.Clean)
                {
                    LgcForm.cv_Alarms.DelAlarm(obj.cv_AlarmList[i]);
                }
            }

            if (!LgcForm.cv_Alarms.cv_AlarmList.Any())
            {
                LgcForm.AddBuzzerCommand(false);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSamplingDataAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSamplingDataAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            string log = "";
            CommonData.HIRATA.MDSamplingDataAction obj = m_Object as CommonData.HIRATA.MDSamplingDataAction;
            switch(obj.PAction)
            {
                case DataEidtAction.Add:
                    foreach (CommonData.HIRATA.SamplingIem recipe in obj.SamplingDatas)
                    {
                        if (LgcForm.cv_SamplingData.AddSamplingItem(recipe))
                        {
                        }
                    }
                    break;
                case DataEidtAction.Del:
                    foreach (CommonData.HIRATA.SamplingIem recipe in obj.SamplingDatas)
                    {
                        if (LgcForm.cv_SamplingData.DelSamplingItem(recipe))
                        {
                        }
                    }
                    break;
                case DataEidtAction.Edit:
                    foreach (CommonData.HIRATA.SamplingIem recipe in obj.SamplingDatas)
                    {
                        if (LgcForm.cv_SamplingData.ModifySamplingItem(recipe))
                        {
                        }
                    }
                    break;
            };

            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            string log = "";
            CommonData.HIRATA.MDRecipeAction obj = m_Object as CommonData.HIRATA.MDRecipeAction;
            if (obj.PAction == CommonData.HIRATA.DataEidtAction.Add)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (LgcForm.cv_Recipes.AddRecipe(recipe))
                    {
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.New;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.Del)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (LgcForm.cv_Recipes.DelRecipe(recipe))
                    {
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Delete;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.Edit)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (LgcForm.cv_Recipes.ModifyRecipe(recipe))
                    {
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Modity;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.SetCur)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (LgcForm.cv_Recipes.IsRecipeExist(recipe.PId))
                    {
                        LgcForm.cv_Recipes.SetCurRecipe(recipe.PId);
                    }
                    /*
                    if (LgcForm.cv_Recipes.ModifyRecipe(recipe))
                    {
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Modity;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                    }
                    */
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        
        protected override void ProcessSamplingDataReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSamplingDataReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            SendMmfReplyObject(typeof(CommonData.HIRATA.SamplingData).Name, LgcForm.cv_SamplingData, m_Ticket, typeof(CommonData.HIRATA.MDSamplingDataReq).Name, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            SendMmfReplyObject(typeof(CommonData.HIRATA.RecipeData).Name, LgcForm.cv_Recipes, m_Ticket, typeof(CommonData.HIRATA.MDRecipeReq).Name, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            SendAlarmData();
            SendAlarmReq(MmfEventClientEventType.etReply, false, m_Ticket);
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name, new CommonData.HIRATA.MDBCAliveAndPlcConnect());
            LgcForm.GetBufferById(1).SendDataViaMmf();
            LgcForm.GetAlignerById(1).SendDataViaMmf();
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
            string log = "[Recv Online Req]\n";
            CommonData.HIRATA.MDOnlineRequest obj = m_Object as CommonData.HIRATA.MDOnlineRequest;
            log += "obj content : Cur Mode :" + obj.PCurMode.ToString() + " Change Mode : " + obj.PChangeMode.ToString() + Environment.NewLine;
            CommonData.HIRATA.MDOnlineRequest rtn = new MDOnlineRequest();
            rtn.PResult = Result.OK;
            rtn.PType = MmfEventClientEventType.etReply;
            if (obj.PType == CommonData.HIRATA.MmfEventClientEventType.etRequest)
            {
                if (obj.PChangeMode == OnlineMode.Offline)
                {
                    LgcForm.PSystemData.PSystemOnlineMode = obj.PChangeMode;
                    SendOnlineReq(CommonData.HIRATA.MmfEventClientEventType.etReply, LgcForm.PSystemData.PSystemOnlineMode, false);
                    log += "Change online mode successful";
                }
                else if (obj.PChangeMode == OnlineMode.Control)
                {
                    if (LgcForm.PSystemData.PPlcConnect && LgcForm.PSystemData.PBcAlive)
                    {
                        LgcForm.PSystemData.PSystemOnlineMode = obj.PChangeMode;
                        SendOnlineReq(CommonData.HIRATA.MmfEventClientEventType.etReply, LgcForm.PSystemData.PSystemOnlineMode, false);
                        log += "Change online mode successful";
                    }
                    else
                    {
                        SendOnlineReq(CommonData.HIRATA.MmfEventClientEventType.etReply, LgcForm.PSystemData.PSystemOnlineMode, false);
                        SendMsgToUi(true, false, 10000, "Can't change CIM mode , please check BC status / PLC connection ");
                        log += "Change online mode fail , check BC/PLC status";
                    }
                }
                else
                {
                    SendMsgToUi(true, false, 10000, "Can't change CIM mode : " + obj.PChangeMode.ToString());
                    SendOnlineReq(CommonData.HIRATA.MmfEventClientEventType.etReply, LgcForm.PSystemData.PSystemOnlineMode, false);
                    log += "Change online mode fail , check UI' msg content : " + obj.PChangeMode.ToString();
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessInitialize(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessInitialize(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            CommonData.HIRATA.MDInitial obj = m_Object as CommonData.HIRATA.MDInitial;
            if (LgcForm.PSystemData.PRobotConnect && LgcForm.PSystemData.PRobotInline != EquipmentInlineMode.None)
            {
                LgcForm.GetRobotById(1).SetInitilize(obj.cv_IsForce);
                LgcForm.GetAlignerById(1).cv_Data.PPreAction = AlignerPreAction.None;
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
                {
                    cv_TimeChart.RestartTimeChart(i);
                }
                SendInitialize(InitialAction.Initial, MmfEventClientEventType.etReply, false, m_Ticket, Result.OK);
            }
            else
            {
                SendInitialize(InitialAction.Initial, MmfEventClientEventType.etReply, false, m_Ticket, Result.NG);
                LgcForm.ShowMsg("Initialize failure , please check Robot connect / Robot current mode.", true, false);
            }
            SendTimeChartStepMsg();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSetTimeOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSetTimeOut(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            CommonData.HIRATA.MDSetTimeOut obj = m_Object as CommonData.HIRATA.MDSetTimeOut;
            SendMmfReplyObject(typeof(CommonData.HIRATA.MDSetTimeOut).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDSetTimeOut).Name);
            SendTimeOutSetting(MmfEventClientEventType.etReply, obj, false);

            LgcForm.cv_TimeoutData.cv_IdleDelayTime = obj.PIdleTIme;
            LgcForm.cv_TimeoutData.cv_IntervalTime = obj.PIntervalTIme;
            LgcForm.cv_TimeoutData.cv_T0Time = obj.PT0TIme;
            LgcForm.cv_TimeoutData.cv_T1Time = obj.PT1TIme;
            LgcForm.cv_TimeoutData.cv_T3Time = obj.PT3TIme;
            LgcForm.cv_TimeoutData.cv_TsTime = obj.PTsTIme;
            LgcForm.cv_TimeoutData.cv_TeTime = obj.PTeTIme;
            LgcForm.cv_TimeoutData.cv_ApiT3TIme = obj.PApiT3TIme;
            LgcForm.cv_TimeoutData.cv_TmTime = obj.PTmTIme;
            LgcForm.cv_TimeoutData.cv_TmTime = obj.PTmTIme;
            LgcForm.cv_TimeoutData.SaveToFile();
            SetTimeChartTimeOut();
            SendTimeoutData();


            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSystemData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSystemData(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            SendMmfReplyObject(typeof(CommonData.HIRATA.SystemData).Name, LgcForm.PSystemData, m_Ticket, typeof(CommonData.HIRATA.SystemData).Name, KParseObjToXmlPropertyType.Field);
            CommonData.HIRATA.MDOnlineRequest online_obj = new CommonData.HIRATA.MDOnlineRequest();
            online_obj.PType = CommonData.HIRATA.MmfEventClientEventType.etNotify;
            online_obj.PCurMode = LgcForm.PSystemData.PSystemOnlineMode;
            SendMmfReplyObject(typeof(CommonData.HIRATA.MDOnlineRequest).Name, online_obj, m_Ticket, typeof(CommonData.HIRATA.MDOnlineRequest).Name, KParseObjToXmlPropertyType.Field);

            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessTimeoutData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            /*
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessTimeoutData(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            */
        }
        protected override void ProcessOperatorModeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessOperatorModeChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            string log = "[Recv Operator Mode Change]" + Environment.NewLine;
            CommonData.HIRATA.MDOperationModeChange obj = m_Object as CommonData.HIRATA.MDOperationModeChange;
            log += "Cur Operation mode :" + LgcForm.cv_SystemData.POperationMode.ToString() + " Change Mode : " + obj.PChangeOperationMode.ToString() + Environment.NewLine;
            log += "Change Mode use force feature : " +obj.PIsForce.ToString() +  Environment.NewLine;
            if (obj.PType == CommonData.HIRATA.MmfEventClientEventType.etRequest)
            {
                if (!LgcForm.cv_Alarms.IsHasAlarm() && LgcForm.PSystemData.PRobotInline == EquipmentInlineMode.Remote)
                {
                    if ( (LgcForm.cv_RobotManaulJobPath.Count > 0) && obj.PChangeOperationMode == OperationMode.Auto )
                    {
                        LgcForm.cv_RobotManaulJobPath.Clear();
                        SendMsgToUi(true, false, 10000, "System auto clear robot manual job.");
                    }
                    //LgcForm.PSystemData.POperationMode = obj.PChangeOperationMode;
                    if(obj.PChangeOperationMode == OperationMode.Manual)
                    {
                        if(LgcForm.cv_RobotJobPath.Count > 0)
                        {
                            if(!obj.PIsForce)
                            {
                            LgcForm.cv_IsCycleStop = true;
                        }
                        else
                        {
                                LgcForm.cv_IsCycleStop = false;
                                LgcForm.PSystemData.POperationMode = OperationMode.Manual;
                            }
                        }
                        else
                        {
                            LgcForm.PSystemData.POperationMode = OperationMode.Manual;
                        }
                    }
                    else
                    {
                        LgcForm.PSystemData.POperationMode = OperationMode.Auto;
                    }
                    SendOperationMode(false , LgcForm.PSystemData.POperationMode, MmfEventClientEventType.etReply, false, m_Ticket);
                }
                else
                {
                    SendMsgToUi(true, false, 10000, "Can't change Operation mode ( check alarm and Robot status)!!!");
                    SendOperationMode(false , LgcForm.PSystemData.POperationMode, MmfEventClientEventType.etReply, false, m_Ticket);
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSystemDataReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSystemDataReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            SendSystemDataReq(MmfEventClientEventType.etReply, false, m_Ticket);
            SendSystemData();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessTimeOutReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessTimeOutReq(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            SendTimeoutData();
            SendTimeOutReq(MmfEventClientEventType.etReply, false, m_Ticket);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRobotAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRobotAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            string log = "";
            CommonData.HIRATA.MDRobotAction obj = m_Object as CommonData.HIRATA.MDRobotAction;
            log += "Robot RobotId : " + obj.RobotId + " Arm : " + obj.Source.PArm + " Action : " + obj.PAction.ToString() + " Target : " + obj.Source.PTarget.ToString() +
                    " TargetId : " + obj.Source.Id + " TargetSlot : " + obj.Source.Slot + Environment.NewLine;
            obj.PResult = CommonData.HIRATA.Result.NG;
            Robot robot = LgcForm.GetRobotById(1);
            if(LgcForm.PSystemData.POperationMode != OperationMode.Manual)
            {
                LgcForm.ShowMsg("Manual Operator please change to Manual Mode.", true, false);
                obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
                Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDRobotAction).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDRobotAction).Name);
                return;
            }
            if (!robot.IsBusy)
            {
                if (CheckTargetCanAccess(obj.Source.PTarget, obj.PAction, obj.Source.Id, obj.Source.Slot) &&
                    CheckRobotCanAccess(obj.RobotId, obj.Source.PArm, obj.PAction)
                    )
                {
                    obj.PResult = CommonData.HIRATA.Result.OK;
                    APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                    if (obj.PAction == CommonData.HIRATA.RobotAction.Put) robot_command = APIEnum.RobotCommand.WaferPut;
                    else if (obj.PAction == CommonData.HIRATA.RobotAction.Get) robot_command = APIEnum.RobotCommand.WaferGet;
                    else if (obj.PAction == CommonData.HIRATA.RobotAction.TopPut) robot_command = APIEnum.RobotCommand.TopWaferPut;
                    /*
                else if (obj.PAction == CommonData.HIRATA.RobotAction.PutWait) robot_command = APIEnum.RobotCommand.PutStandby;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.GetWait) robot_command = APIEnum.RobotCommand.GetStandby;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.TopGet) robot_command = APIEnum.RobotCommand.TopWaferGet;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.TopPut) robot_command = APIEnum.RobotCommand.TopWaferPut;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.TopGetWait) robot_command = APIEnum.RobotCommand.TopGetStandby;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.TopPutWait) robot_command = APIEnum.RobotCommand.TopPutStandby;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.HighExchange) robot_command = APIEnum.RobotCommand.GetStandbyArmExtend;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.Initialize) robot_command = APIEnum.RobotCommand.PutStandbyArmExtend;
                else if (obj.PAction == CommonData.HIRATA.RobotAction.ActionComplete) robot_command = APIEnum.RobotCommand.TopPutStandbyArmExtend;
                */
                    List<string> para = new List<string>();
                    para.Add(((int)obj.Source.PArm).ToString());
                    if (obj.Source.PTarget == CommonData.HIRATA.ActionTarget.Eq)
                    {
                        para.Add("Stage" + LgcForm.GetEqById(obj.Source.Id).cv_Comm.cv_RobotPosition.ToString());
                    }
                    else if (obj.Source.PTarget == CommonData.HIRATA.ActionTarget.Port)
                    {
                        para.Add("P" + obj.Source.Id);
                    }
                    else if (obj.Source.PTarget == CommonData.HIRATA.ActionTarget.Buffer)
                    {
                        para.Add("Stage9");
                    }
                    else if (obj.Source.PTarget == CommonData.HIRATA.ActionTarget.Aligner)
                    {
                        para.Add("Aligner" + obj.Source.Id);
                    }
                    para.Add(obj.Source.Slot.ToString());
                    RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                    if (obj.PAction == CommonData.HIRATA.RobotAction.Get)// || obj.PAction == CommonData.HIRATA.RobotAction.GetWait || obj.PAction == CommonData.HIRATA.RobotAction.TopGet
                    //|| obj.PAction == CommonData.HIRATA.RobotAction.TopGetWait)
                    {
                        tmp_job = new RobotJob(obj.RobotId, CommonData.HIRATA.RobotArm.rabNone, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                    }
                    else if (obj.PAction == CommonData.HIRATA.RobotAction.Put || obj.PAction == CommonData.HIRATA.RobotAction.TopPut)// || obj.PAction == CommonData.HIRATA.RobotAction.PutWait || obj.PAction == CommonData.HIRATA.RobotAction.TopPutWait
                    //|| obj.PAction == CommonData.HIRATA.RobotAction.TopPut)
                    {
                        tmp_job = new RobotJob(obj.RobotId, obj.Source.PArm, CommonData.HIRATA.RobotArm.rabNone, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                    }
                    else if (obj.PAction == RobotAction.Exchange)
                    {
                        if (obj.Source.PArm == RobotArm.rbaDown)
                            tmp_job = new RobotJob(obj.RobotId, CommonData.HIRATA.RobotArm.rbaUp, obj.Source.PArm, RobotAction.Exchange, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                        else if (obj.Source.PArm == RobotArm.rbaUp)
                            tmp_job = new RobotJob(obj.RobotId, CommonData.HIRATA.RobotArm.rbaDown, obj.Source.PArm, RobotAction.Exchange, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                    }
                    else if (obj.PAction == RobotAction.PutGetAligner)
                    {
                        tmp_job = new RobotJob(1, obj.Source.PArm, obj.Source.PArm, RobotAction.Exchange, ActionTarget.Aligner, 1, 1, false);
                        if (obj.Source.PTarget == ActionTarget.Aligner)
                        {
                            tmp_job.PManualExchangeForAligner = true;
                            tmp_job.PManualExchangeForAlignerDeg = obj.PAlignerDeg;
                        }
                    }

                    if (!obj.PUseHS)
                    {
                        CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(), APIEnum.CommnadDevice.Robot, 0, para);
                        robot.SetRobotTransferAction(tmp_command, tmp_job);
                    }
                    else
                    {
                        if ((!LgcForm.GetRobotById(1).IsBusy) && (LgcForm.PSystemData.POperationMode != OperationMode.Auto) &&
                            (LgcForm.cv_RobotManaulJobPath.Count == 0))
                        {
                            LgcForm.cv_RobotManaulJobPath.Enqueue(tmp_job);
                        }
                        else
                        {
                            LgcForm.ShowMsg("Robot busy , please do Initialize actioin.", true, false);
                        }
                    }
                }
            }
            else
            {
                LgcForm.ShowMsg("Robot has job,  please check", true, false);
            }
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDRobotAction).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDRobotAction).Name);

            WriteLog(LogLevelType.General, log);

            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessChangePortSlotType(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessChangePortSlotType(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            CommonData.HIRATA.MDChangePortSlotType obj = m_Object as CommonData.HIRATA.MDChangePortSlotType;
            Port port = LgcForm.GetPortById(obj.PPortId);
            int type = obj.PSlotType;
            if ((port.PPortStatus != PortStaus.LDRQ) || (port.PLotStatus != LotStatus.Empty))
            {
                SendPortSlotTypeChange(obj.PPortId, obj.PSlotType, MmfEventClientEventType.etReply, false, Result.NG, m_Ticket);
                LgcForm.ShowMsg("Port not at LDRQ , please remove foup", true, false);
                return;
            }
            List<string> paras = new List<string>();
            if (type == 0)
            {
                port.cv_Data.cv_SlotCount = 25;
                port.cv_SlotCount = 25;
                port.cv_Data.PEfemPortType = 0;
                port.cv_Data.SaveToFile();
                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_SysLayoutFile);
                tmp.ItemsByName["PortList"].Items[obj.PPortId - 1].Attributes["Capacity"] = "25";
                tmp.SaveToFile(CommonData.HIRATA.CommonStaticData.g_SysLayoutFile);
                paras.Add(type.ToString());
                obj.PResult = Result.OK;
                CommonData.HIRATA.CommandData command_obj = new CommandData(APIEnum.CommandType.LoadPort,
                    APIEnum.LoadPortCommand.SetType.ToString(), APIEnum.CommnadDevice.P, obj.PPortId, paras);
                port.SendDataViaMmf();
                LgcForm.GetRobotById(1).cv_Comm.SetPortSlotTypeChange(command_obj);
                SendPortSlotTypeChange(obj.PPortId, obj.PSlotType, MmfEventClientEventType.etReply, false, Result.OK, m_Ticket);
                port.SendDataViaMmf();
            }
            else if (type == 4)
            {
                port.cv_Data.cv_SlotCount = 13;
                port.cv_SlotCount = 13;
                port.cv_Data.PEfemPortType = 4;
                port.cv_Data.SaveToFile();

                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_SysLayoutFile);
                tmp.ItemsByName["PortList"].Items[obj.PPortId - 1].Attributes["Capacity"] = "13";
                tmp.SaveToFile(CommonData.HIRATA.CommonStaticData.g_SysLayoutFile);

                paras.Add(type.ToString());
                CommonData.HIRATA.CommandData command_obj = new CommandData(APIEnum.CommandType.LoadPort,
                    APIEnum.LoadPortCommand.SetType.ToString(), APIEnum.CommnadDevice.P, obj.PPortId, paras);
                LgcForm.GetRobotById(1).cv_Comm.SetPortSlotTypeChange(command_obj);
                obj.PResult = Result.OK;
                port.SendDataViaMmf();
                SendPortSlotTypeChange(obj.PPortId, obj.PSlotType, MmfEventClientEventType.etReply, false, Result.OK, m_Ticket);
                port.SendDataViaMmf();
            }
            else
            {
                SendPortSlotTypeChange(obj.PPortId, obj.PSlotType, MmfEventClientEventType.etReply, false, Result.NG, m_Ticket);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessOcrDecideReply(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessChangePortSlotType(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            MDShowOcrDecideReply obj = m_Object as MDShowOcrDecideReply;
            Aligner aligner = LgcForm.GetAlignerById(1);
            if(LgcForm.PSystemData.POcrMode == OCRMode.ErrorHold)
            {
                if(obj.POcrDecide == OCRMode.SkipRead)
                {
                    aligner.cv_Data.GlassDataMap[1].PId = obj.PKeyInValue;
                }
            }
            aligner.cv_Data.GlassDataMap[1].POcrDecide = obj.POcrDecide;
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region Process BC msg
        void ProcessBcAlarm(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            CommonData.HIRATA.MDBCAlarmReportToLGC obj = m_Object as CommonData.HIRATA.MDBCAlarmReportToLGC;
            LgcForm.EditAlarm(obj.AlarmItem);
        }
        void ProcessRecipeExist(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDBCRecipeExist obj = m_Object as CommonData.HIRATA.MDBCRecipeExist;
            Port job_port = LgcForm.GetPortById(obj.PPortId);

            CommonData.HIRATA.MDBCRecipeExistReply report = new MDBCRecipeExistReply();
            report.PNode = 2;
            report.PPortId = obj.PPortId;
            for (int i = 0; i < obj.cv_Recipes.Count; i++)
            {
                bool is_exist = true;
                if (!LgcForm.cv_Recipes.cv_RecipeList.Exists(x => int.Parse(x.cv_Id) == obj.cv_Recipes[i]) && obj.cv_Recipes[i] != 0)
                {
                    is_exist = false;
                }
                report.cv_RecipesExist.Add(is_exist);
            }
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeExistReply).Name, report, KParseObjToXmlPropertyType.Field);

            //WriteOut
        }
        void ProcessRecipeBodyReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDBCRecipeBodyQuery obj = m_Object as CommonData.HIRATA.MDBCRecipeBodyQuery;
            int recipe_no = obj.PRecipeId;
            int index = LgcForm.cv_Recipes.cv_RecipeList.FindIndex(x => int.Parse(x.cv_Id) == recipe_no);
            if (-1 != index)
            {
                RecipeItem report_recipe = LgcForm.cv_Recipes.cv_RecipeList[index];
                CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                report.PAction = RecipeBodyReportType.Query;
                report.PUnit = 0;
                report.PRecipe = report_recipe;
                report.PFlowNo = (int)report_recipe.PFlow;
                SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report, KParseObjToXmlPropertyType.Field);
            }
            else
            {
                LgcForm.ShowMsg("BC Query recipe : " + recipe_no.ToString() + " , not exist !!!", true, false);
            }
            //WriteOut
        }
        void ProcessBcPortCommand(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCPortCommand obj = m_Object as CommonData.HIRATA.MDBCPortCommand;
            Port job_port = LgcForm.GetPortById((int)obj.PPortId);
            string log = "Recv BC Port Command : Port : " + obj.PPortId.ToString() + " Command : " + obj.PPortCommand.ToString() + "\n";
            log += job_port.cv_Data.GetPortStatusStringForLogUse() + "\n";
            if(LgcForm.PSystemData.PSystemOnlineMode != OnlineMode.Control)
            {
                LgcForm.ShowMsg("Recv Port Command : " + obj.PPortCommand.ToString() + " but not CIM ON", true, false);
                log += "Recv Port Command : " + obj.PPortCommand.ToString() + " but not CIM ON\n";
            }
            LgcForm.WriteLog(LogLevelType.General, log);
            log = "";
            switch (obj.PPortCommand)
            {
                case BCPortCommand.End:
                    if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.Process)
                        {
                            job_port.PLotStatus = LotStatus.ProcessEnd;
                            log += "Set Port : " + obj.PPortId + " Process End\n";
                        }
                        else
                        {
                            LgcForm.ShowMsg("BC send Quit command then set End for Port : " + obj.PPortId + " , but port not at process status.", true, false);
                            log += "BC send Quit command then set End for Port : " + obj.PPortId + " , but port not at process status.\n";
                        }
                    }
                    break;
                    //the same with BC sepc quit
                case BCPortCommand.Cancel:
                    if (job_port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (job_port.PLotStatus == LotStatus.MappingEnd || job_port.PLotStatus == LotStatus.WaitCommand || job_port.PLotStatus == LotStatus.WaitReserve)
                        {
                            //job_port.PLotStatus = LotStatus.Cancel;
                            job_port.cv_Data.cv_IsWaitCancel = true;
                            LgcForm.ShowMsg("BC send Quit command then set cancel for Port : " + obj.PPortId, true, false);
                            log += "Set Port : " + obj.PPortId + " cv_IsWaitCancel true\n";
                        }
                        else if (job_port.PLotStatus == LotStatus.Process)
                        {
                            //job_port.PLotStatus = LotStatus.Abort;
                            job_port.cv_Data.cv_IsWaitAbort = true;
                            LgcForm.ShowMsg("BC send Quit command then set abort for Port : " + obj.PPortId, true, false);
                            log += "Set Port : " + obj.PPortId + " cv_IsWaitAbort true\n";
                        }
                    }
                    else if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.MappingEnd || job_port.PLotStatus == LotStatus.Reserved)// || job_port.PLotStatus == LotStatus.Process)
                        {
                            job_port.cv_Data.cv_IsWaitCancel = true;
                            LgcForm.ShowMsg("BC send Quit command then set cancel for Port : " + obj.PPortId, true, false);
                            log += "Set Port : " + obj.PPortId + " cv_IsWaitCancel true\n";
                        }
                        else if (job_port.PLotStatus == LotStatus.Process)
                        {
                            job_port.cv_Data.cv_IsWaitAbort = true;
                            LgcForm.ShowMsg("BC send Quit command then set abort for Port : " + obj.PPortId, true, false);
                            log += "Set Port : " + obj.PPortId + " cv_IsWaitAbort true\n";
                        }
                        else
                        {
                            LgcForm.ShowMsg("BC send Quit command for Port : " + obj.PPortId +
                                " but the port is unloader , so reject bc command!!! ", true, false);
                            log += "BC send Quit command for Port : " + obj.PPortId + " but the port is unloader , so reject bc command!!! \n";
                        }
                    }
                    break;
                case BCPortCommand.Finish:
                    if (job_port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (job_port.PLotStatus != LotStatus.ProcessEnd && job_port.PLotStatus != LotStatus.Cancel && job_port.PLotStatus != LotStatus.Abort)
                        {
                            LgcForm.ShowMsg("Recv BC Port Command : " + obj.PPortCommand.ToString() + " , but Port status in at : " + job_port.PLotStatus.ToString()
                                , true, false);
                            log += "Recv BC Port Command : " + obj.PPortCommand.ToString() + " , but Port status in at : " + job_port.PLotStatus.ToString() + "\n";
                        }
                        else
                        {
                            job_port.cv_Data.PWaitUnload = true;
                            log += "Set Port : " + obj.PPortId + " PWaitUnload true\n";
                        }
                    }
                    else if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.ProcessEnd || job_port.PLotStatus == LotStatus.Cancel || job_port.PLotStatus == LotStatus.Abort)
                        {
                            job_port.cv_Data.PWaitUnload = true;
                            log += "Set Port : " + obj.PPortId + " PWaitUnload true\n";
                        }
                        else
                        {
                            LgcForm.ShowMsg("Recv BC Port Command : " + obj.PPortCommand.ToString() + " , but Port status in at : " + job_port.PLotStatus.ToString()
                                , true, false);
                            log += "Recv BC Port Command : " + obj.PPortCommand.ToString() + " , but Port status in at : " + job_port.PLotStatus.ToString() + "\n";
                        }
                    }
                    break;
                case BCPortCommand.Retry:
                    //if (job_port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (job_port.PLotStatus == LotStatus.MappingEnd)
                        {
                            job_port.PLotStatus = LotStatus.FoupSensorOn;
                            LgcForm.GetRobotById(1).cv_Comm.SetGetMappingData(obj.PPortId);
                            log += "Set Port : " + obj.PPortId + " Re-mapping\n";
                        }
                        else
                        {
                            LgcForm.ShowMsg("BC send retry command for Port : " + obj.PPortId + 
                                " but the port isn't at mapping end status, so reject bc command!!! ", true, false);
                            log += "BC send retry command for Port : " + obj.PPortId + " but the port isn't at mapping end status, so reject bc command!!! \n";
                        }
                    }
                    break;
                    // ODF port mode is fixed. Can't change.
                case BCPortCommand.Loader:
                    break;
                case BCPortCommand.Unloader:
                    break;
                case BCPortCommand.LDUD:
                    break;
                case BCPortCommand.Start:
                    if (job_port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (job_port.PLotStatus != LotStatus.WaitCommand)
                        {
                            LgcForm.ShowMsg("Recv BC Port Command : " + obj.PPortCommand.ToString() + " , but Port status not in Wait Reserve", true, false);
                            log += "Recv BC Port Command : " + obj.PPortCommand.ToString() + " , but Port status not in Wait Reserve\n";
                        }
                        else
                        {
                            int history = 0;
                            //history+ job_port
                            for (int slot = 1; slot <= job_port.cv_Data.cv_SlotCount; slot++)
                            {
                                if (job_port.cv_Data.GlassDataMap[slot].PHasSensor && job_port.cv_Data.GlassDataMap[slot].PHasData)
                                {
                                    history++;
                                }
                            }
                            LgcForm.cv_GlassCountData.PHistoryCount += history;
                            job_port.PLotStatus = LotStatus.Process;
                            LgcForm.AddPortToProcessList(obj.PPortId);
                            log += "Set Port " + obj.PPortId + " Lot status Porcess \n";
                        }
                    }
                    break;

                case BCPortCommand.Reserve:
                    if (job_port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (job_port.PLotStatus != LotStatus.WaitReserve)
                        {
                            LgcForm.ShowMsg("Recv BC Load Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in Wait Reserve", true, false);
                            log += "Recv BC Load Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in Wait Reserve\n";
                        }
                        else
                        {
                            job_port.PLotStatus = LotStatus.Reserved;
                            log += "Set Port : " + obj.PPortId + " Reserved\n";
                            job_port.PLotStatus = LotStatus.WaitCommand;
                            log += "Set Port : " + obj.PPortId + " WaitCommand\n";
                        }
                    }
                    if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus != LotStatus.MappingEnd)
                        {
                            LgcForm.ShowMsg("Recv BC Unload Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in MappingEnd", true, false);
                            log += "Recv BC Unload Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in MappingEnd\n";
                        }
                        else
                        {
                            job_port.PLotStatus = LotStatus.Reserved;
                            LgcForm.cv_InProcessPort.Add(job_port.cv_Id);
                            log += "Set Port : " + obj.PPortId + " Reserved\n";
                        }
                    }
                    break;
                case BCPortCommand.Pause:
                    if (job_port.PLotStatus != LotStatus.Process)
                    {
                        LgcForm.ShowMsg("Recv BC Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in Process ", true, false);
                        log += "Recv BC Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in Process \n";
                    }
                    else
                    {
                        job_port.PLotStatus = LotStatus.Pause;
                        log += "Set Port : " + obj.PPortId + " Pause\n";
                    }
                    break;
                case BCPortCommand.Resume:
                    if (job_port.PLotStatus != LotStatus.Pause)
                    {
                        LgcForm.ShowMsg("Recv BC Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in Pause ", true, false);
                        log += "Recv BC Port Command : " + obj.PPortCommand.ToString() + ", but Port status not in Pause \n";

                    }
                    else
                    {
                        job_port.PLotStatus = LotStatus.Process;
                        log += "Set Port : " + obj.PPortId + " Process\n";
                    }
                    break;
            };
            LgcForm.WriteLog(LogLevelType.General, log);
            LgcForm.WriteLog(LogLevelType.General, "Cur port status : " + job_port.cv_Data.GetPortStatusStringForLogUse());
            LgcForm.WriteLog(LogLevelType.General, CommonData.HIRATA.CommonStaticData.g_SplitLine);
            WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessBcIntervalTime(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDBCIndexInterval obj = m_Object as CommonData.HIRATA.MDBCIndexInterval;
            LgcForm.cv_TimeoutData.PIntervalTime = obj.PInterval * 1000;
            //WriteOut
        }
        void ProcessBcIdleTime(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDBCIdleDelayTime obj = m_Object as CommonData.HIRATA.MDBCIdleDelayTime;
            LgcForm.cv_TimeoutData.PIdleDelayTime = obj.PIdleDelayTime * 1000;
            //WriteOut
        }
        void ProcessBcTimeAdjust(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDBCTimeAdjust obj = m_Object as CommonData.HIRATA.MDBCTimeAdjust;
            SYSTEMTIME time = new SYSTEMTIME();
            time.wYear = (ushort)obj.PYear;
            time.wMonth = (ushort)obj.PMon;
            time.wDay = (ushort)obj.PDay;
            time.wHour = (ushort)obj.PHour;
            time.wMinute = (ushort)obj.PMin;
            time.wSecond = (ushort)obj.PSec;
            Global.SystemTimeAdjust(obj.PYear, obj.PMon, obj.PDay, obj.PHour, obj.PMin, obj.PSec);
            //WriteOut
        }

        #endregion

        #region AssignProcessFunctions

        void ProcessApiErrorReset(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            if (LgcForm.cv_Alarms.IsHasAlarm())
            {
                Robot rb = LgcForm.GetRobotById(1);
                CommonData.HIRATA.MDApiErrorReset obj = m_Object as CommonData.HIRATA.MDApiErrorReset;
                CommandData command_obj = new CommandData();
                command_obj = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(), APIEnum.CommnadDevice.Robot, 0);
                rb.cv_Comm.SetRobotCommonAction(command_obj);
                for (int portid = 1; portid <= CommonData.HIRATA.CommonStaticData.g_PortNumber; portid++)
                {
                    command_obj = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(), APIEnum.CommnadDevice.P, portid);
                    rb.cv_Comm.SetRobotCommonAction(command_obj);
                }
                command_obj = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(), APIEnum.CommnadDevice.Aligner, 1);
                rb.cv_Comm.SetRobotCommonAction(command_obj);
            }
            //WriteOut
        }
        void ProcessApiCommand(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDApiCommand obj = m_Object as CommonData.HIRATA.MDApiCommand;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDApiCommand).Name, KParseObjToXmlPropertyType.Field);
            int port_id = obj.CommandData.cv_DeviceId;
            Port port = LgcForm.GetPortById(port_id);
            if (obj.CommandData.PCommandType == APIEnum.CommandType.Common && obj.CommandData.PCommonCommand == APIEnum.CommonCommand.Home &&
                obj.CommandData.PCommandDevice == APIEnum.CommnadDevice.P) //sForce)
            {
                if (LgcForm.PSystemData.PSystemOnlineMode == OnlineMode.Control)
                {
                    if (port.PLotStatus == LotStatus.Process)
                    {
                        port.cv_Data.cv_IsWaitAbort = true;
                        //port.PLotStatus = LotStatus.Abort;
                    }
                    else
                    {
                        port.cv_Data.cv_IsWaitAbort = true;
                        //port.PLotStatus = LotStatus.Cancel;
                    }
                    LgcForm.ShowMsg("[CIM ON] Chnage to auto after manual unload!!!", false, false);
                    return;
                }
                else
                {
                    if (LgcForm.PSystemData.POperationMode != OperationMode.Manual)
                    {
                        LgcForm.ShowMsg("CIM OFF unload , Please change to manual mode.", true, false);
                        return;
                    }

                    if (port.PLotStatus == LotStatus.Process)
                    {
                        //port.cv_Data.cv_IsWaitAbort = true;
                        port.PLotStatus = LotStatus.Abort;
                    }
                    else
                    {
                        //port.cv_Data.cv_IsWaitAbort = true;
                        port.PLotStatus = LotStatus.Cancel;
                    }
                    port.PPortStatus = PortStaus.UDRQ;
                }
            }
            else if (obj.CommandData.PCommandType == APIEnum.CommandType.Robot && obj.CommandData.PRobotCommand == APIEnum.RobotCommand.SetRobotSpeed)
            {
                if (LgcForm.PSystemData.POperationMode != OperationMode.Manual)
                {
                    LgcForm.ShowMsg("Please change to manual mode.", true, false);
                    return;
                }
                LgcForm.GetRobotById(1).cv_WaitRobotSpeed = Convert.ToInt16(obj.CommandData.cv_ParaList[0]);
            }
            else if (obj.CommandData.PCommandType == APIEnum.CommandType.IO && obj.CommandData.PIoCommand == APIEnum.IoCommand.SetFFUVoltage)
            {
                if (LgcForm.PSystemData.POperationMode != OperationMode.Manual)
                {
                    LgcForm.ShowMsg("Please change to manual mode.", true, false);
                    return;
                }
                LgcForm.GetRobotById(1).cv_WaitFfuSpeed = Convert.ToInt16(obj.CommandData.cv_ParaList[0]);
            }
            else if (obj.CommandData.PCommandType == APIEnum.CommandType.IO && obj.CommandData.PIoCommand == APIEnum.IoCommand.Buzzer)  //Ref20230421 Tommy Add
            {
                //LgcForm.GetRobotById(1).cv_Comm.SetRobotCommonAction(obj.CommandData);
                if (obj.CommandData.cv_ParaList[0] == "0")
                {
                    LgcForm.AddBuzzerCommand(false);
                }
                return;
            }
            if (LgcForm.PSystemData.POperationMode != OperationMode.Manual)
            {
                LgcForm.ShowMsg("Please change to manual mode.", true, false);
                return;
            }
            LgcForm.GetRobotById(1).cv_Comm.SetRobotCommonAction(obj.CommandData);
            //WriteOut
        }
        void ProcessRobotInlineChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDRobotInlineChange obj = m_Object as CommonData.HIRATA.MDRobotInlineChange;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDRobotInlineChange).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDRobotInlineChange).Name, KParseObjToXmlPropertyType.Field);
            LgcForm.GetRobotById(1).cv_Comm.SetApiCommonCommand(obj.PChangeInlineMode == EquipmentInlineMode.Local ? APIEnum.APICommand.Local : APIEnum.APICommand.Remote);
            //WriteOut
        }
        void ProcessPortData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            try
            {
                WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
                string log = "";
                if (m_SourceModule == "UI")
                {
                    log = "[Recv Port data form UI]\n";
                    CommonData.HIRATA.PortData obj = m_Object as CommonData.HIRATA.PortData;
                    CommonData.HIRATA.PortData tmp = LgcForm.cv_PortContainer[(int)obj.PId].cv_Data;
                    tmp.GlassDataMap = tmp.GlassDataMap;
                    Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.PortData).Name, tmp, KParseObjToXmlPropertyType.Field);
                }
                else if (m_SourceModule == "CIM")
                {
                    log = "[Recv Port data form CIM]\n";
                    CommonData.HIRATA.PortData obj = m_Object as CommonData.HIRATA.PortData;
                    Port job_port = LgcForm.GetPortById((int)obj.PId);
                    if (job_port != null)
                    {
                        log += job_port.cv_Data.GetPortStatusStringForLogUse() + "\n";
                        if (job_port.PLotStatus == LotStatus.MappingEnd)
                        {
                            job_port.cv_Data.PWorkCount = obj.PWorkCount;
                            job_port.cv_Data.PFoupSeq = obj.PFoupSeq;
                            job_port.cv_Data.PLotId = obj.PLotId;
                            log += "recv PWorkCount : " + obj.PWorkCount + "\n";
                            log += "recv PFoupSeq : " + obj.PFoupSeq + "\n";
                            log += "recv PLotId : " + obj.PLotId + "\n";
                            Dictionary<int, GlassData> recv_glass = new Dictionary<int, GlassData>();
                            foreach (GlassData data in obj.cv_GlassDataList)
                            {
                                recv_glass[Convert.ToInt16(data.PSlotInEq)] = data;
                                log += "Recv Slot : " + data.PSlotInEq + "\n";
                            }

                            bool error = false;
                            int sum = 0;
                            for (int i = 1; i <= job_port.cv_SlotCount; i++)
                            {
                                if (job_port.cv_Data.GlassDataMap[i].PHasSensor)
                                {
                                    if (!recv_glass[i].PHasData)
                                    {
                                        LgcForm.ShowMsg("[Foup Data ERROR] : Slot " + i.ToString() + "hasn't data , but has sensor", false, false);
                                        log += "Data unmatch : Slot " + i.ToString() + "hasn't data , but has sensor\n";
                                        error = true;
                                        break;
                                    }
                                    else
                                    {
                                        recv_glass[i].POcrResult = OCRResult.None;
                                        recv_glass[i].PPortProductionCategory = job_port
                                            .cv_Data.PProductionType;
                                        recv_glass[i].PSourcePort = (uint)job_port.cv_Id;
                                        if (recv_glass[i].PProductionCategory != ProductCategory.Glass && recv_glass[i].PProductionCategory != ProductCategory.Wafer)
                                        {
                                            LgcForm.ShowMsg("[Foup Data ERROR] : BC data  but Production Category errro : " + recv_glass[i].PProductionCategory.ToString(), false, false);
                                            log += "BC data  but Production Category errro : Slot : " + i.ToString() + " ," + recv_glass[i].PProductionCategory.ToString() + "\n";
                                            error = true;
                                            break;
                                        }
                                        sum++;
                                    }
                                }
                                else
                                {
                                    if (recv_glass[i].PHasData)
                                    {
                                        LgcForm.ShowMsg("[Foup Data ERROR] : Slot " + i.ToString() + "hasn't sensor , but has data", false, false);
                                        log += "Slot " + i.ToString() + "hasn't sensor , but has data\n";
                                        error = true;
                                        break;
                                    }
                                }
                            }

                            if (!error)
                            {
                                LgcForm.cv_GlassCountData.PHistoryCount += sum;
                                job_port.cv_Data.GlassDataList = obj.cv_GlassDataList;
                                Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.PortData).Name, job_port.cv_Data, KParseObjToXmlPropertyType.Field);
                            }
                            else
                            {
                                AlarmItem alarm = new AlarmItem();
                                alarm.PCode = Alarmtable.BcFoupDataError.ToString();
                                alarm.PLevel = AlarmLevele.Light;
                                alarm.PMainDescription = "Bc Foup Data Error";
                                alarm.PStatus = AlarmStatus.Occur;
                                LgcForm.EditAlarm(alarm);
                                job_port.cv_Data.cv_IsWaitCancel = true;
                                return;
                            }
                            //check every slot recipe is the same for node 2.
                            int tmp_recipe = -1;
                            for (int i = 1; i <= job_port.cv_Data.cv_SlotCount; i++)
                            {
                                if (job_port.cv_Data.GlassDataMap[i].PHasSensor && job_port.cv_Data.GlassDataMap[i].PProcessFlag == ProcessFlag.Need)
                                {
                                    int node = job_port.cv_Data.GlassDataMap[i].cv_Nods.FindIndex(x => x.cv_NodeId == 2);
                                    GlassDataNodeItem item = job_port.cv_Data.GlassDataMap[i].cv_Nods[node];
                                    if(tmp_recipe == -1)
                                    {
                                        tmp_recipe = item.cv_Recipe;
                                    }
                                    else
                                    {
                                        if(tmp_recipe != item.cv_Recipe)
                                        {
                                            log += "Slot " + i.ToString() + ". Recipe No. not the same\n";
                                            error = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (error)
                            {
                                AlarmItem alarm = new AlarmItem();
                                alarm.PCode = Alarmtable.FoupDataContainsOverOneRecipe.ToString();
                                alarm.PLevel = AlarmLevele.Light;
                                alarm.PMainDescription = "Foup Data Contains Over One Recipe";
                                alarm.PStatus = AlarmStatus.Occur;
                                LgcForm.EditAlarm(alarm);
                                LgcForm.ShowMsg(alarm.PMainDescription, false, false);
                                job_port.cv_Data.cv_IsWaitCancel = true;
                                return;
                            }

                            bool need_change_recipe = false;
                            int want_change_recipe = -1;
                            log += "change recipe : " + tmp_recipe + " Cur recipe : " + LgcForm.cv_Recipes.PCurRecipeId + "\n";
                            if (tmp_recipe != Convert.ToInt32(LgcForm.cv_Recipes.PCurRecipeId))
                            {
                                need_change_recipe = true;
                                want_change_recipe = tmp_recipe;
                                bool can_change_recipe = true;
                                Robot rb = LgcForm.GetRobotById(1);
                                Aligner aligner = LgcForm.GetAlignerById(1);
                                Buffer buffer = LgcForm.GetBufferById(1);
                                for (int j = 1; j <= CommonData.HIRATA.CommonStaticData.g_PortNumber; j++)
                                {
                                    if (j == job_port.cv_Data.PId) continue;
                                    Port port = LgcForm.GetPortById(j);
                                    log += "port : " + port.cv_Data.PId + " status : " + port.PPortStatus + " foup status : " + port.PLotStatus + "\n";
                                    if (port.PPortStatus != PortStaus.LDRQ && port.PPortStatus != PortStaus.UDCM && port.PPortStatus != PortStaus.UDRQ &&
                                        port.cv_Data.PPortMode != PortMode.Unloader)
                                    {
                                        if (port.IsHasAnyDataAndSensor())
                                        {
                                            can_change_recipe = false;
                                            string tmp3 = "Port : " + port.cv_Data.PId + " has data or sensor set can_change_recipe false\n";
                                            LgcForm.ShowMsg(tmp3, true, false);
                                            log += tmp3;
                                            break;
                                        }
                                    }
                                }
                                if (can_change_recipe)
                                {
                                    if (rb.IsBusy || aligner.IsHasAnyDataAndSensor() || buffer.IsHasAnyDataAndSensor() || rb.IsHasAnyDataAndSensor())
                                    {
                                        can_change_recipe = false;
                                        string tmp4 = "Robot busy : " + rb.IsBusy + "\n";
                                        tmp4 += "Robot busy : " + rb.IsBusy + "\n";
                                        tmp4 += "aligner IsHasAnyDataAndSensor : " + aligner.IsHasAnyDataAndSensor() + "\n";
                                        tmp4 += "buffer IsHasAnyDataAndSensor : " + buffer.IsHasAnyDataAndSensor() + "\n";
                                        tmp4 += "rb IsHasAnyDataAndSensor : " + rb.IsHasAnyDataAndSensor() + "\n";
                                        tmp4 += "set can_change_recipe false\n";
                                        LgcForm.ShowMsg(tmp4, true, false);
                                        log += tmp4;
                                    }
                                }
                                if (!can_change_recipe)
                                {
                                    AlarmItem alarm = new AlarmItem();
                                    alarm.PCode = Alarmtable.BcDataDownLoadRecipeERROR.ToString();
                                    alarm.PLevel = AlarmLevele.Light;
                                    alarm.PMainDescription = "Bc DataDownLoad Recipe ERROR";
                                    alarm.PSubDescription = "Main S/W has substrate data or robot is busy.";
                                    alarm.PStatus = AlarmStatus.Occur;
                                    LgcForm.EditAlarm(alarm);
                                    LgcForm.ShowMsg("Data download : Recipe unmatch : Cur is " + LgcForm.cv_Recipes.PCurRecipeId.ToString() +
                                        " Recv : " + tmp_recipe.ToString(), true, false);
                                    job_port.cv_Data.cv_IsWaitCancel = true;
                                    error = true;
                                }
                            }
                            if (!error)
                            {
                                if (need_change_recipe)
                                {
                                    if (LgcForm.cv_Recipes.IsRecipeExist(want_change_recipe.ToString()))
                                    {
                                        LgcForm.cv_Recipes.SetCurRecipe(want_change_recipe.ToString());
                                        job_port.cv_Data.PCurPPID = LgcForm.FindHightestPriorityPPID(job_port.cv_Id);
                                        job_port.PLotStatus = LotStatus.WaitReserve;
                                        log += "Set Port : " + job_port.cv_Id + " WaitReserve\n";
                                    }
                                    else
                                    {
                                        AlarmItem alarm = new AlarmItem();
                                        alarm.PCode = Alarmtable.FoupDataRecipeEFEMNotHas.ToString();
                                        alarm.PLevel = AlarmLevele.Light;
                                        alarm.PMainDescription = "Foup Data Recipe EFEM Not Has";
                                        alarm.PStatus = AlarmStatus.Occur;
                                        LgcForm.EditAlarm(alarm);
                                        LgcForm.ShowMsg("Data download : Recipe unmatch : Cur is " + LgcForm.cv_Recipes.PCurRecipeId.ToString(), false, false);
                                        job_port.cv_Data.cv_IsWaitCancel = true;
                                        error = true;
                                    }
                                }
                                else
                                {
                                    job_port.cv_Data.PCurPPID = LgcForm.FindHightestPriorityPPID(job_port.cv_Id);
                                    job_port.PLotStatus = LotStatus.WaitReserve;
                                    log += "Set Port : " + job_port.cv_Id + " WaitReserve\n";
                                }
                            }
                        }
                        else
                        {
                            LgcForm.ShowMsg("Recv BC Foup Data Download , But Port Status not in Mapping End", true, false);
                            log += "Recv BC Foup Data Download , But Port Status not in Mapping End\n";
                        }
                    }
                }
                WriteLog(LogLevelType.Detail, log);
                WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            }
            catch (Exception e)
            {
                WriteLog(LogLevelType.Error, e.StackTrace.ToString());
            }
        }
        void ProcessEqData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.EqData obj = m_Object as CommonData.HIRATA.EqData;
            CommonData.HIRATA.EqData tmp = LgcForm.cv_EqContainer[(int)obj.PId].cv_Data;
            tmp.GlassDataMap = tmp.GlassDataMap;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.EqData).Name, tmp, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        void ProcessBufferData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.BufferData obj = m_Object as CommonData.HIRATA.BufferData;
            CommonData.HIRATA.BufferData tmp = LgcForm.cv_BufferContainer[(int)obj.PId].cv_Data;
            tmp.GlassDataMap = tmp.GlassDataMap;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.BufferData).Name, tmp, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        void ProcessBcAliveAndPlcConnect(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.MDBCAliveAndPlcConnect obj = m_Object as CommonData.HIRATA.MDBCAliveAndPlcConnect;
            LgcForm.PSystemData.PBcAlive = obj.PBcAlive;
            LgcForm.PSystemData.PPlcConnect = obj.PPlcConnect;
            if (!LgcForm.PSystemData.PBcAlive)
            {
                AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.BcDie.ToString();
                alarm.PLevel = AlarmLevele.Light;
                alarm.PMainDescription = "Bc Die";
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
            }
            else
            {
                AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.BcDie.ToString();
                alarm.PLevel = AlarmLevele.Light;
                alarm.PMainDescription = "Bc Die";
                alarm.PStatus = AlarmStatus.Clean;
                LgcForm.EditAlarm(alarm);
            }
            if (!LgcForm.PSystemData.PPlcConnect)
            {
                AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.PlcDisconnect.ToString();
                alarm.PLevel = AlarmLevele.Light;
                alarm.PMainDescription = "PLC Disconnect";
                alarm.PStatus = AlarmStatus.Occur;
                LgcForm.EditAlarm(alarm);
            }
            else
            {
                AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.PlcDisconnect.ToString();
                alarm.PLevel = AlarmLevele.Light;
                alarm.PMainDescription = "PLC Disconnect";
                alarm.PStatus = AlarmStatus.Clean;
                LgcForm.EditAlarm(alarm);
            }
            //PlcDisconnect
            //WriteOut
        }
        void ProcessRobotData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            CommonData.HIRATA.RobotData obj = m_Object as CommonData.HIRATA.RobotData;
            CommonData.HIRATA.RobotData tmp = LgcForm.cv_RobotContainer[(int)obj.PId].cv_Data;
            tmp.GlassDataMap = tmp.GlassDataMap;
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.RobotData).Name, tmp, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        void ProcessCstDataReply(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDPopMonitorFormRep obj = m_Object as CommonData.HIRATA.MDPopMonitorFormRep;
            if (obj.PResult == CommonData.HIRATA.Result.OK)
            {
                Port job_port = LgcForm.GetPortById(obj.PortId);
                CommonData.HIRATA.PortData data = obj.PortData;

                data.GlassDataList = data.cv_GlassDataList;
                job_port.cv_Data = data;
                job_port.PLotStatus = CommonData.HIRATA.LotStatus.WaitReserve;
                if (LgcForm.PSystemData.PSystemOnlineMode == CommonData.HIRATA.OnlineMode.Offline)
                {
                    job_port.PLotStatus = CommonData.HIRATA.LotStatus.Process;
                }
                job_port.cv_Data.SaveToFile();
                LgcForm.AddPortToProcessList(job_port.cv_Id);
                int sum = 0;
                foreach(GlassData tmp_data in data.cv_GlassDataList)
                {
                    if (tmp_data.PHasSensor && tmp_data.PHasData)
                        sum++;
                }
                LgcForm.cv_GlassCountData.PHistoryCount += sum;
            }
            else
            {
                LgcForm.GetRobotById(1).cv_Comm.SetPortUnloadAction(obj.PortId);
            }
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        /*
        void ProcessOperationModeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDOperationModeChange obj = m_Object as CommonData.HIRATA.MDOperationModeChange;
            this.SendMmfReplyObject(typeof(CommonData.HIRATA.MDOperationModeChange).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDOperationModeChange).Name, KParseObjToXmlPropertyType.Field);
            log += "Cur Operation mode :" + LgcForm.cv_SystemData.POperationMode.ToString() + " Change Mode : " + obj.PChangeOperationMode.ToString() + Environment.NewLine;
            bool rtn = true;
            if (obj.PType == CommonData.HIRATA.MmfEventClientEventType.etRequest)
            {
                // if want to check some condition , add codes at here.
                if (rtn)
                {
                    LgcForm.PSystemData.POperationMode = obj.PChangeOperationMode;
                }
                else
                {
                    SendMsgToUi(true, false, 10000, "Can't change Operation mode!!!");
                }
            }
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
         * */
        void ProcessEqStatusNoti(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDEqDataReq obj = m_Object as CommonData.HIRATA.MDEqDataReq;
            SendMmfReplyObject(typeof(CommonData.HIRATA.MDEqDataReq).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDEqDataReq).Name, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }

        void ProcessRobotStatusNoti(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDRobotDataReq obj = m_Object as CommonData.HIRATA.MDRobotDataReq;
            if (LgcForm.cv_RobotContainer.Count == 0) return;
            CommonData.HIRATA.RobotData tmp = LgcForm.cv_RobotContainer[(int)obj.Robots[0]].cv_Data;
            tmp.GlassDataMap = tmp.GlassDataMap;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.RobotData).Name, tmp, m_Ticket, typeof(CommonData.HIRATA.MDRobotDataReq).Name, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessPortActionRep(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDPortAction obj = m_Object as CommonData.HIRATA.MDPortAction;

            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessPortStatusNoti(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "Send Port Data!!!";
            CommonData.HIRATA.MDPortDataReq obj = m_Object as CommonData.HIRATA.MDPortDataReq;
            CommonData.HIRATA.PortData tmp = LgcForm.cv_PortContainer[(int)obj.Ports[0]].cv_Data;
            tmp.GlassDataMap = tmp.GlassDataMap;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.PortData).Name, tmp, m_Ticket, typeof(CommonData.HIRATA.MDPortDataReq).Name, KParseObjToXmlPropertyType.Field);
            LgcForm.WritePortToPlc((int)obj.Ports[0]);
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessDataEditRep(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDDataAction obj = m_Object as CommonData.HIRATA.MDDataAction;
            obj.PResult = CommonData.HIRATA.Result.OK;
            SendMmfReplyObject(typeof(CommonData.HIRATA.MDDataAction).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDDataAction).Name, KParseObjToXmlPropertyType.Field);
            GlassData del_data = null;
            if (obj.PAction == CommonData.HIRATA.DataEidtAction.Del)
            {
                switch (obj.Source.PTarget)
                {
                    case CommonData.HIRATA.ActionTarget.Port:
                        Port port = LgcForm.GetPortById(obj.Source.Id);
                        del_data = port.cv_Data.GlassDataMap[obj.Source.Slot];
                        port.cv_Data.DelSlotData(obj.Source.Slot);
                        port.SendDataViaMmf();
                        port.cv_Data.SaveToFile();
                        break;
                    case CommonData.HIRATA.ActionTarget.Aligner:
                        Aligner aligner = LgcForm.GetAlignerById(obj.Source.Id);
                        del_data = aligner.cv_Data.GlassDataMap[obj.Source.Slot];
                        aligner.cv_Data.DelSlotData(obj.Source.Slot);
                        aligner.SendDataViaMmf();
                        aligner.cv_Data.SaveToFile();
                        break;
                    case CommonData.HIRATA.ActionTarget.Buffer:
                        Buffer buffer = LgcForm.GetBufferById(obj.Source.Id);
                        del_data = buffer.cv_Data.GlassDataMap[obj.Source.Slot];
                        buffer.cv_Data.DelSlotData(obj.Source.Slot);
                        buffer.SendDataViaMmf();
                        buffer.cv_Data.SaveToFile();
                        break;
                    case CommonData.HIRATA.ActionTarget.Eq:
                        Eq eq = LgcForm.GetEqById(obj.Source.Id);
                        eq.cv_Data.DelSlotData(obj.Source.Slot);
                        eq.SendDataViaMmf();
                        eq.cv_Data.SaveToFile();
                        break;
                    case CommonData.HIRATA.ActionTarget.Robot:
                        Robot robot = LgcForm.GetRobotById(obj.Source.Id);
                        del_data = robot.cv_Data.GlassDataMap[obj.Source.Slot];
                        robot.cv_Data.DelSlotData(obj.Source.Slot);
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                        break;
                };
                if (del_data != null)
                {
                    CommonData.HIRATA.MDBCRemoveReport report_bc = new MDBCRemoveReport();
                    report_bc.PAction = RemoveDataType.WorkRemove;
                    report_bc.PGlass = del_data;
                    report_bc.POpid = obj.Opid;
                    report_bc.PReason = obj.Reason;
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRemoveReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                }
                obj.GlassData = null;
                obj = null;
            }
            else
            {
                switch (obj.Source.PTarget)
                {
                    case CommonData.HIRATA.ActionTarget.Port:
                        Port port = LgcForm.GetPortById(obj.Source.Id);
                        del_data = obj.GlassData;
                        port.cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                        port.cv_Data.GlassDataMap[obj.Source.Slot].POcrResult = OCRResult.None;
                        if (port.cv_Data.PProductionType == ProductCategory.Wafer)
                            port.cv_Data.GlassDataMap[obj.Source.Slot].PPortProductionCategory = ProductCategory.Wafer;
                        else if (port.cv_Data.PProductionType == ProductCategory.Glass)
                            port.cv_Data.GlassDataMap[obj.Source.Slot].PPortProductionCategory = ProductCategory.Glass;
                        port.cv_Data.GlassDataMap[obj.Source.Slot].cv_SlotInEq = (uint)obj.Source.Slot;
                        port.SendDataViaMmf();
                        port.cv_Data.SaveToFile();
                        obj = null;
                        break;
                    case CommonData.HIRATA.ActionTarget.Aligner:
                        Aligner aligner = LgcForm.GetAlignerById(obj.Source.Id);
                        del_data = obj.GlassData;
                        aligner.cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                        aligner.cv_Data.GlassDataMap[obj.Source.Slot].cv_SlotInEq = (uint)obj.Source.Slot;
                        aligner.SendDataViaMmf();
                        aligner.cv_Data.SaveToFile();
                        obj = null;
                        break;
                    case CommonData.HIRATA.ActionTarget.Buffer:
                        Buffer buffer = LgcForm.GetBufferById(obj.Source.Id);
                        del_data = obj.GlassData;
                        buffer.cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                        buffer.cv_Data.GlassDataMap[obj.Source.Slot].cv_SlotInEq = (uint)obj.Source.Slot;
                        buffer.SendDataViaMmf();
                        buffer.cv_Data.SaveToFile();
                        obj = null;
                        break;
                    case CommonData.HIRATA.ActionTarget.Eq:
                        Eq eq = LgcForm.GetEqById(obj.Source.Id);
                        del_data = obj.GlassData;
                        eq.cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                        eq.cv_Data.GlassDataMap[obj.Source.Slot].cv_SlotInEq = (uint)obj.Source.Slot;
                        eq.SendDataViaMmf();
                        eq.cv_Data.SaveToFile();
                        obj = null;
                        break;
                    case CommonData.HIRATA.ActionTarget.Robot:
                        Robot robot = LgcForm.GetRobotById(obj.Source.Id);
                        del_data = obj.GlassData;
                        robot.cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                        robot.cv_Data.GlassDataMap[obj.Source.Slot].cv_SlotInEq = (uint)obj.Source.Slot;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                        obj = null;
                        break;
                };
                if (del_data != null)
                {
                    CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                    report_bc.PGlass = del_data;
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                }
            }

            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {//MDBCDataRequest
            CommonData.HIRATA.MDBCDataRequest obj = m_Object as CommonData.HIRATA.MDBCDataRequest;
            if (m_SourceModule == "UI")
            {
            }
            else if (m_SourceModule == "CIM")
            {
                if (obj.PResult == Result.NG)
                {
                    AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.BcHsDataRequestReplyNG.ToString();
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PMainDescription = "Data Request , BC reply NG";
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    LgcForm.EditAlarm(alarm);
                }
                else if (obj.PResult == Result.OK)
                {
                    switch (obj.Source.PTarget)
                    {
                        case CommonData.HIRATA.ActionTarget.Port:
                            LgcForm.GetPortById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                            LgcForm.GetPortById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PSlotInEq = (uint)obj.Source.Slot;
                            LgcForm.GetPortById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PHasSensor = true;
                            LgcForm.GetPortById(obj.Source.Id).cv_Data.GlassDataMap = LgcForm.GetPortById(obj.Source.Id).cv_Data.GlassDataMap;
                            obj.GlassData = null;
                            LgcForm.GetPortById(obj.Source.Id).SendDataViaMmf();
                            obj = null;
                            break;
                        case CommonData.HIRATA.ActionTarget.Aligner:
                            LgcForm.GetAlignerById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                            LgcForm.GetAlignerById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PSlotInEq = (uint)obj.Source.Slot;
                            LgcForm.GetAlignerById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PHasSensor = true;
                            LgcForm.GetAlignerById(obj.Source.Id).cv_Data.GlassDataMap = LgcForm.GetAlignerById(obj.Source.Id).cv_Data.GlassDataMap;
                            LgcForm.GetAlignerById(obj.Source.Id).SendDataViaMmf();
                            obj = null;
                            break;
                        case CommonData.HIRATA.ActionTarget.Buffer:
                            LgcForm.GetBufferById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                            LgcForm.GetBufferById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PSlotInEq = (uint)obj.Source.Slot;
                            LgcForm.GetBufferById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PHasSensor = true;
                            LgcForm.GetBufferById(obj.Source.Id).cv_Data.GlassDataMap = LgcForm.GetBufferById(obj.Source.Id).cv_Data.GlassDataMap;
                            LgcForm.GetBufferById(obj.Source.Id).SendDataViaMmf();
                            obj = null;
                            break;
                        case CommonData.HIRATA.ActionTarget.Robot:
                            LgcForm.GetRobotById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot] = obj.GlassData;
                            LgcForm.GetRobotById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PSlotInEq = (uint)obj.Source.Slot;
                            LgcForm.GetRobotById(obj.Source.Id).cv_Data.GlassDataMap[obj.Source.Slot].PHasSensor = true;
                            LgcForm.GetRobotById(obj.Source.Id).cv_Data.GlassDataMap = LgcForm.GetRobotById(obj.Source.Id).cv_Data.GlassDataMap;
                            LgcForm.GetRobotById(obj.Source.Id).SendDataViaMmf();
                            obj = null;
                            break;
                    };
                    LgcForm.ShowMsg("Data Request Successful. Please re-open data screen to check.", false, false);
                }
            }
        }
        void ProcessOcrMode(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            CommonData.HIRATA.MDOcrMode obj = m_Object as CommonData.HIRATA.MDOcrMode;
            if (obj.PType == MmfEventClientEventType.etRequest)
            {
                SendMmfReplyObject(typeof(CommonData.HIRATA.MDOcrMode).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDOcrMode).Name, KParseObjToXmlPropertyType.Field);
            }
            LgcForm.PSystemData.POcrMode = obj.POcrMode;
            LgcForm.cv_Mio.SetPortValue(0x344d, (int)LgcForm.PSystemData.POcrMode + (1 << 4));

        }

        void ProcessGlassCheck(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            CommonData.HIRATA.MDGlassDataCheck obj = m_Object as CommonData.HIRATA.MDGlassDataCheck;
            if (obj.PType == MmfEventClientEventType.etRequest)
            {
                SendMmfReplyObject(typeof(CommonData.HIRATA.MDGlassDataCheck).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDGlassDataCheck).Name, KParseObjToXmlPropertyType.Field);
            }
            LgcForm.PSystemData.IsCheckSlot = obj.PCheckWorkSlot;
            LgcForm.PSystemData.IsCheckSeq = obj.PCheckFoupSeq;
            LgcForm.PSystemData.IsCheckRecipe = obj.PCheckRecipeNo;
            LgcForm.PSystemData.IsCheckId = obj.PCheckWorkId;
        }

        #endregion
        void ProcessRsetTImechart(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            MDResetTimeChart obj = m_Object as MDResetTimeChart;
            cv_TimeChart.RestartTimeChart(obj.cv_TimeChartId);
        }
        void ProcessForceCom(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            CommonData.HIRATA.MDForceCom obj = m_Object as CommonData.HIRATA.MDForceCom;
            int id = obj.cv_TimeChartId;
            cv_TimeChart.JumpToStep(id, TimechartNormal.STEP_ID_ForceCom);
            cv_TimeChart.SetTrigger(id, TimechartNormal.STEP_ID_ForceCom);
        }
        void ProcessForceIni(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            CommonData.HIRATA.MDForceIni obj = m_Object as CommonData.HIRATA.MDForceIni;
            int id = obj.cv_TimeChartId;
            cv_TimeChart.JumpToStep(id, TimechartNormal.STEP_ID_ForceIni);
            cv_TimeChart.SetTrigger(id, TimechartNormal.STEP_ID_ForceIni);
        }
        void ProcessRobotApiVersionNoti(string m_ChannelId, string m_MessageId, object m_Object)
        {
            //WriteIn
            string log = "";
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessDeviceBackHomeNoti(string m_ChannelId, string m_MessageId, object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDDeviceHome obj = m_Object as CommonData.HIRATA.MDDeviceHome;
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        public void SendOnLineRequest(CommonData.HIRATA.MmfEventClientEventType m_Type, CommonData.HIRATA.OnlineMode m_Mode)
        {
            //WriteIn
            CommonData.HIRATA.MDOnlineRequest obj = new CommonData.HIRATA.MDOnlineRequest();
            obj.PType = m_Type;
            obj.PCurMode = m_Mode;
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDOnlineRequest).Name, obj, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        void SendMsgToUi(bool m_AutoClean, bool m_WaitReply, int m_TimeOut, string m_Msg)
        {
            //WriteIn
            CommonData.HIRATA.MDShowMsg obj = new CommonData.HIRATA.MDShowMsg();
            CommonData.HIRATA.Msg msg_item = new CommonData.HIRATA.Msg();
            msg_item.Txt = m_Msg;
            msg_item.PAutoClean = m_AutoClean;
            msg_item.PUserRep = m_WaitReply;
            msg_item.TimeOut = (ulong)m_TimeOut;
            obj.Msg = msg_item;
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDShowMsg).Name, obj);
            //WriteOut
        }
        bool CheckRobotCanAccess(int m_RobotId, CommonData.HIRATA.RobotArm m_Arm, CommonData.HIRATA.RobotAction m_Action)
        {
            bool rtn = false;
            Robot robot = LgcForm.GetRobotById(m_RobotId);
            if (m_Action == CommonData.HIRATA.RobotAction.Get)
            {
                if (!robot.cv_Data.GlassDataMap[(int)m_Arm].PHasSensor && !robot.cv_Data.GlassDataMap[(int)m_Arm].PHasData)
                {
                    rtn = true;
                }
                else
                {
                    LgcForm.ShowMsg("Robot has data or sensor , please check", true, false);
                }
            }
            else if ((m_Action == CommonData.HIRATA.RobotAction.Put || m_Action == CommonData.HIRATA.RobotAction.TopPut))// || (m_Action == CommonData.HIRATA.RobotAction.PutGetAligner))
            {
                if (robot.cv_Data.GlassDataMap[(int)m_Arm].PHasSensor && robot.cv_Data.GlassDataMap[(int)m_Arm].PHasData)
                {
                    rtn = true;
                }
                else
                {
                    LgcForm.ShowMsg("Robot hasn't data or sensor , please check", true, false);
                }
            }
            else if (m_Action == RobotAction.Exchange)
            {
                if (m_Arm == RobotArm.rbaDown)
                {
                    if ((!robot.cv_Data.GlassDataMap[(int)m_Arm].PHasSensor) && (!robot.cv_Data.GlassDataMap[(int)m_Arm].PHasData) &&
                         (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor) && (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
                        )
                    {
                        rtn = true;
                    }
                    else
                    {
                        LgcForm.ShowMsg("Robot arm data or sensor error , please check", true, false);
                    }
                }
                else if (m_Arm == RobotArm.rbaUp)
                {
                    if ((!robot.cv_Data.GlassDataMap[(int)m_Arm].PHasSensor) && (!robot.cv_Data.GlassDataMap[(int)m_Arm].PHasData) &&
                         (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor) && (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData)
                        )
                    {
                        rtn = true;
                    }
                    else
                    {
                        LgcForm.ShowMsg("Robot arm data or sensor error , please check", true, false);
                    }
                }
            }
            else if (m_Action == RobotAction.PutGetAligner)
            {
                if (robot.cv_Data.GlassDataMap[(int)m_Arm].PHasSensor && robot.cv_Data.GlassDataMap[(int)m_Arm].PHasData)
                {
                    rtn = true;
                }
                else
                {
                    LgcForm.ShowMsg("Robot arm : " + m_Arm.ToString() +
                        " data or sensor error , please check", true, false);
                }
            }
            if (!rtn)
            {
                LgcForm.ShowMsg("Robot can't access , please check Robot data", true, false);
            }
            return rtn;
        }
        bool CheckTargetCanAccess(CommonData.HIRATA.ActionTarget m_Target, CommonData.HIRATA.RobotAction m_Action, int m_Id, int m_Slot)
        {
            bool rtn = false;
            if ((m_Action == CommonData.HIRATA.RobotAction.Put || m_Action == CommonData.HIRATA.RobotAction.TopPut))// || (m_Action == CommonData.HIRATA.RobotAction.PutGetAligner))// || m_Action == CommonData.HIRATA.RobotAction.TopPut)
            {
                switch (m_Target)
                {
                    case CommonData.HIRATA.ActionTarget.Aligner:
                        Aligner aligner = LgcForm.GetAlignerById(m_Id);
                        if (aligner != null)
                        {
                            if (aligner.CanAccess(true, 1))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    case CommonData.HIRATA.ActionTarget.Buffer:
                        Buffer buffer = LgcForm.GetBufferById(m_Id);
                        if (buffer != null)
                        {
                            if (buffer.CanAccess(true, m_Slot))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    case CommonData.HIRATA.ActionTarget.Port:
                        Port port = LgcForm.GetPortById(m_Id);
                        if (port != null)
                        {
                            if (port.CanAccess(true, m_Slot))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    case CommonData.HIRATA.ActionTarget.Eq:
                        Eq eq = LgcForm.GetEqById(m_Id);
                        if (eq != null)
                        {
                            if (eq.CanAccess(true, m_Slot))
                            {
                                rtn = true;
                            }
                        }
                        break;
                };
            }
            else if (m_Action == CommonData.HIRATA.RobotAction.Get)// || m_Action == CommonData.HIRATA.RobotAction.TopGet)
            {
                switch (m_Target)
                {
                    case CommonData.HIRATA.ActionTarget.Aligner:
                        Aligner aligner = LgcForm.GetAlignerById(m_Id);
                        if (aligner != null)
                        {
                            if (aligner.CanAccess(false, 1))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    case CommonData.HIRATA.ActionTarget.Buffer:
                        Buffer buffer = LgcForm.GetBufferById(m_Id);
                        if (buffer != null)
                        {
                            if (buffer.CanAccess(false, m_Slot))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    case CommonData.HIRATA.ActionTarget.Port:
                        Port port = LgcForm.GetPortById(m_Id);
                        if (port != null)
                        {
                            if (port.CanAccess(false, m_Slot))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    case CommonData.HIRATA.ActionTarget.Eq:
                        Eq eq = LgcForm.GetEqById(m_Id);
                        if (eq != null)
                        {
                            if (eq.CanAccess(false, m_Slot))
                            {
                                rtn = true;
                            }
                        }
                        break;
                };
            }
            else if (m_Action == CommonData.HIRATA.RobotAction.PutGetAligner)
            {
                Aligner aligner = LgcForm.GetAlignerById(m_Id);
                if (aligner != null)
                {
                    if (aligner.CanAccess(true, 1))
                    {
                        rtn = true;
                    }
                }
            }
            else if (m_Action == CommonData.HIRATA.RobotAction.Exchange)
            {
                switch (m_Target)
                {
                    case CommonData.HIRATA.ActionTarget.Eq:
                        Eq eq = LgcForm.GetEqById(m_Id);
                        if (eq != null)
                        {
                            if (eq.CanAccess(false, m_Slot , true))
                            {
                                rtn = true;
                            }
                        }
                        break;
                    default:
                        LgcForm.ShowMsg("Manual Exhange Only EQ use ( except VAS ).", true, false);
                        break;
                };
            }
            else if (m_Action == CommonData.HIRATA.RobotAction.GetWait || m_Action == CommonData.HIRATA.RobotAction.PutWait ||
                m_Action == CommonData.HIRATA.RobotAction.TopGetWait || m_Action == CommonData.HIRATA.RobotAction.TopPutWait ||
                m_Action == CommonData.HIRATA.RobotAction.TopGet)
            {
                rtn = true;
            }
            if (!rtn)
            {
                LgcForm.ShowMsg("target can't access , please check target side", true, false);
            }
            return rtn;
        }
        private void SendTimeChartStepMsg()
        {
            //WriteNormalIn
            CommonData.HIRATA.MDTimeChartChange timeChartStep = new MDTimeChartChange();
            for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
            {
                int step = cv_TimechartController.GetTimeChart().GetCurrentStep(i);
                timeChartStep.cv_Steps.Add(step);
                timeChartStep.cv_StepsName.Add(cv_TimeChartParser.GetStepName(i, step));
            }
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDTimeChartChange).Name, timeChartStep, KParseObjToXmlPropertyType.Field);
            //WriteNormalOut
        }
        public void SendBcTreansferReport(DataFlowAction m_Action , GlassData m_Data , int m_Port = 0 , int m_Slot=0)
        {
            CommonData.HIRATA.MDBCWorkTransferReport obj = new MDBCWorkTransferReport();
            obj.PAction = m_Action;
            obj.PGlassData = m_Data;
            obj.PPortNo = m_Port;
            obj.PSlotNo = m_Slot;
            obj.PUnitNo = 0;
            obj.PType = MmfEventClientEventType.etNotify;
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkTransferReport).Name, obj, KParseObjToXmlPropertyType.Field);
        }
        public void SendBcOcrReport(GlassData m_Data , string m_OcrString)
        {
            MDBCOcrReadReport obj = new MDBCOcrReadReport();
            obj.PGlass = m_Data;
            obj.PMatch = true;
            if(m_Data.POcrResult != OCRResult.OK)
            {
                obj.PMatch = false;
            }
            obj.POcrMode = LgcForm.PSystemData.POcrMode;
            obj.PReadFromOCR = m_OcrString;
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCOcrReadReport).Name, obj, KParseObjToXmlPropertyType.Field);
        }
        public void SendBcLastSubstrateReport(GlassData m_Data , int m_Port, int m_Slot)
        {
            MDBCLastProcessStartReport obj = new MDBCLastProcessStartReport();
            obj.PGlass = m_Data;
            obj.PPortNo = m_Port;
            obj.PSlotNo = m_Slot;
            SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCLastProcessStartReport).Name, obj, KParseObjToXmlPropertyType.Field);            
        }
    }
}
