using System;
using System.Collections.Generic;
using CommonData.HIRATA;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using CommonData;
using BaseAp;
using System.Reflection;
using System.Diagnostics;

namespace LGC
{
    public partial class LgcForm : BaseForm
    {
        //Wait test_form = new Wait();
        public static KMemoLog cv_AlarmLog;
        public static Dictionary<string, List<AlarmItem>> cv_ApiAlarm = new Dictionary<string, List<AlarmItem>>();
        public static Dictionary<int, List<AllDevice>> cv_CurRecipeFlowStepSetting = new Dictionary<int, List<AllDevice>>();
        public static Queue<RobotJob> cv_RobotJobPath = new Queue<RobotJob>();
        public static Queue<RobotJob> cv_RobotJobPathCompare = new Queue<RobotJob>();
        public static Queue<RobotJob> cv_RobotManaulJobPath = new Queue<RobotJob>();
        public static List<int> cv_InProcessPort = new List<int>();
        internal static Dictionary<int, Eq> cv_EqContainer = new Dictionary<int, Eq>();
        internal static Dictionary<int, Port> cv_PortContainer = new Dictionary<int, Port>();
        internal static Dictionary<int, Robot> cv_RobotContainer = new Dictionary<int, Robot>();
        internal static Dictionary<int, Buffer> cv_BufferContainer = new Dictionary<int, Buffer>();
        internal static Dictionary<int, Aligner> cv_AlignerContainer = new Dictionary<int, Aligner>();
        internal static bool cv_IsPreView = false;
        internal static bool cv_IsCycleStop = false;
        internal bool cv_PutGlassStandby = false;
        internal bool cv_VasLoadGlassBeforeUnload = false;
        internal bool cv_CheckFirstStepWhenPutGlass = false;
        internal bool cv_GetPutStandbyExceptVas = false;
        internal bool cv_CheckEqDataLocalMode = false;
        //MMF
        internal static LGCController cv_MmfController = null;

