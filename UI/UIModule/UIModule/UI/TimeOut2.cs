using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KgsCommon;

namespace UI
{
    public partial class UiTimeOut : Form
    {
        KIniFile cv_TimeFile = null;

        string cv_TimePath = System.IO.Directory.GetCurrentDirectory() + "\\..\\Config\\HKCTIMEOUT.ini";

        public UiTimeOut()
        {
            InitializeComponent();
            InitTimeFile();
            IniEachTimeValue();
            SetInterfaceAndIdelDelayToGB();
        }

        private void IniEachTimeValue()
        {
            cv_TbIDTime.Text = cv_TimeFile.ReadString("Delay", "Delay", "2");
            cv_TbIFT2.Text = cv_TimeFile.ReadString("Interface", "T2", "10");
            cv_TbIFT3.Text = cv_TimeFile.ReadString("Interface", "T3", "10");
            cv_TbIFT4.Text = cv_TimeFile.ReadString("Interface", "T4", "10");
            cv_TbIFT5.Text = cv_TimeFile.ReadString("Interface", "T2", "10");
            cv_TbIFTm.Text = cv_TimeFile.ReadString("Interface", "Tm", "10");

            float tn = 0, t1 = 0, t2 = 0, t3 = 0 , t4=0;
            tn = SysUtils.StrToInt(cv_TimeFile.ReadString("HSTimeout", "Tn", "0.5"));
            t1 = SysUtils.StrToInt(cv_TimeFile.ReadString("HSTimeout", "T1", "5"));
            t2 = SysUtils.StrToInt(cv_TimeFile.ReadString("HSTimeout", "T2", "5"));
            t3 = SysUtils.StrToInt(cv_TimeFile.ReadString("HSTimeout", "T3", "0.2"));
            t4 = SysUtils.StrToInt(cv_TimeFile.ReadString("HSTimeout", "T4", "2"));
            cv_TbHSTn.Text = (tn/1000).ToString();
            cv_TbHST1.Text = (t1/1000).ToString();
            cv_TbHST2.Text = (t2/1000).ToString();
            cv_TbHST3.Text = (t3/1000).ToString();
            cv_TbHST4.Text = (t4/1000).ToString();
        }

        private void InitTimeFile()
        {
            if (cv_TimeFile == null)
            {
                if (!string.IsNullOrEmpty(cv_TimePath))
                {
                    if (SysUtils.FileExists(cv_TimePath))
                    {
                        cv_TimeFile = new KIniFile(cv_TimePath);
                    }
                }
            }
            else
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetHandShakeTimeOut();
            SetInterfaceAndIdelDelayToGB();
            SaveFile();
            this.Hide();
            string user_log = "[ User push TimeOut Set button ] \n";
            user_log += "Delay : " + cv_TbIDTime.Text.Trim() + "\n";
            user_log += "Interface T2 : " + cv_TbIFT2.Text.Trim() + "\n";
            user_log += "Interface T3 : " + cv_TbIFT3.Text.Trim() + "\n";
            user_log += "Interface T4 : " + cv_TbIFT4.Text.Trim() + "\n";
            user_log += "Interface T5 : " + cv_TbIFT5.Text.Trim() + "\n";
            user_log += "Interface Tm : " + cv_TbIFTm.Text.Trim() + "\n";
            user_log += "HSTimeout Tn : " + cv_TbHSTn.Text.Trim() + "\n";
            user_log += "HSTimeout T1 : " + cv_TbHST1.Text.Trim() + "\n";
            user_log += "HSTimeout T2 : " + cv_TbIFT2.Text.Trim() + "\n";
            user_log += "HSTimeout T3 : " + cv_TbIFT3.Text.Trim() + "\n";
            user_log += "HSTimeout T4 : " + cv_TbIFT4.Text.Trim() + "\n";
            if (!string.IsNullOrEmpty(user_log))
            {
                /*
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
                */
            }
        }

