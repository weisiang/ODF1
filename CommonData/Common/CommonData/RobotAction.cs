using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonData.HIRATA
{
    public class RobotJob
    {
        public int cv_RobotId;
        public int cv_GetArm;
        public int cv_PutArm;
        public int cv_Action;
        public int cv_Target;
        public int cv_TargetId;
        public int cv_TargetSlot;
        public bool cv_UseHs;
        public bool cv_IsWaitGet = true;
        public bool cv_IsWaitPut = true;
        public bool cv_ManualExchangeForAligner = false;
        public string cv_ManualExchangeForAlignerDeg = "";

        public string PManualExchangeForAlignerDeg
        {
            get { return cv_ManualExchangeForAlignerDeg; }
            set { cv_ManualExchangeForAlignerDeg = value; }
        }

        public bool PManualExchangeForAligner
        {
            get { return cv_ManualExchangeForAligner; }
            set { cv_ManualExchangeForAligner = value; }
        }
   
        public int PRobotId
        {
            get { return cv_RobotId; }
            set { cv_RobotId = value; }
        }
        public RobotArm PGetArm
        {
            get { return (RobotArm)cv_GetArm; }
            set { cv_GetArm = (int)value; }
        }
        public RobotArm PPutArm
        {
            get { return (RobotArm)cv_PutArm; }
            set { cv_PutArm = (int)value; }
        }
        public RobotAction PAction
        {
            get { return (RobotAction)cv_Action; }
            set { cv_Action = (int)value; }
        }
        public ActionTarget PTarget
        {
            get { return (ActionTarget)cv_Target; }
            set { cv_Target = (int)value; }
        }
        public int PTargetId
        {
            get { return cv_TargetId; }
            set { cv_TargetId = value; }
        }
        public int PTargetSlot
        {
            get { return cv_TargetSlot; }
            set { cv_TargetSlot = value; }
        }
        public bool PUseHs
        {
            get { return cv_UseHs; }
            set { cv_UseHs = value; }
        }

        public bool PIsWaitGet
        {
            get { return cv_IsWaitGet; }
            set { cv_IsWaitGet = value; }
        }
        public bool PisWaitPut
        {
            get { return cv_IsWaitPut; }
            set { cv_IsWaitPut = value; }
        }

        public RobotJob(int m_RobotId, RobotArm m_PutArm, RobotArm m_GetArm, RobotAction m_Action, ActionTarget m_Target, int m_TargetId, int m_TargetSlot, bool m_UseHs = true)
        {
            PRobotId = m_RobotId;
            PPutArm = m_PutArm;
            PGetArm = m_GetArm;
            PAction = m_Action;
            PTarget = m_Target;
            PTargetId = m_TargetId;
            PTargetSlot = m_TargetSlot;
            PUseHs = m_UseHs;
        }
        public RobotJob()
        {
        }
    }
}
