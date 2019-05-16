using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using CommonData;
using BaseAp;

namespace CIM
{
    class TimechartBcWorkDataResponse : TimechartControllerBase.TimechartInstanceBase
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

        public static int STEP_ID_BcDateTimeCalibration = 1;

        int cv_Value;
        int cv_Port = 0x0004;

        public TimechartBcWorkDataResponse(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
            AssignRunningStepEventFunction(STEP_ID_BcDateTimeCalibration, OnRunning_BcDateTimeCalibration);
            //AssignEnterStepEventFunction(STEP_ID_BcDateTimeCalibration, OnEnter_BcDateTimeCalibration);
        }

        void OnRunning_BcDateTimeCalibration(int m_StepId)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process OnRunning_BcDateTimeCalibration]";
                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    log += "index change to " + cv_Value.ToString();
                    CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                    BcDateTimeCalibration();
                }
            }
            catch (Exception ex)
            {
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_BcDateTimeCalibration(int m_StepId)
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcDateTimeCalibration()
        {
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                int word1 = cv_MemoryIoClient.GetPortValue(0x0);
                int word2 = cv_MemoryIoClient.GetPortValue(0x01);
                int word3 = cv_MemoryIoClient.GetPortValue(0x02);
                int word4 = cv_MemoryIoClient.GetPortValue(0x04);

                CommonData.HIRATA.MDBCTimeAdjust obj = new CommonData.HIRATA.MDBCTimeAdjust();

                int intValue = 182;
                // Convert integer 182 as a hex in a string variable
                string hexValue = intValue.ToString("X");
                // Convert the hex string back to the number
                int intAgain = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

                string value = ((word1 & 0xFF00) >> 8).ToString("X2");
                obj.PYear = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                value = (word1 & 0x00FF).ToString("X2");
                obj.PMon = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                value = ((word2 & 0xFF00) >> 8).ToString("X2");
                obj.PDay = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                value = (word2 & 0x00FF).ToString("X2");
                obj.PHour = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                value = ((word3 & 0xFF00) >> 8).ToString("X2");
                obj.PMin = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                value = (word3 & 0x00FF).ToString("X2");
                obj.PSec = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                value = (word4 & 0x00FF).ToString("X2");
                obj.PWeek = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCTimeAdjust).Name, obj);
            }
            catch (Exception ex)
            {
                CimForm.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimForm.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}
