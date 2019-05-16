using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KgsCommon;
using CommonData.HIRATA;
using System.Reflection;

namespace BaseAp 
{
    public partial class BaseForm : Form
    {
        protected Dictionary<string, bool> cv_ModuleAlive = null;
        protected Dictionary<string, string> cv_ModuleWait = null;
        protected bool cv_IsModuleCommIni = false;
        protected FdModule cv_Module = FdModule.None;
        protected KDateTime cv_MemDateTime = SysUtils.Now();
        //private KTimer cv_Timer;
        protected System.Timers.Timer cv_Timer;
        public static KMemoLog cv_Log;
        public static KMemoryIOClient cv_Mio = null;
        public static CommonData.HIRATA.SystemData cv_SystemData = new SystemData();
        public static SystemData PSystemData
        {
            get { return cv_SystemData; }
            set { cv_SystemData = value; }
        }

        public static AlarmData cv_Alarms = new AlarmData();
        public static AccountData cv_AccountData = new AccountData();
        public static RecipeData cv_Recipes = new RecipeData();
        public static TimeOutData cv_TimeoutData = new TimeOutData();
        public static GlassCountData cv_GlassCountData = new GlassCountData();

        public BaseForm()
        {
            InitializeComponent();
        }
        public BaseForm(string[] args , FdModule m_Module)
        {
            InitializeComponent();
            cv_Module = m_Module;
            CommonData.HIRATA.CommonStaticData.Init(cv_Module.ToString());
            IniModuleWait();
            LinkCommonDataEvent();
            initMio();
            initLog();
            InitTimer();

        }
        ~BaseForm()
        {
            /*
            if (cv_Timer != null)
            {
                cv_Timer.Enabled = false;
                cv_Timer.Close();
                cv_Mio.Close();
            }
            */
        }

        private void IniModuleWait()
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            KIniFile ini = new KIniFile(CommonData.HIRATA.CommonStaticData.g_ModuleSystemIniFile) ;
            ini.ReadSection("WaitModule", tmp);
            cv_ModuleWait = tmp;
        }

