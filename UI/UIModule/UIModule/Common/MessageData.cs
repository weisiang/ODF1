using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    #region OnLineRequest
    public class MDOnlineRequest
    {
        public int CurMode;
        public int ChangeMode;
        public Common.OnlineMode PCurMode
        {
            get { return (Common.OnlineMode)CurMode; }
            set { CurMode = (int)value; }
        }
        public Common.OnlineMode PChangeMode
        {
            get { return (Common.OnlineMode)ChangeMode; }
            set { ChangeMode = (int)value; }
        }
    }
    #endregion

    #region HsmsConnectStatus
    public class MDHsmsConnectStatus
    {
        public UInt64 Tick;
        public int Status;
        public int OnlineMode;
        public bool PStatus
        {
            get { return Status == 1 ? true : false; }
            set { Status = Convert.ToInt16(value); }
        }
        public Common.OnlineMode POnlineMode
        {
            get { return (Common.OnlineMode)OnlineMode; }
            set { OnlineMode = (int)value; }
        }
    }
    #endregion

    #region EqStatus
    public class MDEqStatusList
    {
        public int tick;
        public int Id;
        public List<Eq> Eqs = new List<Eq>();
    }
    public class Eq
    {
        public int Id;
        public int Status;
        public int Connect;
        public int InLine;
        public List<int> SubStatus = new List<int>();
        public Common.EquipmentStatus PStatus
        {
            get { return (Common.EquipmentStatus)Status; }
            set { Status = (int)value; }
        }
        public bool PConnect
        {
            get { return Connect == 1 ? true : false; }
            set { Connect = Convert.ToInt16(value); }
        }
        public Common.EquipmentInlineMode PInLine
        {
            get { return (Common.EquipmentInlineMode)InLine; }
            set { InLine = (int)value; }
        }
        public Common.EquipmentSubStatus this[int i]
        {
            get { return (Common.EquipmentSubStatus)SubStatus[i]; }
            set { SubStatus[i] = (int)value; }
        }
        public void  GetSubStatusList(out List<Common.EquipmentSubStatus> m_List)
        {
            m_List = new List<Common.EquipmentSubStatus>();
            for (int i = 0; i < SubStatus.Count; i++)
            {
                m_List.Add(this[i]);
            }
        }
    }
    #endregion

    #region AlarmReport
    public class MDAlarmReport
    {
        public UInt64 Tick;
        public List<AlarmItem> AlarmList = new List<AlarmItem>();
    }
    public class AlarmItem
    {
        public string Code;
        public int Level;
        public int Status;
        public string Msg;
        public int EqId;
        public string Time;
        public Common.AlarmLevele PLevel
        {
            get { return (Common.AlarmLevele)Level; }
            set { Level = (int)value; }
        }
        public Common.AlarmStatus PStatus
        {
            get { return (Common.AlarmStatus) Status; }
            set { Status = (int)value; }
        }
        public string PTime
        {
            get { return Time; }
        }
        public int PEqId
        {
            get { return EqId; }
        }
        public string PMsg
        {
            get { return Msg; }
        }
        public string PCode
        {
            get { return Code; }
        }
    }
    #endregion

    #region LogInOut
    public class MDLogInOut
    {
        public string Id;
        public int Permission;
        public int Action;
        public Common.UserPermission PPermission
        {
            get
            { return (UserPermission)Permission; }
            set
            { Permission = (int)value; }
        }
        public Common.LogInOut PAction
        {
            get { return (Common.LogInOut)Action; }
            set { Action = (int)value; }
        }
    }
    #endregion

    #region ShowMsg
    public class MDShowMsg
    {
        public UInt64 Tick;
        public int Choice;
        public string Msg;
        public List<Msg> Msgs = new List<Msg>();
        public Common.UserChoice PChoice
        {
            get { return (Common.UserChoice)Choice; }
            set { Choice = (int)value; }
        }
    }
    public class Msg
    {
        public int AutoClean;
        public UInt64 TimeOut;
        public int UserRep;
        public int Choice;
        public string Txt;
        public bool PAutoClean
        {
            get { return AutoClean == 1 ? true : false; }
            set { AutoClean = Convert.ToInt16(value); }
        }
        public bool PUserRep
        {
            get { return UserRep == 1 ? true: false; }
            set { UserRep = Convert.ToInt16(value); }
        }
        public Common.UserChoice PChoice
        {
            get { return (Common.UserChoice)Choice; }
            set { Choice = (int)value; }
        }
    }
    #endregion

    #region PopOpidForm
    public class MDPopOpidForm
    {
        public UInt64 Tick;
        public int Port;
        public ReplyData Reply;
    }
    public class ReplyData
    {
        public string Id;
        public string OpId;
        public string CstSeq;
    }
    #endregion

    #region PopMonitorForm
    public class MDPopMonitorForm
    {
        public UInt64 Tick;
        public int Port;
        public int Result;
        public List<GlassDataItem> GlassDatas = new List<GlassDataItem>();
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    }
    /*
    public class MDPopMonitorFormRep
    {
        public UInt64 Tick;
        public int Result;
        public int Port;
        public List<GlassDataItem> GlassDatas = new List<GlassDataItem>();
    }
    */
    #endregion

    #region Initial
    public class MdInitial
    {
        public int Action;
        public int Result;
        public Common.InitialAction PAction
        {
            get { return (Common.InitialAction)Action; }
            set { Action = (int)value; }
        }
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    };
    #endregion

    #region Recipe Setting
    public class MDRecipeSetting
    {
        public int Action;
        public List<Recipe> Recipes = new List<Recipe>();
    }
    #endregion

    #region RobotAction
    public class MDRobotAction
    {
        public UInt64 Tick;
        public int Action;
        public int Result;
        public Robot Robot = new Robot();
        public Source Source = new Source();
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
        public Common.RobotAction PAction
        {
            get { return (Common.RobotAction)Action; }
            set { Action = (int)value; }
        } 
    }
    #endregion

    #region Robot Mapping
    public class MDRobotMapping
    {
        public Robot Robot = new Robot();
        public Port Port = new Port();
        public int Result;
        public string MappingData;
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region MDRobotStatus
    public class MDRobotStatus
    {
        public UInt64 Tick;
        public List<Robot> Robots = new List<Robot>();
    }
    #endregion

    #region Port Action
    public class MDPortAction
    {
        public UInt64 Tick;
        public Port Port = new Port();
        public int Action;
        public int Result;
        public Common.PortAction PAction
        {
            get { return (Common.PortAction)Action; }
            set { Action = (int)value; }
        }
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Port Load / unload
    public class MDPortLdUd
    {
        public Port Port = new Port();
        public int Action;
        public string Mapping;
        public Common.PortStaus PAction
        {
            get { return (Common.PortStaus)Action; }
        }
    }
    #endregion

    #region Port status
    public class MDPortStatus
    {
        public UInt64 Tick;
        public Port Port = new Port();
    }
    #endregion

    #region Data Flow
    public class MDDataFlow
    {
        public int Action;
        public Source Source = new Source();
        public GlassDataItem GlassDataItem = new GlassDataItem();
        public Common.DataFlowAction PAction
        {
            get { return (Common.DataFlowAction)Action; }
            set { Action = (int)value; }
        }
    }
    #endregion

    #region Data Action
    public class MDDataAction
    {
        public UInt64 Tick;
        public int Result;
        public int Action;
        public Source Source = new Source();
        public GlassDataItem OldGlassDataItem = new GlassDataItem();
        public GlassDataItem NewGlassDataItem = new GlassDataItem();
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
        public Common.DataEidtAction PAction
        {
            get { return (Common.DataEidtAction)Action; }
            set { Action = (int)value; }
        }
    }
    #endregion

    #region Data Request
    public class MDDataRequest
    {
        public UInt64 Tick;
        public Source Source = new Source();
        public LotDataItem LotData = new LotDataItem();
        public List<GlassDataItem> GlassDatas = new List<GlassDataItem>();
    }
    #endregion

    #region Buzzer
    public class MDBuzzerControl
    {
        public int Action;
        public int Result;
        public CommonData.BuzzerAction PAction
        {
            get { return (CommonData.BuzzerAction)Action; }
            set { Action = (int)value; }
        }
        public CommonData.Result PResult
        {
            get { return (CommonData.Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Robot Inline Change
    public class MDRobotInlineChange
    {
        public int Tick;
        public int InLine;
        public int Result;
        public Common.EquipmentInlineMode PInLine
        {
            get { return (EquipmentInlineMode)InLine; }
            set { InLine = (int)value; }
        }
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Robot API Show/Hide
    public class MDRobotApiShow
    {
        public int Tick;
        public int Show;
        public int Result;
        public bool PShow
        {
            get { return Show == 1 ? true : false; }
            set { Show = Convert.ToInt32 (value); }
        }
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Device Home
    public class MDDeviceHome
    {
        public int Tick;
        public int Action;
        public int Result;
        public string Device;
        public Common.HomeAction PAction
        {
            get { return (Common.HomeAction)Action; }
            set { Action = Convert.ToInt32(value); }
        }
        public Common.Result PResult
        {
            get { return (Common.Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion



    public class Robot
    {
        public int Id;
        public int Arm;
        public int Status;
        public int Connect;
        public int InLine;
        public int UpSensor;
        public int DownSensor;
        public Common.RobotArm PArm
        {
            get { return (Common.RobotArm)Arm; }
            set { Arm = (int)value; }
        }
        public Common.EquipmentStatus PStatus
        {
            get { return (Common.EquipmentStatus)Status; }
            set { Status = (int)value; }
        }
        public bool IsConnect
        {
            get { return Connect == 1 ? true : false; }
            set { Connect = Convert.ToInt16(value); }
        }
        public CommonData.EquipmentInlineMode PInLIne
        {
            get { return (Common.EquipmentInlineMode)InLine; }
            set { InLine = (int)value; }
        }
        public bool PUpSensor
        {
            get { return UpSensor == 1 ? true : false; }
            set { UpSensor = Convert.ToInt32(value); }
        }
        public bool PDownSensor
        {
            get { return DownSensor == 1 ? true : false; }
            set { DownSensor = Convert.ToInt32(value); }
        }
    }
    public class Port
    {
        public UInt64 Tick;
        public int Id;
        public int Clamp;
        public int CST;
        public int PortStatus;
        public string Mapping;
        public int LotStatus;
        public int Mode;
        public int Type;
        public int Enable;
        public int AGV;
        public int Slot;
        public Common.PortClamp PClamp
        {
            get { return (PortClamp)Clamp; }
            set { Clamp = (int)value; }
        }
        public Common.PortHasCst PCst 
        {
            get { return (PortHasCst)CST; }
            set { CST = (int)value; }
        }
        public Common.PortEnable PEnable
        {
            get { return (PortEnable)Enable; }
            set { Enable = (int)value; }
        }
        public Common.PortAgv PAGV
        {
            get { return (PortAgv)AGV; }
            set { AGV = (int)value; }
        }
        public Common.PortStaus PPortStatus
        {
            get { return (PortStaus)PortStatus; }
            set { PortStatus = (int)value; }
        }
        public Common.LotStatus PLotStatus
        {
            get { return (Common.LotStatus) LotStatus; }
            set { LotStatus = (int)value; }
        }
        public Common.PortMode PMode
        {
            get { return (PortMode)Mode; }
            set { Mode = (int)value; }
        }
        public Common.PortType PType
        {
            get { return (PortType)Type; }
            set { Type = (int)value; }
        }
    }
    public class Source
    {
        public int Target;
        public int Id;
        public int Slot;
        public int Arm;
        public Common.ActionTarget PTarget
        {
            get { return (Common.ActionTarget)Target; }
            set { Target = (int)value; }
        }
        public Common.RobotArm PArm
        {
            get { return (Common.RobotArm)Arm; }
            set { Arm = (int)value; }
        }

    }
    //every project need to defined it
    public class GlassDataItem
    {
        //if at Robot. 1.Up , 2. Down ( check RobotArm Enum)
        public int SlotInEq;
    } 
    public class Recipe
    {

    }

    public class LotDataItem
    {
        public string LotId = "";
    }


}
