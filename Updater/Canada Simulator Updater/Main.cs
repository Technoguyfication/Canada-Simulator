using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canada_Simulator_Updater
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            VersionLabel.Text = args[1];
        }
        #region Buttons

            private void ExitButton_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void UpdateButton_Click(object sender, EventArgs e)
            {

            }

        #endregion

        #region Methods

            public void Update()
            {

            }
        #endregion
    }
}
