using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftSuperflatTool
{
    public partial class ChangeNum : Form
    {
        public string blockName = "";
        public int blockNum = 0;

        public void updateUI()
        {
            label3.Text = blockName;
            numericUpDown1.Value = blockNum;
        }

        public ChangeNum()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            blockNum = int.Parse(numericUpDown1.Value.ToString()) ;
            this.DialogResult = DialogResult.OK;
        }
    }
}
