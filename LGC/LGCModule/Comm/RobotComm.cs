using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HirataRbAPI;
using CommonData.HIRATA;

namespace LGC.Comm
{
    internal class RobotComm : ObjComm
    {
        public RBController cv_Controller = null;
        public CommonData.HIRATA.GlassData cv_GlassDataGetFromEq = null;
        public RobotComm(string m_Ip , int m_Port)
        {
            cv_Controller = new RBController(m_Ip, m_Port);
        }
        public bool SetRobotTransferAction(CommandData m_Command)
        {
            bool rtn = false;
            if(cv_Controller.Connected)
            {
                if(cv_Controller.SendCommand(m_Command))
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        public bool SetRobotCommonAction(CommandData m_Command)
        {
            bool rtn = false;
            if (cv_Controller.Connected)
            {
                    try
                    {
                        if (cv_Controller.SendCommand(m_Command))
                        {
                            rtn = true;
                        }
                    }
                    catch(Exception e)
                    {
                    }
            }
            if(!rtn)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.SendApiComandError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "Send API Comand Error ( API disconnected maybe)";
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PUnit = 0;
                LgcForm.EditAlarm(alarm);
            }
            return rtn;
        }

        #region Port
        public bool SetPortLoadAction(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.Load.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public bool SetPortUnloadAction(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.Unload.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public bool SetGetMappingData(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.GetWaferSlot2.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public bool SetPortSlotTypeChange(CommandData command)
        {
            //CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.GetWaferSlot2.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public void SetLoadUnloadLed(bool isLoadLed, SignalTowerControl m_Control, int m_Id)
        {
            /*
            List<string> paras = new List<string>();
            paras.Add(m_Control.ToString());
            CommandData obj = null;
            if (isLoadLed)
                obj = new CommonData.HIRATA.CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.LEDLoad.ToString(), CommonData.HIRATA.APIEnum.CommnadDevice.P, m_Id, paras);
            else
                obj = new CommonData.HIRATA.CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.LEDUnLoad.ToString(), CommonData.HIRATA.APIEnum.CommnadDevice.P, m_Id, paras);
            SetRobotCommonAction(obj);
            */
        }
        public bool SetOperatorAccessButton(SignalTowerControl m_Control , int m_PortId)
        {
            /*
            List<string> paras = new List<string>();
            paras.Add(m_Control.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.SetOperatorAccessButton.ToString(), APIEnum.CommnadDevice.P, m_PortId , paras);
            SetRobotCommonAction(command);
             */
            return true;
        }
        public bool SetPortClampAction(int m_PortId , APIEnum.LoadPortCommand m_Action)
        {
            bool rtn = false;
            CommandData command = null;
            if (m_Action == APIEnum.LoadPortCommand.Clamp)
            {
                command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.Clamp.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            }
            else if (m_Action == APIEnum.LoadPortCommand.UnClamp)
            {
                command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.UnClamp.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            }
            if(command != null)
            {
                SetRobotCommonAction(command);
                rtn = true;
            }
            return rtn;
        }

        #endregion

        #region Robot
        public void SetRobotStop()
        {
            CommandData command = new CommandData(APIEnum.CommandType.Robot , APIEnum.RobotCommand.Stop.ToString(), APIEnum.CommnadDevice.Robot, 0);
            SetRobotCommonAction(command);
        }
        public void SetRobotRestart()
        {
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.ReStart.ToString(), APIEnum.CommnadDevice.Robot, 0);
            SetRobotCommonAction(command);
        }
        public void SetRobotSpeed(int m_Speed)
        {
            List<string> paras = new List<string>();
            paras.Add(m_Speed.ToString());
            paras.Add(m_Speed.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.SetRobotSpeed.ToString(), APIEnum.CommnadDevice.Robot, 0 , paras);
            SetRobotCommonAction(command);
        }
        public void SetRobotWaferGet(RobotArm m_Arm , APIEnum.CommnadDevice m_Device , int m_DeviceId , int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add( ((int)m_Arm).ToString());

            if(m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcForm.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcForm.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.WaferGet.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
            SetRobotCommonAction(command);
        }
        public void SetRobotWaferPut(RobotArm m_Arm , APIEnum.CommnadDevice m_Device , int m_DeviceId , int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add( ((int)m_Arm).ToString());

            if(m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcForm.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcForm.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.WaferPut.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
            SetRobotCommonAction(command);
        }
        public void SetRobotGetStandby(RobotArm m_Arm , APIEnum.CommnadDevice m_Device , int m_DeviceId , int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add( ((int)m_Arm).ToString());

            if(m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcForm.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcForm.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.GetStandby.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
            SetRobotCommonAction(command);
        }
        public void SetRobotPutStandby(RobotArm m_Arm , APIEnum.CommnadDevice m_Device , int m_DeviceId , int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add( ((int)m_Arm).ToString());

            if(m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcForm.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcForm.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.PutStandby.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
            SetRobotCommonAction(command);
        }
        //vas down arm (glass) get from down slot
        public void SetRobotGetStandbyArmExtend(RobotArm m_Arm,int m_Slot , bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaDown && m_Slot == 1 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcForm.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.GetStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                SetRobotCommonAction(command);
            }
        }
        //vas up arm (glass) put to down slot
        public void SetRobotPutStandbyArmExtend(RobotArm m_Arm,int m_Slot , bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaUp && m_Slot == 1 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcForm.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.PutStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                SetRobotCommonAction(command);
            }
        }
        //vas down arm (glass) put to up slot ( step 1 )
        public void SetRobotTopPutStandbyArmExtend(RobotArm m_Arm,int m_Slot , bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaDown && m_Slot == 2 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcForm.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.TopPutStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                SetRobotCommonAction(command);
            }
        }
        //vas down arm (glass) put to up slot ( step 2 )
        public void SetRobotTopWaferPut(RobotArm m_Arm, int m_Slot, bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaDown && m_Slot == 2 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcForm.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.GetStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                SetRobotCommonAction(command);
            }
        }

        #endregion

        #region Common
        public bool SetAllPortStatus()
        {
            for(int i = 1 ; i<= CommonData.HIRATA.CommonStaticData.g_PortNumber ; i++)
            {
                SetStatus(APIEnum.CommnadDevice.P, i);
            }
            return true;
        }
        public void SetStatus(APIEnum.CommnadDevice m_Device , int m_PoerId=0)
        {
            CommandData command = null;
            int id = m_PoerId;
            switch(m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.Robot, id);
                    break;
                case APIEnum.CommnadDevice.P:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(), 
                        CommonData.HIRATA.APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.CommnadDevice.Buffer:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.GetBufferStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.IO, id);
                    break;
                case APIEnum.CommnadDevice.EFEM:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.EFEM, id);
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(), 
                        CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        public void SetHome(APIEnum.CommnadDevice m_Device , int m_PoerId=0)
        {
            CommandData command = null;
            int id = m_PoerId;
            switch(m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    command = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.Home.ToString(), APIEnum.CommnadDevice.Robot, id);
                    break;
                case APIEnum.CommnadDevice.P:
                    command = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.Home.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    command = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.Home.ToString(), APIEnum.CommnadDevice.Aligner, id);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        public bool SetAllPortHome()
        {
            bool is_need = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = LgcForm.GetPortById(i);
                //tmp don't check data.
                if ((port.cv_Data.PPortHasCst == PortHasCst.Has))//) && (port.PPortStatus != PortStaus.LDCM))
                {
                    if (!LgcForm.GetRobotById(1).cv_IsForce)
                    {
                        if (port.PPortStatus != PortStaus.LDCM)
                        {
                            SetHome(APIEnum.CommnadDevice.P, i);
                            is_need = true;
                        }
                        else
                        {
                            port.cv_IsHome = true;
                        }
                    }
                    else
                    {
                        SetHome(APIEnum.CommnadDevice.P, i);
                        is_need = true;
                    }
                }
                else
                {
                    port.cv_IsHome = true;
                }
            }
            return is_need;
        }
        public void SetErrorReset(APIEnum.CommnadDevice m_Device , int m_PoerId=0)
        {
            CommandData command = null;
            int id = m_PoerId;
            switch(m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.Robot, id);
                    break;
                case APIEnum.CommnadDevice.P:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(), 
                        CommonData.HIRATA.APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(), 
                        CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        #endregion

        #region API
        public void SetApiCommonCommand(APIEnum.APICommand m_Command)
        {
            CommandData command = null;
            switch (m_Command)
            {
                case APIEnum.APICommand.CurrentMode:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.CurrentMode.ToString(),APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Remote:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Remote.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Local:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Local.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Version:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Version.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Hide:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Hide.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Show:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Show.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        #endregion

        #region aligner
        public void SetAlignerAlignment(float m_Value)
        {
            List<string> para = new List<string>();
            para.Add(m_Value.ToString());
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.Alignment.ToString(), CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, 1, para);
            SetRobotCommonAction(command);
        }
        public void SetAlignerVaccum(bool m_IsOn)
        {
            List<string> para = new List<string>();
            if (m_IsOn)
            {
                para.Add("On");
            }
            else
            {
                para.Add("Off");
            }
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.AlignerVacuum.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, 1, para);
            SetRobotCommonAction(command);
        }
        public void SetAlignerFindNotch()
        {
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.FindNotch.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, 1);
            SetRobotCommonAction(command);
        }
        public void SetAlignerToAngle()
        {
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.ToAngle.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, 1);
            SetRobotCommonAction(command);
        }
        public void SetAlignerReadDegree(float m_Value)
        {
            List<string> para = new List<string>();
            para.Add(m_Value.ToString());
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.SetIDReaderDegree.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, 1 , para);
            SetRobotCommonAction(command);
        }
        public void SetAlignerDegree(float m_Value)
        {
            List<string> para = new List<string>();
            para.Add(m_Value.ToString());
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.SetAlignerDegree.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, 1, para);
            SetRobotCommonAction(command);
        }
        #endregion

