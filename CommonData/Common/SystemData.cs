using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class SystemData : CommonDatabase
    {
        public delegate void SendDataViaMmf();
        public event SendDataViaMmf OnRobotStatusChange;
        public event SendDataViaMmf OnSystemStatusChange;
        public SendDataViaMmf OnSystemDataChange
        {
            set;
            get;
        }

        public int cv_SystemStatus = 0;
        public int cv_SystemOnlineMode = 0;
        public int cv_RobotOnlineMode = 0;
        public int cv_RobotStatus = 0;
        public int cv_OperationMode = 0;
        public int cv_OcrMode = 0;
        public bool cv_IsPlcConnect = false;
        public bool cv_IsBcAlive = false;
        public bool cv_RobotConnect = false;
        public string cv_RobotVersion = "";
        public int cv_DataCheckRule = 0;
        public bool cv_InitializeOk = false;
        public bool cv_Initializing = false;
        public bool cv_OntMode = true;
        public int cv_RobotSpeed = 0;
        public int cv_FFUSpeed = 0;


        private KDateTime cv_IdleTime = SysUtils.Now();
        public KDateTime PIdleTime
        {
            get { return cv_IdleTime; }
            set { cv_IdleTime = value; }
        }
        public int PRobotSpeed
        {
            get { return cv_RobotSpeed; }
            set 
            {
                if (cv_RobotSpeed != value)
                {
                    cv_RobotSpeed = value;
                    SaveToFile();
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public int PFFUSpeed
        {
            get { return cv_FFUSpeed; }
            set 
            {
                if (cv_FFUSpeed != value)
                {
                    cv_FFUSpeed = value;
                    SaveToFile();
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public bool PInitaiizing
        {
            get { return cv_Initializing; }
            set
            {
                if (cv_Initializing != value)
                {
                    cv_Initializing = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public bool PInitaiizeOk
        {
            get { return cv_InitializeOk; }
            set
            {
                if (cv_InitializeOk != value)
                {
                    cv_InitializeOk = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public bool PONT
        {
            get { return cv_OntMode; }
            set
            {
                if (cv_OntMode != value)
                {
                    cv_OntMode = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                        SaveToFile();
                    }
                }
            }
        }
        public bool IsCheckRecipe
        {
            get { return (cv_DataCheckRule & 0x1) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFFE) + Convert.ToInt16(value); }
        }
        public bool IsCheckId
        {
            get { return ((cv_DataCheckRule & 0x2) >> 1) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFFD) + (Convert.ToInt16(value) << 1); }

        }
        public bool IsCheckSlot
        {
            get { return ((cv_DataCheckRule & 0x4) >> 2) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFFB) + (Convert.ToInt16(value) << 2); }
        }
        public bool IsCheckSeq
        {
            get { return ((cv_DataCheckRule & 0x8) >> 3) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFF7) + (Convert.ToInt16(value) << 3); }
        }
        public int PDataCheckRule
        {
            get { return cv_DataCheckRule; }
            set
            {
                if (cv_IsAutoSave)
                {
                    SaveToFile();
                }
                if (cv_DataCheckRule != value)
                {
                    cv_DataCheckRule = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public bool PPlcConnect
        {
            get { return cv_IsPlcConnect; }
            set
            {
                if (cv_IsPlcConnect != value)
                {
                    cv_IsPlcConnect = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public bool PBcAlive
        {
            get { return cv_IsBcAlive; }
            set
            {
                if (cv_IsBcAlive != value)
                {
                    cv_IsBcAlive = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public bool PRobotConnect
        {
            get { return cv_RobotConnect; }
            set
            {
                if (cv_RobotConnect != value)
                {
                    /*
                    if (value == false)
                    {
                        cv_SystemStatus = (int)CommonData.HIRATA.EquipmentStatus.Down;
                    }
                    */
                    cv_RobotConnect = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public string PRobotVersion
        {
            get { return cv_RobotVersion; }
            set
            {
                if (cv_RobotVersion != value)
                {
                    cv_RobotVersion = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public CommonData.HIRATA.OCRMode POcrMode
        {
            get { return (CommonData.HIRATA.OCRMode)cv_OcrMode; }
            set
            {
                if ((CommonData.HIRATA.OCRMode)cv_OcrMode != value)
                {
                    cv_OcrMode = (int)value;
                    if (cv_IsAutoSave)
                    {
                        SaveToFile();
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public CommonData.HIRATA.OperationMode POperationMode
        {
            get { return (CommonData.HIRATA.OperationMode)cv_OperationMode; }
            set
            {
                if ((CommonData.HIRATA.OperationMode)cv_OperationMode != value)
                {
                    cv_OperationMode = (int)value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public CommonData.HIRATA.EquipmentInlineMode PRobotInline
        {
            get { return (CommonData.HIRATA.EquipmentInlineMode)cv_RobotOnlineMode; }
            set
            {
                if ((CommonData.HIRATA.EquipmentInlineMode)cv_RobotOnlineMode != value)
                {
                    cv_RobotOnlineMode = (int)value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public CommonData.HIRATA.OnlineMode PSystemOnlineMode
        {
            get { return (CommonData.HIRATA.OnlineMode)cv_SystemOnlineMode; }
            set
            {
                if ((CommonData.HIRATA.OnlineMode)cv_SystemOnlineMode != value)
                {
                    cv_SystemOnlineMode = (int)value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }
        public CommonData.HIRATA.EquipmentStatus PRobotStatus
        {
            get { return (CommonData.HIRATA.EquipmentStatus)cv_RobotStatus; }
            set
            {
                if ((CommonData.HIRATA.EquipmentStatus)cv_RobotStatus != value)
                {
                    cv_RobotStatus = (int)value;
                    /*
                    cv_SystemStatus = (int)value;
                    */
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                    if(OnRobotStatusChange != null)
                    {
                        OnRobotStatusChange();
                    }
                }
            }
        }
        public CommonData.HIRATA.EquipmentStatus PSystemStatus
        {
            get { return (CommonData.HIRATA.EquipmentStatus)cv_SystemStatus; }
            set
            {
                if ((CommonData.HIRATA.EquipmentStatus)cv_SystemStatus != value)
                {
                    if (value == EquipmentStatus.WaitIdle)
                    {
                        PIdleTime = SysUtils.Now();
                        cv_SystemStatus = (int)value;
                    }
                    else
                    {
                        cv_SystemStatus = (int)value;
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                    if(OnSystemStatusChange != null)
                    {
                        OnSystemStatusChange();
                    }
                }
            }
        }
        public void Clone(SystemData m_OtherData)
        {
            cv_SystemStatus = m_OtherData.cv_SystemStatus;
            cv_SystemOnlineMode = m_OtherData.cv_SystemOnlineMode;
            cv_RobotOnlineMode = m_OtherData.cv_RobotOnlineMode;
            cv_OperationMode = m_OtherData.cv_OperationMode;
            cv_OcrMode = m_OtherData.cv_OcrMode;
            cv_IsPlcConnect = m_OtherData.cv_IsPlcConnect;
            cv_IsBcAlive = m_OtherData.cv_IsBcAlive;
            cv_RobotConnect = m_OtherData.cv_RobotConnect;
            cv_RobotVersion = m_OtherData.cv_RobotVersion;
            cv_DataCheckRule = m_OtherData.cv_DataCheckRule;
            cv_InitializeOk = m_OtherData.cv_InitializeOk;
            cv_OntMode = m_OtherData.cv_OntMode;
        }
        public void LoadFromFile()
        {
            string ori_path = cv_FilePath;

            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["System"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["System"].ItemNumber != 0)
                {
                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName["SystemData"]);
                }
            }
            PSystemStatus = EquipmentStatus.None;
            PSystemOnlineMode = OnlineMode.Offline;
            PRobotConnect = false;
            PRobotInline = EquipmentInlineMode.None;
            PRobotStatus = EquipmentStatus.Idle;
            POperationMode = OperationMode.Manual;
            PPlcConnect = false;
            PBcAlive = false;
            cv_RobotVersion = "";
            cv_InitializeOk = false;
            cv_Initializing = false;
            if (cv_FilePath != ori_path)
            {
                cv_FilePath = ori_path;
            }

        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<System/>";
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            tmp.ItemsByName["System"].AddItem(body);
            lock (cv_obj)
            {
                try
                {
                    tmp.SaveToFile(cv_FilePath, true);
                }
                catch (Exception e)
                {
                }
            }
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
    }
}
