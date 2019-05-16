using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class GlassDataNodeItem
    {
        public int cv_NodeId;
        public int cv_ProcessHistory;
        public int cv_ProcessAbnormal;
        public int cv_Recipe;
        public int PNodeId
        {
            get { return cv_NodeId; }
            set { cv_NodeId = value; }
        }
        public int PProcessHistory
        {
            get { return cv_ProcessHistory; }
            set { cv_ProcessHistory = value; }
        }
        public bool PProcessAbnormal
        {
            get { return cv_ProcessAbnormal == 1 ? true : false ; }
            set { cv_ProcessAbnormal = Convert.ToInt16(value); }
        }
        public int PRecipe
        {
            get { return cv_Recipe; }
            set { cv_Recipe = value; }
        }
        public GlassDataNodeItem(int m_NodeId , int m_Value)
        {
            cv_NodeId = m_NodeId;
            cv_ProcessHistory = (m_Value & 0x6000) >> 13;
            cv_Recipe = (m_Value & 0x03FF);
            cv_ProcessAbnormal = (m_Value & 0x1000) >> 12;
        }
        public GlassDataNodeItem()
        {

        }
        public int GetWordValue()
        {
            int rtn = 0;
            rtn += (cv_ProcessHistory << 13) + (cv_ProcessAbnormal << 12) + cv_Recipe;
            return rtn;
        }
    }

    public class GlassData
    {
        public string cv_EnterBufferTime = "";
        public int cv_LastDevice = 0;
        public uint cv_CimMode = 0;
        public uint cv_FoupSeq = 0;
        public uint cv_WorkSlot = 0;
        public uint cv_WorkOrderNo = 0;
        public string cv_Id = "";
        public uint cv_SlotInEq = 0;
        public uint cv_SourcePort = 0;
        public uint cv_WorkType = 0;
        public uint cv_ProductionCategory = 0;
        public uint cv_PortProductionCategory = 0;
        public uint cv_GlassJudge = 0;
        public uint cv_ProcessFlag = 0;
        public int cv_OcrDecide = -1;
        public string cv_Ppid = "";

        public uint cv_Priority = 0;
        public uint cv_OcrRead = 0;
        public int cv_OcrResult = -1;

        public uint cv_AssamblyFlag = 0;
        public uint cv_AssamblyResult = 0;

        public uint cv_HasSensor = 0;
        public uint cv_HasData = 0;
        
        public bool cv_IsWaitAlignerRotation = false;
        public bool cv_IsWaitAlignerVaccumOn = false;
        public bool cv_IsWaitAlignerVaccumOff = false;
        public bool cv_IsWaitOcr = false;
        public string PPID
        {
            get { return cv_Ppid; }
            set { cv_Ppid = value; }
        }
        public AllDevice PLastDevice
        {
            get { return (AllDevice)cv_LastDevice; }
            set { cv_LastDevice = (int)value; }
        }
        public bool PIsWaitOcr
        {
            get { return cv_IsWaitOcr; }
            set { cv_IsWaitOcr = value; }
        }

        public List<GlassDataNodeItem> cv_Nods = new List<GlassDataNodeItem>();

        public OCRMode POcrDecide
        {
            set { cv_OcrDecide = (int)value; }
            get { return (OCRMode)cv_OcrDecide; }
        }
        public AssambleNeed PAssamblyFlag
        {
            get { return (AssambleNeed)cv_AssamblyFlag; }
            set { cv_AssamblyFlag = (uint)(value); }
        }
        public AssambleResult PAssamblyResult
        {
            get { return (AssambleResult)cv_AssamblyResult; }
            set { cv_AssamblyResult = (uint)(value); }
        }
        public OnlineMode PCimMode
        {
            get { return (CommonData.HIRATA.OnlineMode)cv_CimMode; }
            set { cv_CimMode = (UInt16)value; }
        }

        public KDateTime PEnterBufferTime
        {
            set { cv_EnterBufferTime = value.DateTimeString(); }
            get
            {
                KDateTime time = new KDateTime(cv_EnterBufferTime);
                return time;
            }
        }
        public uint PSourcePort
        {
            get { return cv_SourcePort; }
            set { cv_SourcePort = value; }
        }
        public uint PFoupSeq
        {
            get { return cv_FoupSeq; }
            set { cv_FoupSeq = value; }
        }

        public uint PWorkSlot
        {
            get { return cv_WorkSlot; }
            set { cv_WorkSlot = value; }
        }

        public uint PWorkOrderNo
        {
            get { return cv_WorkOrderNo; }
            set { cv_WorkOrderNo = value; }
        }

        public string PId
        {
            get { return cv_Id; }
            set { cv_Id = value; }
        }

        public uint PSlotInEq
        {
            get { return cv_SlotInEq; }
            set { cv_SlotInEq = value; }
        }
        public CommonData.HIRATA.WorkType PWorkType
        {
            get { return (CommonData.HIRATA.WorkType)cv_WorkType; }
            set { cv_WorkType = (UInt16)value; }
        }

        public CommonData.HIRATA.ProductCategory PProductionCategory
        {
            get { return (CommonData.HIRATA.ProductCategory)cv_ProductionCategory; }
            set { cv_ProductionCategory = (UInt16)value; }
        }
        public CommonData.HIRATA.ProductCategory PPortProductionCategory
        {
            get { return (CommonData.HIRATA.ProductCategory)cv_PortProductionCategory; }
            set { cv_PortProductionCategory = (UInt16)value; }
        }

        public CommonData.HIRATA.GlassJudge PGlassJudge
        {
            get { return (CommonData.HIRATA.GlassJudge)cv_GlassJudge; }
            set { cv_GlassJudge = (UInt16)value; }
        }

        public CommonData.HIRATA.ProcessFlag PProcessFlag
        {
            get { return (CommonData.HIRATA.ProcessFlag)cv_ProcessFlag; }
            set { cv_ProcessFlag = (UInt16)value; }
        }

        public bool PHasSensor
        {
            get { return cv_HasSensor == 1 ? true : false; }
            set { cv_HasSensor = Convert.ToUInt16(value); }
        }

        public bool PHasData
        {
            get 
            {
                if (IsNull())
                {
                    cv_HasData = 0;
                    return false;
                }
                else
                {
                    cv_HasData = 1;
                    return true;
                }
            }
        }

        public uint PPriority
        {
            get { return cv_Priority; }
            set { cv_Priority = value; }
        }

        public OCRRead POcrRead
        {
            get { return (CommonData.HIRATA.OCRRead)cv_OcrRead; }
            set { cv_OcrRead = (UInt16)value; }
        }
        public OCRResult POcrResult
        {
            get { return (CommonData.HIRATA.OCRResult)cv_OcrResult; }
            set { cv_OcrResult = (Int16)value; }
        }

        public GlassData()
        {
            for(int i = 1 ; i <= 15 ; i++)
            {
                GlassDataNodeItem tmp = new GlassDataNodeItem(i, 0);
                if(cv_Nods == null)
                {
                    cv_Nods = new List<GlassDataNodeItem>();
                }
                cv_Nods.Add(tmp);
            }
        }
        //read glass data form PLC.
        public GlassData(KMemoryIOClient m_MemoIO , int m_PortAddress)
        {
            Dismantle( m_MemoIO , m_PortAddress);
        }
        public void Write(KMemoryIOClient m_MemoIO, int m_PortAddress)
        {
            Combine(m_MemoIO , m_PortAddress);
        }
        public void Dismantle( KMemoryIOClient m_MemoIO ,  int m_PortAddress)
        {
            if(cv_Nods == null)
            {
                cv_Nods = new List<GlassDataNodeItem>();
            }
            cv_Nods.Clear();

            this.PCimMode = (OnlineMode)( (m_MemoIO.GetPortValue(m_PortAddress) & 0x8000) >> 15);
            this.PFoupSeq = (uint)(m_MemoIO.GetPortValue(m_PortAddress) & 0x00FF);

            this.PWorkSlot = (uint)(m_MemoIO.GetPortValue(m_PortAddress + 1) & 0x00FF);
            this.PWorkOrderNo = (uint)( (m_MemoIO.GetPortValue(m_PortAddress + 1) & 0xFF00) >> 8);

             byte[] rtn_byte = null;
            m_MemoIO.GetBinaryLengthData(m_PortAddress + 2 , ref rtn_byte, 10);
            this.PId = System.Text.Encoding.Default.GetString(rtn_byte).Trim();

            rtn_byte = null;
            m_MemoIO.GetBinaryLengthData(m_PortAddress + 12, ref rtn_byte, 10);
            this.PPID = System.Text.Encoding.Default.GetString(rtn_byte).Trim() ;

            this.PWorkType = (WorkType)((m_MemoIO.GetPortValue(m_PortAddress + 32) & 0xF000) >> 12);
            this.PProductionCategory = (ProductCategory)(m_MemoIO.GetPortValue(m_PortAddress + 32 ) & 0x00FF) ;
            this.PGlassJudge = (GlassJudge)( (m_MemoIO.GetPortValue(m_PortAddress + 32) & 0x00F0) >> 4 );
            this.PProcessFlag = (ProcessFlag)((m_MemoIO.GetPortValue(m_PortAddress + 32) & 0x0F00) >> 8);

            this.PPriority = (uint)((m_MemoIO.GetPortValue(m_PortAddress + 33) & 0xF000) >> 12);
            this.POcrRead = (OCRRead)((m_MemoIO.GetPortValue(m_PortAddress + 33) & 0x0010) >> 4);
            this.POcrResult = (OCRResult)((m_MemoIO.GetPortValue(m_PortAddress + 33) & 0x0003) );

            this.PAssamblyFlag = (m_MemoIO.GetPortValue(m_PortAddress + 34) & 0x0001) == 1 ? AssambleNeed.Need : AssambleNeed.NotNeed;
            this.PAssamblyResult = ((m_MemoIO.GetPortValue(m_PortAddress + 34) & 0x0010) >> 4) == 1 ? AssambleResult.Complete : AssambleResult.NotComplete;

            for(int i = 0 ; i<15 ; i++)
            {
                GlassDataNodeItem tmp = new GlassDataNodeItem(i + 1, m_MemoIO.GetPortValue(m_PortAddress + i + 35 ));
                cv_Nods.Add(tmp);
            }
        }
        private void Combine(KMemoryIOClient m_MemoIO, int m_PortAddress)
        {
            int value =( (int)this.PCimMode << 15 ) + (int)this.PFoupSeq  ;
            m_MemoIO.SetPortValue(m_PortAddress, value);

            value = ((int)this.PWorkOrderNo << 8) + (int)this.PWorkSlot;
            m_MemoIO.SetPortValue(m_PortAddress + 1, value);

            string str = SysUtils.GetFixedLengthString(this.PId, 10 * 2);
            m_MemoIO.SetBinaryLengthData(m_PortAddress + 2 , SysUtils.StringToByteArray(str), 10, false);

            str = SysUtils.GetFixedLengthString(this.PPID, 10 * 2);
            m_MemoIO.SetBinaryLengthData(m_PortAddress + 12, SysUtils.StringToByteArray(str), 10, false);

            value = ((int)this.PWorkType << 12) + ((int)this.PProductionCategory) + ((int)this.PGlassJudge << 4) + ((int)this.PProcessFlag << 8); 
            m_MemoIO.SetPortValue(m_PortAddress + 32, value);

            value = ((int)this.PPriority << 12) + ((int)this.POcrResult) + ((int)this.POcrRead << 4);
            m_MemoIO.SetPortValue(m_PortAddress + 33, value);

            value = ( Convert.ToInt16(this.PAssamblyResult) << 4) + ( Convert.ToInt16(this.PAssamblyFlag));
            m_MemoIO.SetPortValue(m_PortAddress + 34, value);

            byte[] node_byte = new byte[15*2];
            int node_value = 0;
            foreach(GlassDataNodeItem item in this.cv_Nods)
            {
                node_value = item.GetWordValue();
                node_byte[(item.PNodeId << 1) - 2] = Convert.ToByte(node_value & 0x00ff); 
                node_byte[(item.PNodeId << 1) - 1] = Convert.ToByte( (node_value & 0xff00) >> 8 ); 
            }
            m_MemoIO.SetBinaryLengthData(m_PortAddress + 35, node_byte ,15 , false); 
        }

        public void WriteWokeNoOnly(KMemoryIOClient m_MemoIO, int m_PortAddress)
        {
            int value = ((int)this.PCimMode << 15) + (int)this.PFoupSeq;
            m_MemoIO.SetPortValue(m_PortAddress, value);

            value = ((int)this.PWorkOrderNo << 8) + (int)this.PWorkSlot;
            m_MemoIO.SetPortValue(m_PortAddress + 1, value);
        }
        public int GetProcessCountByNode(int m_Node)
        {
            int rtn = -1;
            if(this.PHasData)
            {
                int index = cv_Nods.FindIndex(x => x.PNodeId == m_Node);
                if(index != -1)
                {
                    GlassDataNodeItem node = cv_Nods[index];
                    rtn = node.PProcessHistory;
                }
            }
            return rtn;
        }

        public bool IsNull()
        {
            bool rtn = false;
            if(cv_FoupSeq == 0 && cv_WorkSlot == 0 && PId == "")
            {
                rtn = true;
            }
            return rtn;
        }
        public bool IsEnterEq()
        {
            bool rtn = false;
            foreach(GlassDataNodeItem item in cv_Nods)
            {
                if(item.PProcessHistory > 0)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        public string GetGlassDataStr()
        {
            string data = "Obj Data:\n";
            data += "cv_EnterBufferTime : " + cv_EnterBufferTime + "\n";
            data += "cv_LastDevice : " + cv_LastDevice.ToString() + "\n";
            data += "cv_CimMode : " + cv_CimMode.ToString() + "\n";
            data += "cv_FoupSeq : " + cv_FoupSeq.ToString() + "\n";
            data += "cv_WorkSlot : " + cv_WorkSlot.ToString() + "\n";
            data += "cv_WorkOrderNo : " + cv_WorkOrderNo.ToString() + "\n";
            data += "cv_Id : " + cv_Id.ToString() + "\n";
            data += "cv_SlotInEq : " + cv_SlotInEq.ToString() + "\n";
            data += "cv_SourcePort : " + cv_SourcePort.ToString() + "\n";
            data += "cv_WorkType : " + cv_WorkType.ToString() + "\n";
            data += "cv_ProductionCategory : " + cv_ProductionCategory.ToString() + "\n";
            data += "cv_PortProductionCategory : " + cv_PortProductionCategory.ToString() + "\n";
            data += "cv_GlassJudge : " + cv_GlassJudge.ToString() + "\n";
            data += "cv_ProcessFlag : " + cv_ProcessFlag.ToString() + "\n";
            data += "cv_OcrDecide : " + cv_OcrDecide.ToString() + "\n";
            data += "cv_Ppid : " + cv_Ppid.ToString() + "\n";
            data += "cv_Priority : " + cv_Priority.ToString() + "\n";
            data += "cv_OcrRead : " + cv_OcrRead.ToString() + "\n";
            data += "cv_OcrResult : " + cv_OcrResult.ToString() + "\n";
            data += "cv_AssamblyFlag : " + cv_AssamblyFlag.ToString() + "\n";
            data += "cv_AssamblyResult : " + cv_AssamblyResult.ToString() + "\n";
            data += "cv_HasSensor : " + cv_HasSensor.ToString() + "\n";
            data += "cv_HasData : " + cv_HasData.ToString() + "\n";
            data += "cv_IsWaitAlignerRotation : " + cv_IsWaitAlignerRotation.ToString() + "\n";
            data += "cv_IsWaitAlignerVaccumOn : " + cv_IsWaitAlignerVaccumOn.ToString() + "\n";
            data += "cv_IsWaitAlignerVaccumOff : " + cv_IsWaitAlignerVaccumOff.ToString() + "\n";
            data += "cv_IsWaitOcr : " + cv_IsWaitOcr.ToString() + "\n";
            return data;
        }
    }
}