        #region IO
        public void SetSignalTower(SignalTowerColor m_Color , SignalTowerControl m_Control)
        {
            List<string> paras = new List<string>();
            paras.Add(m_Color.ToString() + m_Control.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.SignalTower.ToString() , APIEnum.CommnadDevice.IO, 0, paras);
            SetRobotCommonAction(command);
        }
        public void SetSetFFUVoltage(int m_Speed)
        {
            List<string> paras = new List<string>();
            paras.Add(m_Speed.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.SetFFUVoltage.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
            SetRobotCommonAction(command);
        }
        public void SetBuzzer(bool m_IsOn)
        {
            List<string> para = new List<string>();
            if (m_IsOn) para.Add("1");
            else para.Add("0");
            CommandData command_obj = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.Buzzer.ToString()
                , APIEnum.CommnadDevice.IO, 0, para);
            SetRobotCommonAction(command_obj);
        }
        public void GetBufferProtrusionSensor()
        {
            CommandData command_obj = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.GetBufferProtrusionSensor.ToString()
                , APIEnum.CommnadDevice.IO, 0);
            SetRobotCommonAction(command_obj);
        }
        #endregion

        #region RFID
        public bool SetReadRFIDRead(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.RFID, APIEnum.RfidCommand.ReadFoupID.ToString(), APIEnum.CommnadDevice.RFID, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        #endregion

        #region OCR
        public void SetOcrRead()
        {
            CommandData command = new CommandData(APIEnum.CommandType.OCR, APIEnum.OcrCommand.Read.ToString(), APIEnum.CommnadDevice.OCRReader, 1);
            SetRobotCommonAction(command);
        }
        public void SetOcrConnect()
        {
            CommandData command = new CommandData(APIEnum.CommandType.OCR, APIEnum.OcrCommand.Connect.ToString(), APIEnum.CommnadDevice.OCRReader, 1);
            SetRobotCommonAction(command);
        }
        #endregion
    }
}
