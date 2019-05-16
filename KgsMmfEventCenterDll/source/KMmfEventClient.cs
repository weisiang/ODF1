// Copyright (c) 2000-2018, Kingroup Systems Corporation. All rights reserved.
//
// History:
// Date         Reference       Person          Descriptions
// ---------- 	-------------- 	--------------  ---------------------------
// 2018/07/17    	            Cassius         Initial Implementation
//
//---------------------------------------------------------------------------
// Version: 1.0.0.1001
// 若需修改請回報
//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace KgsCommon
{
    public class KMmfEventClient
    {
        //---------------------------------------------------------------------------
        public delegate void KMmfEventClientMessageEvent(string m_SourceModule, int m_Type, string m_MessageId, string m_RequestNotifyMessageId, uint m_Ticket, KXmlItem m_Body);
        public event KMmfEventClientMessageEvent OnMessageReceived;


        //---------------------------------------------------------------------------
        public enum KMmfEventClientEventType { etRequest = 1, etReply = 2, etNotify = 3 };
        public class KMmfEventClientEventMessage
        {
            public string SourceModule = "";
            public KMmfEventClientEventType Type = KMmfEventClientEventType.etNotify;
            public string RequestNotifyMessageId = "";
            public string ReplyMessageId = "";
            public uint Ticket = 0;
            public KXmlItem Body;
        };

        //---------------------------------------------------------------------------
        private class RequestReplyData
        {
            public string MessageId;
            public uint Ticket;
            public Int64 StartTime;
            public bool RecvReplyFlag;
            public KMmfEventClientEventMessage ReplyMessage;
        };

        //---------------------------------------------------------------------------
        private string                  cv_MmfEventId;
        private uint                    cv_Serial;

        private KCriticalSection        cv_Cs = new KCriticalSection();
        private KMmfEventBus            cv_EventBus;
        private KFileLog                cv_FileLog;
        private KXmlItem                cv_XmlTemplate = new KXmlItem();
        private Int64                   cv_LastRecvTime = 0;
        private Dictionary<string, bool> cv_ConnectionStatus = new Dictionary<string, bool>();
        private KTimer                  cv_ProcessTimer;
        private KXmlItem                cv_NullBodyItem;

        Dictionary<uint, RequestReplyData> cv_OpenRequestMap = new Dictionary<uint,RequestReplyData>();

        public Dictionary<string, bool> ConnectionStatus
        {
            get
            {
                return cv_ConnectionStatus;
            }
        }


        //---------------------------------------------------------------------------
        public KMmfEventClient(string m_MmfEventId, KFileLog m_FileLog)
        {
            cv_MmfEventId = m_MmfEventId;
            cv_FileLog = m_FileLog;
            Random rand = new Random();
            cv_Serial = (uint)rand.Next(1, 10000);

            cv_XmlTemplate.Text = "<Message><SourceModule>Source</SourceModule><Type>Request</Type><RequestNotifyMessageId>RequestId</RequestNotifyMessageId><Ticket>0</Ticket><Body KGS_TYPE =\"L\"></Body></Message>";

            cv_NullBodyItem = new KXmlItem();
            cv_NullBodyItem.Name = "Body";
            cv_NullBodyItem.ItemType = KXmlItemType.itxList;
            cv_NullBodyItem.ItemNumber = 0;

            cv_EventBus = new KMmfEventBus();
            cv_EventBus.MmfEventId = m_MmfEventId;
            cv_EventBus.Master = true;
            cv_EventBus.ThreadEventEnabled = true;
            cv_EventBus.OnMessageReceived += OnMmfEventMessageReceive;
            cv_EventBus.Open();

            cv_ProcessTimer = new KTimer();
            cv_ProcessTimer.ThreadEventEnabled = true;
            cv_ProcessTimer.Interval = 5000;
            cv_ProcessTimer.OnTimer += OnProcessTimer;
            cv_ProcessTimer.Enabled = true;

            this.SendNotifyMessage("MmfEventDispatchServerConnectionStatusQuery", null);
        }

        //---------------------------------------------------------------------------
        void OnProcessTimer()
        {
            Int64 current_time = this.GetSystemTickCount();
            Int64 diff = current_time - cv_LastRecvTime;
            if( diff < 0 )
            {
                cv_LastRecvTime = current_time;
            }

            if( cv_LastRecvTime >= 20000 )
            {
                cv_LastRecvTime = current_time;
                this.SendNotifyMessage("MmfEventDispatchServerConnectionStatusQuery", null);
            }
        }

        //---------------------------------------------------------------------------
        public void Close()
        {
            cv_EventBus.Close();
        }

        //---------------------------------------------------------------------------
        public Int64 GetSystemTickCount()
        {
            Int64 tick_count = SysUtils.GetTickCount() & 0x00000000FFFFFFFF;
            return tick_count;
        }

        //---------------------------------------------------------------------------
        public uint SendRequestMessage(string m_MessageId, KXmlItem m_Body)
        {
            if( m_Body == null )
            {
                if (cv_NullBodyItem.ItemType != KXmlItemType.itxList || cv_NullBodyItem.ItemNumber != 0 || cv_NullBodyItem.Name != "Body" )
                {
                    cv_NullBodyItem.Name = "Body";
                    cv_NullBodyItem.ItemType = KXmlItemType.itxList;
                    cv_NullBodyItem.ItemNumber = 0;
                }
                m_Body = cv_NullBodyItem;
            }

            cv_FileLog.WriteLog("Enter KMmfEventClient::SendRequestMessage", 4);
            cv_FileLog.WriteLog("MessageId: " + m_MessageId, 2);
            cv_FileLog.WriteLog(m_Body.XmlByStrings, 3);

            uint ticket = 0;
            KXmlItem message = null;
            cv_Cs.Enter();
            try
            {
                message = cv_XmlTemplate.Clone();
                ticket = cv_Serial;
                ++cv_Serial;
            }
            catch(Exception e)
            {
                try
                {
                    cv_FileLog.WriteLog(e.Message, 1);
                }
                catch
                {
                }
            }
            cv_Cs.Leave();

            message.ItemsByName["SourceModule"].AsString = cv_MmfEventId;
            message.ItemsByName["Type"].AsString = "Request";
            message.ItemsByName["RequestNotifyMessageId"].AsString = m_MessageId;
            message.ItemsByName["Ticket"].AsString = ticket.ToString();

            if( m_Body.Name == "Body" && m_Body.ItemType == KXmlItemType.itxList )
            {
                message.ItemsByName["Body"] = m_Body;
            }
            else
            {
                KXmlItem body_item = message.ItemsByName["Body"];
                body_item.ItemType = KXmlItemType.itxList;
                body_item.ItemNumber = 1;
                body_item.Items[0] = m_Body;
            }

            cv_EventBus.SendMessage("EventChannel", m_MessageId, message);

            cv_FileLog.WriteLog("Leave KMmfEventClient::SendRequestMessage", 4);

            return ticket;
        }

        //---------------------------------------------------------------------------
        public bool SendRequestMessageTimeout(string m_RequestMessageId, KXmlItem m_RequestBody, out uint m_Ticket, out string m_ReplyMessageId, out KXmlItem m_ReplyBody, int m_Timeout)
        {
            cv_FileLog.WriteLog("Enter KMmfEventClient::SendRequestMessageTimeout", 4);
            cv_FileLog.WriteLog("MessageId: " + m_RequestMessageId, 2);
            cv_FileLog.WriteLog(m_RequestBody.XmlByStrings, 3);

            m_Ticket = 0;
            m_ReplyMessageId = string.Empty;
            m_ReplyBody = null;

            KXmlItem message = null;
            cv_Cs.Enter();
            try
            {
                message = cv_XmlTemplate.Clone();
                m_Ticket = cv_Serial;
                ++cv_Serial;
            }
            catch(Exception e)
            {
                try
                {
                    cv_FileLog.WriteLog(e.Message, 1);
                }
                catch
                {
                }
            }
            cv_Cs.Leave();

            message.ItemsByName["SourceModule"].AsString = cv_MmfEventId;
            message.ItemsByName["Type"].AsString = "Request";
            message.ItemsByName["RequestNotifyMessageId"].AsString = m_RequestMessageId;
            message.ItemsByName["Ticket"].AsString = m_Ticket.ToString();

            if (m_RequestBody.Name == "Body" && m_RequestBody.ItemType == KXmlItemType.itxList)
            {
                message.ItemsByName["Body"] = m_RequestBody;
            }
            else
            {
                KXmlItem body_item = message.ItemsByName["Body"];
                body_item.ItemType = KXmlItemType.itxList;
                body_item.ItemNumber = 1;
                body_item.Items[0] = m_RequestBody;
            }

            bool result = false;
            Int64 start_time = GetSystemTickCount();
            if( m_Timeout > 0 )
            {
                RequestReplyData open_req_data = new RequestReplyData();
                open_req_data.MessageId     = m_RequestMessageId;
                open_req_data.Ticket        = m_Ticket;
                open_req_data.StartTime     = start_time;
                open_req_data.RecvReplyFlag = false;

                cv_Cs.Enter();
                try
                {
                    cv_OpenRequestMap[m_Ticket] = open_req_data;
                }
                catch(Exception e)
                {
                    try
                    {
                        cv_FileLog.WriteLog(e.Message, 1);
                    }
                    catch
                    {
                    }
                }
                cv_Cs.Leave();
            }

            cv_EventBus.SendMessage("EventChannel", m_RequestMessageId, message);

            if( m_Timeout > 0 )
            {
                SysUtils.Sleep(50);
                Int64 diff, current_time;
                KMmfEventClient.KMmfEventClientEventMessage reply_message = null;
                while (true)
                {
                    cv_Cs.Enter();
                    try
                    {
                        if( cv_OpenRequestMap[m_Ticket].RecvReplyFlag )
                        {
                            reply_message = cv_OpenRequestMap[m_Ticket].ReplyMessage;
                            if (reply_message != null)
                            {
                                result = true;
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        try
                        {
                            cv_FileLog.WriteLog(e.Message, 1);
                        }
                        catch
                        {
                        }
                    }
                    cv_Cs.Leave();

                    if( result )
                    {
                        break;
                    }
                    else
                    {
                        current_time = GetSystemTickCount();
                        diff = current_time - start_time;
                        if( diff < 0 )
                        {
                            start_time = current_time;
                        }
                        if( diff >= m_Timeout )
                        {
                            break;
                        }
                    }

                    SysUtils.Sleep(100);
                }

                cv_Cs.Enter();
                try
                {
                    if( cv_OpenRequestMap.ContainsKey(m_Ticket) )
                    {
                        cv_OpenRequestMap.Remove(m_Ticket);
                    }
                }
                catch(Exception e)
                {
                    try
                    {
                        cv_FileLog.WriteLog(e.Message, 1);
                    }
                    catch
                    {
                    }
                }
                cv_Cs.Leave();

                if( result )
                {
                    m_ReplyMessageId = reply_message.ReplyMessageId;
                    m_ReplyBody = reply_message.Body;
                }
            }

            cv_FileLog.WriteLog("Leave KMmfEventClient::SendRequestMessageTimeout", 4);

            return result;
        }

        //---------------------------------------------------------------------------
        public void SendReplyMessage(string m_MessageId, KXmlItem m_Body, uint m_Ticket, string m_RequestMessageId)
        {
            if (m_Body == null)
            {
                if (cv_NullBodyItem.ItemType != KXmlItemType.itxList || cv_NullBodyItem.ItemNumber != 0 || cv_NullBodyItem.Name != "Body")
                {
                    cv_NullBodyItem.Name = "Body";
                    cv_NullBodyItem.ItemType = KXmlItemType.itxList;
                    cv_NullBodyItem.ItemNumber = 0;
                }
                m_Body = cv_NullBodyItem;
            }

            cv_FileLog.WriteLog("Enter KMmfEventClient::SendReplyMessage", 4);
            cv_FileLog.WriteLog("MessageId: " + m_MessageId, 2);
            cv_FileLog.WriteLog(m_Body.XmlByStrings, 3);

            KXmlItem message = null;
            cv_Cs.Enter();
            try
            {
                message = cv_XmlTemplate.Clone();
            }
            catch(Exception e)
            {
                try
                {
                    cv_FileLog.WriteLog(e.Message, 1);
                }
                catch
                {
                }
            }
            cv_Cs.Leave();

            message.ItemsByName["SourceModule"].AsString = cv_MmfEventId;
            message.ItemsByName["Type"].AsString = "Reply";
            message.ItemsByName["RequestNotifyMessageId"].AsString = m_RequestMessageId;
            message.ItemsByName["Ticket"].AsString = m_Ticket.ToString();
            
            if( m_Body.Name == "Body" && m_Body.ItemType == KXmlItemType.itxList )
            {
                message.ItemsByName["Body"] = m_Body;
            }
            else
            {
                KXmlItem body_item = message.ItemsByName["Body"];
                body_item.ItemType = KXmlItemType.itxList;
                body_item.ItemNumber = 1;
                body_item.Items[0] = m_Body;
            }

            cv_EventBus.SendMessage("EventChannel", m_MessageId, message);

            cv_FileLog.WriteLog("Leave KMmfEventClient::SendReplyMessage", 4);
        }

        //---------------------------------------------------------------------------
        public void SendNotifyMessage(string m_MessageId, KXmlItem m_Body)
        {
            if (m_Body == null)
            {
                if (cv_NullBodyItem.ItemType != KXmlItemType.itxList || cv_NullBodyItem.ItemNumber != 0 || cv_NullBodyItem.Name != "Body")
                {
                    cv_NullBodyItem.Name = "Body";
                    cv_NullBodyItem.ItemType = KXmlItemType.itxList;
                    cv_NullBodyItem.ItemNumber = 0;
                }
                m_Body = cv_NullBodyItem;
            }

            cv_FileLog.WriteLog("Enter KMmfEventClient::SendNotifyMessage", 4);
            cv_FileLog.WriteLog("MessageId: " + m_MessageId, 2);
            cv_FileLog.WriteLog(m_Body.XmlByStrings, 3);

            uint ticket = 0;
            KXmlItem message = null;
            cv_Cs.Enter();
            try
            {
                message = cv_XmlTemplate.Clone();
                ticket = cv_Serial;
                ++cv_Serial;
            }
            catch(Exception e)
            {
                try
                {
                    cv_FileLog.WriteLog(e.Message, 1);
                }
                catch
                {
                }
            }
            cv_Cs.Leave();

            message.ItemsByName["SourceModule"].AsString = cv_MmfEventId;
            message.ItemsByName["Type"].AsString = "Notify";
            message.ItemsByName["RequestNotifyMessageId"].AsString = m_MessageId;
            message.ItemsByName["Ticket"].AsString = ticket.ToString();

            if( m_Body.Name == "Body" && m_Body.ItemType == KXmlItemType.itxList )
            {
                message.ItemsByName["Body"] = m_Body;
            }
            else
            {
                KXmlItem body_item = message.ItemsByName["Body"];
                body_item.ItemType = KXmlItemType.itxList;
                body_item.ItemNumber = 1;
                body_item.Items[0] = m_Body;
            }

            cv_EventBus.SendMessage("EventChannel", m_MessageId, message);

            cv_FileLog.WriteLog("Leave KMmfEventClient::SendNotifyMessage", 4);
        }

        //---------------------------------------------------------------------------
        private void OnMmfEventMessageReceive(string m_ChannelId, string m_MessageId, KXmlItem m_Data)
        {
            cv_FileLog.WriteLog("Enter KMmfEventClient::OnMmfEventMessageReceive", 4);
            cv_FileLog.WriteLog("MessageId: " + m_MessageId, 2);
            cv_FileLog.WriteLog(m_Data.XmlByStrings, 3);

            cv_LastRecvTime = GetSystemTickCount();
            if ( m_MessageId == "MmfEventDispatchServerConnectionStatusNotify" )
            {
                if( m_Data.IsItemExist("ConnectionStatus") )
                {
                    KXmlItem connection_list_item = m_Data.ItemsByName["ConnectionStatus"];
                    if( connection_list_item.ItemType == KXmlItemType.itxList && connection_list_item.ItemNumber > 0 )
                    {
                        string name;
                        bool status;
                        int i, count;
                        count = connection_list_item.ItemNumber;
                        for( i=0; i<count; ++i )
                        {
                            name = connection_list_item.Items[i].ItemsByName["Name"].AsString;
                            status = (SysUtils.StrToInt(connection_list_item.Items[i].ItemsByName["Status"].AsString) != 0);

                            cv_ConnectionStatus[name] = status;
                        }
                    }
                }
            }
            else
            {
                if (m_Data.IsItemExist("SourceModule") &&
                    m_Data.IsItemExist("Type") &&
                    m_Data.IsItemExist("RequestNotifyMessageId") &&
                    m_Data.IsItemExist("Ticket") &&
                    m_Data.IsItemExist("Body"))
                {
                    KMmfEventClientEventMessage recv_message = new KMmfEventClientEventMessage();
                    recv_message.SourceModule = SysUtils.Trim(m_Data.ItemsByName["SourceModule"].AsString);

                    string type_str = SysUtils.Trim(m_Data.ItemsByName["Type"].AsString);
                    if (type_str == "Request")
                    {
                        recv_message.Type = KMmfEventClientEventType.etRequest;
                    }
                    else if (type_str == "Reply")
                    {
                        recv_message.Type = KMmfEventClientEventType.etReply;
                    }
                    else
                    {
                        recv_message.Type = KMmfEventClientEventType.etNotify;
                    }

                    string req_notify_message_id = SysUtils.Trim(m_Data.ItemsByName["RequestNotifyMessageId"].AsString);

                    string ticket_str = SysUtils.Trim(m_Data.ItemsByName["Ticket"].AsString);
                    uint ticket = 0;
                    uint.TryParse(ticket_str, out ticket);

                    recv_message.RequestNotifyMessageId = req_notify_message_id;
                    recv_message.Ticket = ticket;
                    recv_message.Body = m_Data.ItemsByName["Body"];

                    if (recv_message.Type == KMmfEventClientEventType.etReply)
                    {
                        recv_message.ReplyMessageId = m_MessageId;
                        cv_Cs.Enter();
                        try
                        {
                            if (cv_OpenRequestMap.ContainsKey(ticket))
                            {
                                RequestReplyData req_reply_data = cv_OpenRequestMap[ticket];
                                if (req_reply_data.MessageId == req_notify_message_id)
                                {
                                    req_reply_data.ReplyMessage = recv_message;
                                    req_reply_data.RecvReplyFlag = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            try
                            {
                                cv_FileLog.WriteLog(e.Message, 1);
                            }
                            catch
                            {
                            }
                        }
                        cv_Cs.Leave();
                    }

                    if (OnMessageReceived != null)
                    {
                        try
                        {
                            OnMessageReceived(recv_message.SourceModule, (int)recv_message.Type, m_MessageId, recv_message.RequestNotifyMessageId, recv_message.Ticket, recv_message.Body);
                        }
                        catch (Exception e)
                        {
                            try
                            {
                                cv_FileLog.WriteLog(e.Message, 1);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            cv_FileLog.WriteLog("Leave KMmfEventClient::OnMmfEventMessageReceive", 4);
        }

    }
}
