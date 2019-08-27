using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartEqFetchReport : TimechartControllerBase.TimechartInstanceBase
    {
        public static int TIMECHART_ID_BcDateTimeCalibration = 1;
        public static int TIMECHART_ID_BcDisplayMessage = 2;
        public static int TIMECHART_ID_BcFoupDataDownload = 3;
        public static int TIMECHART_ID_BcIdleDelayCommand = 4;
        public static int TIMECHART_ID_BcIndexIntervalCommand = 5;
        public static int TIMECHART_ID_BcPortCommand = 6;
        public static int TIMECHART_ID_BcRecipeBodyQuery = 7;
        public static int TIMECHART_ID_BcRecipeExistCommand = 8;
        public static int TIMECHART_ID_EqAlarmReport = 9;
        public static int TIMECHART_ID_EqFetchReport = 10;
        public static int TIMECHART_ID_EqLastWorkProcessStartReport = 11;
        public static int TIMECHART_ID_EqRecipeListReport = 12;
        public static int TIMECHART_ID_EqReceiveReport = 13;
        public static int TIMECHART_ID_EqRecipeBodyReport = 14;
        public static int TIMECHART_ID_EqRecipeExistReport = 15;
        public static int TIMECHART_ID_EqSendReport = 16;
        public static int TIMECHART_ID_EqStoreReport = 17;
        public static int TIMECHART_ID_EqVCRReadReport = 18;
        public static int TIMECHART_ID_EqWorkDataRemoveReport = 19;
        public static int TIMECHART_ID_EqWorkDataRequest = 20;
        public static int TIMECHART_ID_EqWorkDataUpdateReport = 21;



        public static int STEP_ID_TriggerFetchIndex = 1;
        public static int STEP_ID_WaitTm = 2;
        public static int STEP_ID_WaitInterval = 3;

        public static string FetchIndex = "FetchIndex";

        public TimechartEqFetchReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            AssignEnterStepEventFunction(STEP_ID_TriggerFetchIndex, OnEnter_TriggerFetchIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }

        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqFetchReport ProcessJob]\n";
                CommonData.HIRATA.MDBCWorkTransferReport obj = m_obj as CommonData.HIRATA.MDBCWorkTransferReport;
                byte[] tmp = new byte[26];
                Array.Clear(tmp, 0, tmp.Length);
                int value = (obj.PUnitNo << 12) + (obj.PPortNo << 8) + obj.PSlotNo;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                value = (((int)obj.PGlassData.PCimMode) << 15) + (int)obj.PGlassData.PFoupSeq;
                tmp[2] = Convert.ToByte(value & 0x00ff);
                tmp[3] = Convert.ToByte((value & 0xff00) >> 8);
                value = (((int)obj.PGlassData.PWorkOrderNo) << 8) + (int)obj.PGlassData.PWorkSlot;
                tmp[4] = Convert.ToByte(value & 0x00ff);
                tmp[5] = Convert.ToByte((value & 0xff00) >> 8);

                string id = SysUtils.GetFixedLengthString(obj.PGlassData.PId, 20);
                byte[] bytes = Encoding.ASCII.GetBytes(id);
                for (int i = 0; i < 20; i++)
                {
                    tmp[6 + i] = bytes[i];
                }
                log += "Report unit no : " + obj.PUnitNo + "\n";
                log += "Report Port no : " + obj.PPortNo + "\n";
                log += "Report Slot no : " + obj.PSlotNo + "\n";
                log += "Report CIM Mode : " + obj.PGlassData.PCimMode + "\n";
                log += "Report PFoup Seq : " + obj.PGlassData.PFoupSeq + "\n";
                log += "Report Work Order No : " + obj.PGlassData.PWorkOrderNo + "\n";
                log += "Report Work Slot : " + obj.PGlassData.PWorkSlot + "\n";
                log += "Report id : " + id + "\n";
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);

                cv_MemoryIoClient.SetBinaryLengthData(0x345F, tmp, 13, false);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitTm, cv_Tm);
                JumpToStep(cv_TimechartId, STEP_ID_WaitTm);
            }
            catch (Exception ex)
            {
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        void OnEnter_WaitTm(int m_StepId)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x346C);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x346C, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void OnEnter_TriggerFetchIndex(int m_StepId)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_WaitInterval(int m_StepId)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}
