using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace MinecraftSuperflatTool
{
    public partial class Form1 : Form
    {
        string res = "";

        List<BlockItem> items = new List<BlockItem>();

        string swqxf = "_swqx.txt";
        string blockf = "_block.txt";

        Dictionary<string, string> swqx = new Dictionary<string, string>();
        Dictionary<string, string> block = new Dictionary<string, string>();

        List<int> queryResult = new List<int>();
        int nowIndex = -1;

        public Form1()
        {
            InitializeComponent();
        }

        void init()
        {
            // load resuorces
            try
            {
                var lines = File.ReadAllLines(swqxf, Encoding.UTF8);
                foreach(var line in lines)
                {
                    var items = line.Trim().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length >= 2)
                    {
                        swqx[items[0]] = items[1];
                    }
                }

                lines = File.ReadAllLines(blockf, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var items = line.Trim().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length >= 2)
                    {
                        block[items[0]] = items[1];
                    }
                }

                foreach(var qx in swqx)
                {
                    cb_swqx.Items.Add(qx.Key);

                }
                listBoxBlocksExist.Items.Clear();
                foreach(var b in block)
                {
                    listBoxBlocks.Items.Add(b.Key);
                   
                }
                cb_swqx.SelectedIndex = 1;
                gb_cz.Visible = false;
                gb_hdyj.Visible = false;
                gb_kk.Visible = false;
                gb_qxjg.Visible = false;
                gb_ys.Visible = false;
            }
            catch
            {
                
            }

        }

        void query(string str)
        {
            try
            {
                nowIndex = -1;
                queryResult.Clear();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    for(int i = 0; i < listBoxBlocks.Items.Count; i++)
                    {
                        if (listBoxBlocks.Items[i].ToString().Contains(str))
                        {
                            queryResult.Add(i);
                        }
                    }
                }
            }
            catch { }

        }

        int nextQueryResult()
        {
            if (queryResult.Count > 0)
            {
                nowIndex = (nowIndex + 1) % queryResult.Count;
            }
            else
            {
                nowIndex = -1;
            }
            if (nowIndex < 0) return -1;
            else return queryResult[nowIndex];
        }

        int lastQueryResult()
        {
            if (queryResult.Count > 0)
            {
                nowIndex = (queryResult.Count + nowIndex - 1) % queryResult.Count;
            }
            else
            {
                nowIndex = -1;
            }
            if (nowIndex < 0) return -1;
            else return queryResult[nowIndex];
        }

        void generateResult()
        {
            try
            {
                res = "";
                for(int i = listBoxBlocksExist.Items.Count - 1; i >= 0; i--)
                {
                    var items = listBoxBlocksExist.Items[i].ToString().Trim().Split('*');
                    if (items.Length >= 2)
                    {
                        string id = block[items[0].Trim()];
                        int num = int.Parse(items[1]);
                        res += $"{num}*minecraft:{id},";
                    }
                }
                if (res.EndsWith(",")) res = res.Substring(0, res.Length - 1);
                if (string.IsNullOrWhiteSpace(res))
                {
                    // no block

                }

                res += $";minecraft:{swqx[cb_swqx.Text.Trim()]};";
                if (box_cz.Checked)
                {
                    res += $"village(size={nb_cz_dx.Value} distance={nb_cz_jj.Value}),";
                }
                if (box_kk.Checked)
                {
                    res += $"mineshaft(chance={nb_kk_md.Value}),";
                }
                if (box_ys.Checked)
                {
                    res += $"stronghold(distance={nb_ys_jl.Value} count={nb_ys_sl.Value} spread={nb_ys_md.Value}),";
                }
                if (box_qxjg.Checked)
                {
                    res += $"biome_1(distance={nb_qxjg_jl.Value}),";
                }
                if (box_dl.Checked)
                {
                    res += $"dungeon,";
                }
                if (box_zs.Checked)
                {
                    res += $"decoration,";
                }
                if (box_hp.Checked)
                {
                    res += $"lake,";
                }
                if (box_yjc.Checked)
                {
                    res += $"lava_lake,";
                }
                if (box_hdyj.Checked)
                {
                    res += $"oceanmonument(spacing={nb_hdyj_dx.Value} separation={nb_hdyj_jj.Value}),";
                }
                if (res.EndsWith(",") || res.EndsWith(";")) res = res.Substring(0, res.Length - 1);
                textBoxResult.Text = res;
            }
            catch
            {

            }
        }










        private void Form1_Shown(object sender, EventArgs e)
        {
            init();
        }

        private void buttonAddBlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxBlocks.SelectedItems.Count > 0)
                {
                    string res = $"{listBoxBlocks.SelectedItem} * {nb_blockNum.Value}";
                    int index = 0;
                    if (listBoxBlocksExist.SelectedIndex > 0) index = listBoxBlocksExist.SelectedIndex;
                    listBoxBlocksExist.Items.Insert(index, res);
                    if (listBoxBlocksExist.Items.Count == 1) listBoxBlocksExist.SelectedIndex = 0;
                    generateResult();
                }
            }
            catch { }

        }

        private void buttonDeleteBlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxBlocksExist.SelectedItems.Count > 0)
                {
                    int index = listBoxBlocksExist.SelectedIndex;
                    listBoxBlocksExist.Items.RemoveAt(index);
                    listBoxBlocksExist.SelectedIndex = index-1;
                    generateResult();
                }
            }
            catch
            {

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBoxBlocks.SelectedIndex = lastQueryResult();
            listBoxBlocks.TopIndex = listBoxBlocks.SelectedIndex;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBoxBlocks.SelectedIndex = nextQueryResult();
            listBoxBlocks.TopIndex = listBoxBlocks.SelectedIndex;

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBoxBlocksExist.Items.Clear();
            generateResult();
        }

        private void buttonCopyResult_Click(object sender, EventArgs e)
        {
            textBoxResult.SelectAll();
            Clipboard.SetDataObject(textBoxResult.Text.Trim());
        }



        private void textBoxQuery_TextChanged(object sender, EventArgs e)
        {
            string res = textBoxQuery.Text;
            query(res.Trim());
            listBoxBlocks.SelectedIndex = nextQueryResult();
            listBoxBlocks.TopIndex = listBoxBlocks.SelectedIndex;
        }

        private void box_cz_CheckedChanged(object sender, EventArgs e)
        {
            gb_cz.Visible = box_cz.Checked;
            generateResult();
        }

        private void box_kk_CheckedChanged(object sender, EventArgs e)
        {
            gb_kk.Visible = box_kk.Checked;
            generateResult();
        }

        private void box_ys_CheckedChanged(object sender, EventArgs e)
        {
            gb_ys.Visible = box_ys.Checked;
            generateResult();
        }

        private void box_qxjg_CheckedChanged(object sender, EventArgs e)
        {
            gb_qxjg.Visible = box_qxjg.Checked;
            generateResult();
        }

        private void box_hdyj_CheckedChanged(object sender, EventArgs e)
        {
            gb_hdyj.Visible = box_hdyj.Checked;
            generateResult();
        }

        private void nb_cz_dx_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_cz_jj_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_kk_md_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_ys_jl_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_ys_sl_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_ys_md_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_qxjg_jl_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_hdyj_dx_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void nb_hdyj_jj_ValueChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void cb_swqx_SelectedIndexChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void box_dl_CheckedChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void box_zs_CheckedChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void box_hp_CheckedChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void box_yjc_CheckedChanged(object sender, EventArgs e)
        {
            generateResult();
        }

        private void listBoxBlocksExist_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listBoxBlocksExist.SelectedItems.Count > 0)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }

        private void 上移至顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxBlocksExist.SelectedIndex;
            if (index > 0)
            {
                string str = listBoxBlocksExist.Items[index].ToString();
                listBoxBlocksExist.Items.RemoveAt(index);
                listBoxBlocksExist.Items.Insert(0, str);
                listBoxBlocksExist.SelectedIndex =0;
            }
            generateResult();
        }

        private void 上移一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxBlocksExist.SelectedIndex;
            if (index > 0)
            {
                string str = listBoxBlocksExist.Items[index].ToString();
                listBoxBlocksExist.Items.RemoveAt(index);
                listBoxBlocksExist.Items.Insert(index - 1, str);
                listBoxBlocksExist.SelectedIndex = index - 1;
            }
            generateResult();
        }

        private void 下移一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxBlocksExist.SelectedIndex;
            if (index < listBoxBlocksExist.Items.Count - 1)
            {
                string str = listBoxBlocksExist.Items[index].ToString();
                listBoxBlocksExist.Items.RemoveAt(index);
                listBoxBlocksExist.Items.Insert(index + 1, str);
                listBoxBlocksExist.SelectedIndex = index + 1;
            }
            generateResult();
        }

        private void 下移至底ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxBlocksExist.SelectedIndex;
            if (index < listBoxBlocksExist.Items.Count - 1)
            {
                string str = listBoxBlocksExist.Items[index].ToString();
                listBoxBlocksExist.Items.RemoveAt(index);
                listBoxBlocksExist.Items.Insert(listBoxBlocksExist.Items.Count, str);
                listBoxBlocksExist.SelectedIndex = listBoxBlocksExist.Items.Count - 1;
            }
            generateResult();
        }

        private void 修改数量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeNum cnb = new ChangeNum();

                var items = listBoxBlocksExist.SelectedItem.ToString().Split('*');
                cnb.blockName = items[0].Trim();
                cnb.blockNum = int.Parse(items[1]);
                cnb.updateUI();
                //Visible = false;
                if (cnb.ShowDialog() == DialogResult.OK)
                {
                    listBoxBlocksExist.Items[listBoxBlocksExist.SelectedIndex] = $"{cnb.blockName} * {cnb.blockNum}";
                }
                cnb.Dispose();
                //Visible = true;
                generateResult();
            }
            catch
            {

            }
           
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBoxBlocksExist.SelectedIndex;
                if (index >= 0)
                {
                    string str = listBoxBlocksExist.Items[index].ToString();
                    listBoxBlocksExist.Items.Insert(index + 1, str);
                    listBoxBlocksExist.SelectedIndex = index + 1;
                }
                generateResult();
            }
            catch { }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBoxBlocksExist.SelectedIndex;
                if (index >= 0)
                {
                    string str = listBoxBlocksExist.Items[index].ToString();
                    listBoxBlocksExist.Items.RemoveAt(index);
                    listBoxBlocksExist.SelectedIndex = index - 1;
                }
                generateResult();
            }
            catch { }
        }
    }

    class BlockItem
    {
        public string blockid = "";
        public int num = 0;
    }
}
