using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartEqRecipeExistReport : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_TriggerRecipeExistIndex = 1;
        public static int STEP_ID_WaitInterval = 3;
        public static int STEP_ID_WaitTm = 2;

        public TimechartEqRecipeExistReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            AssignEnterStepEventFunction(STEP_ID_TriggerRecipeExistIndex, OnEnter_TriggerRecipeExistIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override void ProcessJob(object m_obj)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqRecipeExistReport ProcessJob]\n";
                CommonData.HIRATA.MDBCRecipeExistReply obj = m_obj as CommonData.HIRATA.MDBCRecipeExistReply;
                byte[] tmp = new byte[3 << 1];
                Array.Clear(tmp, 0, tmp.Length);

                int value = (obj.PPortId << 4) + (obj.PNode << 8);
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                log += "Report Port : " + obj.PPortId.ToString() + "\n";
                log += "Report node : " + obj.PNode.ToString() + "\n";

                UInt32 sum = 0;
                for (int i = 0; i < obj.cv_RecipesExist.Count; i++)
                {
                    sum += (UInt32)((obj.cv_RecipesExist[i] == false ? 1 : 0) << i);
                }
                log += "Report exist from slot 1 : " + sum;

                tmp[2] = Convert.ToByte(sum & 0x000000ff);
                tmp[3] = Convert.ToByte((sum & 0x0000ff00) >> 8);
                tmp[4] = Convert.ToByte((sum & 0x00ff0000) >> 16);
                tmp[5] = Convert.ToByte((sum & 0xff000000) >> 24);

                cv_MemoryIoClient.SetBinaryLengthData(0x3518, tmp, 3, false);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitTm, cv_Tm);
                JumpToStep(cv_TimechartId, STEP_ID_WaitTm);
            }
            catch (Exception ex)
            {
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_WaitTm(int m_StepId)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x351B);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x351B, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void OnEnter_TriggerRecipeExistIndex(int m_StepId)
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
