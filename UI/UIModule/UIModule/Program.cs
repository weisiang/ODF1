using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;

namespace UI
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Mutex _mutex = new System.Threading.Mutex(false, SysUtils.ChangeFileExt(SysUtils.ExtractFileName(SysUtils.GetExeName()), ""));
            if (!_mutex.WaitOne(0, false))
            {
                MessageBox.Show("Program run already");
                Environment.Exit(-1);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UiForm(args));
        }
    }
}
