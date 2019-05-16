using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class TimeOutData : CommonDatabase
    {
        public delegate void DeleTimeOutDataChange();
        public event DeleTimeOutDataChange EventTimeOutDataChange;

        public int cv_IdleDelayTime = 0;
        public int cv_IntervalTime = 0;
        public int cv_TsTime = 0;
        public int cv_TeTime = 0;
        public int cv_TmTime = 0;
        public int cv_T0Time = 0;
        public int cv_T1Time = 0;
        public int cv_T3Time = 0;
        public int cv_ApiT3TIme = 0;
        public int PApiT3TIme
        {
            get { return cv_ApiT3TIme; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (value != cv_ApiT3TIme)
                        {
                            cv_ApiT3TIme = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PT3Time
        {
            get { return cv_T3Time; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_T3Time != value)
                        {
                            cv_T3Time = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }

                }
            }
        }
        public int PT1Time
        {
            get { return cv_T1Time; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_T1Time != value)
                        {
                            cv_T1Time = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PT0Time
        {
            get { return cv_T0Time; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_T0Time != value)
                        {
                            cv_T0Time = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PTeTime
        {
            get { return cv_TeTime; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_TeTime != value)
                        {
                            cv_TeTime = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PTsTime
        {
            get { return cv_TsTime; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_TsTime != value)
                        {
                            cv_TsTime = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PTmTime
        {
            get { return cv_TmTime; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_TmTime != value)
                        {
                            cv_TmTime = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PIntervalTime
        {
            get { return cv_IntervalTime; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_IntervalTime != value)
                        {
                            cv_IntervalTime = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        public int PIdleDelayTime
        {
            get { return cv_IdleDelayTime; }
            set
            {
                lock (cv_obj)
                {
                    try
                    {
                        if (cv_IdleDelayTime != value)
                        {
                            cv_IdleDelayTime = value;
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            if (EventTimeOutDataChange != null)
                            {
                                EventTimeOutDataChange();
                            }
                        }
                    }
                    catch (Exception e)
                    {
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
                if (timeout_xml.ItemsByName["TimeOut"].ItemType == KXmlItemType.itxList && timeout_xml.ItemsByName["TimeOut"].ItemNumber != 0)
                {
                    this.LoadFromXml(timeout_xml);
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<TimeOut/>";
            tmp.ItemsByName["TimeOut"].AddItem(this.GetXml());
            tmp.SaveToFile(cv_FilePath, true);
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
        public void Clone(TimeOutData TimeOutData)
        {
            cv_IdleDelayTime = TimeOutData.cv_IdleDelayTime;
            cv_IntervalTime = TimeOutData.cv_IntervalTime;
            cv_TsTime = TimeOutData.cv_TsTime;
            cv_TeTime = TimeOutData.cv_TeTime;
            cv_TmTime = TimeOutData.cv_TmTime;
            cv_T0Time = TimeOutData.cv_T0Time;
            cv_T1Time = TimeOutData.cv_T1Time;
            cv_T3Time = TimeOutData.cv_T3Time;
            cv_ApiT3TIme = TimeOutData.cv_ApiT3TIme;
        }
        private KXmlItem GetXml()
        {
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            return body;
        }
        private void LoadFromXml(KXmlItem m_Xml)
        {
            EventCenterBase.ParseXmlToObject(this, m_Xml.ItemsByName[typeof(TimeOutData).Name]);
        }
    }
}
