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
using AdvancedSharpAdbClient;
using ChromeDroid_TabMan.Connection_and_Import;
using ChromeDroid_TabMan.DTOs;
using System.Linq;

namespace ChromeDroid_TabMan
{
    public partial class MainForm : Form
    {
        string jsonLocation = string.Empty;
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
        private void FillMyTreeView(ITabsContainer tabsContainer)
        {
            tabListTree.BeginUpdate();
            tabListTree.Nodes.Add(new TreeNode("**Unidentified BaseURLs**"));
            int baseUrlIdx = 0;
            foreach (var baseUrl in tabsContainer.BaseUrlToTabInfCollectionMap.Keys)
            {
                var currMap = tabsContainer.BaseUrlToTabInfCollectionMap;
                tabListTree.Nodes.Add(new TreeNode(baseUrl));
                int childNodesCount=0;
                tabListTree.Nodes[baseUrlIdx].Nodes.AddRange(currMap[baseUrl].Select(x => new TreeNode(x.URL)).ToArray());
                baseUrlIdx++;
            }
            tabListTree.EndUpdate();
            tabListTree.Update();
        }

        private void FillDataGridView1(ITabsContainer tabsContainer)
        {
            foreach (TabInf tab in tabsContainer.AllTabInfs)
            {
                DataRow dr = dt.NewRow();
                dr["TabNum"] = tab.TabNum;
                dr["Title"] = tab.LastKnownTitle;
                dr["URL"] = tab.URL;
                dr["Base URL"] = tab.BaseWebsite;
                dt.Rows.Add(dr);
            }
            this.dataGridView1.DataSource = dt;
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
            //IAdbConnector adbConnector = new StaticSocketNameChromiumAdbConnector(adbPath, ConfigHelper.ADB.Chrome_PackageName, ConfigHelper.ADB.Chrome_ForwardParameter_Remote);
            IAdbConnector adbConnector = new DynamicSocketNamePidAtEndChromiumAdbConnector(adbPath, ConfigHelper.ADB.Edge_PackageName, ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd);
            ITabsJsonFetcher tabsJsonFetcher = new AdbTabsJsonFetcher(adbConnector);
            jsonLocation = tabsJsonFetcher.FetchTabsJson();

            connectGroupBox.ForeColor = Color.Lime;
            importAndProcessGroupbox.ForeColor = Color.Orange;
        }

        private void button_ImportAndProcess_Click(object sender, EventArgs e)
        {
            groupBox1.ForeColor = Color.White;
            importAndProcessGroupbox.ForeColor = Color.Orange;

            dt.Clear();
            tabListTree.Nodes.Clear();

            ITabsImporter tabsImporter = new JsonToTabsImporter(jsonLocation);
            var tabContainer = tabsImporter.Import();

            FillDataGridView1(tabContainer);
            FillMyTreeView(tabContainer);


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

            ITabsJsonFetcher tabsJsonFetcher = new SelectFileDialogTabsJsonFetcher();
            jsonLocation = tabsJsonFetcher.FetchTabsJson();

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
