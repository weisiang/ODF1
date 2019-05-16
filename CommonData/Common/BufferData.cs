using System;
using KgsCommon;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{
    public class BufferData : ObjData
    {
        public int cv_Slot1Type = 0;
        public int cv_Slot2Type = 0;
        public int cv_Slot3Type = 0;
        public int cv_Slot4Type = 0;
        public int cv_Slot5Type = 0;
        public int cv_Slot6Type = 0;
        private Dictionary<int, BufferSlotType> cv_Types = new Dictionary<int, BufferSlotType>();
        public BufferData(int m_id, int m_SlotCount)
            : base(m_id, m_SlotCount)
        {
            cv_FilePath = CommonData.HIRATA.CommonStaticData.g_WorkFolder + "\\" + typeof(BufferData).Name + m_id.ToString();
        }
        public BufferData()
            : base(0, CommonData.HIRATA.CommonStaticData.g_CstSize)
        {
        }
        public BufferSlotType PSlot1Type
        {
            get { return (BufferSlotType)cv_Slot1Type; }
            set { cv_Slot1Type = (int)value; }
        }
        public BufferSlotType PSlot2Type
        {
            get { return (BufferSlotType)cv_Slot2Type; }
            set { cv_Slot2Type = (int)value; }

        }
        public BufferSlotType PSlot3Type
        {
            get { return (BufferSlotType)cv_Slot3Type; }
            set { cv_Slot3Type = (int)value; }
        }
        public BufferSlotType PSlot4Type
        {
            get { return (BufferSlotType)cv_Slot4Type; }
            set { cv_Slot4Type = (int)value; }
        }
        public BufferSlotType PSlot5Type
        {
            get { return (BufferSlotType)cv_Slot5Type; }
            set { cv_Slot5Type = (int)value; }
        }
        public BufferSlotType PSlot6Type
        {
            get { return (BufferSlotType)cv_Slot6Type; }
            set { cv_Slot6Type = (int)value; }
        }
        public int IsFreeSlot(BufferSlotType m_Type)
        {
            int slot = -1;
            for (int i = 1; i <= (int)cv_SlotCount ; i++)
            {
                if (cv_Types[i] == m_Type && !GlassDataMap[i].PHasData && !GlassDataMap[i].PHasSensor)
                {
                    slot = i;
                    break;
                }
            }
            return slot;
        }
        public bool GetUnloadSlot(BufferSlotType m_Type , out int m_Slot)
        {
            int slot = -1;
            for (int i = 1;  i <= cv_SlotCount; i++)
            {
                if (cv_Types[i] == m_Type)
                {
                    if (GlassDataMap[i].PHasData && GlassDataMap[i].PHasSensor)
                    {
                        if(slot == -1)
                        slot = i;
                        else
                        {
                            if(GlassDataMap[i].PEnterBufferTime < GlassDataMap[slot].PEnterBufferTime)
                            {
                                slot = i;
                            }
                        }
                    }
                }
            }
            m_Slot = slot;
            return m_Slot == -1 ? false : true;
        }
        public void SetSlotType(params BufferSlotType[] m_Para)
        {
            PSlot1Type = m_Para[0];
            cv_Types[1] = PSlot1Type;
            PSlot2Type = m_Para[1];
            cv_Types[2] = PSlot2Type;
            PSlot3Type = m_Para[2];
            cv_Types[3] = PSlot3Type;
            PSlot4Type = m_Para[3];
            cv_Types[4] = PSlot4Type;
            PSlot5Type = m_Para[4];
            cv_Types[5] = PSlot5Type;
            PSlot6Type = m_Para[5];
            cv_Types[6] = PSlot6Type;
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                string ori_path = cv_FilePath;
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["Data"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["Data"].ItemNumber != 0)
                {
                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName[typeof(BufferData).Name]);
                    this.GlassDataList = this.cv_GlassDataList;
                }
                if(cv_FilePath != ori_path)
                {
                    cv_FilePath = ori_path;
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<Data/>";
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            tmp.ItemsByName["Data"].AddItem(body);
            lock (cv_Obj)
            {
                try
                {
                    tmp.SaveToFile(cv_FilePath, true);
                }
                catch(Exception e)
                {
                }
            }
        }
    }
}
