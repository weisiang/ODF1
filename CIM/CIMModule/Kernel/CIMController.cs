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
using CommonData;
using CommonData.HIRATA;
using BaseAp;

namespace CIM
{
    class CIMController : BaseMmfController
    {
        public KTimeCharts cv_TimeChart;
        TimechartController cv_TimechartController;

        KMemoryIOClient cv_Driver;
        //KTimer cv_Timer;
        public static CIMController g_Controller = null;

        public CIMController() : base("CommonData.HIRATA" , "CIM")
        {
            InitTimeChart();
            AssignProcessFunctions();
            //InitTimer();
            g_Controller = this;
        }
        ~CIMController()
        {
            /*
            if(cv_Timer.Enabled)
            {
                cv_Timer.Close();
            }
            */
        }
        void InitTimer()
        {
            /*
            cv_Timer = new KTimer();
            cv_Timer.Interval = 1000;
            cv_Timer.OnTimer += Timer_OnTimer;
            cv_Timer.ThreadEventEnabled = false;
            cv_Timer.Enabled = true;
            cv_Timer.Open();
            */
        }
        void InitTimeChart()
        {

            //cv_TimechartController = new TimechartController(Global.ConfigPathname + "\\timecharts.xml");
            cv_TimechartController = new TimechartController(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + "\\" +
               CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\timecharts.xml");
            cv_TimeChart = cv_TimechartController.GetTimeChart();
            cv_Driver = cv_TimechartController.GetmemoryIoClient();
        }
        void WriteTimeToPLC()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            DateTime tmp = DateTime.Now;
            string year_mon = "0x" + (tmp.Year - 2000).ToString() + (tmp.Month.ToString().PadLeft(2,'0'));
            string day_hour = "0x" + tmp.Day.ToString().PadLeft(2,'0') + (tmp.Hour.ToString().PadLeft(2,'0'));
            string min_sec = "0x" + tmp.Minute.ToString().PadLeft(2,'0') + (tmp.Second.ToString().PadLeft(2, '0'));
            string week = "0x" + ((int)tmp.DayOfWeek).ToString();
            byte[] tmp2 = new byte[8];

            tmp2[0] =Convert.ToByte(Convert.ToInt32(year_mon, 16) & 0x00ff );
            tmp2[1] =Convert.ToByte((Convert.ToInt32(year_mon, 16) & 0xff00) >> 8 );
            tmp2[2] = Convert.ToByte(Convert.ToInt32(day_hour, 16) & 0x00ff);
            tmp2[3] = Convert.ToByte((Convert.ToInt32(day_hour, 16) & 0xff00) >> 8);
            tmp2[4] = Convert.ToByte(Convert.ToInt32(min_sec, 16) & 0x00ff);
            tmp2[5] = Convert.ToByte((Convert.ToInt32(min_sec, 16) & 0xff00) >> 8);
            tmp2[6] = Convert.ToByte(Convert.ToInt32(week, 16) & 0x00ff);
            tmp2[7] = 0;
            cv_Driver.SetBinaryLengthData(0x3440, tmp2, 4, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void Timer_OnTimer()
        {
            /*
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                WriteTimeToPLC();
            }
            catch
            {
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            */
        }

        #region base
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
            base.ProcessAlarmChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);

            byte[] tmp = new byte[1 << 1];
            tmp[0] = 0;
            tmp[1] = Convert.ToByte(((CimForm.cv_Alarms.IsHasAlarm() ? 1 : 0) << 4) + (CimForm.cv_Alarms.IsHasWarning() ? 1 : 0));

            cv_Driver.SetBinaryLengthData(0x3445, tmp, 1 , false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            CommonData.HIRATA.MDAlarmAction tmp = m_Object as CommonData.HIRATA.MDAlarmAction;
            cv_TimechartController.GetTimeChartInstance(TimechartEqAlarmReport.TIMECHART_ID_EqAlarmReport).AddJob(tmp.AlarmData);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeAction(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeAction(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRecipeChange(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            cv_TimechartController.GetTimeChartInstance(TimechartRecipeListReport.TIMECHART_ID_EqRecipeListReport).AddJob(m_Object);

            byte[] tmp = new byte[1<<1];
            RecipeItem cur_recipe = null;
            if (CimForm.cv_Recipes.GetCurRecipe(out cur_recipe))
            {
                int value = Convert.ToInt32(cur_recipe.PId.Trim()); // obj.PCurReipe;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                cv_Driver.SetBinaryLengthData(0x3447, tmp, 1 , false);
            }

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
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessInitialize(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSetTimeOut(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessGlassCountData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            GlassCountData obj = m_Object as GlassCountData;
            BaseForm.cv_GlassCountData.Clone(obj);
            byte[] tmp = new byte[4<<1];
            int value = BaseForm.cv_GlassCountData.PProductCount;
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);

            value = BaseForm.cv_GlassCountData.PDummyCount;
            tmp[2] = Convert.ToByte(value & 0x00ff);
            tmp[3] = Convert.ToByte((value & 0xff00) >> 8);

            value = BaseForm.cv_GlassCountData.PHistoryCount;
            tmp[4] = Convert.ToByte(value & 0x000000ff);
            tmp[5] = Convert.ToByte((value & 0x0000ff00) >> 8);
            tmp[6] = Convert.ToByte((value & 0x00ff0000) >> 16);
            tmp[7] = Convert.ToByte((value & 0xff000000) >> 24);
            cv_Driver.SetBinaryLengthData(0x3448, tmp, 4 , false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSystemData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSystemData( m_SourceModule,  m_Type,  m_MessageId, m_RequestNotifyMessageId,  m_Ticket,  m_Object);

            //Equipment status
            byte[] tmp = new byte[10];
            Array.Clear(tmp, 0, tmp.Length);

            if (CimForm.PSystemData.PSystemStatus == EquipmentStatus.WaitIdle)
            {
                cv_Driver.SetPortValue(0x344F, (int)EquipmentStatus.Run);
                tmp[0] = Convert.ToByte((int)EquipmentStatus.Run);
            }
            else
            {
                tmp[0] = Convert.ToByte((int)CimForm.PSystemData.PSystemStatus);
                cv_Driver.SetPortValue(0x344F, (int)CimForm.PSystemData.PSystemStatus);
            }
            cv_Driver.SetBinaryLengthData(0x344F, tmp, 5, false);

            tmp = null;
            tmp = new byte[11 << 1];
            int value = ((CimForm.PSystemData.PSystemOnlineMode == OnlineMode.Control ? 1 : 0) << 4) +
                (CimForm.PSystemData.POperationMode == OperationMode.Manual ? 0 : 1) + (2 << 8);
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);

            cv_Driver.SetBinaryLengthData(0x3444, tmp, 1, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessTimeoutData(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessTimeoutData(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);

            byte[] tmp = new byte[1<<2];
            Array.Clear(tmp, 0, tmp.Length);
            int value = (CimForm.cv_TimeoutData.PIdleDelayTime / 1000 + ((CimForm.cv_TimeoutData.PIntervalTime / 1000) << 12));
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
            cv_Driver.SetBinaryLengthData(0x3446, tmp, 1, false);

            if (CimForm.cv_TimeoutData.PIntervalTime != cv_TimechartController.IntervalTime)
            {
                cv_TimechartController.IntervalTime = CimForm.cv_TimeoutData.PIntervalTime;
            }
            if (CimForm.cv_TimeoutData.PTsTime != cv_TimechartController.TsTime)
            {
                cv_TimechartController.TsTime = CimForm.cv_TimeoutData.PTsTime;
            }
            if (CimForm.cv_TimeoutData.PTeTime != cv_TimechartController.TeTime)
            {
                cv_TimechartController.TeTime = CimForm.cv_TimeoutData.PTeTime;
            }
            if (CimForm.cv_TimeoutData.PTmTime != cv_TimechartController.TmTime)
            {
                cv_TimechartController.TmTime = CimForm.cv_TimeoutData.PTmTime;
            }

            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        protected override void AssignProcessFunctions()
        {
            base.AssignProcessFunctions();
            AssignMmfEventObjectFunction(typeof( CommonData.HIRATA.MDBCEquipmentInfo).Name , ProcessEquipmentInfo);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCWorkTransferReport).Name, ProcessGlassDataTransferReport);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCRemoveReport).Name, ProcessWorkDataRemoveReport);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, ProcessWorkDataUpdateReport);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCLastProcessStartReport).Name, ProcessLastWorkProcessStart);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, ProcessRecipeBodyReport);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCOcrReadReport).Name, ProcessVCRReadOutReport);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCRecipeExistReply).Name, ProcessEqRecipeExistReport);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessWorkDataRequest);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name, ProcessBcAliveAncPlcConnectReq);
        }

        #region process event
        void ProcessBcAliveAncPlcConnectReq(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCAliveAndPlcConnect obj = m_Object as CommonData.HIRATA.MDBCAliveAndPlcConnect;
            obj.PBcAlive = CimForm.BcAlive;
            obj.PPlcConnect = CimForm.PlcConnect;
            Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name, obj, m_Ticket
                , typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        //Equipment status
        void ProcessEquipmentInfo(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCEquipmentInfo obj = m_Object as CommonData.HIRATA.MDBCEquipmentInfo;
            byte[] tmp = new byte[22];
            Array.Clear(tmp, 0, tmp.Length);

            int value = ((int)obj.POperationMode) + (((int)obj.PCimMode) << 4) + (((int)obj.PNodeNo) << 8);
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte( (value & 0xff00) >> 8 );

            value = ( Convert.ToInt32( obj.PAlarm) << 12) + ( Convert.ToInt32( obj.PWarnning) <<8);
            tmp[3] = Convert.ToByte((value & 0xff00) >> 8);

            value = ( Convert.ToInt32( obj.PIdleDelayTime) ) + ( Convert.ToInt32( obj.PIndexInterval) <<12);
            tmp[4] = Convert.ToByte(value & 0x00ff);
            tmp[5] = Convert.ToByte((value & 0xff00) >> 8);

            value = (Convert.ToInt32(obj.PCurrentRecipe));
            tmp[6] = Convert.ToByte(value & 0x00ff);
            tmp[7] = Convert.ToByte((value & 0xff00) >> 8);

            value = (Convert.ToInt32(obj.PCheckRecipe) << 3) + (Convert.ToInt32(obj.PCheckFoupSeq) << 2) + (Convert.ToInt32(obj.PCheckWorkSlot) << 1) + Convert.ToInt32(obj.PCheckWorkId);
            tmp[16] = Convert.ToByte(value & 0x00ff);

            value = (Convert.ToInt32(obj.POcr1Enable) << 4) + ((int)obj.POcr1Mode);
            tmp[18] = Convert.ToByte(value & 0x00ff);
            tmp[19] = Convert.ToByte((value & 0xff00) >> 8);

            value = (Convert.ToInt32(obj.POcr2Enable) << 4) + ((int)obj.POcr2Mode);
            tmp[20] = Convert.ToByte(value & 0x00ff);
            tmp[21] = Convert.ToByte((value & 0xff00) >> 8);

            cv_Driver.SetBinaryLengthData(0x3444, tmp, 11, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessGlassDataTransferReport(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCWorkTransferReport obj = m_Object as CommonData.HIRATA.MDBCWorkTransferReport;
            if(obj.PAction == CommonData.HIRATA.DataFlowAction.Fetch)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqFetchReport).AddJob(m_Object);
            else if (obj.PAction == CommonData.HIRATA.DataFlowAction.Store)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqStoreReport).AddJob(m_Object);
            else if (obj.PAction == CommonData.HIRATA.DataFlowAction.Receive)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqReceiveReport).AddJob(m_Object);
            else if (obj.PAction == CommonData.HIRATA.DataFlowAction.Send)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqSendReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessWorkDataRemoveReport(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqWorkDataRemoveReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessWorkDataUpdateReport(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqWorkDataUpdateReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessLastWorkProcessStart(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqLastWorkProcessStartReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessEqRecipeExistReport(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeExistReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessRecipeBodyReport(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeBodyReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessVCRReadOutReport(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqVCRReadReport.TIMECHART_ID_EqVCRReadReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessWorkDataRequest(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.FAKE tmp = new FAKE();
            SendMmfReplyObject(typeof(CommonData.HIRATA.FAKE).Name, tmp, m_Ticket, typeof(CommonData.HIRATA.MDBCDataRequest).Name , KParseObjToXmlPropertyType.Field);
            cv_TimechartController.GetTimeChartInstance(TimechartEqVCRReadReport.TIMECHART_ID_EqWorkDataRequest).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        #endregion

        #region send mmf event
        public void SendBcAliveAndPlcConnect(bool m_PlcConned , bool m_BcAlive)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCAliveAndPlcConnect obj = new CommonData.HIRATA.MDBCAliveAndPlcConnect();
            obj.PBcAlive = m_BcAlive;
            obj.PPlcConnect = m_PlcConned;
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etNotify;
            this.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCAliveAndPlcConnect).Name, obj);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion
    }
}
