using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EqData
    {
        public Dictionary<int, Common.GlassData> cv_GlassDataList = new Dictionary<int, Common.GlassData>();

        public int cv_MaxSlot = 0;
        public EqData( int m_MaxSlot)
        {
            cv_MaxSlot = m_MaxSlot;
            for(int i = 0; i< cv_MaxSlot; i++)
            {
                cv_GlassDataList.Add(i + 1, null);
            }
        }
        public void InitializeGlassList()
        {
            if (cv_GlassDataList != null)
            {
                cv_GlassDataList.Clear();
            }
            for (int i = 1; i <= cv_MaxSlot; i++)
            {
                cv_GlassDataList.Add(i, null);
            }

        }

    }
}
