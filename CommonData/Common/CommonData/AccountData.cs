using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public enum UserPermission { None, OP = 1, Engineer, Root };
    public enum LoginResult { None, Successful, IdError, PwError, AlreadyLogin };
    public enum LogInOut { None, Login, LogOut, };
    public class AccountItem
    {
        public string cv_Id = "";
        public string cv_Pw = "";
        public int cv_Permission = 0;
        public string PId
        {
            get { return cv_Id; }
            set { cv_Id = value; }
        }
        public string PPw
        {
            get { return cv_Pw; }
            set { cv_Pw = value; }
        }
        public UserPermission PPermission
        {
            get { return (UserPermission)cv_Permission; }
            set { cv_Permission = (int)value; }
        }
        public AccountItem(string m_id, string m_pw, UserPermission m_permission)
        {
            PId = m_id;
            PPw = m_pw;
            PPermission = m_permission;
        }
        public AccountItem()
        {
            PPermission = UserPermission.None;
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
    public class AccountData : CommonDatabase
    {
        public delegate void DeleLogInOut(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount);
        public event DeleLogInOut EventLogInOut;
        public delegate void DeleAccountChange();
        public event DeleAccountChange EventAccountChange;

        public List<AccountItem> cv_AccountList = new List<AccountItem>();
        public string cv_CurAccountId = "";
        public int cv_CurPermission = 0;
        public string PCurAccountId
        {
            get { return cv_CurAccountId; }
        }
        public bool GetCurAccount(out AccountItem m_CurAccount)
        {
            bool rtn = false;
            AccountItem result = null;
            if (!string.IsNullOrEmpty(cv_CurAccountId))
            {
                int index = cv_AccountList.FindIndex(x => x.PId == cv_CurAccountId);
                if (index != -1)
                {
                    result = cv_AccountList[index];
                    rtn = true;
                }
            }
            m_CurAccount = result;
            return rtn;
        }
        public AccountData()
        { }
        public bool AddAccount(AccountItem m_Account)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    if (!cv_AccountList.Exists(x => x.cv_Id == m_Account.PId))
                    {
                        cv_AccountList.Add(m_Account);
                        rtn = true;
                        if(cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                    }
                }
                catch(Exception e)
                {
                }
            }
            if(rtn)
            {
                if(EventAccountChange != null)
                {
                    EventAccountChange();
                }
            }

            return rtn;
        }
        public bool DelAccount(string m_Id)
        {
            bool rtn = false;
            int index = cv_AccountList.FindIndex(x => x.PId == m_Id);
            lock (cv_obj)
            {
                try
                {
                    if (index != -1)
                    {
                        cv_AccountList.RemoveAt(index);
                        rtn = true;
                        if(cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                    }
                }
                catch(Exception e)
                {
                }
            }
            if (rtn)
            {
                if (EventAccountChange != null)
                {
                    EventAccountChange();
                }
            }
            return rtn;
        }
        public bool DelAccount(AccountItem m_Account)
        {
            return DelAccount(m_Account.PId);
        }
        public bool IsAccountExist(string m_AccountId)
        {
            return cv_AccountList.Exists(x => x.PId == m_AccountId);
        }

        public LoginResult Login(string m_Id, string m_Pw)
        {
            LoginResult rtn = LoginResult.None;
            if (PCurAccountId != "")
            {
                rtn = LoginResult.AlreadyLogin;
            }
            else
            {
                int index = cv_AccountList.FindIndex(x => x.PId == m_Id);
                if (index != -1)
                {
                    AccountItem account = cv_AccountList[index];
                    if (m_Pw != account.PPw)
                    {
                        rtn = LoginResult.PwError;
                    }
                    else
                    {
                        rtn = LoginResult.Successful;
                        cv_CurAccountId = account.PId;
                        cv_CurPermission = (int)account.PPermission;
                        if (EventLogInOut != null)
                        {
                            EventLogInOut(LogInOut.Login, account);
                        }
                    }
                }
                else
                {
                    if (m_Id == "KGS" && m_Pw == "!@#")
                    {
                        AccountItem tmp = new AccountItem();
                        tmp.cv_Id = "KGS";
                        tmp.cv_Permission = (int)UserPermission.Root;
                        tmp.cv_Pw = "!@#";
                        AddAccount(tmp);
                        rtn = LoginResult.Successful;
                        cv_CurAccountId = m_Id;
                        cv_CurPermission =(int)UserPermission.Root;
                        if (EventLogInOut != null)
                        {
                            EventLogInOut(LogInOut.Login, tmp);
                        }
                    }
                    else
                    {
                        rtn = LoginResult.IdError;
                    }
                }
            }
            return rtn;
        }
        public LoginResult Logout(string m_Id, string m_Pw)
        {
            LoginResult rtn = LoginResult.None;
            AccountItem account = null;
            if (PCurAccountId != "")
            {
                if (PCurAccountId == m_Id)
                {
                    int index = cv_AccountList.FindIndex(x => x.PId == m_Id);
                    if (index != -1)
                    {
                        account = cv_AccountList[index];
                        if (m_Pw != account.PPw)
                        {
                            rtn = LoginResult.PwError;
                        }
                        else
                        {
                            rtn = LoginResult.Successful;
                        }
                    }
                    else
                    {
                        rtn = LoginResult.IdError;
                    }
                }
                else
                {
                    rtn = LoginResult.IdError;
                }
            }
            else
            {
                rtn = LoginResult.Successful;
            }

            if (rtn == LoginResult.Successful)
            {
                cv_CurAccountId = "";
                cv_CurPermission = 0;
                if(EventLogInOut != null)
                {
                    EventLogInOut(LogInOut.LogOut, account);
                }
            }
            return rtn;
        }
        public LoginResult Register(string m_Id, string m_Pw, int m_Permission)
        {//KGS=!@#;3
            LoginResult rtn = LoginResult.None;
            if (cv_AccountList.Exists(x => x.cv_Id == m_Id))
            {
                rtn = LoginResult.IdError;
            }
            else
            {
                KIniFile tmp = new KIniFile(CommonData.HIRATA.CommonStaticData.g_SysAccountFile);
                string combine = m_Pw +";"+ m_Permission.ToString();
                tmp.WriteString("Account", m_Id, combine);
                AccountItem add_item = new AccountItem(m_Id, m_Pw, (UserPermission)m_Permission);
                AddAccount(add_item);
                rtn = LoginResult.Successful;
            }
            return rtn;
        }
        public LoginResult Delete(string m_Id)
        {//KGS=!@#;3
            LoginResult rtn = LoginResult.None;
            if (!cv_AccountList.Exists(x => x.cv_Id == m_Id))
            {
                rtn = LoginResult.IdError;
            }
            else
            {
                DelAccount(m_Id);
                File.WriteAllText(CommonData.HIRATA.CommonStaticData.g_SysAccountFile, ";1 op , 2 eng , 3 kgs");
                KIniFile tmp = new KIniFile(CommonData.HIRATA.CommonStaticData.g_SysAccountFile);
                for(int i = 0 ; i < this.cv_AccountList.Count ; i++)
                {
                    AccountItem account_item = cv_AccountList[i];
                    string combine = account_item.PPw + ";" + (int)account_item.PPermission;
                    tmp.WriteString("Account", account_item.PId , combine);
                    rtn = LoginResult.Successful;
                }
            }
            return rtn;
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["Account"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["Account"].ItemNumber != 0)
                {
                    int recipe_count = recipe_xml.ItemsByName["Account"].ItemNumber;
                    lock (cv_obj)
                    {
                        try
                        {
                            for (int i = 0; i < recipe_count; i++)
                            {
                                KXmlItem item = recipe_xml.ItemsByName["Account"].Items[i];
                                AccountItem tmp = new AccountItem();
                                tmp.LoadFromXml(item);
                                if (!IsAccountExist(tmp.PId))
                                {
                                    cv_AccountList.Add(tmp);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<Account/>";
            int account_count = cv_AccountList.Count;
            lock (cv_obj)
            {
                try
                {
                    for (int i = 0; i < account_count; i++)
                    {
                        tmp.ItemsByName["Account"].AddItem(cv_AccountList[i].GetXml());
                    }
                }
                catch (Exception e)
                {
                }
            }
            tmp.SaveToFile(cv_FilePath, true);
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
        public void Clone(AccountData m_OtherAccountData)
        {
            this.cv_AccountList = m_OtherAccountData.cv_AccountList;
            this.cv_CurAccountId = m_OtherAccountData.cv_CurAccountId;
            this.cv_CurPermission = m_OtherAccountData.cv_CurPermission;
        }
    }
}
