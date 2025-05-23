using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;
using System.Reflection;
using CommonData.HIRATA;

namespace BaseAp
{
    public class BaseMmfController : MmfEventCenter
    {
        public static KMemoLog cv_ControllerLog;
        private string module;
        public BaseMmfController(string m_NameSpace, string m_Module= ""):base(m_Module)
        {
            InitialBase();
            MmfObjectNamespace = m_NameSpace;
            cv_Assembly = Assembly.LoadFrom(SysUtils.ExtractFileDir(SysUtils.GetExeName()) + "\\CommonData.dll");
            logsSetting();
            Open();
        }
        void logsSetting()
        {
            string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;
            KFileLog cv_MmfClientLog;
            cv_MmfClientLog = new KFileLog();
            cv_MmfClientLog.LoadFromIni(Global.LogIniPathname, "MmfEventClient");
            cv_MmfClientLog.LogFileName = enviPath + "\\MmfClient.log";
            cv_MmfClientLog.SaveToIni(Global.LogIniPathname, "MmfEventClient");
            cv_MmfClientLog = null;

            cv_ControllerLog = new KMemoLog();
            cv_ControllerLog.LoadFromIni(Global.LogIniPathname, "CONTROLLER");
            cv_ControllerLog.LogFileName = enviPath + "\\Controller.log";
            cv_ControllerLog.SaveToIni(Global.LogIniPathname, "CONTROLLER");
            cv_ControllerLog.WriteLog("Create Controller Log");

            Global.DebugLog = new KFileLog();
            Global.DebugLog.LoadFromIni(Global.LogIniPathname, "DebugLog");
            Global.DebugLog.LogFileName = enviPath + "\\Debug.log";
            Global.DebugLog.SaveToIni(Global.LogIniPathname, "DebugLog");
            Global.DebugLog.WriteLog("Create DebugLog");
        }
        void InitialBase()
        {
            Global.LogIniPathname = CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile;
            Global.SystemIniPathname = CommonData.HIRATA.CommonStaticData.g_ModuleSystemIniFile;
            Global.Controller = this;
        }
        public static void WriteLog(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)(SamekLogLevelType)Enum.Parse(typeof(SamekLogLevelType), m_Type.ToString());
            if (m_Type == LogLevelType.NormalFunctionInOut)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                    if(m_FunInOut == FunInOut.Leave)
                    {
                        log += "\n---------------------------------------------"; 
                    }
                }
            }
            else if (m_Type == LogLevelType.TimerFunction)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[Timer FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                }
            }
            else
            {
                log = "[" + m_Type.ToString() + " ]" + m_str;
            }

            if (cv_ControllerLog != null)
            {
                lock (cv_ControllerLog)
                {
                    try
                    {
                        cv_ControllerLog.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        override public void WriteDebugLog(List<string> m_Logs, int m_Level)
        {
            if (Global.DebugLog != null)
            {
                Global.DebugLog.WriteLog(m_Logs, m_Level);
            }
        }
        override public void WriteDebugLog(string m_Log, int m_Level)
        {
            if (Global.DebugLog != null)
            {
                Global.DebugLog.WriteLog(m_Log, m_Level);
            }
        }
        protected virtual void AssignProcessFunctions()
        {
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.AccountData).Name, ProcessAccountChange);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDLogInOut).Name, ProcessLogInOut);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDAccountReq).Name, ProcessAccountReq);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.AlarmData).Name, ProcessAlarmChange);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDAlarmAction).Name, ProcessAlarmAction);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDAlarmReq).Name, ProcessAlarmReq);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.RecipeData).Name, ProcessRecipeChange);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRecipeAction).Name, ProcessRecipeAction);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRecipeReq).Name, ProcessRecipeReq);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.TimeOutData).Name, ProcessTimeoutData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDSetTimeOut).Name, ProcessSetTimeOut);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDOnlineRequest).Name, ProcessOnlineReq);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDInitial).Name, ProcessInitialize);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.SystemData).Name, ProcessSystemData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDSystemReq).Name, ProcessSystemDataReq);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDOperationModeChange).Name, ProcessOperatorModeChange);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDGlassCountReq).Name, ProcessGlassCountDataReq);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.GlassCountData).Name, ProcessGlassCountData);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDTimeoutDataReq).Name, ProcessTimeOutReq);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotAction).Name, ProcessRobotAction);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDOntMode).Name, ProcessOntMode);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDChangePortSlotType).Name, ProcessChangePortSlotType);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDShowOcrDecide).Name, ProcessShowOcrDecide);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDShowOcrDecideReply).Name, ProcessOcrDecideReply);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotJobAction).Name, ProcessRobotJobAction);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.SamplingData).Name, ProcessSamplingDataChange);

            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDSamplingDataReq).Name, ProcessSamplingDataReq);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDSamplingDataAction).Name, ProcessSamplingDataAction);
            


        }

        #region base receive
        protected virtual void ProcessChangePortSlotType(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOntMode(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDOntMode obj = m_Object as CommonData.HIRATA.MDOntMode;
            bool is_on = obj.PIsOn;
            BaseForm.PSystemData.PONT = is_on;
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAccountChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.AccountData obj = m_Object as CommonData.HIRATA.AccountData;
            BaseForm.cv_AccountData.Clone(obj);
            string log = "";
            log += "cv_CurAccountId = " + BaseForm.cv_AccountData.PCurAccountId + Environment.NewLine;
            log += "cv_CurPermission = " + BaseForm.cv_AccountData.cv_CurPermission.ToString() + Environment.NewLine;
            log += "Account list : " + Environment.NewLine;
            for (int i = 0; i < BaseForm.cv_AccountData.cv_AccountList.Count; i++ )
            {
                log += "Id : " + BaseForm.cv_AccountData.cv_AccountList[i].PId;
                log += ". Pw : " + BaseForm.cv_AccountData.cv_AccountList[i].PPw;
                log += ". Permission : " + BaseForm.cv_AccountData.cv_AccountList[i].PPermission.ToString() + Environment.NewLine;
            }
            if(!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessLogInOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDLogInOut obj = m_Object as CommonData.HIRATA.MDLogInOut;
            string log = "LogInOut Action : " + obj.PAction.ToString();
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAlarmChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.AlarmData obj = m_Object as CommonData.HIRATA.AlarmData;
            BaseForm.cv_Alarms.Clone(obj);
            string log = "";
            log += "Alarm list : " + Environment.NewLine;
            for (int i = 0; i < BaseForm.cv_Alarms.cv_AlarmList.Count; i++)
            {
                log += "Code : " + BaseForm.cv_Alarms.cv_AlarmList[i].PCode;
                log += ". Level : " + BaseForm.cv_Alarms.cv_AlarmList[i].PLevel.ToString();
                log += ". Time : " + BaseForm.cv_Alarms.cv_AlarmList[i].PTime.ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAlarmAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDAlarmAction obj = m_Object as MDAlarmAction;
            string log = "";
            log += "Alarm list : " + Environment.NewLine;
            for (int i = 0; i < BaseForm.cv_Alarms.cv_AlarmList.Count; i++)
            {
                log += "Code : " + BaseForm.cv_Alarms.cv_AlarmList[i].PCode;
                log += ". Level : " + BaseForm.cv_Alarms.cv_AlarmList[i].PLevel.ToString();
                log += ". Time : " + BaseForm.cv_Alarms.cv_AlarmList[i].PTime.ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSamplingDataAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDSamplingDataAction obj = m_Object as MDSamplingDataAction;
            string log = "Sampling data Action : " + obj.PAction.ToString(); 
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRecipeAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDRecipeAction obj = m_Object as MDRecipeAction;
            string log = "Recipe Action : " + obj.PAction.ToString(); 
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSamplingDataChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SamplingData obj = m_Object as SamplingData;
            BaseForm.cv_SamplingData.Clone(obj);
            string log = "";
            log += "Sampling data list : " + Environment.NewLine;
            for (int i = 0; i < BaseForm.cv_SamplingData.cv_SamplingList.Count; i++)
            {
                log += "Id : " + BaseForm.cv_SamplingData.cv_SamplingList[i].PNo.ToString();
                log += ". hit slot : " + string.Join("," , BaseForm.cv_SamplingData.cv_SamplingList[i].cv_hitSlot) + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRecipeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            RecipeData obj = m_Object as RecipeData;
            BaseForm.cv_Recipes.Clone(obj);
            string log = "";
            log += "Current Recipe : " + obj.PCurRecipeId + Environment.NewLine;
            log += "Recipe list : " + Environment.NewLine;
            for (int i = 0; i < BaseForm.cv_Recipes.cv_RecipeList.Count; i++)
            {
                log += "Id : " + BaseForm.cv_Recipes.cv_RecipeList[i].PId;
                log += ". Flow : " + BaseForm.cv_Recipes.cv_RecipeList[i].PFlow.ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        
        protected virtual void ProcessSamplingDataReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRecipeReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAlarmReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAccountReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOnlineReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessInitialize(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSetTimeOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSystemData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SystemData obj = m_Object as SystemData;
            BaseForm.PSystemData.Clone(obj);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessTimeoutData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            TimeOutData obj = m_Object as TimeOutData;
            BaseForm.cv_TimeoutData.Clone(obj);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOperatorModeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSystemDataReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessGlassCountData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            GlassCountData obj = m_Object as GlassCountData;
            BaseForm.cv_GlassCountData.Clone(obj);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessGlassCountDataReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessTimeOutReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRobotAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessShowOcrDecide(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOcrDecideReply(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRobotJobAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region base send
        public virtual bool SendAccountData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendMmfNotifyObject(typeof(AccountData).Name, BaseForm.cv_AccountData, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return true;
        }
        public virtual bool SendLogInOut(LogInOut m_Action , MmfEventClientEventType m_Type)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDLogInOut obj = new CommonData.HIRATA.MDLogInOut();
            obj.PAction = m_Action;
            obj.PType = m_Type;
            SendMmfRequestObject(typeof(CommonData.HIRATA.MDLogInOut).Name, obj, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return true;
        }
        public virtual bool SendAccountReq(MmfEventClientEventType m_Type, bool m_IsTimeoutType)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            MDAccountReq obj = new MDAccountReq();
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                SendMmfNotifyObject(typeof(MDAccountReq).Name, obj, KParseObjToXmlPropertyType.Field);
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;
                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDAccountReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDAccountReq).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendAlarmData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendMmfNotifyObject(typeof(AlarmData).Name, BaseForm.cv_Alarms, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return true;
        }
        public virtual bool SendAlarmAction(AlarmData m_AlarmData , MmfEventClientEventType m_Type)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool rtn = true;
            MDAlarmAction obj = new MDAlarmAction();
            obj.PType = m_Type;
            obj.AlarmData = m_AlarmData;
            SendMmfNotifyObject(typeof(MDAlarmAction).Name, obj, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        public virtual bool SendAlarmReq(MmfEventClientEventType m_Type , bool m_IsTimeoutType , uint m_Ticket=0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            MDAlarmReq obj = new MDAlarmReq();
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify)
                {
                    SendMmfNotifyObject(typeof(MDAlarmReq).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {
                    SendMmfReplyObject(typeof(MDAlarmReq).Name, obj, m_Ticket, typeof(MDAlarmReq).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;
                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDAlarmReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDAlarmReq).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendSamplingData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool rtn = true ;
            SendMmfNotifyObject(typeof(SamplingData).Name, BaseForm.cv_SamplingData, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        public virtual bool SendRecipeData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool rtn = true ;
            SendMmfNotifyObject(typeof(RecipeData).Name, BaseForm.cv_Recipes, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        public virtual bool SendRecipeAction(DataEidtAction m_Action, List<RecipeItem> m_Recipes, MmfEventClientEventType m_Type )
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = true;
            MDRecipeAction obj = new MDRecipeAction();
            obj.PType = m_Type;
            obj.PAction = m_Action;
            obj.Recipes = m_Recipes;
            SendMmfNotifyObject(typeof(MDRecipeAction).Name, obj , KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        } //MDSamplingDataReq

        public virtual bool SendSamplingDataReq(MmfEventClientEventType m_Type , bool m_IsTimeoutType)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            MDSamplingDataReq obj = new MDSamplingDataReq();
            obj.PType = m_Type;

            if (!m_IsTimeoutType)
            {
                SendMmfNotifyObject(typeof(MDSamplingDataReq).Name, obj, KParseObjToXmlPropertyType.Field);
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDSamplingDataReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDSamplingDataReq).Name, FunInOut.None);
                }
                else
                {
                    result = true ;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendRecipeReq(MmfEventClientEventType m_Type , bool m_IsTimeoutType)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            MDRecipeReq obj = new MDRecipeReq();
            obj.PType = m_Type;

            if (!m_IsTimeoutType)
            {
                SendMmfNotifyObject(typeof(MDRecipeReq).Name, obj, KParseObjToXmlPropertyType.Field);
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDRecipeReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDRecipeReq).Name, FunInOut.None);
                }
                else
                {
                    result = true ;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendOnlineReq(MmfEventClientEventType m_Type, OnlineMode m_Mode, bool m_IsTimeoutType)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDOnlineRequest obj = new CommonData.HIRATA.MDOnlineRequest();
            obj.PType = m_Type;
            obj.PCurMode = m_Mode;
            obj.PChangeMode = m_Mode;
            if (!m_IsTimeoutType)
            {
                SendMmfNotifyObject(typeof(CommonData.HIRATA.MDOnlineRequest).Name, obj, KParseObjToXmlPropertyType.Field);
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDOnlineRequest).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDOnlineRequest).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendInitialize(InitialAction m_Action, MmfEventClientEventType m_Type, bool m_IsTimeoutType ,uint m_Ticket=0 ,  Result m_Result = Result.OK , bool m_IsForce=false)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDInitial obj = new MDInitial();
            obj.PAction = m_Action;
            obj.PResult = m_Result;
            obj.PType = m_Type;
            obj.cv_IsForce = m_IsForce;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify)
                {
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDInitial).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {
                    SendMmfReplyObject(typeof(CommonData.HIRATA.MDInitial).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDInitial).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDInitial).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDInitial).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendTimeOutSetting(MmfEventClientEventType m_Type, MDSetTimeOut m_TimeOutObj, bool m_IsTimeoutType , uint m_Ticket=0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDSetTimeOut obj = m_TimeOutObj;
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify)
                {
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDSetTimeOut).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {
                    SendMmfReplyObject(typeof(CommonData.HIRATA.MDSetTimeOut).Name, obj, m_Ticket , typeof(CommonData.HIRATA.MDSetTimeOut).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object tmp = null;
                uint ticket;
                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDSetTimeOut).Name, obj, out ticket, out rtn, out tmp, 3000))
                {
                    //log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDSetTimeOut).Name;
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual void SendSystemData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendMmfNotifyObject(typeof(SystemData).Name, BaseForm.PSystemData, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public virtual bool SendSystemDataReq(MmfEventClientEventType m_Type , bool m_IsTimeoutType , UInt32 m_Ticket= 0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDSystemReq obj = new CommonData.HIRATA.MDSystemReq();
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify || m_Type == MmfEventClientEventType.etRequest)
                {
                    SendMmfNotifyObject(typeof(MDSystemReq).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {
                    SendMmfReplyObject(typeof(MDSystemReq).Name, obj, m_Ticket, typeof(MDSystemReq).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object tmp = null;
                uint ticket;
                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDSystemReq).Name, obj, out ticket, out rtn, out tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDSystemReq).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendGlassCountReq(MmfEventClientEventType m_Type , bool m_IsTimeoutType , UInt32 m_Ticket= 0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDGlassCountReq obj = new CommonData.HIRATA.MDGlassCountReq();
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify || m_Type == MmfEventClientEventType.etRequest)
                {
                    SendMmfNotifyObject(typeof(MDGlassCountReq).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {
                    SendMmfReplyObject(typeof(MDGlassCountReq).Name, obj, m_Ticket, typeof(MDGlassCountReq).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object tmp = null;
                uint ticket;
                if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDGlassCountReq).Name, obj, out ticket, out rtn, out tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDGlassCountReq).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual void SendGlassCountData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendMmfNotifyObject(typeof(GlassCountData).Name, BaseForm.cv_GlassCountData, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public virtual void SendTimeoutData()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendMmfNotifyObject(typeof(TimeOutData).Name, BaseForm.cv_TimeoutData, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public virtual bool SendOperationMode(bool m_IsForce, OperationMode m_Mode, MmfEventClientEventType m_Type, bool m_IsTimeoutType, UInt32 m_Ticket = 0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            try
            {
                CommonData.HIRATA.MDOperationModeChange obj = new CommonData.HIRATA.MDOperationModeChange();
                obj.PIsForce = m_IsForce;
                obj.PType = m_Type;
                obj.PChangeOperationMode = m_Mode;
                if (!m_IsTimeoutType)
                {
                    if (m_Type == MmfEventClientEventType.etNotify)
                    {
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDOperationModeChange).Name, obj, KParseObjToXmlPropertyType.Field);
                    }
                    else if (m_Type == MmfEventClientEventType.etReply)
                    {
                        SendMmfReplyObject(typeof(CommonData.HIRATA.MDOperationModeChange).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDOperationModeChange).Name, KParseObjToXmlPropertyType.Field);
                    }
                    result = true;
                }
                else
                {
                    string rtn;
                    object tmp = null;
                    uint ticket;
                    if (!SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDOperationModeChange).Name, obj, out ticket, out rtn, out tmp, 3000, KParseObjToXmlPropertyType.Field))
                    {
                        WriteLog(LogLevelType.Warning, "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDOperationModeChange).Name, FunInOut.None);
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(LogLevelType.Error, ex.ToString());
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendTimeOutReq(MmfEventClientEventType m_Type, bool m_IsTimeoutType, uint m_Ticket = 0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDTimeoutDataReq obj = new CommonData.HIRATA.MDTimeoutDataReq();
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify)
                {
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDTimeoutDataReq).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {

                    SendMmfReplyObject(typeof(CommonData.HIRATA.MDTimeoutDataReq).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDTimeoutDataReq).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDTimeoutDataReq).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDTimeoutDataReq).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual bool SendRobotAction(MDRobotAction m_ActionData , MmfEventClientEventType m_Type,  bool m_IsTimeoutType , uint m_Ticket=0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool result = false;
            CommonData.HIRATA.MDRobotAction obj = m_ActionData;
            obj.PType = m_Type;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify)
                {
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDRobotAction).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {

                    SendMmfReplyObject(typeof(CommonData.HIRATA.MDRobotAction).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDRobotAction).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDRobotAction).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDRobotAction).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual void SendOntModeReq(bool m_IsOn)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDOntMode obj = new MDOntMode();
            obj.PIsOn = m_IsOn;
            SendMmfNotifyObject(typeof(MDOntMode).Name, obj , KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public virtual bool SendPortSlotTypeChange(int m_PortId , int m_PortType , MmfEventClientEventType m_Type ,  bool m_IsTimeoutType , Result m_Result=Result.None , uint m_Ticket = 0)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDChangePortSlotType obj = new MDChangePortSlotType();
            obj.PPortId = m_PortId;
            obj.PSlotType = m_PortType;
            obj.PResult = m_Result;
            bool result = false;
            if (!m_IsTimeoutType)
            {
                if (m_Type == MmfEventClientEventType.etNotify)
                {
                    SendMmfNotifyObject(typeof(CommonData.HIRATA.MDChangePortSlotType).Name, obj, KParseObjToXmlPropertyType.Field);
                }
                else if (m_Type == MmfEventClientEventType.etReply)
                {

                    SendMmfReplyObject(typeof(CommonData.HIRATA.MDChangePortSlotType).Name, obj, m_Ticket, typeof(CommonData.HIRATA.MDChangePortSlotType).Name, KParseObjToXmlPropertyType.Field);
                }
                result = true;
            }
            else
            {
                string rtn;
                object rtn_tmp = null;
                uint ticket;

                if (!SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDChangePortSlotType).Name, obj, out ticket, out rtn, out rtn_tmp, 3000, KParseObjToXmlPropertyType.Field))
                {
                    WriteLog(LogLevelType.Warning, "[Time out] Wait : " + typeof(CommonData.HIRATA.MDChangePortSlotType).Name, FunInOut.None);
                }
                else
                {
                    result = true;
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return result;
        }
        public virtual void SendShowOcrDecide()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendMmfNotifyObject(typeof(MDShowOcrDecide).Name, new MDShowOcrDecide() , KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public virtual void SendShowOcrDecideReply(OCRMode m_Mode , string m_NewId="")
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDShowOcrDecideReply obj = new MDShowOcrDecideReply();
            obj.POcrDecide = m_Mode;
            obj.PKeyInValue = m_NewId;
            SendMmfNotifyObject(typeof(MDShowOcrDecideReply).Name, obj , KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public virtual bool SendRobotJobAction( RobotJobAction m_Action)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDRobotJobAction obj = new MDRobotJobAction();
            obj.PAction = m_Action;
            SendMmfNotifyObject(typeof(MDRobotJobAction).Name, obj, KParseObjToXmlPropertyType.Field);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return true;
        }
        #endregion
    }
}