        /// <summary>
        /// Each module override this function to decide Account , Alarm , Recipe Data.
        ///And call this function by derived class.
        /// </summary>
        protected virtual void ModuleInit()
        {
        }
        private void initMio()
        {
            cv_Mio = new KMemoryIOClient();
            cv_Mio.ServerName = "KGSMEMORYIODEMO";
            cv_Mio.Open();
        }
        protected virtual  void initLog()
        {
            if (cv_Log == null)
            {
                string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;
                cv_Log = new KMemoLog();
                cv_Log.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, CommonData.HIRATA.CommonStaticData.g_FDModuleName);
                cv_Log.LogFileName = enviPath + "\\" + CommonData.HIRATA.CommonStaticData.g_FDModuleName + ".log";
                cv_Log.SaveToIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, CommonData.HIRATA.CommonStaticData.g_FDModuleName);
            }
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
                    if (m_FunInOut == FunInOut.Leave)
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

            if (cv_Log != null)
            {
                lock (cv_Log)
                {
                    try
                    {
                        cv_Log.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        private void InitTimer()
        {
            if (cv_Timer == null)
            {
                cv_Timer = new System.Timers.Timer();
                cv_Timer.Interval = 500;
                cv_Timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
                cv_Timer.SynchronizingObject = this;
            }
        }
        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                if (SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_MemDateTime) > 30000)
                {
                    WriteLog(LogLevelType.General, "Mem : " + CommonData.HIRATA.CommonStaticData.GetMemUsage().ToString());

                    cv_MemDateTime = SysUtils.Now();
                }
                DoModuleCommInit();
                //DerivedOnTimer();
            }
            catch(Exception ex)
            {
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected void DoModuleCommInit()
        {
            if (!cv_IsModuleCommIni)
            {
                if (Global.Controller == null) return;
                Dictionary<string, bool> tmp = new Dictionary<string, bool>(Global.Controller.GetMmfClientConnectionStatus());
                bool result = true;
                //if (Global.Controller.GetMmfClientConnectionStatus)
                foreach(KeyValuePair<string,string> wait_module in cv_ModuleWait)
                {
                    if( Regex.Match( wait_module.Value , @"1" , RegexOptions.IgnoreCase).Success)
                    {
                        if(tmp.ContainsKey(wait_module.Key.Trim()))
                        {
                            if(!tmp[wait_module.Key.Trim()])
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
                if(result)
                {
                    string tmp1 = cv_Module.ToString();
                    SendProgramStart();
                    cv_IsModuleCommIni = true;
                }
            }
                /*
            else
            {
                bool result = true;
                //if (Global.Controller.GetMmfClientConnectionStatus)
                foreach (KeyValuePair<string, string> wait_module in cv_ModuleWait)
                {
                    if (Regex.Match(wait_module.Value, @"1", RegexOptions.IgnoreCase).Success)
                    {
                        if (tmp.ContainsKey(wait_module.Key.Trim()))
                        {
                            if (!tmp[wait_module.Key.Trim()] )
                            {
                                WriteLog(LogLevelType.Error, "[ERROE] : " + wait_module.Key + " module crash!!!");
                                result = false;
                                break;
                            }
                        }
                    }
                }
                if (result)
                {
                    /*
                    Process process = new Process();
                    process.StartInfo.FileName = @"C:\Program Files\7-zip\7z.exe";
                    process.StartInfo.Arguments = @"a -mx9 -tzip " + cv_TxTargetFolder.Text + "\\COPYLOG.7z " + cv_TxTargetFolder.Text + "\\COPYLOG";
                    if (File.Exists(process.StartInfo.FileName) && Directory.Exists(cv_TxTargetFolder.Text + "\\COPYLOG"))
                    {
                        process.Start();
                    }
                    SendProgramStart();
                }
            }
                 */
        }
        //derived class can override this timer function , if need.
        protected virtual void DerivedOnTimer()
        {
        }
        protected virtual void SendProgramStart()
        {
        }

        private void LinkCommonDataEvent()
        {
            if(cv_AccountData != null)
            {
                cv_AccountData.EventLogInOut += OnLogInOutEvent;
                cv_AccountData.EventAccountChange += OnAccountChangeEvent;
            }
            if(cv_Alarms != null)
            {
                cv_Alarms.EventAlarmAction += OnAlarmActionEvent;
                cv_Alarms.EventAlarmCharge += OnAlarmChange;
            }
            if(cv_Recipes != null)
            {
                cv_Recipes.EventRecipeAction += OnRecipeActionEvent;
                cv_Recipes.EventRecipeChange += OnRecipeChange;
            }
            if(cv_TimeoutData != null)
            {
                cv_TimeoutData.EventTimeOutDataChange += OnTimeOutDataChange;
            }
            if(cv_GlassCountData != null)
            {
                cv_GlassCountData.EventGlassCountChange += OnGlassCountDataChange;
            }
            if(cv_SystemData != null)
            {
                cv_SystemData.OnSystemDataChange += OnSystemDataChange;
                cv_SystemData.OnRobotStatusChange += OnRobotStatusChange;
                cv_SystemData.OnSystemStatusChange += OnSystemStatusChange;
            }
        }

        #region Link log in/out Event & Event function;
        //Trigger this event When AccountData login/out successful.(UI must override)
        protected virtual void OnLogInOutEvent(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount)
        {
        }

        //Trigger this event When AccountData change.(UI must override)
        protected virtual void OnAccountChangeEvent()
        {
        }
        #endregion

        #region Link Alarm Event & Event function
        //Trigger this event When AlarmData add/del successful.(LGC must override)
        protected virtual void OnAlarmActionEvent(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected virtual void OnAlarmChange()
        {
        }
        #endregion

        #region Link Recipe Event & Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected virtual void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
        }
        //Trigger this event When RecipeData change.(LGC must override)
        protected virtual void OnRecipeChange()
        {
        }
        //Trigger this event When Timeout data change.(LGC must override)
        protected virtual void OnTimeOutDataChange()
        {
        }
        //Trigger this event When Timeout data change.(LGC must override)
        protected virtual void OnGlassCountDataChange()
        {
        }
        //Trigger this event When Timeout data change.(LGC must override)
        protected virtual void OnSystemDataChange()
        {
        }
        protected virtual void OnRobotStatusChange()
        {
        }
        protected virtual void OnSystemStatusChange()
        {
        }
        #endregion
    }
}
