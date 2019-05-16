using System;
using KgsCommon;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonData.HIRATA
{
    public class GlassCountData : CommonDatabase
    {
        public delegate void DeleGlassCountChange();
        public event DeleGlassCountChange EventGlassCountChange;

        public int cv_ProductCount = 0;
        public int cv_DummyCount = 0;
        public int cv_HistoryCount = 0;
        public int PProductCount
        {
            get { return cv_ProductCount; }
            set
            {
                if (value != cv_ProductCount)
                {
                    lock (cv_obj)
                    {
                        try
                        {
                            cv_ProductCount = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventGlassCountChange != null)
                            {
                                EventGlassCountChange();
                            }
                        }
                        catch(Exception e)
                        {
                        }
                    }
                }
            }
        }
        public int PDummyCount
        {
            get { return cv_DummyCount; }
            set
            {
                if(value != cv_DummyCount)
                {
                    lock (cv_obj)
                    {
                        try
                        {
                            cv_DummyCount = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventGlassCountChange != null)
                            {
                                EventGlassCountChange();
                            }
                        }
                        catch(Exception e)
                        {
                        }
                    }
                }
            }
        }
        public int PHistoryCount
        {
            get { return cv_HistoryCount; }
            set
            {
                if (value != cv_HistoryCount)
                {
                    lock (cv_obj)
                    {
                        try
                        {
                            cv_HistoryCount = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventGlassCountChange != null)
                            {
                                EventGlassCountChange();
                            }
                        }
                        catch(Exception e)
                        {
                        }
                    }
                }
            }
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem timeout_xml = new KXmlItem();
                timeout_xml.LoadFromFile(cv_FilePath);
                if (timeout_xml.ItemsByName["GlassCount"].ItemType == KXmlItemType.itxList && timeout_xml.ItemsByName["GlassCount"].ItemNumber != 0)
                {
                    this.LoadFromXml(timeout_xml);
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<GlassCount/>";
            tmp.ItemsByName["GlassCount"].AddItem(this.GetXml());
            tmp.SaveToFile(cv_FilePath, true);
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
        public void Clone(GlassCountData m_OtherData)
        {
            this.cv_ProductCount = m_OtherData.cv_ProductCount;
            this.cv_DummyCount = m_OtherData.cv_DummyCount;
            this.cv_HistoryCount = m_OtherData.cv_HistoryCount;
        }
        private KXmlItem GetXml()
        {
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            return body;
        }
        private void LoadFromXml(KXmlItem m_Xml)
        {
            EventCenterBase.ParseXmlToObject(this, m_Xml.ItemsByName[typeof(GlassCountData).Name]);
        }
    }
}
