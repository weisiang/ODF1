using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using MxLib;
using CommonData.HIRATA;
using System.Reflection;
using System.Diagnostics;
using BaseAp;

namespace CIM
{
    public partial class CimForm : BaseForm
    {
        KDateTime cv_TimerTime = SysUtils.Now();
        private static bool cv_IsPlcConnect = false;
        private static bool cv_IsBcAlive = false;
        public static bool PlcConnect
        {
            get { return cv_IsPlcConnect; }
            set
            {
                if (cv_IsPlcConnect != value)
                {
                    cv_IsPlcConnect = value;
                    cv_MmfController.SendBcAliveAndPlcConnect(PlcConnect, BcAlive);
                }
            }
        }
        public static bool BcAlive
        {
            get { return cv_IsBcAlive; }
            set
            {
                if (cv_IsBcAlive != value)
                {
                    cv_IsBcAlive = value;
                    if(cv_MmfController != null)
                    cv_MmfController.SendBcAliveAndPlcConnect(PlcConnect, cv_IsBcAlive);
                }
            }
        }

        private MDFun cv_MdFun = null;

        internal static Dictionary<int, Eq> cv_EqContainer = new Dictionary<int, Eq>();
        internal static Dictionary<int, Port> cv_PortContainer = new Dictionary<int, Port>();
        internal static Dictionary<int, Robot> cv_RobotContainer = new Dictionary<int, Robot>();
        internal static Dictionary<int, Buffer> cv_BufferContainer = new Dictionary<int, Buffer>();

        internal static CIMController cv_MmfController = null;
        public CimForm(string[] args) : base(args , FdModule.CIM)
        {
            InitializeComponent();
            InitMdFun();
            cv_MmfController = new CIMController();
            PlcConnect = true;
            cv_MmfController.Open();
            CommonData.HIRATA.CommonStaticData.KillTerminal(args);
           // cv_MmfController.SendBcAliveAndPlcConnect(PlcConnect, BcAlive);
            WriteLog(LogLevelType.General, "[CIM module start]");
            AddSimCheckBox();
            string root_path = SysUtils.ExtractFileDir(Assembly.GetExecutingAssembly().Location);
            string lib_info = "[KgsCommonDotNetLib_x64.dll]\n";
            string kgs_lib_path = root_path + "\\KgsCommonDotNetLib_x64.dll";
            lib_info += "Path : " + root_path + "\\KgsCommonDotNetLib_x64.dll\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileVersion + "\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileDescription + "\n";
            lib_info += "\n[KgsCommonX64.dll]\n";
            kgs_lib_path = root_path + "\\KgsCommonX64.dll";
            lib_info += "Path : " + root_path + "\\KgsCommonX64.dll" + "\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileVersion + "\n";
            lib_info += FileVersionInfo.GetVersionInfo(kgs_lib_path).FileDescription + "\n";
            lib_info += "\nProgram Version : " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString() + "\n";
            lib_info += "Path : " + Assembly.GetExecutingAssembly().Location + "\n";
            WriteLog(LogLevelType.General, lib_info);
            cv_Timer.Start();
        }
        ~CimForm()
        {
            int a = 99;
        }
        private void InitMdFun()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_MdFun == null)
            {
                cv_MdFun = new MDFun();
            }
            try
            {
                cv_MdFun.Open();
            }
            catch(Exception e)
            {
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void UpdateSomeStatus()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);

            //gt 0 is connected. lt doesnot.
            try
            {
                if ((cv_MdFun.mdBdLedRead() & 0x0020) > 0)
                {
                    if (!PlcConnect)
                    {
                        PlcConnect = true;
                    }
                }
                else
                {
                    if (PlcConnect)
                    {
                        PlcConnect = false;
                    }
                }
            }
            catch(Exception e)
            {

            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void OnGlassCountDataChange()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            byte[] tmp = new byte[4<<1];
            int value = BaseForm.cv_GlassCountData.PProductCount;
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);

            value = BaseForm.cv_GlassCountData.PDummyCount;
            tmp[2] = Convert.ToByte(value & 0x00ff);
            tmp[3] = Convert.ToByte((value & 0xff00) >> 8);

