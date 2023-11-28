using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChromeDroid_TabMan.ConnectionAndImport;
using ChromeDroid_TabMan.Data;
using ChromeDroid_TabMan.Models;
using Microsoft.EntityFrameworkCore;
using ChromeDroid_TabMan.Auxiliary;

namespace ChromeDroid_TabMan
{
    public partial class MainForm : Form
    {
        string manuallySetJsonLocation = string.Empty;
        DataTable dt = new DataTable();
        public MainForm()
        {
            dt.Columns.Add("Tab Position",typeof(int));
            dt.Columns.Add("Title");
            dt.Columns.Add("URL");
            dt.Columns.Add("Base URL");
            InitializeComponent();

            var tabsList = TabsList.GetInstance();
            //FillMyTreeView(tabsList);
            //tabsList.ExportToHTML();
            //tabsList.ExportToNetscapeBookmarksHTML();
        }

        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{

        //    if(e.RowIndex>0 && e.ColumnIndex==0)
        //    {
        //        if()
        //    }
        //    //if e.RowIndex > 0 And e.ColumnIndex = 0 Then
        //    //                If dgvProduct.Item(0, e.RowIndex - 1).Value = e.Value Then
        //    //                    e.Value = ""
        //    //                ElseIf e.RowIndex < dgvProduct.Rows.Count - 1 Then
        //    //                    dgvProduct.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
        //    //                End If
        //    //End If
        //}
        private void FillMyTreeView(TabsList tabs)
        {
            List<TabInf> gtl = new List<TabInf>(tabs.Tabs);

            gtl.Sort();
            tabListTree.BeginUpdate();
            List<string> basur = new List<string>(tabs.BaseURLs);
            basur.Sort();
            tabListTree.Nodes.Add(new TreeNode("**Unidentified BaseURLs**"));
            foreach (string baseUrl in basur)
            {
                tabListTree.Nodes.Add(new TreeNode(baseUrl));
                int childNodesCount=0;
                foreach (TabInf t in gtl)
                {
                    if (t.baseWebsite!=baseUrl)
                    {
                        if(!basur.Contains(t.baseWebsite))
                        {
                            childNodesCount++;
                            tabListTree.Nodes[0].Nodes.Add(new TreeNode(t.url));
                        }
                        else
                        {
                            break;
                        }
                    }
                    tabListTree.Nodes[basur.IndexOf(baseUrl)+1]
                        .Nodes.Add(new TreeNode(t.url));
                    childNodesCount++;
                }
                gtl.RemoveRange(0, childNodesCount);
            }
            tabListTree.EndUpdate();
            tabListTree.Update();
        }

        private void FillDataGridView1()
        {
            var tabsList = TabsList.GetInstance();
            foreach (TabInf tab in tabsList.Tabs)
            {
                //this.listView1.Items.Add(tab.url);
                DataRow dr = dt.NewRow();
                dr["Tab Position"] = tab.tabPosition;
                dr["Title"] = tab.lastKnownTitle;
                dr["URL"] = tab.url;
                dr["Base URL"] = tab.baseWebsite;
                dt.Rows.Add(dr);
            }
            this.dataGridView1.DataSource = dt;
            //////this.dataGridView1.GridColor = Color.BlueViolet;
            //////this.dataGridView1.BorderStyle = BorderStyle.Fixed3D;

            ////DataGridViewCellStyle DefaultStyle = new DataGridViewCellStyle();
            ////DefaultStyle.Font = new Font(dataGridView1.Font,FontStyle.Regular);
            ////DefaultStyle.BackColor = Color.LightCyan;
            ////DefaultStyle.ForeColor = Color.Black;
            ////dataGridView1.DefaultCellStyle = DefaultStyle;

            dataGridView1.Refresh();
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void button_connectfetchjson_Click(object sender, EventArgs e)
        {
            string clickMessage = "Make sure to:\n 1) Have ADB and proper ADB drivers for your device.\n";
            clickMessage += "2) Have \"USB Debugging\" enabled on the device to recover from.\n";
            clickMessage += "3) Connect ONLY the Android device you want to recover from.\n";
            clickMessage += "4) Allow ADB Debugging from the pop-up on your device.\n";
            clickMessage += "5) Have Chrome open on your Android device.";
            MessageBox.Show(clickMessage, "REQUIREMENTS/INSTRUCTIONS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            groupBox1.ForeColor = Color.White;
            importAndProcessGroupbox.ForeColor = Color.White;
            connectGroupBox.ForeColor = Color.Orange;

            //need to add adb location selection window and an argument to take that in the StartChr... function.
            //ImportUtilities.StartChromeAndroidJsonListServer(string.Empty); //for testing/debugging
            string adbPath = string.Empty;
            adbPath = ImportUtilities.GetADBPathDialog();
            if (adbPath == "-1") return;
            ImportUtilities.StartChromeAndroidJsonListServer(adbPath);
            ImportUtilities.DownloadTabListJSON();

            connectGroupBox.ForeColor = Color.Lime;
            importAndProcessGroupbox.ForeColor = Color.Orange;
        }

        private void button_ImportAndProcess_Click(object sender, EventArgs e)
        {
            groupBox1.ForeColor = Color.White;
            importAndProcessGroupbox.ForeColor = Color.Orange;

            dt.Clear();
            tabListTree.Nodes.Clear();
            var basicTabInfs = ImportUtilities.LoadJson(manuallySetJsonLocation);
            ImportUtilities.GetURLtxtAndTITLEtxtFromJSON(basicTabInfs);

            var tabsList = TabsList.GetInstance();
            tabsList.ResetTabList(); //In case this has already been used once. //The class is singleton.
            tabsList.Process(basicTabInfs);

            FillDataGridView1();
            FillMyTreeView(tabsList);


            importAndProcessGroupbox.ForeColor = Color.Lime;
            groupBox1.ForeColor = Color.Orange;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_exportListHTML_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            var tabsList = TabsList.GetInstance();
            string outputPath =  tabsList.ExportToGroupedListHTML();
            if(groupBox1.ForeColor!=Color.Lime)
            {
                groupBox1.ForeColor = Color.Yellow;
            }
            MessageBox.Show("The HTML List has been exported to: " + outputPath, "List Exported!", MessageBoxButtons.OK);
        }

        private void button_ExportAsBookmarks_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            var tabsList = TabsList.GetInstance();
            string outputPath= tabsList.ExportToNetscapeBookmarksHTML();
            groupBox1.ForeColor = Color.Lime;
            MessageBox.Show("The Bookmarks file has been exported to: " + outputPath, "Bookmarks Exported!", MessageBoxButtons.OK);
        }

        private void button_selectjson_Click(object sender, EventArgs e)
        {
            groupBox1.ForeColor = Color.White;
            importAndProcessGroupbox.ForeColor = Color.White;
            connectGroupBox.ForeColor = Color.Orange;
            MessageBox.Show("Button functionality may not be fully stable.", "!!! WORK IN PROGRESS !!!", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "JSON and JSON.BAK files (*.json, *.json.bak)|*.json;*.json.bak|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    manuallySetJsonLocation = openFileDialog.FileName;
                }
            }

            connectGroupBox.ForeColor = Color.Lime;
            importAndProcessGroupbox.ForeColor = Color.Orange;
        }

        private void button_ExportAsSQLiteDB_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            var tabsList = TabsList.GetInstance();
            string outputPath = tabsList.ExportToSqliteDB();
            groupBox1.ForeColor = Color.Lime;
            MessageBox.Show("The SQLite3 Database has been exported to: " + outputPath, "Bookmarks Exported!", MessageBoxButtons.OK);
        }
    }

}
