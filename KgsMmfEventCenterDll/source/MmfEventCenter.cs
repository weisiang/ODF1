// Copyright (c) 2000-2018, Kingroup Systems Corporation. All rights reserved.
//
// History:
// Date         Reference       Person          Descriptions
// ---------- 	-------------- 	--------------  ---------------------------
// 2018/07/17    	            Cassius         Initial Implementation
// 2019/01/16   R20190116.01	Cassius			改成 thread 處理多個 request
//---------------------------------------------------------------------------
// 若需修改請回報
//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace KgsCommon
{
    public abstract class MmfEventCenter : EventCenterBase
    {
        //---------------------------------------------------------------------------
        internal class MmfEventProcessTimer
        {
            public bool ExtraTimer;
			//START R20190116.01
            private bool cv_IsClosed = false;
            private Queue<MmfEventData> cv_ProcessDataList = new Queue<MmfEventData>();
            private KCriticalSection cv_Cs = new KCriticalSection();
            //private KTimer cv_ProcessTimer;
            private KEvent cv_ThreadEvent = new KEvent();
            private Thread cv_Thread;
            private bool cv_Terminated = false;
            private bool cv_TerminateFlag = false;
            private bool cv_ProcessTimerEnabled = false;
            private MmfEventCenter cv_MmfEventCenter;
			//END R20190116.01

            //---------------------------------------------------------------------------
            public MmfEventProcessTimer(MmfEventCenter m_MmfEventCenter, bool m_ExtraTimer)
            {
                ExtraTimer = m_ExtraTimer;
                cv_MmfEventCenter = m_MmfEventCenter;

                //START R20190116.01
                cv_Thread = new Thread(new ThreadStart(ExecuteHandler));
                cv_Thread.Start();
                //cv_ProcessTimer = new KTimer();
                //cv_ProcessTimer.Interval = 1000;
                //cv_ProcessTimer.ThreadEventEnabled = true;
                //cv_ProcessTimer.OnTimer += OnProcessTimerTimeout;
                //END R20190116.01
            }

            //---------------------------------------------------------------------------
            public void Close()
            {
                //START R20190116.01
                if (cv_Thread != null)
                {
                    cv_Terminated = true;
                    cv_ThreadEvent.SetEvent();
                    for (int i = 0; i < 20; i++)
                    {
                        if (cv_TerminateFlag)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                    }
                    cv_Thread = null;

                    //try
                    //{
                    //    cv_ProcessTimer.OnTimer -= OnProcessTimerTimeout;
                    //    cv_ProcessTimer.Close();
                    //}
                    //catch
                    //{
                    //}
                    //cv_ProcessTimer = null;
                    cv_IsClosed = true;
                }
                //END R20190116.01
            }

            //---------------------------------------------------------------------------
            public void StartProcess(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, KXmlItem m_Body)
            {
                MmfEventData data = new MmfEventData();

                data.SourceModule = m_SourceModule;
                data.Type = m_Type;
                data.MessageId = m_MessageId;
                data.RequestNotifyMessageId = m_RequestNotifyMessageId;
                data.Ticket = m_Ticket;
                data.Body = m_Body;

                cv_Cs.Enter();
                try
                {
                    cv_ProcessDataList.Enqueue(data);
                }
                catch
                {
                }
                cv_Cs.Leave();

                //START R20190116.01
                //cv_ProcessTimer.Enabled = true;
                cv_ProcessTimerEnabled = true;
                cv_ThreadEvent.SetEvent();
                //END R20190116.01
            }

            //---------------------------------------------------------------------------
            //START R20190116.01
            private void ExecuteHandler()
            {
                try
                {
                    while(!cv_Terminated)
                    {
                        cv_ThreadEvent.WaitFor(500);
                        if( cv_Terminated)
                        {
                            break;
                        }

                        if (!cv_ProcessTimerEnabled)
                        {
                            continue;
                        }

                        OnProcessTimerTimeout();

                        if (cv_Terminated)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    cv_Terminated = true;
                    cv_TerminateFlag = true;
                }
                catch
                {
                }
            }
            //END R20190116.01

            //---------------------------------------------------------------------------
            private void OnProcessTimerTimeout()
            {
                //START R20190116.01
                if( cv_IsClosed )
                {
                    return;
                }

                //if (cv_ProcessTimer == null )
                if (cv_Thread == null )
                {
                    return;
                }

                //cv_ProcessTimer.Enabled = false;
                cv_ProcessTimerEnabled = false;
                //END R20190116.01

                MmfEventData data = null;
                cv_Cs.Enter();
                try
                {
                    if( cv_ProcessDataList.Count > 0 )
                    {
                        data = cv_ProcessDataList.Dequeue();
                        cv_ProcessDataList.Clear();
                    }
                }
                catch
                {
                }
                cv_Cs.Leave();

                //START R20190116.01
                try
                {
                    if (data != null)
                    {
                        cv_MmfEventCenter.ProcessMmfEventClientMessageReceived(data.SourceModule, data.Type, data.MessageId, data.RequestNotifyMessageId, data.Ticket, data.Body);
                    }
                }
                catch (Exception ex)
                {
                    cv_MmfEventCenter.WriteDebugLog("[MmfEventCenter][MmfEventProcessTimer] Error! " + ex.Message, KLogLevelType.Error);
                }
                //END R20190116.01

                cv_MmfEventCenter.PushProcessTimer(this);
            }

            public class MmfEventData
            {
                public string SourceModule;
                public int Type;
                public string MessageId;
                public string RequestNotifyMessageId;
                public uint Ticket;
                public KXmlItem Body;
            }
        }


        //---------------------------------------------------------------------------
        public delegate void MmfEventClientMessageHandler(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, KXmlItem m_Body);
        public delegate void MmfEventClientObjectMessageHandler(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object);
        protected Dictionary<string, MmfEventClientMessageHandler> cv_MmfEventClientHandlerSet = new Dictionary<string, MmfEventClientMessageHandler>();
        protected Dictionary<string, MmfEventClientObjectMessageHandler> cv_MmfEventClientObjectHandlerSet = new Dictionary<string, MmfEventClientObjectMessageHandler>();

        protected KMmfEventClient cv_MmfEventClient = null;

        protected KFileLog cv_MmfEventLog = new KFileLog();

        protected Assembly cv_Assembly;

        public bool TriggerOnDispatchMmfMessage = false;
        public bool TriggerOnDispatchMmfObjectMessage = false;
        public bool ConvertMmfMessageToObject = true;
        public bool PassMmfMessageToAppEventMessage = false;
        public bool PassMmfMessageToAppEventObject = true;
        public string MmfObjectNamespace = "Module";

        public int ProcessTimerCount = 5;
        private Queue<MmfEventProcessTimer> cv_ProcessTimerList = new Queue<MmfEventProcessTimer>();
        private Queue<MmfEventProcessTimer> cv_ReleaseProcessTimerList = new Queue<MmfEventProcessTimer>();
        private List<MmfEventProcessTimer> cv_ReleaseProcessTimerList2 = new List<MmfEventProcessTimer>();
        private KTimer cv_ReleaseTimer = new KTimer();

        //---------------------------------------------------------------------------
        string cv_Module = "";
        public MmfEventCenter(string module)
        {
            cv_Module = module;
            string exe_pathname = SysUtils.GetExeName();
            cv_Assembly = Assembly.LoadFrom(exe_pathname);

            cv_ReleaseTimer.Interval = 60000;
            cv_ReleaseTimer.ThreadEventEnabled = true;
            cv_ReleaseTimer.OnTimer += OnReleaseTimerTimeout;
            cv_ReleaseTimer.Enabled = true;
        }

        //---------------------------------------------------------------------------
        private void OnReleaseTimerTimeout()
        {
            if( cv_ReleaseProcessTimerList.Count > 0 )
            {
                if (cv_ReleaseProcessTimerList.Count > 0 )
                {
                    lock (cv_ReleaseProcessTimerList)
                    {
                        foreach (MmfEventProcessTimer process_timer in cv_ReleaseProcessTimerList)
                        {
                            cv_ReleaseProcessTimerList2.Add(process_timer);
                        }
                        cv_ReleaseProcessTimerList.Clear();
                    }
                }

                if (cv_ReleaseProcessTimerList2.Count > 0)
                {
                    foreach (MmfEventProcessTimer process_timer in cv_ReleaseProcessTimerList2)
                    {
                        try
                        {
                            process_timer.Close();
                        }
                        catch
                        {
                        }
                    }

                    cv_ReleaseProcessTimerList2.Clear();
                }
            }
        }

        //---------------------------------------------------------------------------
        public Dictionary<string, bool> GetMmfClientConnectionStatus()
        {
            return cv_MmfEventClient.ConnectionStatus;
        }

        //---------------------------------------------------------------------------
        public override void Open()
        {
            cv_MmfEventLog.LoadFromIni(GlobalBase.LogIniPathname, "MmfEventClient");
            if( string.IsNullOrEmpty(SysUtils.ExtractFileName((cv_MmfEventLog.LogFileName))) )
            {
                cv_MmfEventLog.LogFileName = "Log\\MmfEventClientLog.log";
                cv_MmfEventLog.LogFileSize = 5000000;
            }
            cv_MmfEventLog.SaveToIni(GlobalBase.LogIniPathname, "MmfEventClient");

            if( cv_MmfEventClient == null )
            {
                KIniFile ini = new KIniFile(GlobalBase.SystemIniPathname);
                string module_id = ini.ReadString("Config", "ModuleId", "DEFAULT");
                ini.WriteString("Config", "ModuleId", module_id);

                cv_MmfEventClient = new KMmfEventClient(module_id, cv_MmfEventLog);
                cv_MmfEventClient.OnMessageReceived += OnMmfEventClientMessageReceived;

                ProcessTimerCount = SysUtils.StrToInt(ini.ReadString("Config", "ProcessTimerCount", "5"));
                if( ProcessTimerCount < 1 )
                {
                    ProcessTimerCount = 1;
                }

                int i;
                lock(cv_ProcessTimerList)
                {
                    MmfEventProcessTimer process_time;
                    for( i=0; i<ProcessTimerCount; ++i )
                    {
                        process_time = new MmfEventProcessTimer(this, false);
                        cv_ProcessTimerList.Enqueue(process_time);
                    }
                }

            }

            base.Open();
        }

        //---------------------------------------------------------------------------
        private MmfEventProcessTimer GetProcessTimer()
        {
            MmfEventProcessTimer process_timer = null;
            lock(cv_ProcessTimerList)
            {
                if( cv_ProcessTimerList.Count > 0 )
                {
                    process_timer = cv_ProcessTimerList.Dequeue();
                }
            }

            if (process_timer == null)
            {
                lock (cv_ReleaseProcessTimerList)
                {
                    if (cv_ReleaseProcessTimerList.Count > 0)
                    {
                        process_timer = cv_ReleaseProcessTimerList.Dequeue();
                    }
                }
            }

            if (process_timer == null)
            {
                process_timer = new MmfEventProcessTimer(this, true);
            }

            return process_timer;
        }

        internal void PushProcessTimer(MmfEventProcessTimer m_ProcessTimer)
        {
            if( m_ProcessTimer.ExtraTimer )
            {
                lock (cv_ReleaseProcessTimerList)
                {
                    cv_ReleaseProcessTimerList.Enqueue(m_ProcessTimer);
                }
            }
            else
            {
                lock(cv_ProcessTimerList)
                {
                    cv_ProcessTimerList.Enqueue(m_ProcessTimer);
                }
            }
        }

        //---------------------------------------------------------------------------
        virtual protected void OnDispatchMmfMessage(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, KXmlItem m_Body)
        {

        }

        //---------------------------------------------------------------------------
        virtual protected void OnDispatchMmfObjectMessage(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, Object m_Object)
        {

        }

        //---------------------------------------------------------------------------
        private void OnMmfEventClientMessageReceived(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, KXmlItem m_Body)
        {
            WriteDebugLog("[MmfEventClient] Recv " + m_MessageId + ". Source=" + m_SourceModule, KLogLevelType.MsgName);

            MmfEventProcessTimer process_timer = this.GetProcessTimer();
            if( process_timer != null )
            {
                process_timer.StartProcess(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Body);
            }
        }

        internal void ProcessMmfEventClientMessageReceived(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, KXmlItem m_Body)
        {
            try
            {
                MmfEventClientMessageHandler handler = null;
                lock (cv_MmfEventClientHandlerSet)
                {
                    if (cv_MmfEventClientHandlerSet.ContainsKey(m_MessageId))
                    {
                        handler = cv_MmfEventClientHandlerSet[m_MessageId];
                    }
                }

                if (handler != null)
                {
                    WriteDebugLog("[MmfEventCenter][MmfEvent] Run message handler for " + m_MessageId, KLogLevelType.Detail);
                    handler(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Body);
                }
                else
                {
                    // Not really a warning
                    WriteDebugLog("[MmfEventCenter][MmfEvent] No message handler for " + m_MessageId, KLogLevelType.Detail);
                }
            }
            catch (Exception ex)
            {
                WriteDebugLog("[MmfEventCenter][MmfEvent] Error! " + ex.Message, KLogLevelType.Error);
            }

            if (TriggerOnDispatchMmfMessage)
            {
                try
                {
                    OnDispatchMmfMessage(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Body);
                }
                catch (Exception ex)
                {
                    WriteDebugLog("[MmfEventCenter][MmfEvent] Error! " + ex.Message, KLogLevelType.Error);
                }
            }

            if (PassMmfMessageToAppEventMessage)
            {
                this.SendAppMessage(m_SourceModule, m_MessageId, m_Body);
            }

            if (ConvertMmfMessageToObject)
            {
                try
                {
                    Object obj = null;
                    string type_string = MmfObjectNamespace + "." + m_MessageId;
                    //Type type = Type.GetType(type_string);
                    Type type = cv_Assembly.GetType(type_string);
                    if (type != null)
                    {
                        obj = Activator.CreateInstance(type);
                        if (obj != null)
                        {
                            if (!ParseXmlToObject(obj, m_Body))
                            {
                                obj = null;
                                WriteDebugLog("[MmfEventCenter][MmfEvent] Parse object " + type_string + " failed.", KLogLevelType.Detail);
                            }
                        }
                        else
                        {
                            WriteDebugLog("[MmfEventCenter][MmfEvent] Can not create object " + type_string, KLogLevelType.Detail);
                        }
                    }

                    if (obj != null)
                    {
                        MmfEventClientObjectMessageHandler handler = null;
                        lock (cv_MmfEventClientObjectHandlerSet)
                        {
                            if (cv_MmfEventClientObjectHandlerSet.ContainsKey(m_MessageId))
                            {
                                handler = cv_MmfEventClientObjectHandlerSet[m_MessageId];
                            }
                        }

                        if (handler != null)
                        {
                            WriteDebugLog("[MmfEventCenter][MmfEvent] Run object handler for " + m_MessageId, KLogLevelType.Detail);
                            handler(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, obj);
                        }
                        else
                        {
                            // Not really a warning
                            WriteDebugLog("[MmfEventCenter][MmfEvent] No object handler for " + m_MessageId, KLogLevelType.Detail);
                        }

                        if (TriggerOnDispatchMmfObjectMessage)
                        {
                            try
                            {
                                OnDispatchMmfObjectMessage(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, obj);
                            }
                            catch (Exception ex)
                            {
                                WriteDebugLog("[MmfEventCenter][MmfEvent] Error! " + ex.Message, KLogLevelType.Error);
                            }
                        }

                        if (PassMmfMessageToAppEventObject)
                        {
                            this.SendAppObjectMessage(m_SourceModule, m_MessageId, obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteDebugLog("[MmfEventCenter][MmfEvent] Error! " + ex.Message, KLogLevelType.Error);
                }
            }
        }

        //---------------------------------------------------------------------------
        public uint SendMmfRequestMessage(string m_MessageId, KXmlItem m_Body)
        {
            WriteDebugLog("[SendMmfRequestMessage] Send " + m_MessageId, 2);

            return cv_MmfEventClient.SendRequestMessage(m_MessageId, m_Body);
        }

        //---------------------------------------------------------------------------
        public bool SendMmfRequestMessageTimeout(string m_RequestMessageId, KXmlItem m_RequestBody, out uint m_Ticket, out string m_ReplyMessageId, out KXmlItem m_ReplyBody, int m_Timeout)
        {
            WriteDebugLog("[SendMmfRequestMessageTimeout] Send " + m_RequestMessageId, 2);

            bool result = cv_MmfEventClient.SendRequestMessageTimeout(m_RequestMessageId, m_RequestBody, out m_Ticket, out m_ReplyMessageId, out m_ReplyBody, m_Timeout);
            WriteDebugLog("[SendMmfRequestMessageTimeout] Result=" + (result?"1":"0"), 4);
            if( result )
            {
                if( m_ReplyMessageId != null)
                {
                    WriteDebugLog("[SendMmfRequestMessageTimeout] ReplyMessageId=" + m_ReplyMessageId, 4);
                }
                else
                {
                    result = false;
                }

                if( m_ReplyBody != null )
                {
                    WriteDebugLog(m_ReplyBody.XmlByStrings, 4);
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        //---------------------------------------------------------------------------
        public void SendMmfReplyMessage(string m_MessageId, KXmlItem m_Body, uint m_Ticket, string m_RequestMessageId)
        {
            WriteDebugLog("[SendMmfReplyMessage] Send " + m_MessageId, 2);

            cv_MmfEventClient.SendReplyMessage(m_MessageId, m_Body, m_Ticket, m_RequestMessageId);
        }

        //---------------------------------------------------------------------------
        public void SendMmfNotifyMessage(string m_MessageId, KXmlItem m_Body)
        {
            WriteDebugLog("[SendMmfNotifyMessage] Send " + m_MessageId, 2);

            cv_MmfEventClient.SendNotifyMessage(m_MessageId, m_Body);
        }

        //---------------------------------------------------------------------------
        public uint SendMmfRequestObject(string m_MessageId, Object m_Object)
        {
            return SendMmfRequestObject(m_MessageId, m_Object, KParseObjToXmlPropertyType.All);
        }

        //---------------------------------------------------------------------------
        public uint SendMmfRequestObject(string m_MessageId, Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            WriteDebugLog("[SendMmfRequestObject] Send " + m_MessageId, KLogLevelType.MsgName);

            KXmlItem body = ParseObjectToKXmlItem(m_Object, m_PropertyType);
            body.Name = "Body";

            return cv_MmfEventClient.SendRequestMessage(m_MessageId, body);
        }

        //---------------------------------------------------------------------------
        public bool SendMmfRequestObjectTimeout(string m_RequestMessageId, Object m_RequestObject, out uint m_Ticket, out string m_ReplyMessageId, out Object m_ReplyObject, int m_Timeout)
        {
            return SendMmfRequestObjectTimeout(m_RequestMessageId, m_RequestObject, out m_Ticket, out m_ReplyMessageId, out m_ReplyObject, m_Timeout, KParseObjToXmlPropertyType.All);
        }

        //---------------------------------------------------------------------------
        public bool SendMmfRequestObjectTimeout(string m_RequestMessageId, Object m_RequestObject, out uint m_Ticket, out string m_ReplyMessageId, out Object m_ReplyObject, int m_Timeout, KParseObjToXmlPropertyType m_PropertyType)
        {
            WriteDebugLog("[SendMmfRequestObjectTimeout] Send " + m_RequestMessageId, KLogLevelType.MsgName);

            m_ReplyObject = null;

            KXmlItem request_body = ParseObjectToKXmlItem(m_RequestObject, m_PropertyType);
            request_body.Name = "Body";

            KXmlItem reply_body;
            bool result = cv_MmfEventClient.SendRequestMessageTimeout(m_RequestMessageId, request_body, out m_Ticket, out m_ReplyMessageId, out reply_body, m_Timeout);
            WriteDebugLog("[SendMmfRequestMessageTimeout] Result=" + (result ? "1" : "0"), KLogLevelType.Detail);
            if (result)
            {
                if (m_ReplyMessageId != null)
                {
                    WriteDebugLog("[SendMmfRequestMessageTimeout] ReplyMessageId=" + m_ReplyMessageId, KLogLevelType.MsgName);
                }
                else
                {
                    result = false;
                }

                if (reply_body != null)
                {
                    WriteDebugLog(reply_body.XmlByStrings, KLogLevelType.MsgArg);
                }
                else
                {
                    result = false;
                }

                if (result)
                {
                    Object obj = null;
                    string type_string = MmfObjectNamespace + "." + m_ReplyMessageId;
                    //Type type = Type.GetType(type_string);
                    Type type = cv_Assembly.GetType(type_string);
                    if (type != null)
                    {
                        obj = Activator.CreateInstance(type);
                        if (obj != null)
                        {
                            if (!ParseXmlToObject(obj, reply_body))
                            {
                                result = false;
                                obj = null;
                                WriteDebugLog("[MmfEventCenter][SendMmfRequestObjectTimeout] Parse object " + type_string + " failed.", KLogLevelType.Detail);
                            }
                            else
                            {
                                m_ReplyObject = obj;
                            }
                        }
                        else
                        {
                            result = false;
                            WriteDebugLog("[MmfEventCenter][SendMmfRequestObjectTimeout] Can not create object " + type_string, KLogLevelType.Detail);
                        }
                    }
                }
            }
            return result;
        }

        //---------------------------------------------------------------------------
        public void SendMmfReplyObject(string m_MessageId, Object m_Object, uint m_Ticket, string m_RequestMessageId)
        {
            SendMmfReplyObject(m_MessageId, m_Object, m_Ticket, m_RequestMessageId, KParseObjToXmlPropertyType.All);
        }

        //---------------------------------------------------------------------------
        public void SendMmfReplyObject(string m_MessageId, Object m_Object, uint m_Ticket, string m_RequestMessageId, KParseObjToXmlPropertyType m_PropertyType)
        {
            WriteDebugLog("[SendMmfReplyObject] Send " + m_MessageId, 2);

            KXmlItem body = ParseObjectToKXmlItem(m_Object, m_PropertyType);
            body.Name = "Body";


            cv_MmfEventClient.SendReplyMessage(m_MessageId, body, m_Ticket, m_RequestMessageId);
        }

        //---------------------------------------------------------------------------
        public void SendMmfNotifyObject(string m_MessageId, Object m_Object)
        {
            SendMmfNotifyObject(m_MessageId, m_Object, KParseObjToXmlPropertyType.All);
        }

        //---------------------------------------------------------------------------
        public void SendMmfNotifyObject(string m_MessageId, Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            WriteDebugLog("[SendMmfNotifyObject] Send " + m_MessageId, 2);

            KXmlItem body = ParseObjectToKXmlItem(m_Object, m_PropertyType);
            body.Name = "Body";

            cv_MmfEventClient.SendNotifyMessage(m_MessageId, body);
        }

        //---------------------------------------------------------------------------
        protected void AssignMmfEventMessageFunction(string m_MessageId, MmfEventClientMessageHandler m_Handler)
        {
            lock (cv_MmfEventClientHandlerSet)
            {
                cv_MmfEventClientHandlerSet.Add(m_MessageId, m_Handler);
            }
        }

        //---------------------------------------------------------------------------
        protected void AssignMmfEventObjectFunction(string m_MessageId, MmfEventClientObjectMessageHandler m_Handler)
        {
            lock (cv_MmfEventClientObjectHandlerSet)
            {
                cv_MmfEventClientObjectHandlerSet.Add(m_MessageId, m_Handler);
            }
        }
    }
}
