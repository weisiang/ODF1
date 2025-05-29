using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class SamplingIem
    {
        public int cv_No;
        public int cv_Method;
        public int cv_FromFront;
        public List<int> cv_hitSlot = new List<int>();
        public string cv_Desription = "";
        public string cv_Time = "";
        public int cv_n = 0;
        public int cv_m = 0;
        public string PlistStr
        {
            get
            {
                string rtn = "";
                if(PMethod == SamplingMethod.BySlot)
                rtn = string.Join(",", cv_hitSlot);
                else if(PMethod == SamplingMethod.ByPeriod)
                {
                    rtn = cv_m + "/" + cv_n + " (From : " + (cv_FromFront == 1 ? " Front" : "Back") +")";
                }
                return rtn;
            }
        }
        public int PN
        {
            get { return cv_n; }
            set { cv_n = value; }
        }
        public int PM
        {
            get { return cv_m; }
            set { cv_m = value; }
        }
        public bool PFromfornt
        {
            get { return cv_FromFront==1; }
            set
            {
                cv_FromFront = value ? 1 : 0;
            }
        }
        public string PDesription
        {
            get { return cv_Desription; }
            set { cv_Desription = value; }
        }
        
        public SamplingMethod PMethod
        {
            get { return (SamplingMethod)cv_Method; }
            set
            {
                cv_Method = (int)value;
            }
        }
        public bool PIsFull
        {
            get
            {
                bool rtn = false;
                if(PMethod == SamplingMethod.ByPeriod)
                {
                    if(cv_n == cv_m)
                    {
                        rtn = true;
                    }
                }
                else if(PMethod == SamplingMethod.BySlot)
                {
                    rtn = true;
                    for(int i=1; i<=25;i++)
                    {
                        if(!cv_hitSlot.Contains(i))
                        {
                            rtn = false;
                        }
                    }
                }
                return rtn;
            }
        }
        public int PNo
        {
            get { return cv_No; }
            set
            {
                cv_No = value;
            }
        }
        public string PTime
        {
            get { return cv_Time; }
            set { cv_Time = value; }
        }
        public List<int> PHitList
        {
            get { return cv_hitSlot; }
            set
            {
                cv_hitSlot = value;
            }
        }
        public bool IsHit(int m_slot)
        {
            bool rtn = false;
            if(m_slot != 0 )
            {
                if(cv_hitSlot.Contains(m_slot) )
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        public KXmlItem GetXml()
        {
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            return body;
        }
        public void LoadFromXml(KXmlItem m_Xml)
        {
            EventCenterBase.ParseXmlToObject(this, m_Xml);
        }
    }
    public class SamplingData : CommonDatabase
    {
        public delegate void DeleSamplingAction(DataEidtAction m_Action, List<SamplingIem> m_Samplings);
        public event DeleSamplingAction EventSamplingAction;
        public delegate void DeleSamplingDatahange();
        public event DeleSamplingDatahange EventSamplingDatahange;

        public List<SamplingIem> cv_SamplingList = new List<SamplingIem>();

        public SamplingData()
        {
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["SAMPLING"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["SAMPLING"].ItemNumber != 0)
                {

                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName["SamplingData"]);
                    /*
                    int recipe_count = recipe_xml.ItemsByName["PPID"].ItemNumber;
                    lock (cv_obj)
                    {
                        try
                        {
                            for (int i = 0; i < recipe_count; i++)
                            {
                                KXmlItem item = recipe_xml.ItemsByName["PPID"].Items[i];
                                RecipeItem tmp = new RecipeItem();
                                tmp.LoadFromXml(item);
                                if (!IsRecipeExist(tmp.PId))
                                {
                                    cv_RecipeList.Add(tmp);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    */
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<SAMPLING/>";
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            tmp.ItemsByName["SAMPLING"].AddItem(body);
            //int recipe_count = cv_RecipeList.Count;
            /*
            lock (cv_obj)
            {
                try
                {
                    for (int i = 0; i < recipe_count; i++)
                    {
                        tmp.ItemsByName["PPID"].AddItem(cv_RecipeList[i].GetXml());
                    }
                }
                catch (Exception e)
                {
                }
            }
            */
            tmp.SaveToFile(cv_FilePath, true);
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
        public bool IsSamplingItemExist(int m_ItemId)
        {
            return cv_SamplingList.Exists(x => x.PNo == m_ItemId);
        }
        public bool IsSamplingItemExist(SamplingIem m_SamplingItem)
        {
            return IsSamplingItemExist(m_SamplingItem.PNo);
        }
        public SamplingIem getSamplingItem(int cv_No)
        {
            SamplingIem rtn = null;
            if(IsSamplingItemExist(cv_No))
            {
                int index = cv_SamplingList.FindIndex(x => x.PNo == cv_No);
                if(index != -1)
                {
                    rtn = cv_SamplingList[index];
                }
            }
            return rtn;
        }
        public bool AddSamplingItem(SamplingIem m_SamplingItem)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    if (!IsSamplingItemExist(m_SamplingItem))
                    {
                        cv_SamplingList.Add(m_SamplingItem);
                        if (cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                        if (EventSamplingDatahange != null)
                        {
                            EventSamplingDatahange();
                        }
                        rtn = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            return rtn;
        }
        public void AddSamplingItem(List<SamplingIem> m_SamplingItems)
        {
            foreach (SamplingIem item in m_SamplingItems)
            {
                AddSamplingItem(item);
            }
        }
        public bool DelSamplingItem(SamplingIem m_SamplingItem)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    int index = cv_SamplingList.FindIndex(x => x.PNo == m_SamplingItem.PNo);
                    if (index != -1)
                    {
                        cv_SamplingList.RemoveAt(index);
                        if (cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                        if (EventSamplingDatahange != null)
                        {
                            EventSamplingDatahange();
                        }
                        rtn = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            return rtn;
        }
        public void DelSamplingItem(List<SamplingIem> m_SamplingItems)
        {
            foreach (SamplingIem item in m_SamplingItems)
            {
                DelSamplingItem(item);
            }
        }
        public bool ModifySamplingItem(SamplingIem m_SamplingItem)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    if (IsSamplingItemExist(m_SamplingItem))
                    {
                        int index = cv_SamplingList.FindIndex(x => x.PNo == m_SamplingItem.PNo);
                        if (index != -1)
                        {
                            cv_SamplingList.RemoveAt(index);
                            cv_SamplingList.Add(m_SamplingItem);
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            rtn = true;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            if (rtn)
            {
                if (EventSamplingDatahange != null)
                {
                    EventSamplingDatahange();
                }
            }
            return rtn;
        }
        public void Clone(SamplingData m_OtherSamplingData)
        {
            this.cv_SamplingList = m_OtherSamplingData.cv_SamplingList;
        }
    }
}
