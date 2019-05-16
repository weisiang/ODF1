using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;

namespace Common
{
    public class GlassData
    {
        public string Id
        {
            get;
            set;
        }
        public int Slot
        {
            get;
            set;
        }
        public GlassData()
        {

        }
        public GlassData(KXmlItem m_Xml)
        {

        }
        public GlassData(Common.GlassDataItem m_GlassDataItem)
        {

        }
    }
}
