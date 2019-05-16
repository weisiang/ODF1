using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum UserPermission { None, OP = 1, Engineer, Root };
    public enum AlarmLevele { None, Light, Serious };
    public enum AlarmStatus { None, Occur, Clean };
    public enum EquipmentStatus { None , Run = 1 , Down , Idle , };
    public enum EquipmentInlineMode { None , Remote = 1 , Local ,  Offline };
    public enum EquipmentSubStatus {None, Warning = 1 , Stop, Init, Standby };
    public enum PortAction {None , Clamp =1 , Unclamp,  Cancel, Rewok};
    public enum PortStaus { None, LDRQ, LDCM, UDRQ, UDCM };
    public enum LotStatus { None, Empty, Wait, Run, NormalEnd, AbnotmalEnd, Cancel, Abort };
    public enum PortMode { None, Loader, Unloader, Both };
    public enum PortType { None, OK, NG, MIX };
    public enum PortEnable { Disable, Enable, None };
    public enum PortAgv { None, AGV, MGV };
    public enum PortClamp { Unclamp, Clamp, None };
    public enum PortHasCst { Empty, Has, None };
    public enum RobotAction
    {Put =1 , PutWait, Get,  GetWait,  Exchange, HighExchange , Initialize , ActionComplete };
    public enum ActionTarget { Port = 1 , Eq , Buffer , Robot };
    public enum RobotArm { rbaUp = 1, rbaDown };
    public enum DataFlowAction { None , Fetch , Store , };
    public enum OnlineMode {None,  Control = 1 , Monitor , Local , Offline};
    public enum LogType {Error=1 , Warning=1 , UI=1 , MsgName=2 , MsgArg = 3 , General=3 , NoneTimerInOut = 4 , Timer=5 , RawData = 6 };
    public enum FunInOut {None,   Enter , Leave};
    public enum DataEidtAction { None , Add , Edit , Del};
    public enum LogInOut { None , Login , LogOut,};
    public enum Result { None , OK , NG};
    public enum UserChoice { None , Yes , No};
    public enum InitialAction { None , Initial , Complete};
    public enum HomeAction {  None , Hone , Complete};
    public enum BuzzerAction { None , ON , OFF};
}