        //this function must change file content 
        //and call change PLC time out setting function
        private void SetInterfaceAndIdelDelayToGB()
        {
            string stm = cv_TbIFTm.Text.Trim(),
                st2 = cv_TbIFT2.Text.Trim(),
                st3 = cv_TbIFT3.Text.Trim(),
                st4 = cv_TbIFT4.Text.Trim(),
                st5 = cv_TbIFT5.Text.Trim(),
                sidle = cv_TbIDTime.Text.Trim();
            float tm = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, idle = 0;

            if ((float.TryParse(stm, out tm)) && (float.TryParse(st2, out t2))
                && (float.TryParse(st3, out t3)) && (float.TryParse(st4, out t4))
                && (float.TryParse(st5, out t5)) && (float.TryParse(sidle, out idle)))
            {
                /*
                GB.SetGBTimeOut((int)(tm * 10), (int)(t2 * 10)
                        , (int)(t3 * 10), (int)(t4 * 10), (int)(t5 * 10), (int)(idle * 10));
                */
            }
        }

        private void SetHandShakeTimeOut()
        {
            string hs_t1 = cv_TbHST1.Text.Trim(),
                hs_t2 = cv_TbHST2.Text.Trim(),
                hs_t3 = cv_TbHST3.Text.Trim(),
                hs_tn = cv_TbHSTn.Text.Trim(),
                hs_t4 = cv_TbHST4.Text.Trim();

            float fhs_t1 = 0, fhs_t2 = 0, fhs_t3 = 0, fhs_tn = 0 ,  fhs_t4=0;

            if ((float.TryParse(hs_t1, out fhs_t1)) && (float.TryParse(hs_t2, out fhs_t2))
                && (float.TryParse(hs_t3, out fhs_t3)) && (float.TryParse(hs_tn, out fhs_tn)) && (float.TryParse(hs_t4 , out fhs_t4))
                )
            {
                /*
                CPC.cv_BCEq.ReSetHSTimeout((int)(fhs_t1*1000), (int)(fhs_t2*1000)
                    , (int)(fhs_t3*1000),(int)(fhs_t4*1000) , (int)(fhs_tn*1000));
                */
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void SaveFile()
        {
            cv_TimeFile.WriteString("Delay", "Delay", cv_TbIDTime.Text.Trim());
            cv_TimeFile.WriteString("Interface", "T2", cv_TbIFT2.Text.Trim());
            cv_TimeFile.WriteString("Interface", "T3", cv_TbIFT3.Text.Trim());
            cv_TimeFile.WriteString("Interface", "T4", cv_TbIFT4.Text.Trim());
            cv_TimeFile.WriteString("Interface", "T5", cv_TbIFT5.Text.Trim());
            cv_TimeFile.WriteString("Interface", "Tm", cv_TbIFTm.Text.Trim());

            cv_TimeFile.WriteString("HSTimeout", "Tn", (SysUtils.StrToDouble(cv_TbHSTn.Text.Trim()) * 1000).ToString());
            cv_TimeFile.WriteString("HSTimeout", "T1", (SysUtils.StrToDouble(cv_TbHST1.Text.Trim()) * 1000).ToString());
            cv_TimeFile.WriteString("HSTimeout", "T2", (SysUtils.StrToDouble(cv_TbHST2.Text.Trim()) * 1000).ToString());
            cv_TimeFile.WriteString("HSTimeout", "T3", (SysUtils.StrToDouble(cv_TbHST3.Text.Trim()) * 1000).ToString());
            cv_TimeFile.WriteString("HSTimeout", "T4", (SysUtils.StrToDouble(cv_TbHST4.Text.Trim()) * 1000).ToString());
        }

        private void cv_TimeoutDefault_Click(object sender, EventArgs e)
        {
            // tm : 0.2  , t1 : 4 , t2 : 2 , t3 , 0.5 , t4:0 
            cv_TbIDTime.Text = "10";
            cv_TbIFT2.Text = "30";
            cv_TbIFT3.Text = "2";
            cv_TbIFT4.Text = "60";
            cv_TbIFT5.Text = "2";
            cv_TbIFTm.Text = "0.2";

            cv_TbHSTn.Text = "0.2";
            cv_TbHST1.Text = "4";
            cv_TbHST2.Text = "2";
            cv_TbHST3.Text = "0.5";
            cv_TbHST4.Text = "2";

            string user_log = "[ User push TimeOut Default button ] \n";
            if (!string.IsNullOrEmpty(user_log))
            {
                /*
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
                */
            }
        }
    }
}
