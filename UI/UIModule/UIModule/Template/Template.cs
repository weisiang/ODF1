using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using KgsCommon;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace UI
{
    public partial class Form1 : Form
    {
        KLogPage cv_LogPage = null;
        KMemoryIOClient cv_MemoryIO = null;
        System.Windows.Forms.NotifyIcon notifyIcon;
        int _controlX;
        int _controlY;
        int _cursorX;
        int _cursorY;
        bool cv_resize;
        public static KMemoLog cv_UILog;
        private static KCriticalSection cv_Cs = new KCriticalSection();
        bool isdragdrop = false;
        Panel pan_Menual = null;
        Login cv_LoginForm = null;
        public static Dictionary<int, Panel> cv_EqPanels = new Dictionary<int, Panel>();
        public static Dictionary<int, Panel> cv_PortPanels = new Dictionary<int, Panel>();
        public static Dictionary<int, Panel> cv_RobotPanels = new Dictionary<int, Panel>();
        public static Dictionary<int, Panel> cv_BufferPanels = new Dictionary<int, Panel>();
        private Dictionary<string, Label> cv_ModuleLbl = new Dictionary<string, Label>();

        Panel cv_panLotSummary = null;
        static UIController cv_control = null;
        Dictionary<string, Common.AlarmItem> alarms = new Dictionary<string, Common.AlarmItem>();
        public static BindingList<Common.Account> cv_accounts = new BindingList<Common.Account>();
        public static Common.Account cv_curAccount = null;
        public static BindingList<Common.PortData> cv_lotSummary = new BindingList<Common.PortData>();
        public static List<ThreadObj> cv_threadPool = new List<ThreadObj>();
        KTimer cv_timer;
        bool cv_hadInitial = false;
        bool cv_ReInitial = false;
        public Form1()
        {
            InitializeComponent();
            setFormTitle();
            layoutInit();
            InitIcon();
            initMemoryIoUI();
            InitAllSubForm();
            cv_control = new UIController();
            initModuleListUI();
            ConnectEvent();
            initAccount();
            SetAccountDataGrid();
            reFreshAccountGrid();
            setVetsion();
            BindLotSummary();
            setManualPage();
            InitLogPage();
            initUILog();
            initTimer();
            InitIcon();
            WriteLog(Common.LogType.General , "[UI module start]");
        }

        #region set Manual page
        private void setManualPage()
        {
            var target_list = Enum.GetValues(typeof(Common.ActionTarget));
            string parten = "";
            List<string> parten_list = new List<string>();
            if (CommonStaticData.EqNumber == 0) parten_list.Add("Eq");
            if (CommonStaticData.PortNumber == 0) parten_list.Add("Port");
            if (CommonStaticData.BufferNumber == 0) parten_list.Add("Buffer");
            parten_list.Add("Robot");
            bool need_add_or_synbom = parten_list.Count > 1 ? true : false;
            for(int i=0; i< parten_list.Count; i++)
            {
                parten += parten_list[i];
                if(need_add_or_synbom && i != ( parten_list.Count - 1 ) )
                {
                    parten += "|";
                }
            }
            foreach(var item in target_list)
            {
                if(!Regex.Match( item.ToString() , @parten , RegexOptions.IgnoreCase).Success)
                {
                    cb_RobotActionTarget.Items.Add(item.ToString());
                }
            }
            target_list = Enum.GetValues(typeof(Common.RobotAction));
            foreach (var item in target_list)
            {
                if (!Regex.Match(item.ToString(), @"ini|comp|none", RegexOptions.IgnoreCase).Success)
                {
                    cb_RobotActionName.Items.Add(item.ToString());
                }
            }
            for(int i = 1; i <= CommonStaticData.RobotNumber; i++)
            {
                cb_RobotActionRobotId.Items.Add(i.ToString());
            }

            target_list = Enum.GetValues(typeof(Common.RobotArm));

            foreach(var item in target_list)
            {
                cb_RobotActionArm.Items.Add(item.ToString());
            }

            for(int i = 1; i <= CommonStaticData.PortNumber; i++)
            {
                cb_PortActionPortId.Items.Add(i.ToString());
            }
            target_list = Enum.GetValues(typeof(Common.PortAction));
            foreach (var item in target_list)
            {
                if (!Regex.Match(item.ToString(), @"ini|comp|none", RegexOptions.IgnoreCase).Success)
                {
                    cb_PortActionName.Items.Add(item.ToString());
                }
            }
        }
        private void cb_RobotActionTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmp = cb_RobotActionTarget.Text;
            cb_RobotActionTargetId.SelectedIndex = -1;
            cb_RobotActionTargetId.Items.Clear();
            if(Regex.Match(tmp , @"Eq" , RegexOptions.IgnoreCase).Success)
            {
                for(int i = 1; i<= CommonStaticData.EqNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(i.ToString());
                }
            }
            else if(Regex.Match(tmp , @"Port" , RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonStaticData.PortNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(i.ToString());
                }
            }
            else if(Regex.Match(tmp , @"Buffer" , RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonStaticData.BufferNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(i.ToString());
                }
            }
        }
        private void cb_RobotActionTargetId_SelectedValueChanged(object sender, EventArgs e)
        {
            string target = cb_RobotActionTarget.Text;
            string id = cb_RobotActionTargetId.Text;
            if (string.IsNullOrEmpty(id)) return;
            int id_int = Convert.ToInt16(id);
            cb_RobotActionTargetSlot.SelectedIndex = -1;
            cb_RobotActionTargetSlot.Items.Clear();
            int size = 0;
            if (Regex.Match(target, @"Eq", RegexOptions.IgnoreCase).Success)
            {
                if (Form1.GetEq(Convert.ToInt16(id_int)) != null)
                {
                    size = Form1.GetEq(id_int).cv_EqData.cv_MaxSlot;
                }
                for (int i = 1; i <= size; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(target, @"Port", RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonStaticData.CstSize; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(target, @"Buffer", RegexOptions.IgnoreCase).Success)
            {
                if (Form1.GetBuffer(Convert.ToInt16(id_int)) != null)
                {
                    size = Form1.GetBuffer(id_int).cv_BufferData.cv_MaxSlot;
                }
                for (int i = 1; i <= size; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
        }
        #endregion

        private void InitLogPage()
        {
            if(cv_LogPage == null)
            {
                cv_LogPage = new KLogPage();
                cv_LogPage.Dock = DockStyle.Fill;
                cv_tpLog.Controls.Add(cv_LogPage);
            }
        }
        private void initUILog()
        {
            if (cv_UILog == null)
            {
                string enviPath = Environment.GetEnvironmentVariable("LogPath") + "\\Logs\\UI";
                cv_UILog = new KMemoLog();
                cv_UILog.LoadFromIni(Global.LogIniPathname, "UI");
                cv_UILog.LogFileName = enviPath;
                cv_LogPage.AddMemoLog("UI" , ref cv_UILog);
                cv_UILog.SaveToIni(Global.LogIniPathname, "UI");
            }
            if(UI.UIController.cv_controllerLog != null)
            {
                cv_LogPage.AddMemoLog("Controller", ref UI.UIController.cv_controllerLog);
            }
        }
        private void initTimer()
        {
            if (cv_timer == null)
            {
                cv_timer = new KTimer();
                cv_timer.Interval = 500;
                cv_timer.Open();
                cv_timer.Enabled = true;
                cv_timer.ThreadEventEnabled = false;
                cv_timer.OnTimer += OnTimer;
            }
        }

        public static UI.Port GetPort(int i)
        {
            UI.Port rtn = null;
            if(cv_PortPanels.ContainsKey(i))
            {
                rtn = cv_PortPanels[i].Controls[0] as UI.Port;
            }
            return rtn;
        }
        public static UI.UCEqStatus GetEq(int i)
        {
            UI.UCEqStatus rtn = null;
            if (cv_EqPanels.ContainsKey(i))
            {
                rtn = cv_EqPanels[i].Controls[0] as UI.UCEqStatus;
            }
            return rtn;
        }
        public static UI.Robot GetRobot(int i)
        {
            UI.Robot rtn = null;
            if (cv_RobotPanels.ContainsKey(i))
            {
                rtn = cv_RobotPanels[i].Controls[0] as UI.Robot;
            }
            return rtn;
        }
        public static UI.Buffer GetBuffer(int i)
        {
            UI.Buffer rtn = null;
            if (cv_BufferPanels.ContainsKey(i))
            {
                rtn = cv_BufferPanels[i].Controls[0] as UI.Buffer;
            }
            return rtn;
        }


        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x10;

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void OnTimer()
        {
            WriteLog(Common.LogType.Timer, CommonStaticData.__FUN(), Common.FunInOut.Enter);

            updateTimelbl();

            #region initialize
            if (!cv_hadInitial)
            {
                //if (Global.Controller.GetMmfClientConnectionStatus)
                foreach(KeyValuePair<string , bool > pair in Global.Controller.GetMmfClientConnectionStatus())
                {
                    if(Regex.Match(pair.Key , @"Mainap" , RegexOptions.IgnoreCase).Success)
                    {
                        if(pair.Value)
                        {
                            UIController.g_Controller.SendProgramStart();
                            cv_hadInitial = true;
                        }
                    }
                }
            }
            #endregion

            #region check pop form thread
            if (cv_threadPool.Count > 0)
            {
                foreach (ThreadObj item in cv_threadPool)
                {
                    if ((UInt64)SysUtils.MilliSecondsBetween(item.startTime, SysUtils.Now()) > item.timeout)
                    {
                        IntPtr ptr = FindWindow(null, item.title);
                        if (ptr != IntPtr.Zero)
                        {
                            //找到則關閉MessageBox視窗
                            PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                        cv_threadPool.Remove(item);
                        break;
                    }
                }
            }
            #endregion

            CheckModuleAlive();

            if(cv_tcBar.SelectedTab == cv_tpIo)
            {
                UpdateIOPage();
            }

            WriteLog(Common.LogType.Timer, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void updateTimelbl()
        {
            WriteLog(Common.LogType.Timer, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string time = SysUtils.Now().DateTimeString();
            string[] tmp = time.Split(' ');
            lbl_date.Text = tmp[0];
            lbl_time.Text = tmp[1];
            WriteLog(Common.LogType.Timer, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void UpdateIOPage()
        {
            WriteLog(Common.LogType.Timer, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            for(int i = 0; i<cv_dataViewIo.RowCount ; i++)
            {
                if(cv_dataViewIo.Rows[i].Cells[0].Value != null)
                {
                    cv_dataViewIo.Rows[i].Cells[1].Value = cv_MemoryIO.GetPortValueByName(cv_dataViewIo.Rows[i].Cells[0].Value.ToString());
                }
                if(cv_dataViewIo.Rows[i].Cells[2].Value != null)
                {
                    cv_dataViewIo.Rows[i].Cells[3].Value = cv_MemoryIO.GetPortValueByName(cv_dataViewIo.Rows[i].Cells[2].Value.ToString());
                }
            }
            WriteLog(Common.LogType.Timer, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void CheckModuleAlive()
        {
            if (cv_control.GetMmfClientConnectionStatus().Count != 0)
            {
                foreach (KeyValuePair<string, bool> item_pair in cv_control.GetMmfClientConnectionStatus())
                {
                    if (cv_ModuleLbl.ContainsKey(item_pair.Key))
                    {
                        if (item_pair.Value)
                        {
                            if (cv_ModuleLbl[item_pair.Key].BackColor != Color.Lime)
                            {
                                WriteLog(Common.LogType.Timer, "Moduel : " + item_pair.Key + " Alive : true" );
                                cv_ModuleLbl[item_pair.Key].BackColor = Color.Lime;
                            }
                        }
                        else
                        {
                            if (cv_ModuleLbl[item_pair.Key].BackColor != Color.Gray)
                            {
                                WriteLog(Common.LogType.Timer, "Moduel : " + item_pair.Key + " Alive : false");
                                cv_ModuleLbl[item_pair.Key].BackColor = Color.Gray;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                WriteLog(Common.LogType.Timer, "MmfDispatch don't open!!! Set all module label Gray");
                foreach (Label item in cv_ModuleLbl.Values)
                {
                    item.BackColor = Color.Gray;
                }
            }
        }
        public static void WriteLog(Common.LogType m_Type, string m_str, Common.FunInOut m_FunInOut = Common.FunInOut.None)
        {
            string log = "";
            int level = (int)m_Type;
            if (m_Type == Common.LogType.NoneTimerInOut)
            {
                log = "[FUN_" + m_Type.ToString() + " ]" + m_str;
            }
            else if(m_Type == Common.LogType.Timer && m_FunInOut != Common.FunInOut.None)
            {
                log = "[Timer FUN_" + m_Type.ToString() + " ]" + m_str;
            }
            else
            {
                log = m_str;
            }

            cv_Cs.Enter();
            try
            {
                cv_UILog.WriteLog(log, level);
            }
            catch (Exception e)
            {
            }
            cv_Cs.Leave();
        }
        ~Form1()
        {
            cv_control = null;
        }
        private void setVetsion()
        {
            this.lbl_version.Text += " : " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
        }
        private void recvLogin()
        {
            if (cv_curAccount != null)
            {
                lbl_id.Text += " : " + cv_curAccount.Id;
                lbl_per.Text += " : " + cv_curAccount.Permission.ToString();
            }
        }
        private void BindLotSummary()
        {
            LotSummary tmp = cv_panLotSummary.Controls[0] as LotSummary;
            tmp.GridBinding(cv_lotSummary);
            tmp.reFresh();
        }

        #region Icon
        private void InitIcon()
        {
            if (this.notifyIcon == null)
            {
                this.notifyIcon = new NotifyIcon();
                string ico_path = SysUtils.ExtractFileDir(SysUtils.GetExeName()) + "\\..\\Resources\\kgsIcon.ico";
                this.notifyIcon.Icon = new Icon(ico_path);
                this.Text = "Hirata Controller";
                this.notifyIcon.MouseDoubleClick += new MouseEventHandler(IconDoubleClick);
            }
        }
        private void IconDoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Show();
        }
        #endregion

        #region left tool bar 
        private void cv_btnLogin_Click(object sender, EventArgs e)
        {
            if (cv_LoginForm != null && !cv_LoginForm.Visible)
            {
                cv_LoginForm.Show();
            }
        }
        private void cv_btnLogout_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "";
            if (cv_curAccount != null)
            {
                cv_curAccount = null;
                lbl_id.Text = "Id";
                lbl_per.Text = "Permission";
                log += "log out successful" + Environment.NewLine;
            }
            log += "not log in " + Environment.NewLine;
            WriteLog(Common.LogType.General,log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void oNLINEREMOTEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User press Change to Control mode" + Environment.NewLine;
            UIController.g_Controller.SendOnlineReqestReq(Common.OnlineMode.Offline , Common.OnlineMode.Control);
            WriteLog(Common.LogType.General, log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void oFFLINEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User press Change to Offline mode" + Environment.NewLine;
            UIController.g_Controller.SendOnlineReqestReq(Common.OnlineMode.Offline, Common.OnlineMode.Offline);
            WriteLog(Common.LogType.General, log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void oNLINELOCALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User press Change to Monitor mode" + Environment.NewLine;
            UIController.g_Controller.SendOnlineReqestReq(Common.OnlineMode.Offline, Common.OnlineMode.Monitor);
            WriteLog(Common.LogType.General, log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void ReInit_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User Initialize "  + Environment.NewLine;
            if(!cv_ReInitial)
            {
                cv_ReInitial = true;
                lbl_ReIni.Enabled = false;
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User press exit" + Environment.NewLine ;
            if (MessageBox.Show("Close the prograim ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
            {
                log += "User press OK!" + Environment.NewLine;
                this.cv_timer.Enabled = false;
                this.cv_timer.Close();
                UIController.g_Controller = null;
                Environment.Exit(0);
            }
            else
            {
                log += "User press Cancel!" + Environment.NewLine; 
            }
            WriteLog(Common.LogType.UI, log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void btn_ShowManualIn_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (pan_Menual != null) return;
            pan_Menual = new Panel();
            pan_Menual.Parent = cv_tpLayout;
            pan_Menual.Controls.Add(gpb_RobotAction);
            pan_Menual.Controls.Add(gpb_PortAction);
            pan_Menual.Controls.Add(gpb_AlignerAction);
            pan_Menual.AutoSize = true;
            pan_Menual.Dock = DockStyle.Right;
            cv_tpLayout.Controls.Add(pan_Menual);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void btn_ShowManualOut_Click(object sender, EventArgs e)
        {
            cv_tpManual.Controls.Add(gpb_RobotAction);
            cv_tpManual.Controls.Add(gpb_PortAction);
            cv_tpManual.Controls.Add(gpb_AlignerAction);
            pan_Menual = null;
        }

        #endregion

        #region panel dragdrop
        void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isdragdrop) return;
            Panel pan = (Panel)sender;
            //lbl_mouseY.Text = e.Location.Y.ToString();
            if (e.Button == MouseButtons.Left)
            {
                int tx = Cursor.Position.X;
                int ty = Cursor.Position.Y;
                int _limitX = cv_tpLayout.Left + cv_tpLayout.Width;
                int _limitY = cv_tpLayout.Top + cv_tpLayout.Height;
                int _left = _controlX + (tx - _cursorX);
                int _top = _controlY + (ty - _cursorY);
                if (_left < 0 || _left > _limitX)
                    pan.Left = 0;
                else if (_left > _limitX)
                    pan.Left = _limitX;
                else
                    pan.Left = _left;

                if (_top < 0)
                    pan.Top = 0;
                else if (_top > _limitY)
                    pan.Top = _limitY;
                else
                    pan.Top = _top;
            }
            if (e.Button == MouseButtons.Right )
            {
                if(cv_resize)
                {
                    if (e.X > 50 && e.Y > 50) //set limite
                    {
                        pan.Width = e.X;
                        pan.Height = e.Y;
                    }
                }
                else
                {
                    Point pt = pan.PointToClient(Control.MousePosition);
                    if (pt.X < pan.Width - 5 && pt.Y < pan.Height - 5)
                    {
                        pan.Cursor = Cursors.SizeNWSE;
                        cv_resize = true;
                    }
                    else
                    {
                        pan.Cursor = Cursors.Default;
                        cv_resize = false;
                    }

                }
            }
        }
        private void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isdragdrop) return;
            /*
            cv_mouseOffset = new Point(-e.X, -e.Y);
            */
            Panel tmp = (Panel)sender;
            _controlX = tmp.Left;
            _controlY = tmp.Top;
            _cursorX = Cursor.Position.X;
            _cursorY = Cursor.Position.Y;
        }
        private void setToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isdragdrop = true;
        }
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isdragdrop = false;
            KXmlItem tmp = new KXmlItem();
            tmp.LoadFromFile(CommonStaticData.cv_layoutPos);

            int tmp_pos = 0;
            foreach (KeyValuePair<int, Panel> pair in cv_EqPanels)
            {
                pair.Value.Left = Convert.ToInt32(tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["LEFT"].Trim());
                pair.Value.Top = Convert.ToInt32(tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["TOP"].Trim());

                UI.UCEqStatus tmp_eq = pair.Value.Controls[0] as UI.UCEqStatus;
                if (int.TryParse(tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                    pair.Value.Width = tmp_pos + 10;
                }
                if (int.TryParse(tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                    pair.Value.Height = tmp_pos + 10;
                }

            }
            foreach (KeyValuePair<int, Panel> pair in cv_PortPanels)
            {
                pair.Value.Left = Convert.ToInt32(tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["LEFT"].Trim());
                pair.Value.Top = Convert.ToInt32(tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["TOP"].Trim());

                UI.Port tmp_port = pair.Value.Controls[0] as UI.Port;

                if (int.TryParse(tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        pair.Value.Width = tmp_pos + 10;
                }
                if (int.TryParse(tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        pair.Value.Height = tmp_pos + 10;
                }

            }
            foreach (KeyValuePair<int, Panel> pair in cv_BufferPanels)
            {
                pair.Value.Left = Convert.ToInt32(tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["LEFT"].Trim());
                pair.Value.Top = Convert.ToInt32(tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["TOP"].Trim());

                if (int.TryParse(tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        pair.Value.Width = tmp_pos + 10;
                }
                if (int.TryParse(tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        pair.Value.Height = tmp_pos + 10;
                }
            }

            foreach (KeyValuePair<int, Panel> pair in cv_RobotPanels)
            {
                pair.Value.Left = Convert.ToInt32(tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["LEFT"].Trim());
                pair.Value.Top = Convert.ToInt32(tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["TOP"].Trim());

                UI.Robot tmp_robot = pair.Value.Controls[0] as UI.Robot;
                if (int.TryParse(tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        pair.Value.Width = tmp_pos + 10;
                }
                if (int.TryParse(tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        pair.Value.Height = tmp_pos + 10;
                }
            }

            cv_panLotSummary.Left = Convert.ToInt32(tmp.ItemsByName["LOT"].Attributes["LEFT"].Trim());
            cv_panLotSummary.Top = Convert.ToInt32(tmp.ItemsByName["LOT"].Attributes["TOP"].Trim());
            if (int.TryParse(tmp.ItemsByName["LOT"].Attributes["WIDTH"].Trim(), out tmp_pos))
            {
                if (tmp_pos != 0)
                    cv_panLotSummary.Width = tmp_pos+10;
            }
            if (int.TryParse(tmp.ItemsByName["LOT"].Attributes["HEIGHT"].Trim(), out tmp_pos))
            {
                if (tmp_pos != 0)
                    cv_panLotSummary.Height = tmp_pos+10;
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isdragdrop)
            {
                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(CommonStaticData.cv_layoutPos);
                isdragdrop = false;

                int eq_number = CommonStaticData.EqNumber;
                int port_number = CommonStaticData.PortNumber;
                foreach (KeyValuePair<int, Panel> pair in cv_EqPanels)
                {
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["TOP"] = pair.Value.Top.ToString();
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["LEFT"] = pair.Value.Left.ToString();

                    UCEqStatus tmp_eq = pair.Value.Controls[0] as UCEqStatus;
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["WIDTH"] = tmp_eq.Width.ToString();
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["HEIGHT"] = tmp_eq.Height.ToString();
                }
                foreach (KeyValuePair<int, Panel> pair in cv_PortPanels)
                {
                    tmp.ItemsByName["PORT" + (pair.Key).ToString()].Attributes["TOP"] = pair.Value.Top.ToString();
                    tmp.ItemsByName["PORT" + (pair.Key).ToString()].Attributes["LEFT"] = pair.Value.Left.ToString();

                    UI.Port tmp_port = pair.Value.Controls[0] as UI.Port;
                    tmp.ItemsByName["PORT" + (pair.Key).ToString()].Attributes["WIDTH"] = tmp_port.Width.ToString();
                    tmp.ItemsByName["PORT" + (pair.Key).ToString()].Attributes["HEIGHT"] = tmp_port.Height.ToString();
                }
                foreach (KeyValuePair<int, Panel> pair in cv_RobotPanels)
                {
                    tmp.ItemsByName["ROBOT" + (pair.Key).ToString()].Attributes["TOP"] = pair.Value.Top.ToString();
                    tmp.ItemsByName["ROBOT" + (pair.Key).ToString()].Attributes["LEFT"] = pair.Value.Left.ToString();

                    UI.Robot tmp_robot = pair.Value.Controls[0] as UI.Robot;
                    tmp.ItemsByName["ROBOT" + (pair.Key).ToString()].Attributes["WIDTH"] = tmp_robot.Width.ToString();
                    tmp.ItemsByName["ROBOT" + (pair.Key).ToString()].Attributes["HEIGHT"] = tmp_robot.Height.ToString();
                }
                foreach (KeyValuePair<int, Panel> pair in cv_BufferPanels)
                {
                    tmp.ItemsByName["BUFFER" + (pair.Key).ToString()].Attributes["TOP"] = pair.Value.Top.ToString();
                    tmp.ItemsByName["BUFFER" + (pair.Key).ToString()].Attributes["LEFT"] = pair.Value.Left.ToString();

                    UI.Buffer tmp_buffer = pair.Value.Controls[0] as UI.Buffer;
                    tmp.ItemsByName["BUFFER" + (pair.Key).ToString()].Attributes["WIDTH"] = tmp_buffer.Width.ToString();
                    tmp.ItemsByName["BUFFER" + (pair.Key).ToString()].Attributes["HEIGHT"] = tmp_buffer.Height.ToString();
                }

                tmp.ItemsByName["LOT"].Attributes["TOP"] = cv_panLotSummary.Top.ToString();
                tmp.ItemsByName["LOT"].Attributes["LEFT"] = cv_panLotSummary.Left.ToString();

                UI.LotSummary tmp_lot = cv_panLotSummary.Controls[0] as UI.LotSummary;
                tmp.ItemsByName["LOT"].Attributes["WIDTH"] = tmp_lot.Width.ToString();
                tmp.ItemsByName["LOT"].Attributes["HEIGHT"] = tmp_lot.Height.ToString();
                tmp.SaveToFile(CommonStaticData.cv_layoutPos);
            }
        }
        #endregion
        private void ConnectEvent()
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            cv_control.EventOnlineChangeReq += recvOnlineChangeReq;
            cv_control.EventHsmsConnect += recvHsmsConnect;
            cv_control.EventEquipmentStatusChange += recvEquipmentStatusChange;
            cv_control.EventAlarm += recvAlarm;
            cv_LoginForm.EventLogin += recvLogin;
            cv_control.EventPortStatusChange += recvPortStatusChange;
            cv_control.EventPortLoadUnloadChange += recvPortLoadUnloadChange;
            cv_control.EventPopMgvForm += recvPopMgvForm;
            cv_control.EventPopMonitorForm += recvPopMonitorForm;
            cv_control.EventPortDataFlowNoti += recvPortDataFlowNoti;
            cv_control.EventEqDataFlowNoti += recvEqDataFlowNoti;
            cv_control.EventBufferDataFlowNoti += recvBufferDataFlowNoti;
            cv_control.EventRobotDataFlowNoti += recvRobotDataFlowNoti;
            cv_control.EventReIni += recvReiniCom;
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void reFreshAccountGrid()
        {
            cv_dataViewAccount.DataSource = cv_accounts;
            cv_dataViewAccount.Refresh();
        }
        private void SetAccountDataGrid()
        {
            DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn();
            idCol.DataPropertyName = "Id";
            idCol.HeaderText = "Id";
            idCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            idCol.ReadOnly = true;
            cv_dataViewAccount.Columns.Add(idCol);

            DataGridViewTextBoxColumn permissionCol = new DataGridViewTextBoxColumn();
            permissionCol.DataPropertyName = "Permission";
            permissionCol.HeaderText = "Permission";
            permissionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            permissionCol.ReadOnly = true;
            cv_dataViewAccount.Columns.Add(permissionCol);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.notifyIcon.Visible = true;
        }
        private void InitAllSubForm()
        {
            if (cv_LoginForm == null)
            {
                cv_LoginForm = new Login();
            }
        }
        protected void layoutInit()
        {
            int eq_number = CommonStaticData.EqNumber;
            int port_number = CommonStaticData.PortNumber;
            int robot_number = CommonStaticData.RobotNumber;
            int buffer_number = CommonStaticData.BufferNumber;

            KXmlItem pos = new KXmlItem();
            pos.LoadFromFile(CommonStaticData.cv_layoutPos);

            for (int i = 0; i < eq_number; ++i)
            {
                int max_slot = Convert.ToInt16( CommonStaticData.EqXml.Items[i].Attributes["BufferSize"].Trim()); 
                Panel tmp = new Panel();
                tmp.Parent = cv_tpLayout;
                tmp.AutoSize = false;
                UCEqStatus eq_control = new UCEqStatus(i + 1, max_slot);

                eq_control.Dock = DockStyle.None;
                eq_control.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;

                int tmp_pos = 0;
                if (int.TryParse(pos.ItemsByName["EQ" + (i + 1).ToString()].Attributes["TOP"].Trim(), out tmp_pos))
                {
                    tmp.Top = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["EQ" + (i + 1).ToString()].Attributes["LEFT"].Trim(), out tmp_pos))
                {
                    tmp.Left = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["EQ" + (i + 1).ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                        eq_control.Width = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["EQ" + (i + 1).ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                        eq_control.Height = tmp_pos;
                }
                tmp.Height = eq_control.Size.Height + 10;
                tmp.Width = eq_control.Size.Width + 10;
                tmp.Controls.Add(eq_control);
                cv_EqPanels.Add(i + 1, tmp);
            }

            for (int i = 1; i <= port_number; ++i)
            {
                int max_slot = CommonStaticData.CstSize;
                Panel tmp = new Panel();
                tmp.Parent = cv_tpLayout;
                tmp.AutoSize = false;
                Port port_control = new Port(i , max_slot);
                port_control.Dock = DockStyle.None;
                port_control.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;

                int tmp_pos = 0;
                if (int.TryParse(pos.ItemsByName["PORT" + i.ToString()].Attributes["TOP"].Trim(), out tmp_pos))
                {
                    tmp.Top = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["PORT" + i.ToString()].Attributes["LEFT"].Trim(), out tmp_pos))
                {
                    tmp.Left = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["PORT" + i.ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                        port_control.Width = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["PORT" + i.ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                        port_control.Height = tmp_pos;
                }
                tmp.Height = port_control.Size.Height + 10;
                tmp.Width = port_control.Size.Width + 10;
                tmp.Controls.Add(port_control);
                cv_PortPanels.Add(i, tmp);
                cv_lotSummary.Add(port_control.cv_PortData);
            }

            for (int i = 0; i < buffer_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonStaticData.BufferXml.Items[i].Attributes["BufferSize"].Trim());
                Panel tmp = new Panel();
                tmp.Parent = cv_tpLayout;
                tmp.AutoSize = false;
                Buffer buffer_control = new Buffer(i+1, max_slot);
                buffer_control.Dock = DockStyle.None;
                buffer_control.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;

                int tmp_pos = 0;
                if (int.TryParse(pos.ItemsByName["BUFFER" + (i+1).ToString()].Attributes["TOP"].Trim(), out tmp_pos))
                {
                    tmp.Top = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["BUFFER" + (i+1).ToString()].Attributes["LEFT"].Trim(), out tmp_pos))
                {
                    tmp.Left = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["BUFFER" + (i+1).ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        buffer_control.Width = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["BUFFER" + (i+1).ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if (tmp_pos != 0)
                        buffer_control.Height = tmp_pos;
                }
                tmp.Height = buffer_control.Size.Height + 10;
                tmp.Width = buffer_control.Size.Width + 10;
                tmp.Controls.Add(buffer_control);
                cv_BufferPanels.Add(i+1, tmp);
            }

            for (int i = 1; i <= robot_number; ++i)
            {
                Panel tmp = new Panel();
                tmp.Parent = cv_tpLayout;
                tmp.AutoSize = false;
                Robot robot_control = new Robot(i , Robot.RobotGlassShape.rgsCircle , Robot.RobotGlassShape.rgsCircle);
                robot_control.Dock = DockStyle.None;
                robot_control.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
                int tmp_pos = 0;
                if (int.TryParse(pos.ItemsByName["ROBOT" + i.ToString()].Attributes["TOP"].Trim(), out tmp_pos))
                {
                    tmp.Top = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["ROBOT" + i.ToString()].Attributes["LEFT"].Trim(), out tmp_pos))
                {
                    tmp.Left = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["ROBOT" + i.ToString()].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                        robot_control.Width = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["ROBOT" + i.ToString()].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                        robot_control.Height = tmp_pos;
                }
                tmp.Height = robot_control.Size.Height + 10;
                tmp.Width = robot_control.Size.Width + 10;
                tmp.Controls.Add(robot_control);
                cv_RobotPanels.Add(i, tmp);
            }
            {
                int tmp_pos = 0;
                cv_panLotSummary = new Panel();
                cv_panLotSummary.Parent = cv_tpLayout;
                cv_panLotSummary.AutoSize = false;
                LotSummary tmp = new LotSummary();
                tmp.Dock = DockStyle.None;
                tmp.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
                if (int.TryParse(pos.ItemsByName["LOT"].Attributes["TOP"].Trim(), out tmp_pos))
                {
                    cv_panLotSummary.Top = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["LOT"].Attributes["LEFT"].Trim(), out tmp_pos))
                {
                    cv_panLotSummary.Left = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["LOT"].Attributes["WIDTH"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                    tmp.Width = tmp_pos;
                }
                if (int.TryParse(pos.ItemsByName["LOT"].Attributes["HEIGHT"].Trim(), out tmp_pos))
                {
                    if(tmp_pos != 0)
                    tmp.Height = tmp_pos;
                }
                cv_panLotSummary.Height = tmp.Size.Height + 10;
                cv_panLotSummary.Width = tmp.Size.Width + 10;
                cv_panLotSummary.Controls.Add(tmp);
            }

            ItemMove();
        }
        protected void initModuleListUI()
        {

            int module_number = 0;
            Dictionary<string, string> tmp_modules = new Dictionary<string, string>();
            if (File.Exists(Global.SystemIniPathname))
            {
                KIniFile tmp_sysfile = new KIniFile(Global.SystemIniPathname);
                tmp_sysfile.ReadSection("Module", tmp_modules);
                module_number = tmp_modules.Count;
            }

            int max_width = 0;
            foreach (KeyValuePair<string, string> pair in tmp_modules)
            {
                if (!Regex.Match(pair.Value, @"^0").Success)
                {
                    Label tmp = new Label();
                    tmp.BorderStyle = BorderStyle.FixedSingle;
                    tmp.Height = 20;
                    tmp.Width = 50;
                    tmp.BackColor = Color.Lime;
                    tmp.Text = pair.Key;
                    tmp.Dock = DockStyle.Left;
                    tmp.TextAlign = ContentAlignment.MiddleCenter;
                    pan_module.Controls.Add(tmp);
                    cv_ModuleLbl.Add(pair.Key, tmp);
                    if (tmp.Width > max_width) max_width = tmp.Width;
                }
            }
        }
        private void ItemMove()
        {
            foreach (Panel ctrl in cv_EqPanels.Values)
            {
                ctrl.MouseMove += ctrl_MouseMove;
                ctrl.MouseDown += ctrl_MouseDown;
                ctrl.AllowDrop = true;
            }
            foreach (Panel ctrl in cv_PortPanels.Values)
            {
                ctrl.MouseMove += ctrl_MouseMove;
                ctrl.MouseDown += ctrl_MouseDown;
                ctrl.AllowDrop = true;
            }
            foreach (Panel ctrl in cv_RobotPanels.Values)
            {
                ctrl.MouseMove += ctrl_MouseMove;
                ctrl.MouseDown += ctrl_MouseDown;
                ctrl.AllowDrop = true;
            }
            {
                cv_panLotSummary.MouseMove += ctrl_MouseMove;
                cv_panLotSummary.MouseDown += ctrl_MouseDown;
                cv_panLotSummary.AllowDrop = true;
            }
            foreach (Panel ctrl in cv_BufferPanels.Values)
            {
                ctrl.MouseMove += ctrl_MouseMove;
                ctrl.MouseDown += ctrl_MouseDown;
                ctrl.AllowDrop = true;
            }
        }
        void setFormTitle()
        {
            string log = "";
            this.Text = CommonStaticData.cv_toolId;
            this.Text += " Version : " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            log += "Version : " + this.Text + Environment.NewLine;
            WriteLog(Common.LogType.General, log);
        }
        private void initMemoryIoUI()
        {
            string log = "";
            if (File.Exists(CommonStaticData.cv_memoryIoFile))
            {
                if(cv_MemoryIO == null)
                {
                    cv_MemoryIO = new KMemoryIOClient();
                    cv_MemoryIO.ServerName = "KGSMEMORYIODEMO";
                    cv_MemoryIO.LoadXmlFile(CommonStaticData.cv_memoryIoFile);
                    cv_MemoryIO.Open();
                }
                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(CommonStaticData.cv_memoryIoFile);
                int xml_number = tmp.ItemsByName["Bits"].ItemNumber;
                int row = (int)Math.Ceiling(((float)xml_number / 2));
                cv_dataViewIo.Rows.Add(row);
                string tmp_name = "";
                for (int i = 0; i <= row; i++)
                {
                    tmp_name = tmp.ItemsByName["Bits"].Items[i].Attributes["Name"];
                    cv_dataViewIo.Rows[i].Cells[0].Value = tmp_name;
                    if (i != xml_number)
                        tmp_name = tmp.ItemsByName["Bits"].Items[i + 1].Attributes["Name"];
                    cv_dataViewIo.Rows[i].Cells[2].Value = tmp_name;
                }
            }
            else
            {
                log += "MemoryIO file not found " + Environment.NewLine;
            }
            WriteLog(Common.LogType.General, log);
        }
        private void initAccount()
        {
            string log = "";
            if (File.Exists(Global.SystemIniPathname))
            {
                KIniFile tmp = new KIniFile(Global.SystemIniPathname);
                Dictionary<string, string> accounts = new Dictionary<string, string>();
                tmp.ReadSection("Account", accounts);
                foreach (KeyValuePair<string, string> pair in accounts)
                {
                    string[] sp = pair.Value.Split(';');
                    Common.Account item = new Common.Account(pair.Key.Trim(), sp[0].Trim(), (Common.UserPermission)Convert.ToInt16(sp[1]));
                    cv_accounts.Add(item);
                }
            }
            else
            {
                log += "Account file not found " + Environment.NewLine;
            }
            WriteLog(Common.LogType.General, log);
        }
        private void cv_dataViewAccount_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if(e.RowIndex %2 == 0)
            {
                cv_dataViewAccount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                //cv_dataViewAccount.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.PaleTurquoise;
            }
        }

        #region process controller event ( update UI )

        #region recvEqDataFlowNoti
        private void recvEqDataFlowNoti(int m_EqId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processEqDataFlowNoti(m_EqId);
                }));
            }
            else
            {
                processEqDataFlowNoti(m_EqId);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processEqDataFlowNoti(int m_EqId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            UI.UCEqStatus Eq = Form1.GetEq(m_EqId);
            if(Eq != null)
            {
                Eq.Refresh();
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvRobotDataFlowNoti
        private void recvRobotDataFlowNoti(int m_RobotId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processRobotDataFlowNoti(m_RobotId);
                }));
            }
            else
            {
                processRobotDataFlowNoti(m_RobotId);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processRobotDataFlowNoti(int m_RobotId)
        {
            UI.Robot robot = Form1.GetRobot(m_RobotId);
            if(robot != null)
            {
                robot.Refresh();
            }
        }
        #endregion

        #region recvBufferDataFlowNoti
        private void recvBufferDataFlowNoti(int m_BufferId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processBufferDataFlowNoti(m_BufferId);
                }));
            }
            else
            {
                processBufferDataFlowNoti(m_BufferId);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processBufferDataFlowNoti(int m_BufferId)
        {
            UI.Buffer buffer = Form1.GetBuffer(m_BufferId);
            if(buffer != null)
            {
                buffer.refresh();
            }
        }
        #endregion

        #region recvPortDataFlowNoti
        private void recvPortDataFlowNoti(int m_PortId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processPortDataFlowNoti(m_PortId);
                }));
            }
            else
            {
                processPortDataFlowNoti(m_PortId);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processPortDataFlowNoti(int m_PortId)
        {
            UI.Port port = GetPort(m_PortId);
            if(port != null)
            {
                port.refresh();
            }
        }
        #endregion

        #region recvPortStatusChange
        private void recvPortStatusChange(int m_PortId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    LotSummary tmp = cv_panLotSummary.Controls[0] as LotSummary;
                    tmp.Refresh();
                }));
            }
            else
            {
                LotSummary tmp = cv_panLotSummary.Controls[0] as LotSummary;
                tmp.Refresh();
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvPortLoadUnloadChange
        private void recvPortLoadUnloadChange(int m_PortId , Common.PortStaus m_Status)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processPortLoadUnloadChange();
                }));
            }
            else
            {
                processPortLoadUnloadChange();
            }

            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processPortLoadUnloadChange()
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            reFreshLotStatusUI();
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvOnlineChangeReq
        private void recvOnlineChangeReq()
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    ProcessOnlineChangeReq();
                }));
            }
            else
            {
                ProcessOnlineChangeReq();
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void ProcessOnlineChangeReq()
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            cv_btnSelectMode.Enabled = false;
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }

        private void reFreshLotStatusUI()
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            LotSummary tmp = cv_panLotSummary.Controls[0] as LotSummary;
            tmp.reFresh();
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvHsmsConnect
        private void recvHsmsConnect(bool m_isConnected, Common.OnlineMode m_onlineMode)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    ProcessHsmsConnect(m_isConnected, m_onlineMode);
                }));
            }
            else
            {
                ProcessHsmsConnect(m_isConnected, m_onlineMode);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void ProcessHsmsConnect(bool m_isConnected, Common.OnlineMode m_onlineMode)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string tmp_txt = "";
            Color tmp_color = Color.Gray;
            if (m_isConnected)
            {
                tmp_color = Color.Lime;
                tmp_txt = "Connected";
            }
            else
            {
                tmp_color = Color.Gray;
                tmp_txt = "Disconnected";
            }
            lbl_hsmsStatus.BackColor = tmp_color;
            lbl_hsmsStatus.Text = tmp_txt;
            lbl_systemStatus.Text = m_onlineMode.ToString();
            switch(m_onlineMode)
            {
                case Common.OnlineMode.Control:
                    lbl_systemStatus.BackColor = Color.Lime;
                    break;
                case Common.OnlineMode.Monitor:
                    lbl_systemStatus.BackColor = Color.BlueViolet;
                    break;
                case Common.OnlineMode.Local:
                    lbl_systemStatus.BackColor = Color.Yellow;
                    break;
                case Common.OnlineMode.Offline:
                    lbl_systemStatus.BackColor = Color.LightGray;
                    break;
                default:
                    lbl_systemStatus.BackColor = Color.LightGray;
                    break;
            };
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvPopMgvForm
        private void recvPopMgvForm(int m_PortId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processPopMgvForm(m_PortId);
                }));
            }
            else
            {
                processPopMgvForm(m_PortId);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processPopMgvForm(int m_PortId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            //cv_lotSummary.
            foreach(Common.PortData port in cv_lotSummary)
            {
                if(port.Id == m_PortId)
                {
                    port.ShowMgvForm();
                }
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvPopMonitorForm
        private void recvPopMonitorForm(int m_PortId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processPopMonitorForm(m_PortId);
                }));
            }
            else
            {
                processPopMonitorForm(m_PortId);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processPopMonitorForm(int m_PortId)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            //cv_lotSummary.
            foreach(Common.PortData port in cv_lotSummary)
            {
                if(port.Id == m_PortId)
                {
                    port.ShowMonitorForm();
                }
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recvRobotAction
        private void recvRobotAction(int RobotId, Common.RobotArm m_Arm, Common.RobotAction m_Action, Common.ActionTarget m_Target, int m_Id, int m_Slot)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    processRobotAction(RobotId, m_Arm, m_Action, m_Target, m_Id, m_Slot);
                }));
            }
            else
            {
                processRobotAction(RobotId, m_Arm, m_Action, m_Target, m_Id, m_Slot);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void processRobotAction(int RobotId, Common.RobotArm m_Arm, Common.RobotAction m_Action, Common.ActionTarget m_Target, int m_Id, int m_Slot)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if(m_Action == Common.RobotAction.ActionComplete)
            {
                lbl_ActionArm.Text = "Arm : ";
                lbl_ActionId.Text = "Id : ";
                lbl_ActionName.Text = "Action : ";
                lbl_ActionSlot.Text = "Slot : ";
                lbl_ActionTarget.Text = "Target : ";
            }
            else
            {
                lbl_ActionArm.Text = "Arm : " + m_Arm.ToString();
                lbl_ActionId.Text = "Id : " + RobotId.ToString(); ;
                lbl_ActionName.Text = "Action : " + m_Action.ToString();
                lbl_ActionSlot.Text = "Slot : " + m_Slot.ToString() ;
                lbl_ActionTarget.Text = "Target : " + m_Target.ToString();
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region recv ReIniCom
        public void recvReiniCom()
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            if (!cv_ReInitial)
            {
                cv_ReInitial = true;
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    lbl_ReIni.Enabled = true;
                }));
            }
            else
            {
                lbl_ReIni.Enabled = true;
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion


        private void recvEquipmentStatusChange(int m_unit, bool m_isConnect, Common.EquipmentInlineMode m_inlineMode, Common.EquipmentStatus m_mainStatus, List<Common.EquipmentSubStatus> m_subStatusList)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            UCEqStatus tmp = null;
            if (cv_EqPanels.ContainsKey(m_unit))
            {
                tmp = cv_EqPanels[m_unit].Controls[0] as UCEqStatus;
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    tmp.updateInlineMode(m_inlineMode);
                    tmp.updateMainStatus(m_mainStatus);
                    tmp.updateSubStatus(m_subStatusList);
                }));
            }
            else
            {
                tmp.updateInlineMode(m_inlineMode);
                tmp.updateMainStatus(m_mainStatus);
                tmp.updateSubStatus(m_subStatusList);
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void recvAlarm(Common.MDAlarmReport m_AlarmObj)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            bool is_alarm = false;
            bool is_warning = false;
            Dictionary<string, Common.AlarmItem> tmp_alarms = new Dictionary<string, Common.AlarmItem>();
            if (m_AlarmObj != null)
            {
                for (int i = 0; i < m_AlarmObj.AlarmList.Count ; i++)
                {
                    Common.AlarmItem alarm_item = m_AlarmObj.AlarmList[i];

                    if (!tmp_alarms.ContainsKey(alarm_item.Code.Trim()))
                    {
                        tmp_alarms.Add(alarm_item.Code, alarm_item);

                        if (alarm_item.PLevel == Common.AlarmLevele.Light) 
                        {
                            if (!is_warning) is_warning = true;
                        }
                        else if (alarm_item.PLevel == Common.AlarmLevele.Serious)
                        {
                            if (!is_alarm) is_alarm = true;
                        }
                    }
                }
            }
            //tmp_alarms.Add("1", new Common.Alarm("123", "0001", Common.Alarm.AlarmLevele.Light, "1", "test"));
            alarms = tmp_alarms;
            BindingList<Common.AlarmItem> tmp = new BindingList<Common.AlarmItem>(alarms.Values.ToList());
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    cv_AlarmDataView.DataSource = tmp;
                    cv_AlarmDataView.Refresh();
                    if (is_alarm) lbl_alarmStatus.BackColor = Color.Red;
                    else
                        lbl_alarmStatus.BackColor = SystemColors.ActiveCaption; 
                    if (is_warning) lbl_warningStatus.BackColor = Color.Red;
                    else
                        lbl_warningStatus.BackColor = SystemColors.ActiveCaption;
                }));
            }
            else
            {
                cv_AlarmDataView.DataSource = tmp;
                cv_AlarmDataView.Refresh();
                if (is_alarm) lbl_alarmStatus.BackColor = Color.Red;
                else
                    lbl_alarmStatus.BackColor = SystemColors.ActiveCaption;
                if (is_warning) lbl_warningStatus.BackColor = Color.Red;
                else
                    lbl_warningStatus.BackColor = SystemColors.ActiveCaption;
            }
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region MenuPage
        private void bt_RobotExe_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User press Robot Action EXE" + Environment.NewLine;
            int robot_id = cb_RobotActionRobotId.SelectedIndex + 1;
            int tar = cb_RobotActionTarget.SelectedIndex +1 ;
            int id = cb_RobotActionTargetId.SelectedIndex + 1;
            int slot = cb_RobotActionTargetSlot.SelectedIndex + 1;
            int arm = cb_RobotActionArm.SelectedIndex + 1;
            int action = cb_RobotActionName.SelectedIndex + 1;

            Common.ActionTarget arg_tar = (Common.ActionTarget)tar;
            Common.RobotArm arg_arm = (Common.RobotArm)arm;
            Common.RobotAction arg_action = (Common.RobotAction)action;

            log +="Action : " + arg_action.ToString() + "Robot id : " + robot_id + " Arm : " + arg_arm.ToString()   
                + " Target : " + arg_tar.ToString() + " Target Id : " + id + " Slot : " + slot + Environment.NewLine;

            UIController.g_Controller.SendRobotActionReq(robot_id, arg_action, arg_arm, arg_tar, id, slot);
            WriteLog(Common.LogType.UI, log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void bt_PortExe_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            string log = "User press Port Action EXE" + Environment.NewLine;
            int port_id = cb_PortActionPortId.SelectedIndex + 1;
            int action = cb_PortActionName.SelectedIndex + 1;
            Common.PortAction arg_action = (Common.PortAction)action;
            log += " Port : " + port_id + " Action : " + arg_action.ToString() + Environment.NewLine;
            UIController.g_Controller.SendPortAction(port_id, arg_action);
            WriteLog(Common.LogType.UI, log);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion

        #region AlarmPage
        private void btn_buzzerOff_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            Global.Controller.SendBuzzeOff();
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        private void btn_resetAllAlarm_Click(object sender, EventArgs e)
        {
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Enter);
            Global.Controller.SendAlarmReset(cv_AlarmDataView.DataSource as BindingList<Common.AlarmItem>);
            WriteLog(Common.LogType.NoneTimerInOut, CommonStaticData.__FUN(), Common.FunInOut.Leave);
        }
        #endregion
    }
}