        KDateTime cv_DataTime = SysUtils.Now();
        KDateTime cv_WaitUvRecordTime = SysUtils.Now();
        KTimer cv_RobotActionTimer;
        public LgcForm(string[] args)
            : base(args, FdModule.LGC)
        {
            InitializeComponent();
            LoadAlarmTable();
            cv_MmfController = new LGCController();
            ModuleInit();
            cv_MmfController.SetTimeChartTimeOut();
            layoutInit();
            ParserFlowStep();
            cv_MmfController.Open();
            initTimer();
            cv_MmfController.initTimer();
            LgcForm.cv_Mio.SetPortValue(0x344d, (int)LgcForm.PSystemData.POcrMode + (1 << 4));
            CommonData.HIRATA.CommonStaticData.KillTerminal(args);
            WriteLog(LogLevelType.General, "[LGC module start]");
            string root_path = SysUtils.ExtractFileDir(Assembly.GetExecutingAssembly().Location);
            string lib_info = "[KgsCommonDotNetLib_x64.dll]\n";
            string kgs_lib_path = root_path + "\\KgsCommonDotNetLib_x64.dll";
            lib_info += "Path : " + root_path + "\\KgsCommonDotNetLib_x64.dll\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileVersion + "\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileDescription + "\n";
            lib_info += "\n[KgsCommonX64.dll]\n";
            kgs_lib_path = root_path + "\\KgsCommonX64.dll";
            lib_info += "Path : " + root_path + "\\KgsCommonX64.dll" + "\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileVersion + "\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileDescription + "\n";
            lib_info += "\nProgram Version : " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString() + "\n";
            lib_info += "Path : " + Assembly.GetExecutingAssembly().Location + "\n";
            WriteLog(LogLevelType.General, lib_info);
            //test_form.Show();
            cv_Timer.Start();
        }
        protected override void initLog()
        {
            base.initLog();
            if (cv_AlarmLog == null)
            {
                string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;
                cv_AlarmLog = new KMemoLog();
                cv_AlarmLog.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, "AlarmLog");
                cv_AlarmLog.LogFileName = enviPath + "\\AlarmLog.log";
                cv_AlarmLog.SaveToIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, "AlarmLog");
                /*
                for(int i=1 ; i<10000 ; i++)
                {
                    AlarmItem tmp = new AlarmItem();
                    tmp.PTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    tmp.cv_Code = i.ToString();
                    if(i%2 == 0)
                    tmp.PLevel = AlarmLevele.Light;
                    else
                    tmp.PLevel = AlarmLevele.Serious;
                    tmp.PMainDescription = "test";
                    tmp.PSubDescription = i.ToString();
                    WriteAlarmLog(tmp);
                }
                */
            }
        }
        public static void WriteAlarmLog(CommonData.HIRATA.AlarmItem m_AlarmItem)
        {
            if(cv_AlarmLog != null)
            {
                string log = m_AlarmItem.PTime + ",";
                log += m_AlarmItem.PCode + ",";
                log += m_AlarmItem.PLevel + ",";
                log += m_AlarmItem.PUnit + ",";
                log += m_AlarmItem.PMsg;
                cv_AlarmLog.WriteLog(log);
            }
        }

        public static void SendRobotJobPath()
        {
            //WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDRobotjobPath obj = new MDRobotjobPath();
            if (cv_RobotJobPath != null)
            {
                if (IsNeedSendRobotJobPath())
                {
                obj.RobotJob = cv_RobotJobPath.ToList();
                    cv_MmfController.SendMmfNotifyObject(typeof(MDRobotjobPath).Name, obj, KParseObjToXmlPropertyType.Field);
                }
            }
            else
            {
                obj.RobotJob = new List<RobotJob>();
                cv_MmfController.SendMmfNotifyObject(typeof(MDRobotjobPath).Name, obj, KParseObjToXmlPropertyType.Field);
            }
            //WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public static bool IsNeedSendRobotJobPath()
        {
            //WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool rtn = false;
            if(cv_RobotJobPathCompare.Count != cv_RobotJobPath.Count)
            {
                rtn = true;
            }
            else
            {
                for (int i = 0; i < cv_RobotJobPath.Count; i++)
                {
                    RobotJob tmp1 = cv_RobotJobPath.ElementAt(i);
                    RobotJob tmp2 = cv_RobotJobPathCompare.ElementAt(i);
                    if ((tmp1.cv_Action != tmp2.cv_Action) || (tmp1.cv_GetArm != tmp2.cv_GetArm) || (tmp1.cv_IsWaitGet != tmp2.cv_IsWaitGet) || (tmp1.cv_IsWaitPut != tmp2.cv_IsWaitPut) ||
                    (tmp1.cv_ManualExchangeForAligner != tmp2.cv_ManualExchangeForAligner) || (tmp1.cv_ManualExchangeForAlignerDeg != tmp2.cv_ManualExchangeForAlignerDeg) ||
                        (tmp1.cv_PutArm != tmp2.cv_PutArm) || (tmp1.cv_RobotId != tmp2.cv_RobotId) || (tmp1.cv_Target != tmp2.cv_Target) || (tmp1.cv_TargetId != tmp2.cv_TargetId) ||
                        (tmp1.cv_TargetSlot != tmp2.cv_TargetSlot) || (tmp1.cv_UseHs != tmp2.cv_UseHs))
                    {
                        rtn = true;
                        break;
                    }
                }
            }
            if(rtn)
            {
                cv_RobotJobPathCompare.Clear();
                cv_RobotJobPathCompare = new Queue<RobotJob>(cv_RobotJobPath);
            }
            return rtn;
            //WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void ParserFlowStep()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_CurRecipeFlowStepSetting = null;
            cv_CurRecipeFlowStepSetting = new Dictionary<int, List<AllDevice>>();
            KIniFile stepIni = new KIniFile(CommonStaticData.g_FlowStepSettingFile);
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            RecipeItem recipe = null;
            if (cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe.PFlow == OdfFlow.Flow2_1 || recipe.PFlow == OdfFlow.Flow2_2)
                {
                    WriteLog(LogLevelType.General, "Set Flow2. Change UV get arm : " + RobotArm.rbaDown.ToString() +
                        " put arm : " + RobotArm.rbaUp.ToString());
                    GetEqById((int)EqId.UV_1).PGetArm = RobotArm.rbaDown;
                    GetEqById((int)EqId.UV_1).PPutArm = RobotArm.rbaUp;
                }
                else
                {
                    WriteLog(LogLevelType.General, "Set Not Flow2. Set UV get arm : " + RobotArm.rbaUp.ToString() +
                        " put arm : " + RobotArm.rbaDown.ToString());
                    GetEqById((int)EqId.UV_1).PGetArm = RobotArm.rbaUp;
                    GetEqById((int)EqId.UV_1).PPutArm = RobotArm.rbaDown;
                }

                string section = recipe.PFlow.ToString().Substring(4);
                stepIni.ReadSection(section, tmp);
                foreach (KeyValuePair<string, string> pair in tmp)
                {
                    Match match = Match.Empty;
                    match = Regex.Match(pair.Key, @"\d", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        int step_id = Convert.ToInt16(match.Value);
                        List<string> steps = pair.Value.Split(',').ToList();
                        List<AllDevice> step_devices = new List<AllDevice>();
                        foreach (string step_item in steps)
                        {
                            if (Regex.Match(step_item, @"LP").Success)
                            {
                                step_devices.Add(AllDevice.LP);
                            }
                            else if (Regex.Match(step_item, @"UP").Success)
                            {
                                step_devices.Add(AllDevice.UP);
                            }
                            else if (Regex.Match(step_item, @"Buffer").Success)
                            {
                                step_devices.Add(AllDevice.Buffer);
                            }
                            else if (Regex.Match(step_item, @"Aligner").Success)
                            {
                                step_devices.Add(AllDevice.Aligner);
                            }
                            else if (Regex.Match(step_item, @"EQ").Success)
                            {
                                int eq_id = Convert.ToInt16(step_item.Substring(2));
                                EqId enumid = (EqId)eq_id;
                                AllDevice all_device_item = (AllDevice)Enum.Parse(typeof(AllDevice), enumid.ToString());
                                step_devices.Add(all_device_item);
                            }
                        }
                        cv_CurRecipeFlowStepSetting[step_id] = step_devices;
                    }
                }
            }
            string log = "Set recipe flow : " + Environment.NewLine;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_CurRecipeFlowStepSetting)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        #region Calculate Robot Job Path
        private void CalculateRobotJobPath()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();
            sw.Start();

            Dictionary<int, RobotJob> job_map = new Dictionary<int, RobotJob>();
            if (cv_CurRecipeFlowStepSetting != null && cv_CurRecipeFlowStepSetting.Count > 0)
            {
                int max_step = cv_CurRecipeFlowStepSetting.Count;
                int start_pos = 0;
                int end_pos = 0;
                int first_step = 1;
                for (int step = first_step; step <= max_step; step++)
                {
                    List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[step];
                    if (!cv_stepDevice.Contains(AllDevice.VAS))
                    {
                        if (!CheckFlowCanRun(step))
                        {
                            continue;
                        }
                    }
                    foreach (AllDevice device in cv_stepDevice)
                    {
                        if (device != AllDevice.Aligner && device != AllDevice.LP && device != AllDevice.UP && device != AllDevice.Buffer)
                        {
                            EqId eq_id = EqId.None;
                            int time_chart_instance = 0;
                            int eq_time_chart_cur_step = 0;
                            if (Enum.TryParse<EqId>(device.ToString(), out eq_id))
                            {
                                if (eq_id == EqId.VAS)
                                {
                                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                    time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                }
                                else
                                {
                                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                    time_chart_instance = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                                }
                            }
                            if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                            {
                                EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                                if (gif_type == EqInterFaceType.Unload)
                                {
                                    Eq eq = GetEqById((int)eq_id);
                                    if (eq_id == EqId.VAS)
                                    {
                                        if (cv_VasLoadGlassBeforeUnload)
                                        {
                                            int buffer_slot = 0;
                                            if (GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Glass, out buffer_slot))
                                            {
                                                if (GetEqById((int)eq_id).GetTimeChatCurStep(2) == (int)TimechartNormal.STEP_ID_ActionReady)
                                                {
                                                    job_map[step] = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Buffer, 1, buffer_slot, false);
                                                    job_map[step] = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, 1, 2, true);
                                                }
                                            }
                                        }
                                        job_map[step] = new RobotJob(1, RobotArm.rabNone, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                    }
                                    else
                                    {
                                        job_map[step] = new RobotJob(1, RobotArm.rabNone, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                    }
                                    start_pos = step;
                                    if (FindEndStep(step, ref end_pos, ref job_map))
                                    {
                                        if (end_pos != step)
                                        {
                                            if (start_pos != end_pos && start_pos <= end_pos)
                                            {
                                                GetRobotJobQFormMap(start_pos, end_pos, job_map);
                                                sw.Stop();
                                                WriteLog(LogLevelType.General, "Calculate run mode speed : " + sw.Elapsed.TotalMilliseconds.ToString() + " ms");
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                                else if (gif_type == EqInterFaceType.Exchange)
                                {
                                    Eq eq = GetEqById((int)eq_id);
                                    bool is_change = false;
                                    if (eq_id == EqId.SDP1)
                                    {
                                        EqInterFaceType sdp2_gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance((int)EqGifTimeChartId.TIMECHART_ID_SDP2).cv_ActionType;
                                        if(sdp2_gif_type == EqInterFaceType.Exchange)
                                        {
                                            if (cv_MmfController.cv_TimechartController.GetTimeChartInstance((int)EqGifTimeChartId.TIMECHART_ID_SDP1).cv_DataTime < cv_MmfController.cv_TimechartController.GetTimeChartInstance((int)EqGifTimeChartId.TIMECHART_ID_SDP2).cv_DataTime)
                                            {
                                                job_map[step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)EqId.SDP2, 1, true);
                                                is_change = true;
                                            }
                                        }
                                        if (!is_change)
                                        {
                                            job_map[step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)EqId.SDP1, 1, true);
                                        }
                                    }
                                    else if(eq_id == EqId.SDP2)
                                    {
                                        EqInterFaceType sdp1_gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance((int)EqGifTimeChartId.TIMECHART_ID_SDP1).cv_ActionType;
                                        if (sdp1_gif_type == EqInterFaceType.Exchange)
                                        {
                                            if (cv_MmfController.cv_TimechartController.GetTimeChartInstance((int)EqGifTimeChartId.TIMECHART_ID_SDP1).cv_DataTime > cv_MmfController.cv_TimechartController.GetTimeChartInstance((int)EqGifTimeChartId.TIMECHART_ID_SDP2).cv_DataTime)
                                            {
                                                job_map[step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)EqId.SDP1, 1, true);
                                                is_change = true;
                                            }
                                        }
                                        if(!is_change)
                                        {
                                            job_map[step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)EqId.SDP2, 1, true);                                        }
                                    }
                                    else
                                    {
                                        job_map[step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                    }
                                    if (FindEndStep(step, ref end_pos, ref job_map))
                                    {
                                        if (end_pos != step)
                                        {
                                            start_pos = step;
                                            if (FindPortOrBufferAsFirstStep(step, ref start_pos, ref job_map))
                                            {
                                                if (start_pos != step)
                                                {
                                                    job_map[step].PAction = RobotAction.Exchange;
                                                }
                                            }
                                            else
                                            {
                                                job_map[step].PPutArm = RobotArm.rabNone;
                                            }
                                            GetRobotJobQFormMap(start_pos, end_pos, job_map);
                                            sw.Stop();
                                            WriteLog(LogLevelType.General, "Calculate run mode speed : " + sw.Elapsed.TotalMilliseconds.ToString() + " ms");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sw.Stop();
            WriteLog(LogLevelType.General, "Calculate run mode speed : " + sw.Elapsed.TotalMilliseconds.ToString() + " ms");
        }
        private void CalculateVasGlass()
        {
            RecipeItem recipe = null;
            if (cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe.PFlow != OdfFlow.Flow1_1 && recipe.PFlow != OdfFlow.Flow1_2)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            //Can't take glass from buffer , buz UV can't run.
            if (GetEqById((int)EqId.UV_1).GetStatus() != EquipmentStatus.Idle && GetEqById((int)EqId.UV_1).GetStatus() != EquipmentStatus.Run)
            {
                return;
            }

            int time_chart_instance_up = 0;
            time_chart_instance_up = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
            EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance_up).cv_ActionType;
            int time_chart_instance_down = 0;
            time_chart_instance_down = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
            EqInterFaceType gif_type_down = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance_down).cv_ActionType;
            Buffer buffer = GetBufferById(1);
            Robot robot = GetRobotById(1);
            int buffer_glass_slot = 0;
            //first do unload then load glass
            int eq_time_chart_cur_step_up = 0;
            eq_time_chart_cur_step_up = GetEqById((int)EqId.VAS).GetTimeChatCurStep(2);

            if (eq_time_chart_cur_step_up == TimechartNormal.STEP_ID_ActionReady)
            {
                if (gif_type == EqInterFaceType.Load && gif_type_down != EqInterFaceType.Unload )
                {
                    if (buffer.cv_Data.GetUnloadSlot(BufferSlotType.Glass, out buffer_glass_slot))
                {
                    if (cv_RobotJobPath != null && cv_RobotJobPath.Count == 0 && !robot.IsBusy)
                    {
                            //
                            bool is_first_can_load = false;
                            if (cv_CheckFirstStepWhenPutGlass)
                            {
                                List<AllDevice> dievices = cv_CurRecipeFlowStepSetting[2];
                                List<AllDevice> form_dievices = cv_CurRecipeFlowStepSetting[1];
                                foreach (AllDevice device in dievices)
                                {
                                    if ((int)device >= ((int)AllDevice.SDP1) && (int)device <= ((int)AllDevice.UV_2))
                                    {
                                        if (!CheckFlowCanRun(1))
                                        {
                                            return;
                                        }
                                        EqId eq = (EqId)Enum.Parse(typeof(EqId), device.ToString());
                                        if (GetEqById((int)eq).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                                        {
                                            int time_chart_instance = GetEqById((int)eq).cv_Comm.cv_TimeChatId;
                                            if (eq == EqId.VAS)
                                            {
                                                time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                            }
                                            EqInterFaceType first_gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                                            if (first_gif_type == EqInterFaceType.Load)
                                            {
                                                if (cv_RobotJobPath == null || cv_RobotJobPath.Count == 0)
                                                {
                                                    for (int i = 0; i < form_dievices.Count; i++)
                                                    {
                                                        if (form_dievices[i] == AllDevice.Buffer)
                                                        {
                                                            Buffer bf = GetBufferById(1);
                                                            int buffer_wafter_slot = 0;
                                                            if (bf.cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out buffer_wafter_slot))
                                                            {
                                                                is_first_can_load = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if(!is_first_can_load)
                                {
                                    RobotJob job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Buffer, 1, buffer_glass_slot, false);
                        cv_RobotJobPath.Enqueue(job);
                        job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)EqId.VAS, 2, true);
                        cv_RobotJobPath.Enqueue(job);
                                    return;
                                }
                            }
                            else
                            {
                                RobotJob job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Buffer, 1, buffer_glass_slot, false);
                                cv_RobotJobPath.Enqueue(job);
                                job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)EqId.VAS, 2, true);
                                cv_RobotJobPath.Enqueue(job);
                            }
                        }
                    }
                }
            }
        }
        private bool FindEndStep(int m_CurStep, ref int m_EndPos, ref Dictionary<int, RobotJob> m_JobMap)
        {
            bool rtn = false;
            int pre_step = m_CurStep;
            int now_step = m_CurStep + 1;
            if (cv_CurRecipeFlowStepSetting.ContainsKey(now_step))
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[now_step];
                foreach (AllDevice device in cv_stepDevice)
                {
                    if (device == AllDevice.Aligner)
                    {
                        if (!GetAlignerById(1).cv_Data.GlassDataMap[1].PHasData && !GetAlignerById(1).cv_Data.GlassDataMap[1].PHasSensor)
                        {
                            m_JobMap[now_step] = new RobotJob(1, m_JobMap[pre_step].PGetArm, RobotArm.rabNone, RobotAction.Put,
                                ActionTarget.Aligner, 1, 1, false);
                            if (FindEndStep(now_step, ref m_EndPos, ref m_JobMap))
                            {
                                m_JobMap[now_step].PAction = RobotAction.Exchange;
                                rtn = true;
                                break;
                            }
                            else
                            {
                                m_JobMap.Remove(now_step);
                                rtn = false;
                            }
                        }
                    }// stop point.
                    else if (device == AllDevice.UP)
                    {
                        int port = 0;
                        int slot = 0;
                        if (FindUnloadPortToPutSubstrate(out port, out slot , m_JobMap[pre_step]))
                        {
                            if (m_JobMap[pre_step].PGetArm != RobotArm.rabNone)
                            {
                                m_JobMap[now_step] = new RobotJob(1, m_JobMap[pre_step].PGetArm, RobotArm.rabNone, RobotAction.Put,
                                    ActionTarget.Port, port, slot, false);
                                rtn = true;
                                m_EndPos = now_step;
                                break;
                            }
                        }
                        else
                        {
                            m_JobMap.Remove(now_step);
                            rtn = false;
                        }
                    }
                    else
                    {
                        EqId eq_id = EqId.None;
                        int time_chart_instance = 0;
                        int eq_time_chart_cur_step = 0;
                        if (Enum.TryParse<EqId>(device.ToString(), out eq_id))
                        {
                            if (eq_id == EqId.VAS)
                            {
                                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                            }
                            else
                            {
                                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                time_chart_instance = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                            }
                        }
                        Eq eq = GetEqById((int)eq_id);
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                        {
                            EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                            //stop point.
                            if (gif_type == EqInterFaceType.Load)
                            {
                                if (m_JobMap[pre_step].PTarget == ActionTarget.Aligner)
                                {
                                    if (m_JobMap[pre_step].PGetArm == RobotArm.rabNone)
                                    {
                                        m_JobMap[pre_step].PGetArm = eq.PPutArm;
                                        m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq_id, 1, true);
                                        m_EndPos = now_step;
                                        rtn = true;
                                        break;
                                    }
                                }
                                else
                                {// pre_step always EQ.
                                    if (m_JobMap[pre_step].PAction == RobotAction.Exchange || m_JobMap[pre_step].PAction == RobotAction.Get)
                                    {
                                        if (eq.PPutArm != RobotArm.rbaBoth)
                                        {
                                            if (m_JobMap[pre_step].PGetArm == eq.PPutArm)
                                            {
                                                m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq_id, 1, true);
                                                rtn = true;
                                                m_EndPos = now_step;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            m_JobMap[now_step] = new RobotJob(1, m_JobMap[pre_step].PGetArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq_id, 1, true);
                                            rtn = true;
                                            m_EndPos = now_step;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (gif_type == EqInterFaceType.Exchange)
                            {
                                if (m_JobMap[pre_step].PTarget == ActionTarget.Aligner)
                                {
                                    if (m_JobMap[pre_step].PGetArm == RobotArm.rabNone)
                                    {
                                        m_JobMap[pre_step].PGetArm = eq.PPutArm;
                                        m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Exchange, ActionTarget.Eq, (int)eq_id, 1, true);
                                        rtn = FindEndStep(now_step, ref m_EndPos, ref m_JobMap);
                                        if (rtn)
                                        {
                                            //if this device job ok , not need fine sibling.
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (m_JobMap[pre_step].PGetArm == eq.PPutArm)
                                    {
                                        m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Exchange, ActionTarget.Eq, (int)eq_id, 1, true);
                                        rtn = FindEndStep(now_step, ref m_EndPos, ref m_JobMap);
                                        if (rtn)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!rtn)
                        {
                            if (eq_id == EqId.UV_1 || eq_id == EqId.UV_2)
                            {
                                if (m_JobMap[pre_step].PTargetId == (int)(EqId.VAS))
                                {
                                    m_JobMap[now_step] = new RobotJob(1, m_JobMap[pre_step].PGetArm, RobotArm.rabNone, RobotAction.PutWait, ActionTarget.Eq, (int)eq_id, 1, false);
                                    rtn = true;
                                }
                            }
                        }
                    }
                }
            }
            return rtn;
        }
        //do find glass. PLastDevice at Port , if yes cut the path and re-calculate.( get wafer from the port).
        private void GetRobotJobQFormMap(int m_Start, int m_End, Dictionary<int, RobotJob> m_Map)
        {
            if (cv_RobotJobPath == null)
            {
                cv_RobotJobPath = new Queue<RobotJob>();
            }
            cv_RobotJobPath.Clear();
            RecipeItem recipe = null;
            bool recipe_need_glass = false;
            if (cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe != null)
                {
                    if (recipe.PVasNeedGlass)
                    {
                        recipe_need_glass = true;
                    }
                }
            }
            for (int i = m_Start; i <= m_End; i++)
            {
                bool via_vas = (m_Map[i].PTarget == ActionTarget.Eq && m_Map[i].PTargetId == (int)EqId.VAS && m_Map[i].PTargetSlot == 1 && m_Map[i].PAction == RobotAction.Put);
                bool vas_can_load_glass = GetEqById(4).GetTimeChatCurStep(2) == TimechartNormal.STEP_ID_ActionReady;
                if (via_vas)
                {
                    if (recipe_need_glass)
                    {
#if VASMODIFY
                        cv_RobotJobPath.Enqueue(m_Map[i]);
#else
                        if (vas_can_load_glass)
                        {
                            int slot;
                            if (GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Glass, out slot))
                            {
                                RobotJob tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Buffer, 1, slot, false);
                                cv_RobotJobPath.Enqueue(tmp);
                                tmp = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, 4, 2, true);
                                cv_RobotJobPath.Enqueue(tmp);
                                cv_RobotJobPath.Enqueue(m_Map[i]);
                            }
                            else
                            {
                                cv_RobotJobPath.Clear();
                                return;
                            }
                        }
                        else
                        {
                            cv_RobotJobPath.Clear();
                            return;
                        }
#endif
                    }
                    else
                    {
                        cv_RobotJobPath.Enqueue(m_Map[i]);
                    }
                }
                else
                {
                    cv_RobotJobPath.Enqueue(m_Map[i]);
                }
            }
            /* write this for recovery if has job but can't do.But not use now.
             * 
            Dictionary<int, RobotJob> tmp_map = new Dictionary<int, RobotJob>();
            Queue<RobotJob> backupQ = new Queue<RobotJob>(cv_RobotJobPath);
            int tmp_step = 1;
            while (cv_RobotJobPath.Count > 0)
            {
                tmp_map[tmp_step] = cv_RobotJobPath.Dequeue();
                tmp_step++;
            }
            if (tmp_map != null && tmp_map.Count > 0)
            {
                foreach (int port_id in cv_InProcessPort)
                {
                    Port port = GetPortById(port_id);
                    if (port.PPortStatus == PortStaus.LDCM && port.PLotStatus == LotStatus.Process && port.cv_Data.PProductionType == ProductCategory.Wafer
                        && port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        for (int i = 1; i < port.cv_Data.cv_SlotCount; i++)
                        {
                            if (port.cv_Data.GlassDataMap[i].PLastDevice != AllDevice.None)
                            {
                                GlassData glass = port.cv_Data.GlassDataMap[i];
                                int start_pos = -1;
                                if (glass.PLastDevice == AllDevice.UP)
                                {
                                    foreach (KeyValuePair<int, RobotJob> pair in tmp_map)
                                    {
                                        if (pair.Value.PTarget == ActionTarget.Port)
                                        {
                                            if ((pair.Value.PTargetId == 1) || (pair.Value.PTargetId == 2))
                                            {
                                                start_pos = pair.Key;
                                                break;
                                            }
                                        }
                                    }
                                    if (start_pos != -1)
                                    {
                                        tmp_map[start_pos - 1] = new RobotJob(1, RobotArm.rabNone, tmp_map[start_pos].PPutArm, RobotAction.Get,
                                            ActionTarget.Port, port_id, i, false);
                                        cv_RobotJobPath = new Queue<RobotJob>();
                                        int re_start = start_pos - 1;
                                        while (tmp_map.ContainsKey(re_start))
                                        {
                                            cv_RobotJobPath.Enqueue(tmp_map[re_start]);
                                            re_start++;
                                        }
                                        return;
                                    }
                                }
                                if (glass.PLastDevice == AllDevice.Aligner)
                                {
                                    foreach (KeyValuePair<int, RobotJob> pair in tmp_map)
                                    {
                                        if (pair.Value.PTarget == ActionTarget.Aligner)
                                        {
                                            start_pos = pair.Key;
                                            break;
                                        }
                                    }
                                    if (start_pos != -1)
                                    {
                                        tmp_map[start_pos - 1] = new RobotJob(1, RobotArm.rabNone, tmp_map[start_pos].PPutArm, RobotAction.Get,
                                            ActionTarget.Port, port_id, i, false);
                                        cv_RobotJobPath = new Queue<RobotJob>();
                                        int re_start = start_pos - 1;
                                        while (tmp_map.ContainsKey(re_start))
                                        {
                                            cv_RobotJobPath.Enqueue(tmp_map[re_start]);
                                            re_start++;
                                        }
                                        return;
                                    }
                                }
                                if (((int)glass.PLastDevice) >= ((int)AllDevice.SDP1) && ((int)glass.PLastDevice) <= ((int)AllDevice.UV_2))
                                {
                                    EqId eq_id = (EqId)Enum.Parse(typeof(EqId), glass.PLastDevice.ToString());
                                    foreach (KeyValuePair<int, RobotJob> pair in tmp_map)
                                    {
                                        if (pair.Value.PTarget == ActionTarget.Eq)
                                        {
                                            if (pair.Value.PTargetId == (int)eq_id)
                                            {
                                                start_pos = pair.Key;
                                                break;
                                            }
                                        }
                                    }
                                    if (start_pos != -1)
                                    {
                                        if (GetEqById((int)eq_id).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                                        {
                                            int time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                            if (eq_id != EqId.VAS)
                                            {
                                                time_chart_instance = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                                            }
                                            EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                                            if (gif_type == EqInterFaceType.Load)
                                            {
                                                tmp_map[start_pos - 1] = new RobotJob(1, RobotArm.rabNone, tmp_map[start_pos].PPutArm, RobotAction.Get,
                                                    ActionTarget.Port, port_id, i, false);
                                            }
                                            else if (gif_type == EqInterFaceType.Exchange)
                                            {
                                                if (tmp_map[start_pos].PAction == RobotAction.Get)
                                                {
                                                    tmp_map[start_pos].PAction = RobotAction.Exchange;
                                                    tmp_map[start_pos].PPutArm = GetEqById((int)eq_id).PPutArm;
                                                }
                                                tmp_map[start_pos - 1] = new RobotJob(1, RobotArm.rabNone, tmp_map[start_pos].PPutArm, RobotAction.Get,
                                                    ActionTarget.Port, port_id, i, false);
                                            }
                                            cv_RobotJobPath = new Queue<RobotJob>();
                                            int re_start = start_pos - 1;
                                            while (tmp_map.ContainsKey(re_start))
                                            {
                                                cv_RobotJobPath.Enqueue(tmp_map[re_start]);
                                                re_start++;
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                cv_RobotJobPath = backupQ;
            }
            */
        }
        //only find buffer , not include port.
        private bool FindPortOrBufferAsFirstStep(int m_CurStep, ref int m_StartPos, ref Dictionary<int, RobotJob> m_JobMap)
        {
            bool rtn = false;
            int next_step = m_CurStep;
            int now_step = m_CurStep - 1;
            if (cv_CurRecipeFlowStepSetting.ContainsKey(now_step))
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[now_step];
                cv_stepDevice.Sort(
                        (x, y) =>
                        {
                            return ((int)x).CompareTo((int)y);
                        }
                    );
                foreach (AllDevice device in cv_stepDevice)
                {
                    int slot;
                    if (device == AllDevice.Buffer)
                    {
                        if (GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out slot))
                        {
                            m_StartPos = now_step;
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get, ActionTarget.Buffer, 1, slot, false);
                            rtn = true;
                            break;
                        }
                    }
                    else if (device == AllDevice.LP)
                    {
                        foreach (int port_id in cv_InProcessPort)
                        {
                            Port port = GetPortById(port_id);
                            if (port.PPortStatus == PortStaus.LDCM && port.PLotStatus == LotStatus.Process)
                            {
                                for (int slot_id = 1; slot_id <= port.cv_SlotCount; slot_id++)
                                {
                                    if (port.cv_Data.PProductionType == ProductCategory.Wafer)
                                    {
                                        if ((port.cv_Data.GlassDataMap[slot_id].PHasData) && (port.cv_Data.GlassDataMap[slot_id].PHasSensor))
                                        {
                                            if (port.cv_Data.GlassDataMap[slot_id].PProcessFlag == ProcessFlag.Need)
                                            {
                                                m_StartPos = now_step;
                                                m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get, ActionTarget.Port, port_id, slot_id, false);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rtn;
        }
        // only find PLastDevice is Eq , Can't find Aligner , buz the step next Aligner is mayby can't access.
        private bool DoFirstStepWhenNoEqUnloadExchange()
        {
            bool rtn = false;
            //first find abnormal wafer.
            foreach (int port_id in cv_InProcessPort)
            {
                Port port = GetPortById(port_id);
                if (port.PPortStatus == PortStaus.LDCM && port.PLotStatus == LotStatus.Process)
                {
                    for (int slot_id = 1; slot_id <= port.cv_SlotCount; slot_id++)
                    {
                        //now PLastDevice always is None , So if statement can't enter.
                        int device = (int)port.cv_Data.GlassDataMap[slot_id].PLastDevice;
                        if ((device >= ((int)AllDevice.SDP1)) &&
                            ((int)device <= (int)AllDevice.UV_2))
                        {
                            EqId eq = (EqId)Enum.Parse(typeof(EqId),
                                port.cv_Data.GlassDataMap[slot_id].PLastDevice.ToString());
                            if (GetEqById((int)eq).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                            {
                                int time_chart_instance = GetEqById((int)eq).cv_Comm.cv_TimeChatId;
                                if (eq == EqId.VAS)
                                {
                                    time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                }
                                EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                                if (gif_type == EqInterFaceType.Load)
                                {
                                    if (cv_RobotJobPath == null || cv_RobotJobPath.Count == 0)
                                    {
                                        cv_RobotJobPath = new Queue<RobotJob>();
                                        RobotJob tmp = new RobotJob(1, RobotArm.rabNone,
                                        GetEqById((int)eq).PPutArm, RobotAction.Get, ActionTarget.Port, port_id, slot_id, false);
                                        cv_RobotJobPath.Enqueue(tmp);
                                        tmp = new RobotJob(1, GetEqById((int)eq).PPutArm,
                                        RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq, 1, true);
                                        return true;
                                    }
                                }
                            }
                        }
                        else if (device == (int)AllDevice.Aligner)
                        {

                        }
                    }
                }
            }
            // if not wafer can take out.
            // run normal find first step method.
            //foreach (KeyValuePair<int, List<AllDevice>> pair in cv_CurRecipeFlowStepSetting)
            //{
            List<AllDevice> dievices = cv_CurRecipeFlowStepSetting[2];
            List<AllDevice> form_dievices = cv_CurRecipeFlowStepSetting[1];
            foreach (AllDevice device in dievices)
            {
                if ((int)device >= ((int)AllDevice.SDP1) &&
                    (int)device <= ((int)AllDevice.UV_2))
                {
                    if (!CheckFlowCanRun(1))
                    {
                        return false;
                    }
                    EqId eq = (EqId)Enum.Parse(typeof(EqId), device.ToString());
                    if (GetEqById((int)eq).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                    {
                        int time_chart_instance = GetEqById((int)eq).cv_Comm.cv_TimeChatId;
                        if (eq == EqId.VAS)
                        {
                            time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                        }
                        EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                        if (gif_type == EqInterFaceType.Load)
                        {
                            if (cv_RobotJobPath == null || cv_RobotJobPath.Count == 0)
                            {
                                for (int i = 0; i < form_dievices.Count; i++)
                                {
                                    if (form_dievices[i] == AllDevice.Buffer)
                                    {

                                        Buffer bf = GetBufferById(1);
                                        int slot = 0;
                                        if (bf.cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out slot))
                                        {
                                            cv_RobotJobPath = new Queue<RobotJob>();
                                            RobotJob tmp = new RobotJob(1, RobotArm.rabNone,
                                            GetEqById((int)eq).PPutArm, RobotAction.Get, ActionTarget.Buffer, 1, slot, false);
                                            cv_RobotJobPath.Enqueue(tmp);
                                            tmp = new RobotJob(1, GetEqById((int)eq).PPutArm,
                                            RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq, 1, true);
                                            cv_RobotJobPath.Enqueue(tmp);
                                            return true;
                                        }
                                    }
                                    else if (form_dievices[i] == AllDevice.LP)
                                    {
                                        foreach (int port_id in cv_InProcessPort)
                                        {
                                            Port port = GetPortById(port_id);
                                            if (port.PPortStatus == PortStaus.LDCM && port.PLotStatus == LotStatus.Process)
                                            {
                                                if (port.cv_Data.PProductionType == ProductCategory.Wafer)
                                                {
                                                    int slot = 0;
                                                    if (FindHightestSlotForPPID(port.cv_Data.PCurPPID, port_id, out slot))
                                                    {
                                                        if (port.cv_Data.GlassDataMap[slot].PHasSensor && port.cv_Data.GlassDataMap[slot].PHasData)
                                                        {
                                                            cv_RobotJobPath = new Queue<RobotJob>();
                                                            RobotJob tmp = new RobotJob(1, RobotArm.rabNone,
                                                            GetEqById((int)eq).PPutArm, RobotAction.Get, ActionTarget.Port, port_id, slot, false);
                                                            cv_RobotJobPath.Enqueue(tmp);
                                                            tmp = new RobotJob(1, GetEqById((int)eq).PPutArm,
                                                            RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq, 1, true);
                                                            cv_RobotJobPath.Enqueue(tmp);
                                                            return true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        port.cv_Data.PCurPPID = FindHightestPriorityPPID(port_id);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}
            return rtn;
        }
        // Hard code ,  Aligner low put , up get 
        private bool GetPreActionJob(ProductCategory m_Type, int m_BufferSlot)
        {
            //if want to take out glass , need consider flow. Only 1-1 & 1-2 can take glass , buz other flow not run VAS.
            if (m_Type == ProductCategory.Glass)
            {
                RecipeItem recipe = null;
                if (cv_Recipes.GetCurRecipe(out recipe))
                {
                    if (recipe.PFlow != OdfFlow.Flow1_1 && recipe.PFlow != OdfFlow.Flow1_2)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            bool rtn = false;
            for (int port_id = 0; port_id < cv_InProcessPort.Count; port_id++)
            {
                Port port = LgcForm.GetPortById(cv_InProcessPort[port_id]);
                if (port.PLotStatus == LotStatus.Process && port.PPortStatus == PortStaus.LDCM)
                {
                    if (port.cv_Data.PProductionType == m_Type)
                    {
                        //for (int i = 1; i <= (int)port.cv_Data.cv_SlotCount; i++)
                        int slot = 0;
                        if (FindHightestSlotForPPID(port.cv_Data.PCurPPID, port.cv_Id, out slot))
                        {
                            if (!port.cv_Data.HasDataAndSensor(slot)) continue;
                            if (port.cv_Data.GlassDataMap[slot].PProcessFlag == ProcessFlag.Need && port.cv_Data.GlassDataMap[slot].PLastDevice == AllDevice.None)
                            {
                                if (port.cv_Data.GlassDataMap[slot].POcrResult == OCRResult.None)
                                {
                                    RobotJob tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Port, port.cv_Id, slot, false);
                                    cv_RobotJobPath.Enqueue(tmp);
                                    tmp = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Aligner, 1, 1, false);
                                    cv_RobotJobPath.Enqueue(tmp);
                                    //TODO if first Eqp at load status.
                                    //direct put to Eqp , not need put to buffer.
                                    //***Start
                                    bool is_direct_put_to_eq = false;
                                    if (port.cv_Data.PProductionType == ProductCategory.Wafer)
                                    {
                                        List<AllDevice> dievices = cv_CurRecipeFlowStepSetting[2];
                                        foreach (AllDevice device in dievices)
                                        {
                                            if ((int)device >= ((int)AllDevice.SDP1) &&
                                                (int)device <= ((int)AllDevice.UV_2))
                                            {
                                                if (!CheckFlowCanRun(1))
                                                {
                                                    break;
                                                }
                                                EqId eq = (EqId)Enum.Parse(typeof(EqId), device.ToString());
                                                if (GetEqById((int)eq).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                                                {
                                                    int time_chart_instance = GetEqById((int)eq).cv_Comm.cv_TimeChatId;
                                                    if (eq == EqId.VAS)
                                                    {
                                                        time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                                    }
                                                    EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                                                    if (gif_type == EqInterFaceType.Load)
                                                    {
                                                        int buffer_slot = 0;
                                                        //if buffer hasn't other wafer , can direct put to EQ.
                                                        if (!GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out buffer_slot))
                                                        {
                                                            tmp = new RobotJob(1, RobotArm.rabNone, GetEqById((int)eq).PPutArm, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
                                                            cv_RobotJobPath.Enqueue(tmp);
                                                            tmp = new RobotJob(1, GetEqById((int)eq).PPutArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq, 1, true);
                                                            cv_RobotJobPath.Enqueue(tmp);
                                                            is_direct_put_to_eq = true;
                                                            rtn = true;
                                                            break;
                                                        }
                                                    }
                                                    else if (gif_type == EqInterFaceType.Exchange)
                                                    {
                                                        cv_RobotJobPath.Clear();
                                                        CalculateRobotJobPath();
                                                        if (cv_RobotJobPath.Count > 0)
                                                        {
                                                            Queue<RobotJob> tmp_q = new Queue<RobotJob>();
                                                            RobotJob job = cv_RobotJobPath.Peek();
                                                            if (job.PTarget == ActionTarget.Eq && job.PAction == RobotAction.Get && job.PTargetId == (int)eq)
                                                            {
                                                                tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Port, port.cv_Id, slot, false);
                                                                tmp_q.Enqueue(tmp);
                                                                tmp = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Aligner, 1, 1, false);
                                                                tmp_q.Enqueue(tmp);
                                                                job.PAction = RobotAction.Exchange;
                                                                if (job.PGetArm == RobotArm.rbaDown)
                                                                {
                                                                    tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaUp, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
                                                                    tmp_q.Enqueue(tmp);
                                                                    job.PPutArm = RobotArm.rbaUp;
                                                                }
                                                                else if (job.PGetArm == RobotArm.rbaUp)
                                                                {
                                                                    tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
                                                                    tmp_q.Enqueue(tmp);
                                                                    job.PPutArm = RobotArm.rbaDown;
                                                                }
                                                                else
                                                                {
                                                                    continue;
                                                                }
                                                                while (cv_RobotJobPath.Count > 0)
                                                                {
                                                                    tmp_q.Enqueue(cv_RobotJobPath.Dequeue());
                                                                }
                                                                cv_RobotJobPath.Clear();
                                                                cv_RobotJobPath = tmp_q;
                                                                is_direct_put_to_eq = true;
                                                                rtn = true;
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Port, port.cv_Id, slot, false);
                                                            cv_RobotJobPath.Enqueue(tmp);
                                                            tmp = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Aligner, 1, 1, false);
                                                            cv_RobotJobPath.Enqueue(tmp);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (!is_direct_put_to_eq)
                                        {
                                            AddNormalGetForAlignerAndPutToBufferJobToQ(m_BufferSlot);
                                        }
                                        rtn = true;
                                        break;
                                    }
                                    else // if type is glass port. Direct put to vas glass slot if vas at load status.
                                    {
                                        int time_chart_instance = 0;
                                        time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                                        EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                                        Buffer buffer = GetBufferById(1);
                                        Robot robot = GetRobotById(1);
                                        //check UV1 or UV2 can run. Fab Environmnen maybe hasn't UV2.
                                        if (GetEqById((int)EqId.UV_1).GetStatus() != EquipmentStatus.Idle && GetEqById((int)EqId.UV_1).GetStatus() != EquipmentStatus.Run)
                                        {
                                            AddNormalGetForAlignerAndPutToBufferJobToQ(m_BufferSlot);
                                            rtn = true;
                                            break;
                                        }
                                        else
                                        {
                                            if (GetEqById((int)EqId.VAS).GetTimeChatCurStep(2) == TimechartNormal.STEP_ID_ActionReady)
                                            {
                                                if (gif_type == EqInterFaceType.Load)
                                                {
                                                    int buffer_slot = 0;
                                                    //if buffer hasn't other wafer , can direct put to EQ.
                                                    if (!GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Glass, out buffer_slot))
                                                    {
                                                        tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
                                                        cv_RobotJobPath.Enqueue(tmp);
                                                        tmp = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)EqId.VAS, 2, true);
                                                        cv_RobotJobPath.Enqueue(tmp);
                                                        is_direct_put_to_eq = true;
                                                        rtn = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!is_direct_put_to_eq)
                                            {
                                                AddNormalGetForAlignerAndPutToBufferJobToQ(m_BufferSlot);
                                                rtn = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            port.cv_Data.PCurPPID = FindHightestPriorityPPID(port.cv_Id);
                        }

                        if (rtn)
                        {
                            break;
                        }
                    }
                }
            }
            return rtn;
        }
        private void AddNormalGetForAlignerAndPutToBufferJobToQ(int m_BufferSlot)
        {
            RobotJob tmp;
            tmp = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaUp, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
            cv_RobotJobPath.Enqueue(tmp);
            tmp = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, RobotAction.Put, ActionTarget.Buffer, 1, m_BufferSlot, false);
            cv_RobotJobPath.Enqueue(tmp);
        }
        private void CalculatePreActionJob()
        {
            if (LgcForm.GetRobotById(1).IsHasAnyDataAndSensor()) return;
            if ((LgcForm.PSystemData.POperationMode != OperationMode.Auto) || PSystemData.PSystemStatus == EquipmentStatus.Down) return;
            if (LgcForm.GetRobotById(1).IsBusy || cv_RobotJobPath.Count != 0) return;
            // check VAS is at unload status.
            int time_chart_instance_down = 0;
            time_chart_instance_down = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
            EqInterFaceType gif_type_down = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance_down).cv_ActionType;
            int time_chart_instance_up = 0;
            time_chart_instance_up = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
            EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance_up).cv_ActionType;
            int time_chart_instance_ijp = 0;
            time_chart_instance_ijp = (int)EqGifTimeChartId.TIMECHART_ID_IJP;
            EqInterFaceType gif_type_ijp = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance_ijp).cv_ActionType;
            if ((gif_type_down == EqInterFaceType.Unload) ||
                (gif_type_down == EqInterFaceType.Load && (gif_type_ijp == EqInterFaceType.Unload || gif_type_ijp == EqInterFaceType.Exchange)))
            {
                return;
            }
            //
            if (!LgcForm.GetAlignerById(1).cv_Data.GlassDataMap[1].PHasData && !LgcForm.GetAlignerById(1).cv_Data.GlassDataMap[1].PHasSensor)
            {
                int glass_slot = LgcForm.GetBufferById(1).cv_Data.IsFreeSlot(BufferSlotType.Glass);
                int wafer_slot = LgcForm.GetBufferById(1).cv_Data.IsFreeSlot(BufferSlotType.Wafer);
                RecipeItem cur_recipe = null;
                if (!cv_Recipes.GetCurRecipe(out cur_recipe)) return;
                if (!cur_recipe.PVasNeedGlass)
                {
                    if (wafer_slot != -1)
                        GetPreActionJob(ProductCategory.Wafer, wafer_slot);
                }
                else
                {
                    if ((glass_slot != -1) && (wafer_slot != -1))
                    {
                        ProductCategory type = GetSubstractTypeWantToGetFromCst();
                        if (type == ProductCategory.Glass)
                        {
                            if (!hasGlassPort())
                            {
                                if (wafer_slot != -1)
                                    GetPreActionJob(ProductCategory.Wafer, wafer_slot);
                            }
                            else
                            {
                                if (glass_slot != -1)
                                {
                                    if (!GetPreActionJob(ProductCategory.Glass, glass_slot))
                                    {
                                        GetPreActionJob(ProductCategory.Wafer, wafer_slot);
                                    }
                                }
                            }
                        }
                        else if (type == ProductCategory.Wafer)
                        {
                            if (!hasWaferPort())
                            {
                                if (glass_slot != -1)
                                    GetPreActionJob(ProductCategory.Glass, glass_slot);
                            }
                            else
                            {
                                if (wafer_slot != -1)
                                {
                                    if (!GetPreActionJob(ProductCategory.Wafer, wafer_slot))
                                    {
                                        GetPreActionJob(ProductCategory.Glass, glass_slot);
                                    }
                                }
                            }
                        }
                    }
                    else if (glass_slot != -1)
                    {
                        GetPreActionJob(ProductCategory.Glass, glass_slot);
                    }
                    else if (wafer_slot != -1)
                    {
                        GetPreActionJob(ProductCategory.Wafer, wafer_slot);
                    }
                }
            }
        }
        private bool CheckFlowCanRun(int m_Step)
        {
            //return true;
            bool rtn = true;
            return rtn;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            for (int next_step = m_Step + 1; next_step <= max_step; next_step++)
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[next_step];
                for (int deviceInide = 0; deviceInide <= cv_stepDevice.Count - 1; deviceInide++)
                {
                    EqId eq_id = EqId.None;
                    if (cv_stepDevice[deviceInide] == AllDevice.Aligner || cv_stepDevice[deviceInide] == AllDevice.Buffer || cv_stepDevice[deviceInide] == AllDevice.UP ||
                        cv_stepDevice[deviceInide] == AllDevice.LP)
                    {
                        return true;
                    }
                    if (Enum.TryParse<EqId>(cv_stepDevice[deviceInide].ToString(), out eq_id))
                    {
                        EquipmentStatus tmp = GetEqById((int)eq_id).GetStatus();
                        if (tmp != EquipmentStatus.Idle && tmp != EquipmentStatus.Run)
                        {
                            if (deviceInide == cv_stepDevice.Count - 1)
                            {
                                rtn = false;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (!rtn)
                {
                    break;
                }
            }
            return rtn;
        }
        #endregion
        private void OnRobotActionTimer()
        {
            DerivedTimer();
            CalculateSystemStatus();
            if ((PSystemData.POperationMode == OperationMode.Auto && !cv_Alarms.IsHasAlarm() &&
                PSystemData.PSystemStatus != EquipmentStatus.Down) || cv_IsPreView)
            {
                WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
                Robot robot = GetRobotById(1);
                SetRobotSensorToEq();
                SendRobotJobPath();
                if (cv_RobotJobPath.Count > 1)
                {
                    if (!CheckAutoJobPathOk(cv_RobotJobPath.Count - 1))
                    {
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.RobotJobPathError.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "Robot Job Path Error";
                        alarm.PSubDescription = "Please check auto job path and robot arm.";
                        alarm.PStatus = AlarmStatus.Occur;
                        alarm.PUnit = 0;
                        LgcForm.EditAlarm(alarm);
                        return;
                    }
                }
                if (!cv_IsPreView)
                    DoCstUnload();
                // CheckIsVasPutUpSlotJobStatus : robot already out from VAS , direct set complete signam to VAS.
                //Then the VAS can close chamber.
                //buz job.count >0 , so program direct process "DoRobotJobPath";
                if (!robot.IsBusy || CheckIsVasPutUpSlotJobStatus(cv_RobotJobPath.ElementAt(0)))
                {
#if VASMODIFY
                    // not job but robotArm ( put uv arm ) and already enter VAS.
                    if ((cv_RobotJobPath == null || cv_RobotJobPath.Count == 0))
                    {
                        if (!PSystemData.PONT)
                        {
                            if (robot.cv_Data.TheSlotHasDataOrSensor((int)((GetEqById((int)EqId.UV_1).PPutArm))))
                            {
                                GlassData data = robot.cv_Data.GlassDataMap[(int)((GetEqById((int)EqId.UV_1).PPutArm))];
                                int Uvnode = GetEqById((int)EqId.UV_1).PNode;
                                int Vasnode = GetEqById((int)EqId.VAS).PNode;
                                if (0 != data.cv_Nods[Vasnode - 1].PProcessHistory && data.cv_Nods[Uvnode - 1].PProcessHistory == 0)
                                {
                                    //calculate The Arm Data Wait UV or not.
                                    RobotJob job = new RobotJob(1, GetEqById((int)EqId.UV_1).PPutArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq,
                                        (int)EqId.UV_1, 1, false);
                                    cv_RobotJobPath.Enqueue(job);
                                    cv_WaitUvRecordTime = SysUtils.Now();
                                }
                            }
                        }
                        if(cv_RobotJobPath.Count != 0)
                        {
                            WriteLog(LogLevelType.Detail, "Get job from CalculateVasGlass");
                        }
                    }
#endif
                    if ((cv_RobotJobPath == null || cv_RobotJobPath.Count == 0))
                    {
                        if (!PSystemData.PONT)
                        {
                            if (!robot.IsHasAnyDataAndSensor())
                            {
                                CalculateRobotJobPath();
                            }
                        }
                        if(cv_RobotJobPath.Count != 0)
                        {
                            WriteLog(LogLevelType.Detail, "Get job from CalculateRobotJobPath");
                        }
                    }
                    if ((cv_RobotJobPath == null || cv_RobotJobPath.Count == 0))
                    {
                        if (!PSystemData.PONT)
                        {
                            if (!robot.IsHasAnyDataAndSensor())
                            {
                                DoFirstStepWhenNoEqUnloadExchange();
                            }
                        }
                        if(cv_RobotJobPath.Count != 0)
                        {
                            WriteLog(LogLevelType.Detail, "Get job from DoFirstStepWhenNoEqUnloadExchange");
                        }
                    }
                    if ((cv_RobotJobPath == null || cv_RobotJobPath.Count == 0))
                    {
                        if (!PSystemData.PONT)
                        {
                            if (!robot.IsHasAnyDataAndSensor())
                            {
                                CalculateVasGlass();
                            }
                        }
                        if(cv_RobotJobPath.Count != 0)
                        {
                            WriteLog(LogLevelType.Detail, "Get job from CalculateVasGlass");
                        }
                    }
                    if ((cv_RobotJobPath == null || cv_RobotJobPath.Count == 0))
                    {
                        RecipeItem recipe = null;
                        if (!robot.IsHasAnyDataAndSensor())
                        {
                            if (cv_Recipes.GetCurRecipe(out recipe))
                            {
                                if (recipe.PFlow != OdfFlow.Flow3)
                                {
                                    CalculatePreActionJob();
                                }
                            }
                        }
                        if(cv_RobotJobPath.Count != 0)
                        {
                            WriteLog(LogLevelType.Detail, "Get job from CalculatePreActionJob");
                        }
                    }
                    if (cv_RobotJobPath.Count > 0)
                    {
                        if (!cv_IsPreView)
                            DoRobotJobPath();
                    }
                }
                if (cv_IsPreView)
                {
                    SendRobotJobPath();
                    cv_IsPreView = false;
                }
            }
            if (PSystemData.POperationMode == OperationMode.Manual &&
                PSystemData.PSystemStatus != EquipmentStatus.Down)
            {
                Robot robot = GetRobotById(1);
                SetRobotSensorToEq();
                SendRobotJobPath();
                if (cv_RobotManaulJobPath.Count != 0)
                {
                    if (!robot.IsBusy || CheckIsVasPutUpSlotJobStatus(cv_RobotManaulJobPath.ElementAt(0)))
                    {
                        DoRobotJobPath(true);
                    }
                }
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        #region Do Robot Action for each Eqp.
        private void ProcessPortGetPutJob(RobotAction m_Type)
        {
            RobotJob job = cv_RobotJobPath.Peek();
            Robot robot = GetRobotById(1);
            Port port = GetPortById(job.PTargetId);
            if (m_Type == RobotAction.Get)
            {
                if (port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                {
                    Aligner aligner = GetAlignerById(1);
                    aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
                    robot.cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                    GetPutPort(job.PGetArm, job.PTargetId, job.PTargetSlot, true);
                }
                else
                {
                    if (!port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationMode = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            else if (m_Type == RobotAction.Put)
            {
                if (!port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    !port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                {
                    GetPutPort(job.PPutArm, job.PTargetId, job.PTargetSlot, false);
                }
                else
                {
                    if (port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (PSystemData.PSystemOnlineMode != OnlineMode.Control)
                        {
                            if (PSystemData.POperationMode != OperationMode.Manual)
                            {
                                if (port.cv_Data.PPortMode == PortMode.Unloader)
                                {
                                    if (!port.cv_Data.HasOtherJobHaveToDo())
                                    {
                                        port.cv_Data.PWaitUnload = true;
                                    }
                                }
                            }
                        }
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationMode = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
        }
        private void ProcessBufferGetPutJob(RobotAction m_Type)
        {
            RobotJob job = cv_RobotJobPath.Peek();
            Robot robot = GetRobotById(1);
            Buffer buffer = GetBufferById(1);
            if (m_Type == RobotAction.Get)
            {
                if (buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                {

                    GetPutBuffer(job.PGetArm, job.PTargetId, job.PTargetSlot, true);
                }
                else
                {
                    if (!buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationMode = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            else if (m_Type == RobotAction.Put)
            {
                if (!buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    !buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                {
                    GetPutBuffer(job.PPutArm, 1, job.PTargetSlot, false);
                }
                else
                {
                    if (buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationMode = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
        }
        private void ProcessAlignerGetPutJob(RobotAction m_Type, bool m_IsMaunal = false)
        {
            RobotJob job = null;
            if (!m_IsMaunal)
                job = cv_RobotJobPath.Peek();
            else
                job = cv_RobotManaulJobPath.Peek();

            Aligner aligner = GetAlignerById(job.PTargetId);
            Robot robot = GetRobotById(1);
            if (job.PAction == RobotAction.Get ||
                (job.PAction == RobotAction.Exchange && job.PTarget == ActionTarget.Aligner &&
                job.PIsWaitGet && !job.PisWaitPut))
            {
                if (aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                {
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOn)
                    {
                        robot.cv_Comm.SetAlignerVaccum(true);
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitVuccumOn;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.FindNotch)
                    {
                        robot.cv_Comm.SetAlignerFindNotch();
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitFindNotch;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.OcrConnect)
                    {
                        if (job.PAction != RobotAction.Exchange)
                        {
                            if (aligner.cv_Data.GlassDataMap[1].PProductionCategory == ProductCategory.Wafer)
                            {
                                robot.cv_Comm.SetOcrConnect();
                                aligner.cv_Data.PPreAction = AlignerPreAction.WaitConnect;
                            }
                            else
                            {
                                aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.OK;
                                aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                            }
                        }
                        else
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                        }
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.ReadOcr)
                    {
                        if (PSystemData.POcrMode == OCRMode.SkipRead)
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                        }
                        else
                        {
                            robot.cv_Comm.SetOcrRead();
                            aligner.cv_Data.PPreAction = AlignerPreAction.WaitReadOct;
                        }
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.ToAngle)
                    {
                        robot.cv_Comm.SetAlignerToAngle();
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitToAngle;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff2)
                    {
                        robot.cv_Comm.SetAlignerVaccum(false);
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitVuccomOff2;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.GetAligner)
                    {
                        //job.PIsWaitGet = false;
                        if (PSystemData.POperationMode == OperationMode.Auto)
                        {
                            if (job.PAction == RobotAction.Exchange)
                            {
                                GetPutAligner(job.PGetArm, true);
                                aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                return;
                            }
                            else if (aligner.cv_Data.GlassDataMap[1].POcrResult == OCRResult.Mismatch)
                            {
                                GlassData glass_tmp = aligner.cv_Data.GlassDataMap[1];
                                if (PSystemData.POcrMode == OCRMode.ErrorReturn)
                                {
                                    cv_RobotJobPath.Clear();
                                    cv_RobotJobPath.Enqueue(job);
                                    if (GetPortById((int)glass_tmp.PSourcePort).PPortStatus == PortStaus.LDCM)
                                    {
                                        //if (!GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasData &&
                                        //    !GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasSensor)
                                        //{
                                        Port port = GetPortById((int)glass_tmp.PSourcePort);
                                        int slot = 0;
                                        if (port.cv_Data.WhichSlotCanLoad(out slot))
                                        {
                                            RobotJob return_job = new RobotJob(1, job.PGetArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, (int)glass_tmp.PSourcePort, slot, false);
                                            cv_RobotJobPath.Enqueue(return_job);
                                            GetPutAligner(job.PGetArm, true);
                                            aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                            WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " slot : " +
                                                 slot + " at OCR Error return mode");
                                        }
                                        else
                                        {
                                            WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " can't find slot to put.");
                                        }
                                        //GetPutAligner(job.PGetArm, true);
                                        //aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                        //}
                                    }
                                }
                                else if (PSystemData.POcrMode == OCRMode.ErrorHold)
                                {
                                    if (glass_tmp.POcrDecide == OCRMode.ErrorReturn)
                                    {
                                        cv_RobotJobPath.Clear();
                                        cv_RobotJobPath.Enqueue(job);
                                        if (GetPortById((int)glass_tmp.PSourcePort).PPortStatus == PortStaus.LDCM)
                                        {
                                            //if (!GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasData &&
                                            //    !GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasSensor)
                                            //{
                                            Port port = GetPortById((int)glass_tmp.PSourcePort);
                                            int slot = 0;
                                            if (port.cv_Data.WhichSlotCanLoad(out slot))
                                            {
                                                RobotJob return_job = new RobotJob(1, job.PGetArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, (int)glass_tmp.PSourcePort, slot, false);
                                                cv_RobotJobPath.Enqueue(return_job);
                                                GetPutAligner(job.PGetArm, true);
                                                aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                                WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " slot : " +
                                                     slot + " at OCR Error hold and User press retun mode button");
                                            }
                                            else
                                            {
                                                WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " can't find slot to put.");
                                            }
                                            //}
                                        }
                                    }
                                    else if (glass_tmp.POcrDecide == OCRMode.ErrorSkip || glass_tmp.POcrDecide == OCRMode.SkipRead)
                                    {
                                        if (glass_tmp.POcrDecide == OCRMode.SkipRead)
                                        {
                                            CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                                            report_bc.PGlass = aligner.cv_Data.GlassDataMap[1];
                                            LgcForm.cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                                        }
                                        GetPutAligner(job.PGetArm, true);
                                        aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                    }
                                }
                                else
                                {
                                    GetPutAligner(job.PGetArm, true);
                                    aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                }
                            }
                            else
                            {
                                GetPutAligner(job.PGetArm, true);
                                aligner.cv_Data.PPreAction = AlignerPreAction.None;
                            }
                        }
                        else
                        {
                            GetPutAligner(job.PGetArm, true);
                            aligner.cv_Data.PPreAction = AlignerPreAction.None;
                        }
                    }
                }
                else
                {
                    if (!aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                    {
                        if (!m_IsMaunal)
                        {
                            cv_RobotJobPath.Dequeue();
                            if (cv_IsCycleStop)
                            {
                                PSystemData.POperationMode = OperationMode.Manual;
                                cv_IsCycleStop = false;
                            }
                        }
                        else
                            cv_RobotManaulJobPath.Dequeue();

                        job.PIsWaitGet = false;
                    }
                }
            }
            else if (job.PAction == RobotAction.Put || (
                (job.PAction == RobotAction.Exchange) &&
                (job.PTarget == ActionTarget.Aligner) && (job.PisWaitPut) && (job.PIsWaitGet))
                )
            {
                if (!aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    !aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                {
                    GlassData glass = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                    if (glass.POcrDecide != OCRMode.None)
                    {
                        glass.POcrDecide = OCRMode.None;
                    }
                    RecipeItem recipe = null;
                    if (!cv_Recipes.GetCurRecipe(out recipe))
                    {
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.NotSetCurRecipe.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "Not Set Cur Recipe , please check cur recipe.";
                        alarm.PStatus = AlarmStatus.Occur;
                        alarm.PUnit = 0;
                        LgcForm.EditAlarm(alarm);
                        PSystemData.POperationMode = OperationMode.Manual;
                        return;
                    }
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.None)
                    {
                        //aligner.cv_Data.PPreAction = AlignerPreAction.AlignerHome;
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
                        robot.cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.SetToAngle)
                    {
                        if (job.PAction == RobotAction.Exchange)
                        {
                            if (glass.PProductionCategory == ProductCategory.Wafer)
                            {
                                //if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                //{
                                if(!job.PManualExchangeForAligner)
                                    robot.cv_Comm.SetAlignerDegree(recipe.PWaferVASDegree);
                                else
                                    robot.cv_Comm.SetAlignerDegree((float) Convert.ToDouble(job.PManualExchangeForAlignerDeg));
                                aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff1;
                                //}
                            }
                            else if (glass.PProductionCategory == ProductCategory.Glass)
                            {
                                //if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                {
                                    if (!job.PManualExchangeForAligner)
                                        robot.cv_Comm.SetAlignerDegree(recipe.PWaferVASDegree);
                                    else
                                        robot.cv_Comm.SetAlignerDegree((float)Convert.ToDouble(job.PManualExchangeForAlignerDeg));

                                    aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff1;
                                }
                            }
                        }
                        else
                        {
                            if (glass.PProductionCategory == ProductCategory.Wafer)
                            {
                                if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                {
                                    robot.cv_Comm.SetAlignerDegree(recipe.PWaferIJPDegree);
                                }
                            }
                            else if (glass.PProductionCategory == ProductCategory.Glass)
                            {
                                if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                {
                                    robot.cv_Comm.SetAlignerDegree(recipe.PWaferIJPDegree);
                                }
                                else if (glass.PPortProductionCategory == ProductCategory.Glass)
                                {
                                    robot.cv_Comm.SetAlignerDegree(recipe.PGlassVASDegree);
                                }
                            }
                            aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff1;
                        }
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.PutAligner)
                    {
                        job.PisWaitPut = false;
                        GetPutAligner(job.PPutArm, false);
                        aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOn;
                    }
                }
                else
                {
                    if (!robot.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        aligner.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                        aligner.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationMode = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
        }
        private void SetRobotSensorToEq()
        {
            Robot robot = GetRobotById(1);
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;

            for (int eq_index = 1; eq_index <= (int)EqId.UV_1; eq_index++)
            {
                EqId eq_id = (EqId)eq_index;
                if (eq_id == EqId.VAS)
                {
                    for (int slot = 1; slot <= 2; slot++)
                    {
                        if (slot == 1)
                        {
                            time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                            time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                            SetRobotSensorToEq(RobotArm.rbaUp, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Upper_Arm);
                            SetRobotSensorToEq(RobotArm.rbaDown, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Low_Arm);
                        }
                        else if (slot == 2)
                        {
                            time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                            time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                            SetRobotSensorToEq(RobotArm.rbaUp, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Upper_Arm);
                            SetRobotSensorToEq(RobotArm.rbaDown, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Low_Arm);
                        }
                    }
                }
                else
                {
                    time_chart_id = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    SetRobotSensorToEq(RobotArm.rbaUp, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Upper_Arm);
                    SetRobotSensorToEq(RobotArm.rbaDown, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Low_Arm);
                }
            }
        }
        private void SetRobotSensorToEq(RobotArm m_Arm, int m_PortAddress)
        {
            Robot robot = GetRobotById(1);
            bool up_sensor = robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor;
            bool down_sensor = robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor;
            if (m_Arm == RobotArm.rbaUp)
            {
                cv_Mio.SetPortValue(m_PortAddress, up_sensor ? 1 : 0);
                WriteLog(LogLevelType.TimerFunction, "Set GIF sensor for Up arm" + (up_sensor ? "On" : "off"), FunInOut.None);
            }
            else if (m_Arm == RobotArm.rbaDown)
            {
                cv_Mio.SetPortValue(m_PortAddress, down_sensor ? 1 : 0);
                WriteLog(LogLevelType.TimerFunction, "Set GIF sensor for down arm" + (down_sensor ? "On" : "off"), FunInOut.None);
            }
        }
        private void ProcessEqGetPutJob(RobotAction m_Type, bool m_IsMaunal = false)
        {
            RobotJob job = null;
            Robot robot = GetRobotById(1);
            if (!m_IsMaunal)
            {
                job = cv_RobotJobPath.Peek();
                if (cv_RobotJobPath.Count >= 2)
                {
                    if( (cv_RobotJobPath.ElementAt(1).PTarget == ActionTarget.Aligner ) && (job.PTargetId != (int)EqId.IJP) )
                    {
                        Aligner aligner = GetAlignerById(1);
                        if (aligner.cv_Data.PPreAction != AlignerPreAction.WaitHome && aligner.cv_Data.PPreAction != AlignerPreAction.SetToAngle)
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
                            robot.cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                        }
                    }
                }
            }
            else
                job = cv_RobotManaulJobPath.Peek();
            EqId eq_id = (EqId)(int)job.PTargetId;
            int slot = job.PTargetSlot;
            int eq_time_chart_cur_step = 0;
            EqInterFaceType gif_type = EqInterFaceType.None;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;

            if (eq_id == EqId.VAS)
            {
                if (slot == 1)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                }
                else if (slot == 2)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                }
            }
            else
            {
                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                time_chart_id = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
            }
            if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
            {
                gif_type = time_chart_instance.cv_ActionType;
            }

            if (m_Type == RobotAction.Get)// && (gif_type == EqInterFaceType.Unload || gif_type == EqInterFaceType.Exchange))
            {
                bool robot_get_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor;
                bool robot_get_arm_data = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData;
                GlassData glass = null;
                if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                {
                    if (cv_IsCycleStop)
                    {
                        PSystemData.POperationMode = OperationMode.Manual;
                        cv_IsCycleStop = false;
                    }
                    if (gif_type != EqInterFaceType.Unload && gif_type != EqInterFaceType.Exchange)
                    {
                        return;
                    }
                    if (!robot_get_arm_data && !robot_get_arm_sensor)
                    {
                        glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                        string glass_id = glass.PId;
                        string combination = glass.PAssamblyResult.ToString();
                        for (int i = 1; i <= 15; i++)
                        {
                            int node_index = glass.cv_Nods.FindIndex(x => x.PNodeId == i);
                            if (node_index != -1)
                            {
                                int history = glass.cv_Nods[node_index].cv_ProcessHistory;
                                int recipe = glass.cv_Nods[node_index].cv_Recipe;
                            }
                        }
                        //tmp mark for uv no data case.
                        if (glass.PHasData)
                        {
                            if (!CheckEqSideData(glass, eq_id))
                            {
                                return;
                            }
                            time_chart_instance.SetTrigger(time_chart_id);
                            time_chart_instance.cv_ActionType = EqInterFaceType.Unload;
                            cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Unload_Only_Reply, 1);
                            time_chart_instance.cv_GetData = glass;
                            time_chart_instance.cv_GetArm = job.PGetArm;
                            time_chart_instance.cv_Action = EqInterFaceType.Unload;
                            if (job.PTarget == ActionTarget.Eq && job.PTargetId == (int)EqId.VAS)
                            {
                                if(job.PTargetSlot == 1)
                                {
                                    GetVasStandby();
                                }
                            }
                            else
                            {
                                if ((job.PTarget == ActionTarget.Eq) && (job.PTargetId != (int)EqId.VAS))
                                {
                                    if (cv_GetPutStandbyExceptVas)
                                    {
                                        GetEqStandbyExceptVas(job.PTargetId, job.PTargetSlot, job.PGetArm);
                                    }
                                }
                            }
                        }

                        /*
                        else
                        {
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = CommonData.HIRATA.Alarmtable.UpStreamDataError.ToString();
                            alarm.PLevel = AlarmLevele.Light;
                            alarm.PMainDescription = "Up Stream Data Error";
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PUnit = 0;
                            LgcForm.EditAlarm(alarm);
                        }
                        */
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetStart)
                {
                    if (!robot_get_arm_data && !robot_get_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        bool eq_start = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 1);
                        if (eq_ready && eq_start)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (eq_id != EqId.VAS)
                            {
                                GetPutNormalEq(job.PGetArm, eq_id, 1, true, true);
                            }
                            else
                            {
                                if (job.PGetArm == RobotArm.rbaDown)
                                {
                                    GetVas(2, false);
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetEnd)
                {
                    if (robot_get_arm_sensor && !robot.IsBusy)
                    {
                        glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = glass;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_get_arm_sensor;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                        time_chart_instance.SetTrigger(time_chart_id);
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetVasStandByStart)
                {
                    if (!robot_get_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        if (eq_ready)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (eq_id == EqId.VAS)
                            {
                                if (job.PGetArm == RobotArm.rbaDown)
                                {
                                    GetVas(1, false);
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetVasStandByEnd)
                {
                    if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Stage_Delivery_Ready +
                        time_chart_instance.cv_EqBitStart) == 1)
                    {
                        time_chart_instance.SetTrigger(time_chart_id);
                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Robot_Delivery_Ready +
                            time_chart_instance.cv_RobotBitStart, 0);
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotCommandFinish)
                {
                    if (robot_get_arm_sensor && !robot.IsBusy)
                    {
                        time_chart_instance.SetTrigger(time_chart_id);
                        cv_MmfController.SendBcTreansferReport(DataFlowAction.Receive, robot.cv_Data.GlassDataMap[(int)job.PGetArm]);
                        //SendBcTreansferReport()
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitEqCompleteOn)
                {
                    if (robot_get_arm_sensor && !robot.IsBusy)
                    {
                        if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Transfer_Complete +
                            time_chart_instance.cv_EqBitStart) == 1)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (!m_IsMaunal)
                            {
                                if (cv_IsCycleStop)
                                {
                                    PSystemData.POperationMode = OperationMode.Manual;
                                    cv_IsCycleStop = false;
                                }
                                cv_RobotJobPath.Dequeue();
                            }
                            else
                                cv_RobotManaulJobPath.Dequeue();
                        }
                    }
                }
            }
            else if (m_Type == RobotAction.Put)
            {
                bool robot_put_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
                bool robot_put_arm_data = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData;
                if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                {
                    if (cv_IsCycleStop)
                    {
                        PSystemData.POperationMode = OperationMode.Manual;
                        cv_IsCycleStop = false;
                    }
                    if (gif_type != EqInterFaceType.Load)// && gif_type != EqInterFaceType.Exchange)
                    {
                        return;
                    }
                    if (robot_put_arm_data && robot_put_arm_sensor)
                    {
                        //
                        int node_index = robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods.FindIndex(x => x.PNodeId == 2);
                        if (node_index != -1)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory != 1)
                            {
                                robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory = 1;
                                CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                                report_bc.PGlass = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                                cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                            }
                        }
                        //
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].Write(cv_Mio,
                                time_chart_instance.cv_WriteDataStartPort);
                        GlassData tmp_data = new GlassData(cv_Mio, time_chart_instance.cv_WriteDataStartPort);
                        if (tmp_data.PId.Trim() == robot.cv_Data.GlassDataMap[(int)job.PPutArm].PId.Trim())
                        {
                            time_chart_instance.cv_PutData = tmp_data;
                            time_chart_instance.cv_PutArm = job.PPutArm;
                            time_chart_instance.cv_Action = EqInterFaceType.Load;
                            cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Load_Only_Reply, 1);
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (job.PTargetId == (int)EqId.UV_1)
                            {
                                cv_WaitUvRecordTime = SysUtils.Now();
                            }
                            if (job.PTarget == ActionTarget.Eq && job.PTargetId == (int)EqId.VAS)
                            {
                                if(job.PTargetSlot == 1)
                                {
                                    PutVasStandby(true);
                                }
                                else if(job.PTargetSlot == 2)
                                {
                                    if(cv_PutGlassStandby)
                                    {
                                        PutVasStandby(false);
                                    }
                                }
                            }
                            else
                            {
                                if ((job.PTarget == ActionTarget.Eq) && (job.PTargetId != (int)EqId.VAS))
                                {
                                    if (cv_GetPutStandbyExceptVas)
                                    {
                                        PutEqStandbyExceptVas(job.PTargetId, job.PTargetSlot, job.PPutArm);
                                    }
                                }
                            }

                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutStart)
                {
                    if (robot_put_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        bool eq_start = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 1);
                        if (eq_ready && eq_start)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (eq_id != EqId.VAS)
                            {
                                cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Interlock_2 +
                                    time_chart_instance.cv_RobotBitStart, 0);

                                GetPutNormalEq(job.PPutArm, eq_id, 1, false, true);
                            }
                            else
                            {
                                if (job.PTargetSlot == 1)
                                {
                                    if (job.PPutArm == RobotArm.rbaUp)
                                    {
                                        PutVasSlot(true, 2, true);
                                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Interlock_2 +
                                            time_chart_instance.cv_RobotBitStart, 0);
                                    }
                                }
                                else if (job.PTargetSlot == 2)
                                {
                                    if (job.PPutArm == RobotArm.rbaDown)
                                    {
                                        PutVasSlot(false, 2, true);
                                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Interlock_2 +
                                            time_chart_instance.cv_RobotBitStart, 0);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutEnd)
                {
                    if (!robot_put_arm_sensor) //&& !robot.IsBusy )
                    {
                        if (!robot.IsBusy)
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_put_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                            time_chart_instance.SetTrigger(time_chart_id);
                            robot.SendDataViaMmf();
                            robot.cv_Data.SaveToFile();
                        }
                        else if (LgcForm.CheckIsVasPutUpSlotJobStatus(job))
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_put_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                            //time_chart_instance.SetTrigger(time_chart_id);
                            time_chart_instance.JumpToStep(time_chart_id, (int)TimechartNormal.STEP_ID_WaitRobotCompleteOn);
                            robot.SendDataViaMmf();
                            robot.cv_Data.SaveToFile();
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByStart)
                {
                    if (robot_put_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        if (eq_ready)
                        {
                            if (eq_id == EqId.VAS)
                            {
                                if (job.PTargetSlot == 1)
                                {
                                    if (job.PPutArm == RobotArm.rbaUp)
                                    {
                                        PutVasSlot(true, 1, true);
                                        time_chart_instance.SetTrigger(time_chart_id);
                                    }
                                }
                                else if (job.PTargetSlot == 2)
                                {
                                    if (job.PPutArm == RobotArm.rbaDown)
                                    {
                                        PutVasSlot(false, 1, true);
                                        time_chart_instance.SetTrigger(time_chart_id);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByEnd)
                {
                    if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Stage_Delivery_Ready +
                        time_chart_instance.cv_EqBitStart) == 1)
                    {
                        time_chart_instance.SetTrigger(time_chart_id);
                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Robot_Delivery_Ready +
                            time_chart_instance.cv_RobotBitStart, 0);
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotCommandFinish)
                {
                    if (!robot_put_arm_sensor)// && !robot.IsBusy)
                    {
                        if (!robot.IsBusy)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete, 1);
                            robot.SendDataViaMmf();
                        }
                        else
                        {
                            if (CheckIsVasPutUpSlotJobStatus(job))
                            {
                                if (cv_Mio.GetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete) == 1)
                                {
                                    time_chart_instance.SetTrigger(time_chart_id);
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete, 1);
                                    robot.SendDataViaMmf();
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitEqCompleteOn)
                {
                    if (!robot_put_arm_sensor)// && !robot.IsBusy)
                    {
                        if (!robot.IsBusy)
                        {
                            if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Transfer_Complete +
                                time_chart_instance.cv_EqBitStart) == 1)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                if (!m_IsMaunal)
                                {
                                    cv_RobotJobPath.Dequeue();
                                    if (cv_IsCycleStop)
                                    {
                                        PSystemData.POperationMode = OperationMode.Manual;
                                        cv_IsCycleStop = false;
                                    }
                                }
                                else
                                    cv_RobotManaulJobPath.Dequeue();
                            }
                            else if (CheckIsVasPutUpSlotJobStatus(job))
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                if (!m_IsMaunal)
                                {
                                    cv_RobotJobPath.Dequeue();
                                    if (cv_IsCycleStop)
                                    {
                                        PSystemData.POperationMode = OperationMode.Manual;
                                        cv_IsCycleStop = false;
                                    }
                                }
                                else
                                    cv_RobotManaulJobPath.Dequeue();
                            }
                        }
                        else
                        {
                            if (CheckIsVasPutUpSlotJobStatus(job))
                            {
                                if (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Complete) == 1)
                                {
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete, 0);
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Load_Only_Reply, 0);
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);
                                }
                            }
                        }
                    }
                }
                else
                {
                    /*
                    if (job.PTargetId == (int)EqId.UV_1 && job.PAction == RobotAction.Put)
                    {
                        long diff = SysUtils.MilliSecondsBetween(cv_WaitUvRecordTime, SysUtils.Now());
                        if (diff > 10000)
                        {
                            cv_WaitUvRecordTime = SysUtils.Now();
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = Alarmtable.WaitUvOverTimer.ToString();
                            alarm.PMainDescription = "Wait UV Over Timer";
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Light;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            LgcForm.EditAlarm(alarm);
                        }
                        else if (diff < 0)
                        {
                            cv_WaitUvRecordTime = SysUtils.Now();
                        }
                    }
                    */
                }
            }
            else if (job.PAction == RobotAction.Exchange)
            {
                bool robot_get_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor;
                bool robot_get_arm_data = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData;
                bool robot_put_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
                bool robot_put_arm_data = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData;
                if (eq_id != EqId.VAS)
                {
                    GlassData glass = null;
                    if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                    {
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationMode = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                        if (gif_type != EqInterFaceType.Exchange)
                        {
                            return;
                        }

                        if (!robot_get_arm_data && !robot_get_arm_sensor &&
                            robot_put_arm_data && robot_put_arm_sensor)
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].Write(cv_Mio,
                                time_chart_instance.cv_WriteDataStartPort);

                            GlassData tmp_data = new GlassData(cv_Mio, time_chart_instance.cv_WriteDataStartPort);
                            if (tmp_data.PId.Trim() == robot.cv_Data.GlassDataMap[(int)job.PPutArm].PId.Trim())
                            {

                                glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                                //
                                string glass_id = glass.PId;
                                string combination = glass.PAssamblyResult.ToString();
                                for (int i = 1; i <= 15; i++)
                                {
                                    int node_index = glass.cv_Nods.FindIndex(x => x.PNodeId == i);
                                    if (node_index != -1)
                                    {
                                        int history = glass.cv_Nods[node_index].cv_ProcessHistory;
                                        int recipe = glass.cv_Nods[node_index].cv_Recipe;
                                    }
                                }
                                if (glass.PHasData)
                                {
                                    if (!CheckEqSideData(glass, eq_id))
                                    {
                                        return;
                                    }
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                        (int)RobotSideBitAddressOffset.Exchange_Reply, 1);
                                    time_chart_instance.cv_Action = EqInterFaceType.Exchange;
                                    time_chart_instance.cv_GetData = glass;
                                    time_chart_instance.cv_GetArm = job.PGetArm;
                                    time_chart_instance.cv_PutData = tmp_data;
                                    time_chart_instance.cv_PutArm = job.PPutArm;
                                    time_chart_instance.SetTrigger(time_chart_id);

                                    if(cv_GetPutStandbyExceptVas)
                                    {
                                        GetEqStandbyExceptVas(job.cv_TargetId, job.cv_TargetSlot, job.PGetArm);
                                    }
                                }
                            }
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetStart)
                    {
                        if (!robot_get_arm_sensor && robot_put_arm_sensor)
                        {
                            bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                            bool eq_tr_start = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 1);
                            if (eq_ready && eq_tr_start)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                GetPutNormalEq(job.PGetArm, eq_id, 1, true, true);
                            }
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetEnd)
                    {
                        if (robot_put_arm_sensor && robot_get_arm_sensor)
                        {
                            glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                            robot.cv_Data.GlassDataMap[(int)job.PGetArm] = glass;
                            robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_get_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                            time_chart_instance.JumpToStep(time_chart_id,
                                (int)TimechartNormal.STEP_ID_WaitRobotPutStart);
                            cv_MmfController.SendBcTreansferReport(DataFlowAction.Receive, robot.cv_Data.GlassDataMap[(int)job.PGetArm]);

                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutStart)
                    {
                        if (robot_put_arm_sensor && robot_get_arm_sensor)
                        {
                            bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                            if (eq_ready)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                GetPutNormalEq(job.PPutArm, eq_id, 1, false, true);
                                int node_index = robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods.FindIndex(x => x.PNodeId == 2);
                                if (robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory != 1)
                                {
                                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory = 1;
                                    CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                                    report_bc.PGlass = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                                    cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                                }
                            }
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutEnd)
                    {
                        if (!robot_put_arm_sensor && robot_get_arm_sensor)
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_put_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                            time_chart_instance.SetTrigger(time_chart_id);
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotCommandFinish)
                    {
                        if (!robot_put_arm_sensor && robot_get_arm_sensor && !robot.IsBusy)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitEqCompleteOn)
                    {
                        if (!robot_put_arm_sensor && robot_get_arm_sensor && !robot.IsBusy)
                        {
                            if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Transfer_Complete +
                                    time_chart_instance.cv_EqBitStart) == 1)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                if (!m_IsMaunal)
                                {
                                    cv_RobotJobPath.Dequeue();
                                    if (cv_IsCycleStop)
                                    {
                                        PSystemData.POperationMode = OperationMode.Manual;
                                        cv_IsCycleStop = false;
                                    }
                                }
                                else
                                    cv_RobotManaulJobPath.Dequeue();
                            }
                        }
                    }
                }
            }
        }
        private void DoRobotJobPath(bool m_IsManualCommand = false)
        {
            RobotJob job = null;
            if (m_IsManualCommand)
                job = cv_RobotManaulJobPath.Peek();
            else
                job = cv_RobotJobPath.Peek();
            Robot robot = GetRobotById(1);
            if (!m_IsManualCommand)
            {
                if (cv_RobotJobPath.Count == 0) return;
            }
            else
            {
                if (cv_RobotManaulJobPath.Count == 0) return;
            }
            if (robot.IsBusy && !CheckIsVasPutUpSlotJobStatus(job)) return;
            if (job.PAction == RobotAction.Get)// ||
            // (job.PAction == RobotAction.Exchange && job.PTarget == ActionTarget.Aligner &&
            //job.PIsWaitGet && !job.PisWaitPut))
            {
                if (job.PTarget == ActionTarget.Port)
                {
                    ProcessPortGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Buffer)
                {
                    ProcessBufferGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Aligner)
                {
                    ProcessAlignerGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Eq)
                {
                    ProcessEqGetPutJob(job.PAction, m_IsManualCommand);
                }
            }
            else if (job.PAction == RobotAction.Put)//|| (
            //(job.PAction == RobotAction.Exchange) &&
            //(job.PTarget == ActionTarget.Aligner) && (job.PisWaitPut) && (job.PIsWaitGet))
            //)
            {
                if (job.PTarget == ActionTarget.Aligner)
                {
                    ProcessAlignerGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Buffer)
                {
                    ProcessBufferGetPutJob(RobotAction.Put);
                }
                else if (job.PTarget == ActionTarget.Port)
                {
                    ProcessPortGetPutJob(RobotAction.Put);
                }
                else if (job.PTarget == ActionTarget.Eq)
                {
                    ProcessEqGetPutJob(job.PAction, m_IsManualCommand);
                }
            }
            else if (job.PAction == RobotAction.Exchange)
            {
                if (job.PTarget == ActionTarget.Eq)
                {
                    ProcessEqGetPutJob(job.PAction, m_IsManualCommand);
                }
                else if (job.PTarget == ActionTarget.Aligner)
                {
                    // for putgetAligner use only.
                    ProcessAlignerGetPutJob(job.PAction, m_IsManualCommand);
                }
            }
        }
        private void ProcessAfterUvPutWait()
        {

        }
        #endregion

        #region Find start step ( not use )
        /*
        private bool FindStartStep(int m_CurStep, ref int m_StartPos, ref Dictionary<int, RobotJob> m_JobMap)
        {
            bool rtn = false;
            int next_step = m_CurStep;
            int now_step = m_CurStep - 1;
            int StartPos = now_step;
            if (cv_CurRecipeFlowStepSetting.ContainsKey(now_step))
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[now_step];
                foreach (AllDevice device in cv_stepDevice)
                {
                    if (device == AllDevice.Aligner)
                    {
                        if (GetAlignerById(1).cv_Data.GlassDataMap[1].PHasData && GetAlignerById(1).cv_Data.GlassDataMap[1].PHasSensor)
                        {
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get,
                                ActionTarget.Aligner, 1, 1, false);
                            rtn = FindStartStep(now_step, ref m_StartPos, ref m_JobMap);
                        }
                    }
                    else if (device == AllDevice.LP)
                    {
                        int port = 0;
                        int slot = 0;
                        if (FindUnloadPPortToPutSubstrate(out port, out slot))
                        {
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get,
                                ActionTarget.Port, port, slot, false);
                            rtn = true;
                            StartPos = now_step;
                            break;
                        }
                    }
                    else if (device == AllDevice.Buffer)
                    {
                        int port = 0;
                        int slot = 0;
                        if (GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out slot))
                        {
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get,
                                ActionTarget.Buffer, 1, slot, false);
                            rtn = true;
                            StartPos = now_step;
                            break;
                        }
                    }
                    else
                    {
                        EqId eq_id = EqId.None;
                        int time_chart_instance = 0;
                        int eq_time_chart_cur_step = 0;
                        if (Enum.TryParse<EqId>(device.ToString(), out eq_id))
                        {
                            if (eq_id == EqId.VAS)
                            {
                                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                            }
                            else
                            {
                                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                time_chart_instance = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                            }
                        }
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                        {
                            EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                            Eq eq = GetEqById((int)eq_id);
                            if (gif_type == EqInterFaceType.Unload)
                            {
                                if (m_JobMap[next_step].PTarget == ActionTarget.Aligner)
                                {
                                    m_JobMap[next_step].PPutArm = eq.PGetArm; // = new RobotJob(1, eq.PPutArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq_id, 1, true);
                                    m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                    rtn = true;
                                    StartPos = now_step;
                                    break;
                                }
                                else
                                {
                                    if (m_JobMap[next_step].PPutArm == eq.PGetArm)
                                    {
                                        m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                        rtn = true;
                                        StartPos = now_step;
                                        break;
                                    }
                                }
                            }
                            if (gif_type == EqInterFaceType.Exchange)
                            {
                                if (m_JobMap[next_step].PTarget == ActionTarget.Aligner)
                                {
                                    m_JobMap[next_step].PPutArm = eq.PGetArm;
                                    m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Exchange, ActionTarget.Eq, (int)eq_id, 1, true);
                                }
                                else
                                {
                                    m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Exchange, ActionTarget.Eq, (int)eq_id, 1, true);
                                }

                                if (!FindStartStep(now_step, ref m_StartPos, ref m_JobMap))
                                {
                                    m_JobMap[now_step].PAction = RobotAction.Get;
                                    m_StartPos = now_step;
                                    rtn = true;
                                }
                                else
                                {
                                    rtn = true;
                                }
                            }
                        }
                    }
                }
            }
            m_StartPos = StartPos;
            return rtn;
        }
        */
        #endregion

        private bool FindUnloadPortToPutSubstrate(out int m_Port, out int m_Slot , RobotJob m_Job)
        {
            bool rtn = false;
            int port = 1;
            int slot = 1;
            TimechartNormal time_chart_instance = null;
            int time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_UV_1;
            GlassData glass = null;
            if (m_Job.PTarget == ActionTarget.Eq && (m_Job.PTargetId == (int)EqId.UV_1))
            {
                time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                try
                {
                    glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                }
                catch (Exception e)
                {
                    WriteLog(LogLevelType.Error, "[FindUnloadPortToPutSubstrate] new glass error.");
                    //ShowMsg("[FindUnloadPortToPutSubstrate] new glass data from UV is Error", false, false);
                }
            }

            if (glass == null)
            {
                WriteLog(LogLevelType.Error, "[FindUnloadPortToPutSubstrate] new glass is null.");
                //ShowMsg("[FindUnloadPortToPutSubstrate]  glass data from UV is Error", false, false);
                m_Port = 0;
                m_Slot = 0;
                return false;
            }
            if (glass.PFoupSeq == 0)
            {
                WriteLog(LogLevelType.Error, "[FindUnloadPortToPutSubstrate] UV glass data Error(FoupSeq 0 ).");
                //ShowMsg("[FindUnloadPortToPutSubstrate]  UV glass data Error(FoupSeq 0 ).", false, false);
                m_Port = 0;
                m_Slot = 0;
                return false;
            }

            //first find the same seq port
            for (int port_id = 0; port_id < cv_InProcessPort.Count; port_id++)
            {
                Port job_port = GetPortById(cv_InProcessPort[port_id]);
                if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                {
                    if (job_port.PPortStatus == PortStaus.LDCM)// && job_port.PLotStatus == LotStatus.Process)// && job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.Process || job_port.PLotStatus == LotStatus.Reserved)
                        {
                            if (job_port.cv_Data.HasDataOrSensor())
                            {
                                if (job_port.cv_Data.SeqTheSameWithSubstrate(glass.PFoupSeq))
                                {
                                    int tmp_slot = 0;
                                    if (job_port.cv_Data.WhichSlotCanLoad(out tmp_slot))
                                    {
                                        m_Port = job_port.cv_Id;
                                        m_Slot = tmp_slot;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // find empty foup to put.
            for (int port_id = 0; port_id < cv_InProcessPort.Count; port_id++)
            {
                Port job_port = GetPortById(cv_InProcessPort[port_id]);
                if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                {
                    if (job_port.PPortStatus == PortStaus.LDCM)// && job_port.PLotStatus == LotStatus.Process)// && job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.Process || job_port.PLotStatus == LotStatus.Reserved)
                        {
                            if (!job_port.cv_Data.HasDataOrSensor())
                            {
                                int tmp_slot = 0;
                                if (job_port.cv_Data.WhichSlotCanLoad(out tmp_slot))
                                {
                                    m_Port = job_port.cv_Id;
                                    m_Slot = tmp_slot;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            if (!rtn)
            {
                LgcForm.ShowMsg("Unload Port to put substrate not found , please check!!!", true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = Alarmtable.CannotFindUnloadPortSlotToPutSubstrate.ToString();
                alarm.PMainDescription = "Cannot Find Unload Port Slot To Put Substrate";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                LgcForm.EditAlarm(alarm);
            }
            m_Port = port;
            m_Slot = slot;
            return rtn;
        }

        #region  log in/out Event function
        //Trigger this event When AccountData login/out successful.(UI must override)
        protected override void OnLogInOutEvent(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount)
        {
        }

        //Trigger this event When AccountData change.(UI must override)
        protected override void OnAccountChangeEvent()
        {
        }
        #endregion

        #region Alarm Event function
        //Trigger this event When AlarmData add/del successful.(LGC must override)
        protected override void OnAlarmActionEvent(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
            if (cv_MmfController != null)
            {
                CommonData.HIRATA.MDAlarmAction report_plc = new MDAlarmAction();
                for (int i = 0; i < m_Alarms.Count; i++)
                {
                    if (m_Action == AlarmStatus.Clean)
                    {
                        m_Alarms[i].PStatus = AlarmStatus.Clean;
                        report_plc.AlarmData.cv_AlarmList.Add(m_Alarms[i]);
                    }
                    else if (m_Action == AlarmStatus.Occur)
                    {
                        m_Alarms[i].PStatus = AlarmStatus.Occur;
                        report_plc.AlarmData.cv_AlarmList.Add(m_Alarms[i]);
                    }
                }
                cv_MmfController.SendAlarmAction(report_plc.AlarmData, MmfEventClientEventType.etNotify);
            }
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected override void OnAlarmChange()
        {
            if (cv_Alarms.IsHasAlarm())
            {
                PSystemData.POperationMode = OperationMode.Manual;
                //PSystemData.PSystemStatus = EquipmentStatus.Down;
            }
            if (cv_MmfController != null)
            {
                cv_MmfController.SendAlarmData();
            }
        }
        #endregion

        #region Recipe Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected override void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
            ParserFlowStep();
            if (cv_MmfController != null)
            {
                cv_MmfController.SendRecipeData();
                cv_MmfController.SendRecipeAction(m_Action, m_Recipes, MmfEventClientEventType.etNotify);
            }
        }
        //Trigger this event When RecipeData change.(LGC must override)
        protected override void OnRecipeChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendRecipeData();
            }
        }
        #endregion

        #region Link Timeout data Event
        //Trigger this event When Time out data change(LGC must override)
        protected override void OnTimeOutDataChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendTimeoutData();
            }
        }
        #endregion

        #region Link GlassCount data Event
        //Trigger this event When glass count change.(LGC must override)
        protected override void OnGlassCountDataChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendGlassCountData();
            }
        }
        #endregion

        #region Link System data Event
        protected override void OnSystemStatusChange()
        {
            if (PSystemData.PSystemStatus == EquipmentStatus.Down)
            {
                AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                AddTowerCommand(SignalTowerColor.Red, SignalTowerControl.On);
                if(PSystemData.POperationMode == OperationMode.Auto)
                {
                    PSystemData.POperationMode = OperationMode.Manual;
                }
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Idle)
            {
                AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                AddTowerCommand(SignalTowerColor.Yellow, SignalTowerControl.On);
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Run)
            {
                AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                AddTowerCommand(SignalTowerColor.Green, SignalTowerControl.On);
            }
            if (PSystemData.PRobotConnect)
            {
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
                {
                    if (PSystemData.PSystemStatus != EquipmentStatus.Down)
                    {
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Active_Standby, 1);
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);
                    }
                    else if (PSystemData.PSystemStatus == EquipmentStatus.Down)
                    {
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Active_Standby, 0);
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);

                    }
                }
            }
            else
            {
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
                {
                    cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Active_Standby, 0);
                    cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);
                }
            }
        }
        protected override void OnRobotStatusChange()
        {
            if (PSystemData.PSystemStatus == EquipmentStatus.Down)
            {
                PSystemData.PInitaiizeOk = false;
                PSystemData.PInitaiizing = false;
                GetRobotById(1).CurJob = null; // manual is ok , auto mode : buz has check sensor , so almost ok.
                cv_RobotManaulJobPath.Clear();
                cv_RobotJobPath.Clear();
                SendRobotJobPath();
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Idle)
            {
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Run)
            {
            }
        }
        //Trigger this event When system data change.(LGC must override)
        protected override void OnSystemDataChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendSystemData();
            }
        }
        #endregion
        private void AddTowerCommand(SignalTowerColor m_Color, SignalTowerControl m_Control)
        {
            Robot robot = GetRobotById(1);
            if (robot != null)
            {
                if (robot.cv_TowerJobQ != null)
                {
                    TowerCommand tmp = new TowerCommand(m_Color, m_Control);
                    robot.cv_TowerJobQ.Enqueue(tmp);
                }
            }
        }
        public static void AddBuzzerCommand(bool m_IsOn)
        {
            Robot robot = GetRobotById(1);
            if (robot != null)
            {
                if (robot.cv_BuzzerQ != null)
                {
                    robot.cv_BuzzerQ.Enqueue(m_IsOn);
                }
            }
        }
        protected override void ModuleInit()
        {
            cv_Recipes.SetFilePath(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\PPID.xml");
            cv_Recipes.PIsAutoSave = true;
            cv_Recipes.LoadFromFile();
            cv_Recipes.SaveToFile();

            cv_Alarms.PIsAutoSave = false;
            cv_AccountData.PIsAutoSave = false;

            cv_TimeoutData.SetFilePath(CommonStaticData.g_TimeOutPath);
            cv_TimeoutData.PIsAutoSave = true;
            cv_TimeoutData.LoadFromFile();
            cv_TimeoutData.SaveToFile();

            cv_GlassCountData.SetFilePath(CommonStaticData.g_GlassCountDataPath);
            cv_GlassCountData.PIsAutoSave = false;
            int history = LgcForm.cv_Mio.GetPortValue(0x344A);
            history += (LgcForm.cv_Mio.GetPortValue(0x344B) << 16);
            cv_GlassCountData.PHistoryCount = history;

            PSystemData.SetFilePath(CommonStaticData.g_StatsRecordPath);
            PSystemData.PIsAutoSave = true;
            PSystemData.LoadFromFile();
            PSystemData.SaveToFile();

            KIniFile ini = new KIniFile(CommonData.HIRATA.CommonStaticData.g_ModuleSystemIniFile);
            if(ini.ReadString("Config", "PutGlassStandby", "0").Trim() == "1")
            {
                cv_PutGlassStandby = true;
                WriteLog(LogLevelType.Detail, "Set PutGlassStandby : 1");
            }
            else
            {
                cv_PutGlassStandby = false;
                WriteLog(LogLevelType.Detail, "Set PutGlassStandby : 0");
            }
            //cv_VasLoadGlassBeforeUnload
            if (ini.ReadString("Config", "VasLoadGlassBeforeUnload", "0").Trim() == "1")
            {
                cv_VasLoadGlassBeforeUnload = true;
                WriteLog(LogLevelType.Detail, "Set cv_VasLoadGlassBeforeUnload : 1");
            }
            else
            {
                cv_VasLoadGlassBeforeUnload = false;
                WriteLog(LogLevelType.Detail, "Set cv_VasLoadGlassBeforeUnload : 0");
            }
            //cv_CheckFirstStepWhenPutGlass
            if (ini.ReadString("Config", "CheckFirstStepWhenPutGlass", "0").Trim() == "1")
            {
                cv_CheckFirstStepWhenPutGlass = true;
                WriteLog(LogLevelType.Detail, "Set cv_CheckFirstStepWhenPutGlass : 1");
            }
            else
            {
                cv_CheckFirstStepWhenPutGlass = false;
                WriteLog(LogLevelType.Detail, "Set cv_CheckFirstStepWhenPutGlass : 0");
            }

            if (ini.ReadString("Config", "GetPutStandbyExceptVas", "0").Trim() == "1")
            {
                cv_GetPutStandbyExceptVas = true;
                WriteLog(LogLevelType.Detail, "Set cv_GetPutStandbyExceptVas : 1");
            }
            else
            {
                cv_GetPutStandbyExceptVas = false;
                WriteLog(LogLevelType.Detail, "Set cv_GetPutStandbyExceptVas : 0");
            }
            //check eq data at local mode.
            if (ini.ReadString("Config", "CheckEqDataLocalMode", "1").Trim() == "1")
            {
                cv_CheckEqDataLocalMode = true;
                WriteLog(LogLevelType.Detail, "Set cv_CheckEqDataLocalMode : 1");
            }
            else
            {
                cv_CheckEqDataLocalMode = false;
                WriteLog(LogLevelType.Detail, "Set cv_CheckEqDataLocalMode : 0");
            }

        }
        //when the port can start process , use this this AddPortToProcessList function.
        public static void AddPortToProcessList(int m_Port)
        {
            int index = cv_InProcessPort.FindIndex(x => x == m_Port);
            if (index != -1)
            {
                cv_InProcessPort.RemoveAt(index);
            }
            cv_InProcessPort.Add(m_Port);
        }
        public static void RemovePortToProcessList(int m_Port)
        {
            int index = cv_InProcessPort.FindIndex(x => x == m_Port);
            if (index != -1)
            {
                cv_InProcessPort.RemoveAt(index);
            }
        }
        private void CalculateSubstrateCount()
        {
            int production = 0;
            int dummy = 0;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = LgcForm.GetPortById(i);

                if(port.PPortStatus == PortStaus.LDCM)
                {
                    for (int slot = 1; slot <= port.cv_Data.cv_SlotCount; slot++)
                    {
                        if (port.cv_Data.GlassDataMap[slot].PHasData && port.cv_Data.GlassDataMap[slot].PHasSensor)
                        {
                            if (port.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                                dummy++;
                            else if (port.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                                production++;
                            else if (port.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                                production++;
                        }
                    }
                }
            }
            Robot robot = LgcForm.GetRobotById(1);
            for (int slot = 1; slot <= 2; slot++)
            {
                if (robot.cv_Data.GlassDataMap[slot].PHasData && robot.cv_Data.GlassDataMap[slot].PHasSensor)
                {
                    if (robot.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                        dummy++;
                    else if (robot.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                        production++;
                    else if (robot.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                        production++;
                }
            }
            Buffer buffer = LgcForm.GetBufferById(1);
            for (int slot = 1; slot <= buffer.cv_SlotCount; slot++)
            {
                if (buffer.cv_Data.GlassDataMap[slot].PHasData && buffer.cv_Data.GlassDataMap[slot].PHasSensor)
                {
                    if (buffer.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                        dummy++;
                    else if (buffer.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                        production++;
                    else if (buffer.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                        production++;
                }
            }
            Aligner aligner = LgcForm.GetAlignerById(1);
            for (int slot = 1; slot <= aligner.cv_SlotCount; slot++)
            {
                if (aligner.cv_Data.GlassDataMap[slot].PHasData && aligner.cv_Data.GlassDataMap[slot].PHasSensor)
                {
                    if (aligner.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                        dummy++;
                    else if (aligner.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                        production++;
                    else if (aligner.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                        production++;
                }
            }

            bool is_change = false;
            if (cv_GlassCountData.PProductCount != production)
            {
                is_change = true;
                cv_GlassCountData.PProductCount = production;
            }
            if (cv_GlassCountData.PDummyCount != dummy)
            {
                is_change = true;
                cv_GlassCountData.PDummyCount = dummy;
            }
            /*
            if (is_change)
                cv_MmfController.SendGlassCountData();
            */
        }
        private void initTimer()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_RobotActionTimer == null)
            {
                cv_RobotActionTimer = new KTimer();
                cv_RobotActionTimer.Interval = 200;
                cv_RobotActionTimer.ThreadEventEnabled = false;
                cv_RobotActionTimer.Enabled = true;
                cv_RobotActionTimer.OnTimer += OnRobotActionTimer;
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void SendProgramStart()
        {
            CommonData.HIRATA.MDBCAliveAndPlcConnect obj = new MDBCAliveAndPlcConnect();
            cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name, obj, KParseObjToXmlPropertyType.Field);
        }
        private void DerivedTimer()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendTowerCommand();
            SendBuzzerCommand();
            CalculateSubstrateCount();
            CalculateSystemStatus();
            DoPortChangeToLDRQ();
            WriteRealDataToBc();
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        //Write real time data to BC
        private void WriteRealDataToBc()
        {
            Robot robot = GetRobotById(1);
            Aligner aligner = GetAlignerById(1);
            Buffer buffer = GetBufferById(1);

            robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].WriteWokeNoOnly(cv_Mio, 0x381A);
            robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].WriteWokeNoOnly(cv_Mio, 0x381C);
            aligner.cv_Data.GlassDataMap[1].WriteWokeNoOnly(cv_Mio, 0x381E);
            for (int i = 1; i <= buffer.cv_SlotCount; i++)
            {
                buffer.cv_Data.GlassDataMap[i].WriteWokeNoOnly(cv_Mio, 0x3820 + ((i - 1) << 1));
            }
        }
        void SendTowerCommand()
        {
            Robot robot = GetRobotById(1);
            if (PSystemData.PRobotInline != EquipmentInlineMode.Remote) return;
            if (robot.cv_TowerJobQ.Count != 0)
            {
                TowerCommand tmp = GetRobotById(1).cv_TowerJobQ.Peek();
                if (!tmp.cv_HadSend)
                {
                    robot.cv_Comm.SetSignalTower(tmp.cv_Color, tmp.cv_Control);
                    tmp.cv_HadSend = true;
                }
            }
        }
        void SendBuzzerCommand()
        {
            Robot robot = GetRobotById(1);
            if (PSystemData.PRobotInline != EquipmentInlineMode.Remote) return;
            if (robot.cv_BuzzerQ.Count != 0)
            {
                //bool tmp = GetRobotById(1).cv_BuzzerQ.Peek();
                bool tmp = GetRobotById(1).cv_BuzzerQ.Dequeue();
                robot.cv_Comm.SetBuzzer(tmp);
            }
        }
        private void CalculateSystemStatus()
        {
            bool has_foup = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                if (GetPortById(i).PPortStatus == PortStaus.LDCM)
                {
                    has_foup = true;
                    break;
                }
            }

            if (PSystemData.PSystemStatus == EquipmentStatus.WaitIdle)
            {
                long diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), PSystemData.PIdleTime);
                if (diff > LgcForm.cv_TimeoutData.PIdleDelayTime)
                {
                    PSystemData.PSystemStatus = EquipmentStatus.Idle;
                    AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                    AddTowerCommand(SignalTowerColor.Yellow, SignalTowerControl.On);

                }
                else if (diff < 0)
                {
                    PSystemData.PIdleTime = SysUtils.Now();
                }
            }

            if (has_foup)
            {
                if (!cv_Alarms.IsHasAlarm())
                {
                    if (PSystemData.PSystemStatus != EquipmentStatus.Run)
                    {
                        PSystemData.PSystemStatus = EquipmentStatus.Run;
                        AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                        AddTowerCommand(SignalTowerColor.Green, SignalTowerControl.On);
                    }
                }
                else if (PSystemData.PSystemStatus != EquipmentStatus.Down)
                {
                    PSystemData.PSystemStatus = EquipmentStatus.Down;
                    AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                    AddTowerCommand(SignalTowerColor.Red, SignalTowerControl.On);
                    if (PSystemData.POperationMode == OperationMode.Auto)
                    {
                        PSystemData.POperationMode = OperationMode.Manual;
                    }
                }
            }
            else
            {
                if (!cv_Alarms.IsHasAlarm())
                {
                    if (PSystemData.PSystemStatus != EquipmentStatus.WaitIdle && PSystemData.PSystemStatus != EquipmentStatus.Idle)
                    {
                        if(PSystemData.PSystemStatus == EquipmentStatus.None)
                        {
                            if(PSystemData.PRobotConnect)
                            PSystemData.PSystemStatus = EquipmentStatus.Idle;
                            else
                            PSystemData.PSystemStatus = EquipmentStatus.Down;
                        }
                        else
                        {
                            PSystemData.PSystemStatus = EquipmentStatus.WaitIdle;
                        }
                    }
                }
                else if (PSystemData.PSystemStatus != EquipmentStatus.Down)
                {
                    PSystemData.PSystemStatus = EquipmentStatus.Down;
                    if (PSystemData.POperationMode == OperationMode.Auto)
                    {
                        PSystemData.POperationMode = OperationMode.Manual;
                    }
                }
            }
        }
        private void DoPortChangeToLDRQ()
        {
            Port job_port = null;
            for (int port_id = 1; port_id <= CommonData.HIRATA.CommonStaticData.g_PortNumber; port_id++)
            {
                job_port = GetPortById(port_id);
                if (job_port.PPortStatus == PortStaus.UDCM)
                {
                    long diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), job_port.PLDRQTime);
                    if (diff > 2000)
                    {
                        job_port.PPortStatus = PortStaus.LDRQ;
                        job_port.cv_Data.Clear();
                        job_port.PLDRQTime = SysUtils.Now();
                    }
                    else if (diff < 0)
                    {
                        job_port.PLDRQTime = SysUtils.Now();
                    }
                }
            }
        }
        private void DoCstUnload()
        {
            Port job_port = null;
            if (PSystemData.POperationMode != OperationMode.Auto)
            {
                return;
            }
            for (int port_id = 1; port_id <= CommonData.HIRATA.CommonStaticData.g_PortNumber; port_id++)
            {
                job_port = GetPortById(port_id);
                if (job_port.PPortStatus == PortStaus.LDCM)
                {
                    if (job_port.cv_Data.cv_IsWaitCancel)
                    {
                        if (CheckThePortCanUnload(port_id))
                        {
                            if (job_port.PLotStatus != LotStatus.Process && job_port.PLotStatus != LotStatus.ProcessEnd)
                            {
                                job_port.PLotStatus = LotStatus.Cancel;
                            }
                        }
                    }
                    else if (job_port.cv_Data.cv_IsWaitAbort)
                    {
                        if (CheckThePortCanUnload(port_id))
                        {
                            if (job_port.PLotStatus == LotStatus.Process)
                            {
                                job_port.PLotStatus = LotStatus.Abort;
                            }
                        }
                    }
                    if (job_port.PLotStatus == LotStatus.Process)
                    {
                        if (!job_port.cv_Data.HasOtherJobHaveToDo())
                        {
                            if (CheckThePortCanUnload(port_id))
                            {
                                if (PSystemData.PSystemOnlineMode == OnlineMode.Offline)
                                {
                                    job_port.PLotStatus = LotStatus.ProcessEnd;
                                    job_port.cv_Data.PWaitUnload = true;
                                }
                                else if (PSystemData.PSystemOnlineMode == OnlineMode.Control)
                                {
                                    job_port.PLotStatus = LotStatus.ProcessEnd;
                                }
                            }
                        }
                    }

                    if (job_port.cv_Data.PWaitUnload)
                    {
                        if (CheckThePortCanUnload(port_id))
                        {
                            if (job_port.PLotStatus == LotStatus.Process)
                            {
                                if (!job_port.cv_Data.HasOtherJobHaveToDo())
                                {
                                    job_port.PLotStatus = LotStatus.ProcessEnd;
                                }
                                else
                                {
                                    job_port.PLotStatus = LotStatus.Abort;
                                }
                            }
                            else
                            {
                                if (job_port.PLotStatus != LotStatus.Abort && job_port.PLotStatus != LotStatus.Cancel
                                    && job_port.PLotStatus != LotStatus.ProcessEnd)
                                {
                                    job_port.PLotStatus = LotStatus.Cancel;
                                }
                            }
                            RemovePortToProcessList(port_id);
                            job_port.PPortStatus = PortStaus.UDRQ;
                            job_port.cv_Data.cv_IsWaitAbort = false;
                            job_port.cv_Data.cv_IsWaitCancel = false;
                            LgcForm.GetRobotById(1).cv_Comm.SetPortUnloadAction(port_id);
                            job_port.cv_Data.PWaitUnload = false;
                        }
                    }
                }
            }
        }

        private ProductCategory GetSubstractTypeWantToGetFromCst()
        {
            ProductCategory tmp = ProductCategory.Mask;
            int glass = 0;
            int wafer = 0;
            Buffer buffer = LgcForm.GetBufferById(1);
            for (int i = 1; i <= buffer.cv_Data.cv_SlotCount; i++)
            {
                if (buffer.cv_Data.GlassDataMap[i].PProductionCategory == ProductCategory.Glass)
                    glass++;
                if (buffer.cv_Data.GlassDataMap[i].PProductionCategory == ProductCategory.Wafer)
                    wafer++;
            }


            //
            EqId eq_id = EqId.VAS;
            int time_chart_instance = 0;
            int eq_time_chart_cur_step = 0;
            //   if (Enum.TryParse<EqId>(, out eq_id))
            {
                //  if (eq_id == EqId.VAS)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                    time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                }
            }
            if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
            {
                EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                if (gif_type == EqInterFaceType.Load)
                {
                    //
                    bool is_first_can_load = false;
                    if (cv_CheckFirstStepWhenPutGlass)
                    {
                        List<AllDevice> dievices = cv_CurRecipeFlowStepSetting[2];
                        List<AllDevice> form_dievices = cv_CurRecipeFlowStepSetting[1];
                        foreach (AllDevice device in dievices)
                        {
                            if ((int)device >= ((int)AllDevice.SDP1) && (int)device <= ((int)AllDevice.UV_2))
                            {
                                if (!CheckFlowCanRun(1))
                                {
                                    tmp = ProductCategory.Glass;
                                    break;
                                }
                                EqId eq = (EqId)Enum.Parse(typeof(EqId), device.ToString());
                                if (GetEqById((int)eq).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                                {
                                    int first_time_chart_instance = GetEqById((int)eq).cv_Comm.cv_TimeChatId;
                                    if (eq == EqId.VAS)
                                    {
                                        first_time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                    }
                                    EqInterFaceType first_gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(first_time_chart_instance).cv_ActionType;
                                    if (first_gif_type == EqInterFaceType.Load)
                                    {
                                        if (cv_RobotJobPath == null || cv_RobotJobPath.Count == 0)
                                        {
                                            is_first_can_load = true;
                                            tmp = ProductCategory.Wafer;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!is_first_can_load)
                        {
                            tmp = ProductCategory.Glass;
                        }
                    }
                    else
                    {
                        tmp = ProductCategory.Glass;
                    }
                    //
                }
                else
                {
                    tmp = ProductCategory.Wafer;
                }
            }
            else
            {
                if (wafer < glass)
                    tmp = ProductCategory.Wafer;
                else if (wafer > glass)
                    tmp = ProductCategory.Glass;
                else
                    tmp = ProductCategory.Wafer;
            }
            //
            return tmp;
        }

        #region do port unload
        private bool CheckThePortCanUnload(int m_PortId)
        {
            bool rtn = true;
            Port port = LgcForm.GetPortById(m_PortId);
            bool is_job_port = false;
            foreach (RobotJob job in cv_RobotJobPath)
            {
                if (job.PTarget == ActionTarget.Port && job.PTargetId == m_PortId)
                {
                    is_job_port = true;
                }
            }
            if (!is_job_port)
            {
                if (port.cv_Data.PPortMode == PortMode.Loader)
                {
                    Aligner aligner = LgcForm.GetAlignerById(1);
                    Robot robot = LgcForm.GetRobotById(1);
                    if (PSystemData.POcrMode == OCRMode.ErrorReturn)
                    {
                        if (aligner.cv_Data.GlassDataMap[1].PHasData)
                        {
                            if (aligner.cv_Data.GlassDataMap[1].PSourcePort == m_PortId)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrResult == OCRResult.Fail ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrResult == OCRResult.Mismatch ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrResult == OCRResult.None)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrResult == OCRResult.Fail ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrResult == OCRResult.Mismatch ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrResult == OCRResult.None)
                            {
                                rtn = false;
                            }
                        }
                    }
                    else if (PSystemData.POcrMode == OCRMode.ErrorHold)
                    {
                        if (aligner.cv_Data.GlassDataMap[1].PHasData)
                        {
                            if (aligner.cv_Data.GlassDataMap[1].PSourcePort == m_PortId)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrDecide == OCRMode.ErrorReturn || robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrDecide == OCRMode.None)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrDecide == OCRMode.ErrorReturn || robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrDecide == OCRMode.None)
                            {
                                rtn = false;
                            }
                        }
                    }
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        public bool HasProcessUnloadPort()
        {
            Port port = null;
            bool rtn = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                port = LgcForm.GetPortById(i);
                if (port.PPortStatus == PortStaus.LDCM && (port.PLotStatus == LotStatus.Process || port.PLotStatus == LotStatus.Reserved))
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        #endregion

        #region Robot command
        public bool GetPutAligner(RobotArm m_Arm, bool IsGet)
        {
            Robot robot = LgcForm.GetRobotById(1);
            if (!robot.IsBusy)
            {
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                {
                    robot_command = APIEnum.RobotCommand.WaferGet;
                }
                else
                {
                    robot_command = APIEnum.RobotCommand.WaferPut;
                }
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Aligner1");
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Aligner, 1, 1, false);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetPutPort(RobotArm m_Arm, int m_Port, int m_Slot, bool IsGet)
        {
            Robot robot = LgcForm.GetRobotById(1);
            if (!robot.IsBusy)
            {
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                    robot_command = APIEnum.RobotCommand.WaferGet;
                else
                    robot_command = APIEnum.RobotCommand.WaferPut;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("P" + m_Port.ToString());
                para.Add(m_Slot.ToString());
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Port, m_Port, m_Slot, false);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, m_Port, m_Slot, false);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetPutBuffer(RobotArm m_Arm, int m_BufferId, int m_Slot, bool IsGet)
        {
            Robot robot = LgcForm.GetRobotById(1);
            Buffer buffer = LgcForm.GetBufferById(1);
            if (!robot.IsBusy)
            {
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                    robot_command = APIEnum.RobotCommand.WaferGet;
                else
                    robot_command = APIEnum.RobotCommand.WaferPut;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + buffer.cv_Comm.cv_RobotPosition.ToString());//.cv_RobotPos.ToString());
                para.Add(m_Slot.ToString());
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Buffer, m_BufferId, m_Slot, false);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Buffer, m_BufferId, m_Slot, false);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(), APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetPutNormalEq(RobotArm m_Arm, EqId m_EqId, int m_Slot, bool IsGet, bool m_UseHS = true)
        {
            Robot robot = LgcForm.GetRobotById(1);
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                    robot_command = APIEnum.RobotCommand.WaferGet;
                else
                    robot_command = APIEnum.RobotCommand.WaferPut;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add(m_Slot.ToString());
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Eq, (int)m_EqId, m_Slot, m_UseHS);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)m_EqId, m_Slot, m_UseHS);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool PutVasSlot(bool isSlot1, int m_Step, bool m_UseHS = true)
        {
            Robot robot = LgcForm.GetRobotById(1);
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)EqId.VAS).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                RobotAction action = RobotAction.None;
                if (!isSlot1)
                {
                    if (m_Step == 1)
                    {
                        robot_command = APIEnum.RobotCommand.TopPutStandbyArmExtend;
                        action = RobotAction.TopPutStandbyArmExtend;
                    }
                    if (m_Step == 2)
                    {
                        robot_command = APIEnum.RobotCommand.TopWaferPut;
                        action = RobotAction.TopPut;
                    }
                }
                else
                {
                    if (m_Step == 1)
                    {
                        robot_command = APIEnum.RobotCommand.PutStandbyArmExtend;
                        action = RobotAction.PutStandbyArmExtend;
                    }
                    if (m_Step == 2)
                    {
                        robot_command = APIEnum.RobotCommand.WaferPut;
                        action = RobotAction.Put;
                    }
                }

                List<string> para = new List<string>();
                if (isSlot1)
                    para.Add(((int)RobotArm.rbaUp).ToString());
                else
                    para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                //para.Add(isSlot1 ? "1" : "2");
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (isSlot1)
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)EqId.VAS, 1, m_UseHS);
                }
                else
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)EqId.VAS, 2, m_UseHS);
                }
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool GetEqStandbyExceptVas(int m_EqId , int m_Slot , RobotArm m_Arm)
        {
            Robot robot = LgcForm.GetRobotById(1);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById(m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.GetStandby;
                action = RobotAction.GetWait;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm , action
                    , ActionTarget.Eq, m_EqId , 1, true);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool PutEqStandbyExceptVas(int m_EqId , int m_Slot , RobotArm m_Arm)
        {
            Robot robot = LgcForm.GetRobotById(1);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById(m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.PutStandby;
                action = RobotAction.PutWait;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone , action
                    , ActionTarget.Eq, m_EqId, 1, true);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool GetVasStandby()
        {
            Robot robot = LgcForm.GetRobotById(1);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)EqId.VAS).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.GetStandby;
                action = RobotAction.GetWait;
                List<string> para = new List<string>();
                para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, action
                    , ActionTarget.Eq, (int)EqId.VAS, 1, true);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool PutVasStandby(bool isSlot1)
        {
            Robot robot = LgcForm.GetRobotById(1);
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)EqId.VAS).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                RobotAction action = RobotAction.None;
                if (isSlot1)
                {
                    robot_command = APIEnum.RobotCommand.PutStandby;
                    action = RobotAction.PutWait;
                }
                else
                {
                    robot_command = APIEnum.RobotCommand.TopPutStandby;
                    action = RobotAction.TopPutWait;
                }

                List<string> para = new List<string>();
                if (isSlot1)
                    para.Add(((int)RobotArm.rbaUp).ToString());
                else
                    para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                //para.Add(isSlot1 ? "1" : "2");
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (isSlot1)
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)EqId.VAS, 1, true);
                }
                else
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)EqId.VAS, 2, true);
                }
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetVas(int m_Step, bool m_UseHS = true)
        {
            Robot robot = LgcForm.GetRobotById(1);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)EqId.VAS).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.GetStandbyArmExtend;
                if (m_Step == 1)
                {
                    robot_command = APIEnum.RobotCommand.GetStandbyArmExtend;
                    action = RobotAction.GetStandbyArmExtend;
                }
                if (m_Step == 2)
                {
                    robot_command = APIEnum.RobotCommand.WaferGet;
                    action = RobotAction.Get;
                }
                List<string> para = new List<string>();
                para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, action
                    , ActionTarget.Eq, (int)EqId.VAS, 1, m_UseHS);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        #endregion

        #region Port slot to access
        public int GetPortFree(CommonData.HIRATA.ProductCategory m_Type, int m_Port)
        {
            int slot = 0;
            Port port = LgcForm.GetPortById(m_Port);
            if (port.PLotStatus == LotStatus.Process)
            {
                for (int index = 1; index <= port.cv_SlotCount; index++)
                {
                    if (!port.cv_Data.GlassDataMap[index].PHasSensor)
                    {
                        if (!port.cv_Data.GlassDataMap[index].PHasSensor)
                        {
                            slot = index;
                            break;
                        }
                    }
                }
            }
            return slot;
        }
        public int GetBufferFree(CommonData.HIRATA.ProductCategory m_Type)
        {
            int slot = 0;
            Buffer buffer = LgcForm.GetBufferById(1);
            if (m_Type == ProductCategory.Wafer)
            {
                for (int index = 1; index <= 3; index++)
                {
                    if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                    {
                        if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                        {
                            slot = index;
                            break;
                        }
                    }
                }
            }
            else if (m_Type == ProductCategory.Wafer)
            {
                for (int index = 4; index <= 6; index++)
                {
                    if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                    {
                        if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                        {
                            slot = index;
                            break;
                        }
                    }
                }
            }
            return slot;
        }
        #endregion
        private RobotArm GetRobotArmEnumString(string m_Arm)
        {
            RobotArm arm = RobotArm.rabNone;
            if (Regex.Match(m_Arm, @"up", RegexOptions.IgnoreCase).Success)
            {
                arm = RobotArm.rbaUp;
            }
            else if (Regex.Match(m_Arm, @"low", RegexOptions.IgnoreCase).Success)
            {
                arm = RobotArm.rbaDown;
            }
            else if (Regex.Match(m_Arm, @"both", RegexOptions.IgnoreCase).Success)
            {
                arm = RobotArm.rbaBoth;
            }
            return arm;
        }
        protected void layoutInit()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            int eq_number = CommonData.HIRATA.CommonStaticData.g_EqNumber;
            int port_number = CommonData.HIRATA.CommonStaticData.g_PortNumber;
            int robot_number = CommonData.HIRATA.CommonStaticData.g_RobotNumber;
            int buffer_number = CommonData.HIRATA.CommonStaticData.g_BufferNumber;
            int aligner_number = CommonData.HIRATA.CommonStaticData.g_AlignerNumber;

            for (int i = 0; i < eq_number; ++i)
            {
                int eq_no = i + 1;
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Capacity"].Trim());
                int time_chat_id = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["TimeChat"].Trim());
                int position = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Stage"].Trim());
                int node = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Node"].Trim());
                string tool_id = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["ToolId"].Trim();
                string get_arm = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["GetArm"].Trim();
                string put_arm = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["PutArm"].Trim();

                Eq eq_control = new Eq(eq_no, node, max_slot, GetRobotArmEnumString(get_arm), GetRobotArmEnumString(put_arm), tool_id);
                eq_control.cv_Comm.cv_TimeChatId = time_chat_id;
                eq_control.cv_Comm.cv_RobotPosition = position;
                eq_control.cv_Data.LoadFromFile();
                eq_control.cv_Data.SaveToFile();
                cv_EqContainer.Add(eq_no, eq_control);
            }
            for (int i = 0; i < aligner_number; ++i)
            {
                int eq_no = i + 1;
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_AlignerXml.Items[i].Attributes["Capacity"].Trim());
                Aligner aligner_control = new Aligner(eq_no, max_slot);
                aligner_control.cv_Data.LoadFromFile();
                aligner_control.cv_Data.SaveToFile();
                cv_AlignerContainer.Add(eq_no, aligner_control);
            }

            for (int i = 0; i < port_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_PortXml.Items[i].Attributes["Capacity"].Trim());
                int port_no = i + 1;
                Port port_control = new Port(port_no, max_slot);
                port_control.cv_Data.LoadFromFile();
                port_control.cv_Data.SaveToFile();
                cv_PortContainer.Add(port_no, port_control);
            }

            for (int i = 0; i < buffer_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Capacity"].Trim());
                int position = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Stage"].Trim());
                int buffer_no = i + 1;
                Buffer buffer_control = new Buffer(buffer_no, max_slot);
                buffer_control.cv_Data.SetSlotType(BufferSlotType.Wafer, BufferSlotType.Wafer, BufferSlotType.Wafer, BufferSlotType.Glass, BufferSlotType.Glass, BufferSlotType.Glass);
                buffer_control.cv_Data.LoadFromFile();
                buffer_control.cv_Data.SaveToFile();
                buffer_control.cv_Comm.cv_RobotPosition = position;

                cv_BufferContainer.Add(i + 1, buffer_control);
            }

            for (int i = 0; i < robot_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Capacity"].Trim());
                int robot_no = i + 1;
                string ip = CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["IP"].Trim();
                int socket_port = Convert.ToInt32(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Port"].Trim());
                Robot robot_control = new Robot(robot_no, max_slot, ip, socket_port);
                robot_control.cv_Data.LoadFromFile();
                robot_control.cv_Data.SaveToFile();
                cv_RobotContainer.Add(robot_no, robot_control);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        internal static Port GetPortById(int m_Index)
        {
            Port rtn = null;
            if (cv_PortContainer.ContainsKey(m_Index))
            {
                rtn = cv_PortContainer[m_Index];
            }
            return rtn;
        }
        internal static Eq GetEqById(int m_Id)
        {
            Eq rtn = null;
            if (cv_EqContainer.ContainsKey(m_Id))
            {
                rtn = cv_EqContainer[m_Id];
            }
            return rtn;
        }
        internal static Eq GetEqByNode(int m_Node)
        {
            Eq rtn = null;
            List<Eq> eq_list = cv_EqContainer.Values.ToList();
            int index = eq_list.FindIndex(x => x.PNode == m_Node);
            if (index != -1)
            {
                rtn = eq_list[index];
            }
            return rtn;
        }
        internal static Robot GetRobotById(int i)
        {
            Robot rtn = null;
            if (cv_RobotContainer.ContainsKey(i))
            {
                rtn = cv_RobotContainer[i];
            }
            return rtn;
        }
        internal static Buffer GetBufferById(int i)
        {
            Buffer rtn = null;
            if (cv_BufferContainer.ContainsKey(i))
            {
                rtn = cv_BufferContainer[i];
            }
            return rtn;
        }
        internal static Aligner GetAlignerById(int i)
        {
            Aligner rtn = null;
            if (cv_AlignerContainer.ContainsKey(i))
            {
                rtn = cv_AlignerContainer[i];
            }
            return rtn;
        }
        internal static void ShowMsg(string m_Txt, bool m_AutoClean, bool m_UseReply, int m_Timeout = 30000)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Enter);
            if (Global.Controller != null)
            {
                CommonData.HIRATA.MDShowMsg obj = new MDShowMsg();
                CommonData.HIRATA.Msg msg_obj = new Msg();
                msg_obj.PAutoClean = m_AutoClean;
                msg_obj.PUserRep = m_UseReply;
                msg_obj.TimeOut = (uint)m_Timeout;
                msg_obj.Txt = m_Txt;
                obj.Msg = msg_obj;
                Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDShowMsg).Name, obj);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Leave);
        }
        public static void EditAlarm(CommonData.HIRATA.AlarmItem m_Alarm , bool m_IsApi = false)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Enter);
            if (m_Alarm.PStatus == AlarmStatus.Occur)
            {
                if (!cv_Alarms.cv_AlarmList.Exists(x => x.PCode == m_Alarm.PCode))
                {
                    m_Alarm.PTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    cv_Alarms.AddAlarm(m_Alarm);
                    WriteAlarmLog(m_Alarm);
                    if (m_Alarm.PLevel == AlarmLevele.Serious)
                    {
                        AddBuzzerCommand(true);
                    }
                }
            }
            else if (m_Alarm.PStatus == AlarmStatus.Clean)
            {
                if (cv_Alarms.cv_AlarmList.Exists(x => x.PCode == m_Alarm.PCode))
                {
                    cv_Alarms.DelAlarm(m_Alarm);
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Leave);
        }
        public static void EditAlarm(List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Enter);
            foreach (CommonData.HIRATA.AlarmItem m_Alarm in m_Alarms)
            {
                if (m_Alarm.PStatus == AlarmStatus.Occur)
                {
                    if (!cv_Alarms.cv_AlarmList.Exists(x => x.PCode == m_Alarm.PCode))
                    {
                        m_Alarm.PTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        LgcForm.cv_Alarms.AddAlarm(m_Alarm);
                        WriteAlarmLog(m_Alarm);
                    }
                }
                else if (m_Alarm.PStatus == AlarmStatus.Clean)
                {
                    int index = cv_Alarms.cv_AlarmList.FindIndex(x => x.PCode == m_Alarm.PCode);
                    if (index != -1)
                    {
                        LgcForm.cv_Alarms.DelAlarm(m_Alarm);
                    }
                }
            }
            CheckSystemStatus();
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Leave);
        }
        public static void LoadAlarmTable()
        {
            if (cv_ApiAlarm == null)
            {
                cv_ApiAlarm = new Dictionary<string,List<AlarmItem>>();
            }
            cv_ApiAlarm.Clear();
            for(int i=1 ; i<=10 ; i++)
            {
                if(i<10)
                cv_ApiAlarm[i.ToString().PadLeft(2,'0')] = new List<AlarmItem>();
                else
                cv_ApiAlarm[i.ToString()] = new List<AlarmItem>();
            }
            KXmlItem file = new KXmlItem();

            string file_path = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + "\\" +
            CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\Alarm.xml";
            file.LoadFromFile(file_path);
            int index = 0;
            int alarm_count = file.ItemsByName["Data"].ItemNumber;
            Match match = Match.Empty;
            while(index < alarm_count )
            {
                KXmlItem xml = file.ItemsByName["Data"].Items[index];
                AlarmItem tmp = new AlarmItem();
                tmp.cv_ApiTypeCode = xml.Attributes["Type"].Trim();
                tmp.PCode = xml.Attributes["RepCode"].Trim();
                string level = xml.Attributes["Level"].Trim();
                if(level == "L")
                tmp.PLevel =AlarmLevele.Light;
                else if(level == "S")
                tmp.PLevel =AlarmLevele.Serious;
                match = Match.Empty;
                match = Regex.Match(xml.Attributes["Device"].Trim(), @"\D*", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (Enum.IsDefined(typeof(APIEnum.CommnadDevice), match.Value))
                    {
                        tmp.PCommandDevice = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), match.Value);
                    }
                    else
                    {
                        int a = 19;
                    }
                }
                else
                {
                    int a = 9;
                }
                tmp.PMainDescription = xml.Attributes["Msg"].Trim();
                tmp.cv_ResCode = xml.Attributes["DeviceCode"].Trim();
                tmp.PUnit = Convert.ToInt16(xml.Attributes["Unit"].Trim());
                cv_ApiAlarm[tmp.cv_ApiTypeCode].Add(tmp);
                index++;
            }
        }
        public static void CheckSystemStatus()
        {
            /*
            if (LgcForm.cv_Alarms.IsHasAlarm() ||
                !LgcForm.PSystemData.PRobotConnect
                )
            {
                LgcForm.PSystemData.PSystemStatus = EquipmentStatus.Down;
                LgcForm.PSystemData.POperationMode = OperationMode.Manual;
                if (LgcForm.PSystemData.PRobotInline != EquipmentInlineMode.Local &&
                    LgcForm.PSystemData.PRobotInline != EquipmentInlineMode.None)
                {
                    if (LgcForm.PSystemData.PRobotInline == EquipmentInlineMode.Remote)
                    {
                        LgcForm.PSystemData.PRobotInline = EquipmentInlineMode.WaitLocal;
                        //Form1.GetRobot(1).cv_Comm.SetRobotInline(EquipmentInlineMode.Local);
                    }
                    LgcForm.GetRobotById(1).SetBuzzer(true);
                }
            }
            else if ((LgcForm.PSystemData.PRobotConnect))// && Form1.PSystemData.PRobotInline == EquipmentInlineMode.Local))
            {
                LgcForm.PSystemData.PSystemStatus = EquipmentStatus.Idle;
                LgcForm.PSystemData.POperationMode = OperationMode.Manual;
            }
            /*
        else if ((Form1.PSystemData.PRobotConnect && Form1.PSystemData.PRobotInline == EquipmentInlineMode.Remote))
        {
            Form1.PSystemData.PSystemStatus = EquipmentStatus.Idle;
        }
        */
        }
        public static bool CheckAllPortResetError()
        {
            bool rtn = true;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                if (!GetPortById(i).PIsResetError)
                {
                    return false;
                }
            }
            return rtn;
        }
        public static bool CheckAllPortHome()
        {
            bool rtn = true;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = GetPortById(i);
                if (port.cv_Data.PPortHasCst == PortHasCst.Has)
                {
                    if (!GetPortById(i).PIsHome)
                    {
                        return false;
                    }
                }
            }
            return rtn;
        }
        public static bool CheckAllPortStatus()
        {
            bool rtn = true;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                if (!GetPortById(i).PIsStatus)
                {
                    return false;
                }
            }
            return rtn;
        }
        public static void SendinitComplete()
        {
            Robot rb = GetRobotById(1);
            rb.cv_Initilizing = false;
            rb.cv_HadInit = true;
            PSystemData.PInitaiizeOk = true;
            PSystemData.PInitaiizing = false;
            cv_MmfController.SendInitialize(InitialAction.Complete, MmfEventClientEventType.etNotify, false);
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = GetPortById(i);
                if (port.PPortStatus == PortStaus.LDCM)
                {
                    if (port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (port.PLotStatus == LotStatus.Process)
                        {
                            AddPortToProcessList(i);
                        }
                    }
                    else if (port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (port.PLotStatus == LotStatus.Process || port.PLotStatus == LotStatus.Reserved)
                        {
                            AddPortToProcessList(i);
                        }
                    }
                }
            }
        }
        public static void SendinitCompleteFail()
        {
            Robot rb = GetRobotById(1);
            GetRobotById(1).PIsStatus = false;
            GetRobotById(1).PIsHome = false;
            GetAlignerById(1).PIsStatus = false;
            GetAlignerById(1).PIsHome = false;
            GetBufferById(1).PIsStatus = false;
            GetBufferById(1).PIsHome = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                GetPortById(i).PIsStatus = false;
                GetPortById(i).PIsHome = false;
                GetPortById(i).PIsResetError = false;
                GetPortById(i).PIsStatus = false;
            }

            PSystemData.PInitaiizeOk = false;
            PSystemData.PInitaiizing = false;
            rb.cv_Initilizing = false;
            rb.cv_HadInit = false;
            cv_MmfController.SendInitialize(InitialAction.Complete, MmfEventClientEventType.etNotify, false);
        }
        public bool hasGlassPort()
        {
            bool rtn = false;
            int jos_count = cv_InProcessPort.Count;
            for (int i = 0; i < jos_count; i++)
            {
                Port port = GetPortById(cv_InProcessPort[i]);
                if (port.PLotStatus == LotStatus.Process && port.PPortStatus == PortStaus.LDCM)
                {
                    if (port.cv_Data.PProductionType == ProductCategory.Glass)
                        rtn = true;
                }
            }
            return rtn;
        }
        public bool hasWaferPort()
        {
            bool rtn = false;
            int jos_count = cv_InProcessPort.Count;
            for (int i = 0; i < jos_count; i++)
            {
                if (GetPortById(cv_InProcessPort[i]).PLotStatus == LotStatus.Process && GetPortById(cv_InProcessPort[i]).PPortStatus == PortStaus.LDCM)
                {
                    if (GetPortById(cv_InProcessPort[i]).cv_Data.PProductionType == ProductCategory.Wafer)
                        rtn = true;
                }
            }
            return rtn;
        }
        public static void WritePortToPlc(int m_PortId)
        {
            WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, "WritePortToPlc", CommonData.HIRATA.FunInOut.Enter);
            string log = "[Write Port To PLC Port : " + m_PortId + " ]\n";
            int start = 0;
            if (m_PortId == 1) start = 0x355c;
            if (m_PortId == 2) start = 0x359a;
            if (m_PortId == 3) start = 0x35D8;
            if (m_PortId == 4) start = 0x3616;
            if (m_PortId == 5) start = 0x3654;
            if (m_PortId == 6) start = 0x3692;
            Port port = GetPortById(m_PortId);
            int value = ((int)port.cv_Data.PPortStatus << 4) + (1 << 8) + (m_PortId << 12) + (int)port.cv_Data.PPortMode;
            log += "Port Status : " + port.cv_Data.PPortStatus.ToString() + "\n";
            log += "Lot Status : " + port.PLotStatus.ToString() + "\n";
            log += "Port Mode : " + port.cv_Data.PPortMode.ToString() + "\n";
            log += "Port ProductionType : " + port.cv_Data.PProductionType.ToString() + "\n";
            cv_Mio.SetPortValue(start + 0, value);

            value = (int)port.PLotStatus;

            cv_Mio.SetPortValue(start + 1, value);

            cv_Mio.SetPortValue(start + 2, 0);

            value = (int)port.cv_Data.PProductionType;
            cv_Mio.SetPortValue(start + 3, value);

            int work_count = 0;
            value = 0;

            for (int i = 1; i <= 16; i++)
            {
                if (i <= GetPortById(m_PortId).cv_Data.cv_SlotCount)
                {
                    value += (Convert.ToInt32(port.cv_Data.GlassDataMap[i].PHasSensor) << (i - 1));
                    port.cv_Data.GlassDataMap[i].WriteWokeNoOnly(cv_Mio, start + 12 + 2 * (i - 1));
                    log += "Slot : " + i.ToString() + "CIM Mode : " + port.cv_Data.GlassDataMap[i].PCimMode.ToString();
                    log += " Foup Seq : " + port.cv_Data.GlassDataMap[i].PFoupSeq.ToString();
                    log += " Work Order No : " + port.cv_Data.GlassDataMap[i].PWorkOrderNo.ToString();
                    log += " Work Slot : " + port.cv_Data.GlassDataMap[i].PWorkSlot.ToString() + "\n";
                    if (port.cv_Data.GlassDataMap[i].PHasSensor)
                    {
                        work_count++;
                    }
                }
            }
            cv_Mio.SetPortValue(start + 4, value);
            log += "Slot 1-16 : " + SysUtils.IntToHex(value) + "\n";

            value = 0;
            for (int i = 17; i <= 25; i++)
            {
                if (i <= GetPortById(m_PortId).cv_Data.cv_SlotCount)
                {
                    value += (Convert.ToInt32(port.cv_Data.GlassDataMap[i].PHasSensor) << (i - 16 - 1));
                    port.cv_Data.GlassDataMap[i].WriteWokeNoOnly(cv_Mio, start + 12 + 2 * (i - 1));
                    log += "Slot : " + i.ToString() + "CIM Mode : " + port.cv_Data.GlassDataMap[i].PCimMode.ToString();
                    log += " Foup Seq : " + port.cv_Data.GlassDataMap[i].PFoupSeq.ToString();
                    log += " Work Order No : " + port.cv_Data.GlassDataMap[i].PWorkOrderNo.ToString();
                    log += " Work Slot : " + port.cv_Data.GlassDataMap[i].PWorkSlot.ToString() + "\n";
                    if (port.cv_Data.GlassDataMap[i].PHasSensor)
                    {
                        work_count++;
                    }
                }
            }
            log += "Slot 17-25 : " + SysUtils.IntToHex(value) + "\n";
            log += "work count : " + work_count + "\n";
            cv_Mio.SetPortValue(start + 5, value);
            cv_Mio.SetPortValue(start + 6, work_count);

            log += "lot Id : " + port.cv_Data.PLotId + "\n";
            string id = SysUtils.GetFixedLengthString(port.cv_Data.PLotId, 10);
            cv_Mio.SetBinaryLengthData(start + 7, SysUtils.StringToByteArray(id), 5);

            WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
            WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, "WritePortToPlc", CommonData.HIRATA.FunInOut.Leave);
        }
        private bool CheckEqSideData(GlassData data , EqId m_Eq)
        {
            bool rtn = true;
            if (PSystemData.IsCheckRecipe)
            {
                int index = data.cv_Nods.FindIndex(x => x.cv_NodeId == 2);
                GlassDataNodeItem node = data.cv_Nods[index];
                if (node.cv_Recipe != Convert.ToInt32(LgcForm.cv_Recipes.PCurRecipeId))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RecieUnmatch.ToString();
                    alarm.PMainDescription = "Recv From upstream recipe unmatch";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    //ShowMsg("Recv form upstream recipe un-match", true, false);
                    rtn = false;
                }
            }
            if (PSystemData.IsCheckSeq)
            {
                if (data.PFoupSeq == 0)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.FoupSeqError.ToString();
                    alarm.PMainDescription = "FoupSeq Error";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    // ShowMsg("Recv form upstream Foup Seq 0", true, false);
                    rtn = false;
                }
            }
            if (PSystemData.IsCheckSlot)
            {
                if (data.PWorkSlot == 0)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.WorkSlotError.ToString();
                    alarm.PMainDescription = "Work slot Error";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    // ShowMsg("Recv form upstream slot 0", true, false);
                    rtn = false;
                }
            }
            if (PSystemData.IsCheckId)
            {
                if (string.IsNullOrEmpty(data.PId.Trim().Trim('\0')))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.WorkIdError.ToString();
                    alarm.PMainDescription = "Work Id Error";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    // ShowMsg("Recv form upstream Id empty", true, false);
                    rtn = false;
                }
            }
            if ((PSystemData.PSystemOnlineMode == OnlineMode.Control) ||
                ((PSystemData.PSystemOnlineMode == OnlineMode.Offline) && cv_CheckEqDataLocalMode)
                )
            {
                int node_index = data.cv_Nods.FindIndex(x => x.PNodeId == 2);
                if (node_index != -1)
                {
                    int recipe = data.cv_Nods[node_index].cv_Recipe;
                    if (recipe != Convert.ToInt32(cv_Recipes.PCurRecipeId.Trim()))
                    {
                        AlarmItem alarm = new AlarmItem();
                        alarm.PCode = Alarmtable.InterfaceErrorGlassDataRecipeUnmatch.ToString();
                        alarm.PLevel = AlarmLevele.Light;
                        alarm.PMainDescription = "Interface GlassData Error Recipe Unmatch with EFEM!!!";
                        alarm.PSubDescription = "EQ : " + m_Eq;
                        alarm.PStatus = AlarmStatus.Occur;
                        EditAlarm(alarm);
                        //ShowMsg(alarm.PMainDescription + "\nRecipe from EQ : " + recipe + "EFEM Cur. recipe : " + cv_Recipes.PCurRecipeId.Trim(), false, false);
                        WriteLog(LogLevelType.Warning, alarm.PMainDescription + "\nRecipe from EQ : " + recipe + " EFEM Cur. recipe : " + cv_Recipes.PCurRecipeId.Trim());
                        rtn = false;
                    }
                }
            }
            return rtn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            for (int i = 3 , j=1; i < 7; i++,  j++)
            {
                GlassData tmp = new GlassData();
                tmp.PCimMode = OnlineMode.Control;
                tmp.PFoupSeq = 1;
                tmp.PWorkOrderNo = 1;
                tmp.PWorkSlot = 3;
                tmp.PId = "RSI1M145RX" ;

                CommonData.HIRATA.MDBCWorkTransferReport obj = new MDBCWorkTransferReport();
                obj.PAction = DataFlowAction.Store;
                obj.PGlassData = tmp;
                obj.PPortNo = 2;
                obj.PSlotNo = 3;
                obj.PUnitNo = 0;
                obj.PType = MmfEventClientEventType.etNotify;
                cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkTransferReport).Name, obj, KParseObjToXmlPropertyType.Field);
            }
            */
        }
        public static string FindHightestPriorityPPID(int m_PortId)
        {
            //Args : 1.priority , 2.slot
            Dictionary<int, int> ppid_sort = new Dictionary<int, int>();
            Port port = GetPortById(m_PortId);
            int max_priority = 0;
            for (int slot = 1; slot <= port.cv_Data.cv_SlotCount; slot++)
            {
                GlassData glass = port.cv_Data.GlassDataMap[slot];
                if (glass.PHasData && glass.PHasSensor && glass.PProcessFlag == ProcessFlag.Need)
                {
                    if (!ppid_sort.ContainsKey((int)glass.PPriority))
                    {
                        ppid_sort.Add((int)glass.PPriority, slot);
                        if (glass.PPriority > max_priority)
                        {
                            max_priority = (int)glass.PPriority;
                        }
                    }
                }
            }
            return port.cv_Data.GlassDataMap[ppid_sort[max_priority]].PPID.Trim();
        }
        public static bool FindHightestSlotForPPID(string m_Ppid, int m_PortId, out int m_Slot)
        {
            bool rtn = false;
            Port port = GetPortById(m_PortId);

            //Args : 1.priority , 2.slot
            Dictionary<int, int> slot_sort = new Dictionary<int, int>();
            int max_priority = -1;
            for (int slot = 1; slot <= port.cv_Data.cv_SlotCount; slot++)
            {
                GlassData glass = port.cv_Data.GlassDataMap[slot];
                if (glass.PHasSensor && glass.PHasData)
                {
                    if (glass.PPID.Trim() == m_Ppid.Trim() && glass.POcrResult == OCRResult.None && glass.PProcessFlag == ProcessFlag.Need)
                    {
                        if (!slot_sort.ContainsKey((int)glass.PPriority))
                        {
                            slot_sort.Add((int)glass.PPriority, slot);
                            if ((int)glass.PPriority > max_priority)
                            {
                                max_priority = (int)glass.PPriority;
                            }

                            rtn = true;
                        }
                    }
                }
            }
            m_Slot = 0;
            if (rtn)
            {
                m_Slot = slot_sort[max_priority];
            }
            return rtn;
        }

        private void LgcForm_Shown(object sender, EventArgs e)
        {
            Hide();
        }
        public static void AdjustRobotJob(EqInterFaceType m_Action, int m_TimeChartId)
        {
            if (LgcForm.cv_RobotManaulJobPath.Count != 0)
            {
                RobotJob job = LgcForm.cv_RobotJobPath.Peek();
                WriteJobLog("Adjust manual job", job);
                LgcForm.cv_RobotJobPath.Dequeue();
            }
            else if (LgcForm.cv_RobotJobPath.Count != 0)
            {
                RobotJob job = LgcForm.cv_RobotJobPath.Peek();
                Robot robot = LgcForm.GetRobotById(1);
                WriteJobLog("Adjust Auto job", job);
                //
                if (job.PTarget != ActionTarget.Eq)
                {
                    return;
                }

                EqId eq_id = (EqId)(int)job.PTargetId;
                int slot = job.PTargetSlot;
                int eq_time_chart_cur_step = 0;
                int time_chart_id = -1;
                TimechartNormal time_chart_instance = null;

                if (eq_id == EqId.VAS)
                {
                    if (slot == 1)
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                        time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    }
                    else if (slot == 2)
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                        time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    }
                }
                else
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                    time_chart_id = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                }
                if (m_TimeChartId != time_chart_id)
                {
                    return;
                }
                //
                if (m_Action == EqInterFaceType.Load && job.PAction == RobotAction.Put)
                {
                    if (!robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor)
                    {
                        LgcForm.cv_RobotJobPath.Dequeue();
                    }
                }
                else if (m_Action == EqInterFaceType.Unload && job.PAction == RobotAction.Get)
                {
                    if (robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor)
                    {
                        LgcForm.cv_RobotJobPath.Dequeue();
                        SendRobotJobPath();
                    }
                }
                else if (m_Action == EqInterFaceType.Exchange && job.PAction == RobotAction.Exchange)
                {
                    if (robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor
                        )
                    {
                        LgcForm.cv_RobotJobPath.Dequeue();
                        SendRobotJobPath();
                    }
                    else if (robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor
                        )
                    {
                        job.PAction = RobotAction.Put;
                        SendRobotJobPath();
                    }
                }
            }
        }
        public static void WriteJobLog(string m_Prefix, RobotJob m_Job)
        {
            LgcForm.WriteLog(LogLevelType.General, "[WriteJobLog][" + m_Prefix + "] : " + m_Job.PAction.ToString() +
                " " + m_Job.PTarget.ToString() + " : " + m_Job.PTargetId + " slot : " + m_Job.PTargetSlot +
                "  Get arm : " + m_Job.PGetArm.ToString() + " Put arm : " + m_Job.PPutArm.ToString());
        }
        public static bool CheckAutoJobPathOk(int index)
        {
            bool ok = true;
            RobotJob cur_job = cv_RobotJobPath.ElementAt(index);
            RobotJob pre_job = cv_RobotJobPath.ElementAt(index - 1);
            if (cur_job.PAction == RobotAction.Put)
            {
                if (pre_job.PAction == RobotAction.Put)
                {
                    ok = false;
                }
                else if (pre_job.PAction == RobotAction.Get || pre_job.PAction == RobotAction.Exchange)
                {
                    if (pre_job.PGetArm != cur_job.PPutArm)
                    {
                        ok = false;
                    }
                }
            }
            else if (cur_job.PAction == RobotAction.Get)
            {
                if (pre_job.PAction == RobotAction.Get)
                {
                    ok = false;
                }
                else if (pre_job.PAction == RobotAction.Put || pre_job.PAction == RobotAction.Exchange)
                {
                    if (cur_job.PTarget == ActionTarget.Aligner && pre_job.PTarget == ActionTarget.Aligner)
                    {

                    }
                    else if (pre_job.PPutArm != cur_job.PGetArm)
                    {
                        ok = false;
                    }
                }
            }
            else if (cur_job.PAction == RobotAction.Exchange)
            {
                if (pre_job.PAction == RobotAction.Get)
                {
                    if (cur_job.PPutArm != pre_job.PGetArm)
                    {
                        ok = false;
                    }
                }
                else if (pre_job.PAction == RobotAction.Exchange)//|| pre_job.PAction == RobotAction.Exchange)
                {
                    if (cur_job.PTarget == ActionTarget.Aligner)
                    {
                        if (pre_job.PGetArm != cur_job.PPutArm)
                        {
                            ok = false;
                        }
                    }
                    else
                    {
                        if (pre_job.PTarget != ActionTarget.Aligner)
                        {
                            if (pre_job.PPutArm != cur_job.PGetArm)
                            {
                                ok = false;
                            }
                            if (pre_job.PGetArm != cur_job.PPutArm)
                            {
                                ok = false;
                            }
                        }
                    }
                }
            }
            if (index != 1)
            {
                ok = CheckAutoJobPathOk(index - 1);
            }
            return ok;
        }
        public static bool CheckIsVasPutUpSlotJobStatus(RobotJob m_job)
        {
            bool rtn = false;
            if (m_job.PAction == RobotAction.Put && m_job.PTarget == ActionTarget.Eq && m_job.PTargetId == (int)EqId.VAS && m_job.PTargetSlot == 2)
            {
                EqId eq_id = EqId.VAS;
                int slot = 2;
                int eq_time_chart_cur_step = 0;
                EqInterFaceType gif_type = EqInterFaceType.None;
                int time_chart_id = -1;
                TimechartNormal time_chart_instance = null;

                if (eq_id == EqId.VAS)
                {
                    if (slot == 2)
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                        time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                        if (eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitRobotPutEnd || eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitRobotCommandFinish ||
                            eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitEqCompleteOn)
                        {
                            rtn = true;
                        }
                    }
                }

            }
            return rtn;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RobotJob job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Buffer, 1, 2, false);
            cv_RobotJobPath.Enqueue(job);

            job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rbaUp, RobotAction.Exchange, ActionTarget.Aligner,1, 1, false);
            cv_RobotJobPath.Enqueue(job);

            job = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, 6, 1, false);
            cv_RobotJobPath.Enqueue(job);
        }
    }

    public class TowerCommand
    {
        public SignalTowerColor cv_Color;
        public SignalTowerControl cv_Control;
        public bool cv_HadSend = false;
        public TowerCommand(SignalTowerColor m_Color, SignalTowerControl m_Control)
        {
            cv_Control = m_Control;
            cv_Color = m_Color;
        }
    }
}