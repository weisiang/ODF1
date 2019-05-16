using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;

namespace Common
{
    public class BufferData
    {
        public Dictionary<int, Common.GlassData> cv_GlassDataList = new Dictionary<int, Common.GlassData>();
        int cv_Id;
        public int cv_MaxSlot;
        public BufferData(int m_id , int m_MaxSlot )
        {
            cv_Id = m_id;
            cv_MaxSlot = m_MaxSlot;
            for(int i = 1; i<= cv_MaxSlot; i++)
            {
                cv_GlassDataList.Add(i, null);
            }
        }
        public void InitializeGlassList()
        {
            if(cv_GlassDataList!=null)
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
