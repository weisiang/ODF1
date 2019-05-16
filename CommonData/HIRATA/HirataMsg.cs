using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonData.HIRATA
{
    #region API error reset 
    public class MDApiErrorReset : MessageBase
    {
        public int cv_Ticket;
    }
    #endregion

    #region ForceCom 
    public class MDForceCom : MessageBase
    {
        public int cv_TimeChartId;
    }
    #endregion

    #region ForceIni
    public class MDForceIni : MessageBase
    {
        public int cv_TimeChartId;
    }
    #endregion

    #region EFEM msg
    public class MDEfemStatus : MessageBase
    {
        public int cv_Pressure;
        public int cv_Vacuum;
        public int cv_Ionizer1;
        public int cv_Ionizer2;
        public int cv_Ionizer3;
        public int cv_Ionizer4;
        public int cv_Ionizer5;
        public int cv_Ionizer6;
        public int cv_Ionizer7;
        public int cv_Ionizer8;
        public int cv_FFU1;
        public int cv_FFU2;
        public int cv_FFU3;
        public int cv_FFU4;
        public int cv_FFU5;
        public int cv_FFU6;
        public int cv_FFU7;
        public int cv_FFU8;
        public int cv_FFU9;
        public int cv_FFU10;
        public int cv_FFU11;
        public int cv_RobotMode;
        public int cv_RobotEnable;
        public int cv_Door;
        public int cv_EMO;
        public int cv_Power;
    }

    #endregion

    #region EFEM single
    public class MDEfemStatusSingle: MessageBase
    {
        public int cv_Type = 0;
        public int cv_Value = 0;
        public APIEnum.EventCommand PStatusType
        {
            get { return (APIEnum.EventCommand)cv_Type; }
            set { cv_Type = (int)value; }
        }
        public int PValue
        {
            get { return cv_Value; }
            set { cv_Value = value; }
        }
    }
    #endregion

    #region TimeChart Chamge notify
    public class MDTimeChartChange : MessageBase
    {
        public List<int> cv_Steps = new List<int>();
        public List<string> cv_StepsName = new List<string>();
        public string SDP1 = "";
        public string SDP2 = "";
        public string IJP = "";
        public string VAS_UP = "";
        public string VAS_DOWN = "";
        public string UV_1 = "";
        public string SDP3 = "";
        public string AOI = "";
        public string UV_2 = "";
    }
    #endregion

    #region reset time chart
    public class MDResetTimeChart : MessageBase
    {
        public int cv_TimeChartId =0;
    }
    #endregion

    #region Robot job path 
    public class MDRobotjobPath : MessageBase
    {
        public List<RobotJob> RobotJob = new List<RobotJob>();
    }
    #endregion

    #region Change port slot type
    public class MDChangePortSlotType : MessageBase
    {
        public int cv_Result = 0;
        public int cv_PortId = 0;
        public int cv_SlotType = 0;

        public Result PResult
        {
            get { return (Result)cv_Result; }
            set { cv_Result = (int)value; }
        }
        public int PPortId
        {
            get { return cv_PortId; }
            set { cv_PortId = value; }
        }
        public int PSlotType
        {
            get { return cv_SlotType; }
            set { cv_SlotType = value; }
        }
    }
    #endregion

    #region OCR decide reply
    public class MDShowOcrDecide : MessageBase
    {
    }
    #endregion

    #region Show OCR decide
    public class MDShowOcrDecideReply : MessageBase
    {
        public int cv_OcrDecide = -1;
        public string cv_KeyInValue = "";
        public string PKeyInValue
        {
            set { cv_KeyInValue = value; }
            get { return cv_KeyInValue; }
        }
        public OCRMode POcrDecide
        {
            set { cv_OcrDecide = (int)value; }
            get { return (OCRMode)cv_OcrDecide; }
        }
    }
    #endregion

    #region Robot job action
    public class MDRobotJobAction : MessageBase
    {
        public int cv_Action = -1;
        public RobotJobAction PAction 
        {
            set { cv_Action = (int)value; }
            get { return (RobotJobAction)cv_Action; }
        }
    }
    #endregion
}
