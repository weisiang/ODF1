using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData.HIRATA;
using UI.GUI;

namespace UI
{
    public partial class RobotJobPathForm : Form
    {
        RobotJobPathUI cv_RobotJobPathUI = null;
        public RobotJobPathForm(bool m_IsAutoJobForm=true)
        {
            InitializeComponent();
            cv_RobotJobPathUI = new RobotJobPathUI();
            cv_RobotJobPathUI.Dock = DockStyle.Fill;
            this.Controls.Add(cv_RobotJobPathUI);
            if(!m_IsAutoJobForm)
            {
                cv_RobotJobPathUI.SetButtonDisable();
            }
        }
        public void Refresh(List<RobotJob> m_DataQueue)
        {
            cv_RobotJobPathUI.Refresh(m_DataQueue);
        }

        private void RobotJobPathForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
