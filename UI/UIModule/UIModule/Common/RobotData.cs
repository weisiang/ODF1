using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RobotData
    {
        public Dictionary<int, Common.GlassData> cv_GlassDataList;

        public bool cv_UpSensor = false;
        public bool cv_DownSensor = false;

        public bool Connect
        {
            get;
            set;
        }
        public Common.EquipmentStatus Status
        {
            get;
            set;
        }
        public RobotData()
        {
            cv_GlassDataList = new Dictionary<int , GlassData>();
            cv_GlassDataList.Add((int)RobotArm.rbaUp, null);
            cv_GlassDataList.Add((int)RobotArm.rbaDown, null);
        }
        public void InitializeGlassList()
        {
            if (cv_GlassDataList != null)
            {
                cv_GlassDataList.Clear();
            }
            for (int i = 1; i <= 2; i++)
            {
                cv_GlassDataList.Add(i, null);
            }

        }
    }
}
