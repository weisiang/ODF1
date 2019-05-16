using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LGC
{
    public partial class Wait : Form
    {
        public Wait()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LgcForm.GetVasStandby();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LgcForm.PutVasStandby(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LgcForm.PutVasStandby(true);
        }
    }
}