            value = BaseForm.cv_GlassCountData.PHistoryCount;
            tmp[4] = Convert.ToByte(value & 0x000000ff);
            tmp[5] = Convert.ToByte((value & 0x0000ff00) >> 8);
            tmp[6] = Convert.ToByte((value & 0x00ff0000) >> 16);
            tmp[7] = Convert.ToByte((value & 0xff000000) >> 24);
            cv_Mio.SetBinaryLengthData(0x3448, tmp, 4, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void DerivedOnTimer()
        {
            /*
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            Int64 diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_TimerTime);
            if (diff > 1000)
            {
                UpdateSomeStatus();
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            */
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            this.Hide();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        List<CheckBox> CimSlot = new List<CheckBox>();
        private void AddSimCheckBox()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimSlot.Add(checkBox1);
            CimSlot.Add(checkBox2);
            CimSlot.Add(checkBox3);
            CimSlot.Add(checkBox4);
            CimSlot.Add(checkBox5);
            CimSlot.Add(checkBox6);
            CimSlot.Add(checkBox7);
            CimSlot.Add(checkBox8);
            CimSlot.Add(checkBox9);
            CimSlot.Add(checkBox10);
            CimSlot.Add(checkBox11);
            CimSlot.Add(checkBox12);
            CimSlot.Add(checkBox13);
            CimSlot.Add(checkBox14);
            CimSlot.Add(checkBox15);
            CimSlot.Add(checkBox16);
            CimSlot.Add(checkBox17);
            CimSlot.Add(checkBox18);
            CimSlot.Add(checkBox19);
            CimSlot.Add(checkBox20);
            CimSlot.Add(checkBox21);
            CimSlot.Add(checkBox22);
            CimSlot.Add(checkBox23);
            CimSlot.Add(checkBox24);
            CimSlot.Add(checkBox25);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            //one foup data has 0x651 words.
            // common block has 0xF words.
            //one slot has 64 words.
            int comm_start_add = 0x4E0 + ( Convert.ToInt32( cb_PortId.Text.Trim() )- 1) * 0x651;
            int slot_start = comm_start_add + 16;
            int count = 0;
            int port_id = Convert.ToInt32(cb_PortId.Text.Trim());
            for(int i=0 ; i<25 ; i++)
            {
                if (CimSlot[i].Checked)
                    count++;
            }
            cv_Mio.SetPortValue(comm_start_add, (2 << 12) + (port_id << 8)
                + count);
            cv_Mio.SetPortValue(comm_start_add+1 , Convert.ToInt32(cb_PortId.Text.Trim()) + 100);
            string foup_id = txt_foupId.Text.Trim();
            foup_id = SysUtils.GetFixedLengthString(foup_id, 10);
            cv_Mio.SetBinaryLengthData(comm_start_add + 2, SysUtils.StringToByteArray(foup_id), 10);

            for(int i=0 ; i<25 ;i++)
            {
                GlassData glass = new GlassData();
                if(!CimSlot[i].Checked)
                {
                    glass.Write(cv_Mio, slot_start + (64 * i));
                    continue;
                }
                glass.PCimMode = OnlineMode.Control;
                glass.PFoupSeq = (uint)port_id + 100;
                glass.PWorkSlot = (uint)i + 1;
                glass.PWorkOrderNo = (uint)port_id + 100;
                glass.PId = "P" + port_id.ToString() + "S" + (i+1).ToString();
                if(i %2 == 0)
                glass.PPID = "CIMON1";
                else
                glass.PPID = "CIMON2";
                glass.PWorkType = WorkType.Dummy;
                if (port_id == 3 || port_id == 4)
                    glass.PProductionCategory = ProductCategory.Glass;
                else if (port_id == 5 || port_id == 6)
                    glass.PProductionCategory = ProductCategory.Wafer;
                glass.PGlassJudge = GlassJudge.OK;
                glass.PProcessFlag = ProcessFlag.Need;
                glass.PPriority = (25 % 8) + 1;
                glass.POcrRead = OCRRead.Need;
                glass.PAssamblyFlag = AssambleNeed.Need;
                foreach(GlassDataNodeItem item in glass.cv_Nods)
                {
                    item.PRecipe = 77;
                }
                glass.Write(cv_Mio, slot_start + (64 * i));
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}
