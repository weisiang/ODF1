using System;
using System.Collections.Generic;
using System.Text;

namespace CommonData
{

    public class Cassette
    {
        public int PortNo;
        public int SlotCount;
        public string CassetteId;
        public string LotId;
        public string PCode;
        public string RCode;
        public string OperationId;
        public string PPID;
        public CstProcessStatus CstStatus;
        public LotProcessStatus LotStatus;

        public Dictionary<int, GlassData> SlotData;

        UInt16 cv_SlotCount = 0;
        public Cassette(int)
        {
            SlotCount = cv_SlotCount;

            Initial();

        }


        public Cassette(int m_SlotCount)
        {
            SlotCount = m_SlotCount;

            Initial();

        }

        public void Initial()
        {
            CassetteId = "";
            LotId = "";
            PCode = "";
            RCode = "";
            OperationId = "";
            PPID = "";
            CstStatus = CstProcessStatus.None;
            LotStatus = LotProcessStatus.None;

            SlotData = new Dictionary<int, GlassData>();

            for (int i = 1; i <= SlotCount; i++)
            {
                SlotData.Add(i, new GlassData());
            }
        }
    }
}
