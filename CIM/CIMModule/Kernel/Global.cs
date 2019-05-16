using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KgsCommon;
using System.Runtime.InteropServices;

namespace CIM
{
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }  

    class Global : GlobalBase
    {
        static public string ConfigPathname = string.Empty;
        static public CIMController Controller = null;

        static public KFileLog DebugLog;

        [DllImport("kernel32.dll")]
        public extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);
    }
}
