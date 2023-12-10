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
        ITabsContainer tabsContainer =null;
        public MainForm()
        {
            dt.Columns.Add("TabNum",typeof(int));
            dt.Columns.Add("Title");
            dt.Columns.Add("URL");
            dt.Columns.Add("Base URL");



            InitializeComponent();

            comboBox_BrowserSelect.DataSource = new BrowserComboItem[]
            {
                new BrowserComboItem("Chrome",ConfigHelper.ADB.Chrome_PackageName,ConfigHelper.ADB.Chrome_ForwardParameter_Remote,true),
                new BrowserComboItem("Opera",ConfigHelper.ADB.Opera_PackageName,ConfigHelper.ADB.Opera_ForwardParameter_Remote,true),
                new BrowserComboItem("SamsungInternet",ConfigHelper.ADB.SamsungInternet_PackageName,ConfigHelper.ADB.SamsungInternet_ForwardParameter_Remote,true),
                new BrowserComboItem("Edge",ConfigHelper.ADB.Edge_PackageName,ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd,false),
                new BrowserComboItem("Brave",ConfigHelper.ADB.Brave_PackageName,ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd,false)
            };
            comboBox_BrowserSelect.DisplayMember = nameof(BrowserComboItem.BrowserName);
            comboBox_BrowserSelect.ValueMember = nameof(BrowserComboItem.BrowserDetails);
            comboBox_BrowserSelect.SelectedIndex= 0;


            //var tabsList = TabsList.GetInstance();
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
        private void FillMyTreeView()
        {
            tabListTree.BeginUpdate();
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

        private void FillDataGridView1()
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
            //string clickMessage = "Make sure to:\n 1) Have ADB and proper ADB drivers for your device.\n";
            //clickMessage += "2) Have \"USB Debugging\" enabled on the device to recover from.\n";
            //clickMessage += "3) Connect ONLY the Android device you want to recover from.\n";
            //clickMessage += "4) Allow ADB Debugging from the pop-up on your device.\n";
            //clickMessage += "5) Have Chrome open on your Android device.";
            //MessageBox.Show(clickMessage, "REQUIREMENTS/INSTRUCTIONS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //groupBox1.ForeColor = Color.White;
            //importAndProcessGroupbox.ForeColor = Color.White;
            //connectGroupBox.ForeColor = Color.Orange;

            //need to add adb location selection window and an argument to take that in the StartChr... function.
            //ImportUtilities.StartChromeAndroidJsonListServer(string.Empty); //for testing/debugging
            //string adbPath = string.Empty;
            //adbPath = ImportUtilities.GetADBPathDialog();
            //if (adbPath == "-1") return;

            string adbPath = "C:\\Program Files (x86)\\Minimal ADB and Fastboot\\adb.exe";
            //IAdbConnector adbConnector = new StaticSocketNameChromiumAdbConnector(adbPath, ConfigHelper.ADB.Chrome_PackageName, ConfigHelper.ADB.Chrome_ForwardParameter_Remote);
            IAdbConnector adbConnector; //= new DynamicSocketNamePidAtEndChromiumAdbConnector(adbPath, ConfigHelper.ADB.Edge_PackageName, ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd);
            BrowserDetailsStruct browserDetails = (BrowserDetailsStruct)comboBox_BrowserSelect.SelectedValue;
            if (browserDetails.IsSocketNameFull)
                adbConnector = new StaticSocketNameChromiumAdbConnector(adbPath, browserDetails.PackageName, browserDetails.SocketNameFullOrPartial);
            else
                adbConnector = new DynamicSocketNamePidAtEndChromiumAdbConnector(adbPath, browserDetails.PackageName, browserDetails.SocketNameFullOrPartial);

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
            tabsContainer = tabsImporter.Import();

            FillDataGridView1();
            FillMyTreeView();


            importAndProcessGroupbox.ForeColor = Color.Lime;
            groupBox1.ForeColor = Color.Orange;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_exportListHTMLGrouped_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            ITabsExporter tabsExporter = new GroupedBasicHtmlListTabsExporter();
            var outputPath = tabsExporter.Export(tabsContainer);

            if(groupBox1.ForeColor!=Color.Lime)
            {
                groupBox1.ForeColor = Color.Yellow;
            }
            MessageBox.Show("The HTML List has been exported to: " + outputPath, "List Exported!", MessageBoxButtons.OK);
            button_exportListHTMLGrouped.ForeColor = Color.Lime;
        }

        private void button_ExportAsBookmarksGrouped_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            ITabsExporter tabsExporter = new GroupedNetscapeHtmlBookmarksExporter();
            var outputPath = tabsExporter.Export(tabsContainer);

            groupBox1.ForeColor = Color.Lime;
            MessageBox.Show("The Bookmarks (grouped by base URL) file has been exported to: " + outputPath, "Bookmarks (grouped) Exported!", MessageBoxButtons.OK);
            button_ExportAsBookmarksGrouped.ForeColor = Color.Lime;
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
            ITabsExporter tabsExporter = new SQLiteTabsExporter();
            var outputPath = tabsExporter.Export(tabsContainer);
            groupBox1.ForeColor = Color.Lime;
            MessageBox.Show("The SQLite3 Database has been exported to: " + outputPath, "SQLite Database Exported!", MessageBoxButtons.OK);
            button_ExportAsSQLiteDB.ForeColor = Color.Lime;
        }

        private void button_exportCSV_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            ITabsExporter tabsExporter = new CSVTextFilePairTabsExporter();
            var outputPath = tabsExporter.Export(tabsContainer);
            groupBox1.ForeColor = Color.Lime;
            MessageBox.Show("The CSV text file has been exported to: " + outputPath, "CSV Exported!", MessageBoxButtons.OK);
            button_exportCSV.ForeColor = Color.Lime;
        }

        private void button_ExportAsBookmarks_Click(object sender, EventArgs e)
        {
            //add select path dialog box??
            ITabsExporter tabsExporter = new NetscapeHtmlBookmarksExporter();
            var outputPath = tabsExporter.Export(tabsContainer);
            groupBox1.ForeColor = Color.Lime;
            MessageBox.Show("The Bookmarks file has been exported to: " + outputPath, "Bookmarks Exported!", MessageBoxButtons.OK);
            button_ExportAsBookmarks.ForeColor = Color.Lime;
        }

        private void browserDetailsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }

}
