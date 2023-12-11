using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        string AdbPath { get; set; }
        string jsonLocation = string.Empty;
        DataTable dt = new DataTable();
        ITabsContainer tabsContainer =null;
        List<BrowserComboItem> comboItemsAlignedList = new List<BrowserComboItem>();
        private void InitializeOrResetBrowserListComboBox()
        {
            comboBox_BrowserSelect.DataSource = null;
            comboItemsAlignedList = new List<BrowserComboItem>
            {
                new BrowserComboItem("Chrome",ConfigHelper.ADB.Chrome_PackageName,ConfigHelper.ADB.Chrome_ForwardParameter_Remote,true,false),
                new BrowserComboItem("Opera",ConfigHelper.ADB.Opera_PackageName,ConfigHelper.ADB.Opera_ForwardParameter_Remote,true,false),
                new BrowserComboItem("SamsungInternet",ConfigHelper.ADB.SamsungInternet_PackageName,ConfigHelper.ADB.SamsungInternet_ForwardParameter_Remote,true, false),
                new BrowserComboItem("Edge",ConfigHelper.ADB.Edge_PackageName,ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd,false,false),
                new BrowserComboItem("Brave",ConfigHelper.ADB.Brave_PackageName,ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd,false, false)
            };
            comboBox_BrowserSelect.DataSource = new List<BrowserComboItem>(comboItemsAlignedList);
            //comboBox_BrowserSelect.Items.AddRange(comboItemsAlignedList.ToArray());
            comboBox_BrowserSelect.DisplayMember = nameof(BrowserComboItem.Name);
            comboBox_BrowserSelect.ValueMember = nameof(BrowserComboItem.BrowserDetails);
            comboBox_BrowserSelect.SelectedIndex = 0;
            //comboBox_BrowserSelect.SelectedItem = comboBox_BrowserSelect.Items[0];
        }
        public MainForm()
        {
            dt.Columns.Add("TabNum",typeof(int));
            dt.Columns.Add("Title");
            dt.Columns.Add("URL");
            dt.Columns.Add("Base URL");



            InitializeComponent();

            InitializeOrResetBrowserListComboBox();

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
            if (this.AdbPath == null || this.AdbPath.Length == 0)
            {
                AdbPath = ImportUtilities.GetADBPathDialog();
                if (AdbPath == "-1")
                {
                    AdbPath = string.Empty;
                    return;
                }
            }
            //string adbPath = "C:\\Program Files (x86)\\Minimal ADB and Fastboot\\adb.exe";
            //IAdbConnector adbConnector = new StaticSocketNameChromiumAdbConnector(adbPath, ConfigHelper.ADB.Chrome_PackageName, ConfigHelper.ADB.Chrome_ForwardParameter_Remote);
            IAdbConnector adbConnector; //= new DynamicSocketNamePidAtEndChromiumAdbConnector(adbPath, ConfigHelper.ADB.Edge_PackageName, ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd);
            BrowserDetailsStruct browserDetails = (BrowserDetailsStruct) comboBox_BrowserSelect.SelectedValue;//.SelectedItem).BrowserDetails;
            if (browserDetails.PackageName == null && browserDetails.IsSocketNameFull)
                adbConnector = new DiscoveredSocketOnlyAdbConnector(AdbPath, browserDetails.SocketNameFullOrPartial);
            else if (browserDetails.IsSocketNameFull)
                adbConnector = new StaticSocketNameChromiumAdbConnector(AdbPath, browserDetails.PackageName, browserDetails.SocketNameFullOrPartial);
            else
                adbConnector = new DynamicSocketNamePidAtEndChromiumAdbConnector(AdbPath, browserDetails.PackageName, browserDetails.SocketNameFullOrPartial);

            ITabsJsonFetcher tabsJsonFetcher = new AdbTabsJsonFetcher(adbConnector);
            jsonLocation = tabsJsonFetcher.FetchTabsJson();

            comboBox_BrowserSelect.Enabled= false;

            connectGroupBox.ForeColor = Color.Lime;
            importAndProcessGroupbox.ForeColor = Color.Orange;
        }

        private void button_ImportAndProcess_Click(object sender, EventArgs e)
        {
            groupBox1.ForeColor = Color.White;
            importAndProcessGroupbox.ForeColor = Color.Orange;

            dt.Clear();
            dataGridView1.Refresh();
            tabListTree.Nodes.Clear();
            tabListTree.Refresh();

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

        private void button_ManuallyDiscoverDevToolsSockets_Click(object sender, EventArgs e)//open your targeted browser on your android phone to make sure its devtools socket is open.
        {
            MessageBox.Show("This feature will discover/rediscover and verify DevTools sockets available on your device. \nMake sure your browser of interest is open on your device and your device is connected.\n If one of the listed browsers isn't being verified, it may be listed below as a separate search result.","READ ME!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (this.AdbPath == null || this.AdbPath.Length == 0)
            {
                AdbPath = ImportUtilities.GetADBPathDialog();
                if (AdbPath == "-1")
                {
                    AdbPath = string.Empty;
                    return;
                }
            }

            var devToolsSockets = ImportUtilities.GetDevToolsSockets(AdbPath);
            InitializeOrResetBrowserListComboBox();

            for(int i= 0;i<comboItemsAlignedList.Count();i++)
            {
                var browser = comboItemsAlignedList[i];
                var currBD = browser.BrowserDetails;
                BrowserComboItem newBrowserComboItem = null;
                
                var socketFullName = browser.BrowserDetails.SocketNameFullOrPartial;
                if (!currBD.IsSocketNameFull && currBD.PackageName!=null || currBD.PackageName==ConfigHelper.ADB.Chrome_PackageName)//the || currBD.PackageName==ConfigHelper.ADB.Chrome_PackageName condition fixes the condition where one of the other chromium browsers somehow gets the name default devtools socket before chrome.
                {
                    
                    try
                    {
                        socketFullName = currBD.SocketNameFullOrPartial + "_" + ImportUtilities.GetChromiumBrowserPid(AdbPath, currBD.PackageName, false);
                    }
                    catch(PidNotParsedException pidEx)
                    {
                        continue;
                    }
                    if(devToolsSockets.Any(s=>(socketFullName=="localabstract:"+s.Replace("\r", "").Replace("\n", "").Replace("@", ""))))
                    {
                        browser.BrowserDetails = new BrowserDetailsStruct("[✓]" + currBD.BrowserName, currBD.PackageName,socketFullName, true, true);
                    }
                }
                else
                {
                    if (devToolsSockets.Any(s => (socketFullName == "localabstract:" + s.Replace("\r", "").Replace("\n", "").Replace("@", ""))))
                    {
                        browser.BrowserDetails = new BrowserDetailsStruct("[✓]" + currBD.BrowserName, currBD.PackageName, socketFullName, true, true);
                    }
                }

                if(newBrowserComboItem!=null)
                {
                    comboItemsAlignedList.RemoveAt(i);
                    comboItemsAlignedList.Add(newBrowserComboItem);
                }
                devToolsSockets.RemoveAll(s => (socketFullName == "localabstract:" + s.Replace("\r", "").Replace("\n", "").Replace("@", "")));
            }
            foreach(string socket in devToolsSockets)
            {
                if (socket == string.Empty)
                    continue;
                string nSocket = socket.Replace("\r", "").Replace("\n", "").Replace("@","");
                BrowserComboItem browser = new("[🔍]"+"localabstract:" + nSocket, null, "localabstract:" + nSocket, true, true);
                //comboBox_BrowserSelect.Items.Add(browser);
                comboItemsAlignedList.Add(browser);
            }

            comboBox_BrowserSelect.DataSource = null;
            comboBox_BrowserSelect.DataSource = new List<BrowserComboItem>(comboItemsAlignedList);
            comboBox_BrowserSelect.SelectedIndex = 0;
            comboBox_BrowserSelect.SelectedItem = comboBox_BrowserSelect.Items[0];
            comboBox_BrowserSelect.DisplayMember = nameof(BrowserComboItem.Name);
            comboBox_BrowserSelect.ValueMember = nameof(BrowserComboItem.BrowserDetails);
            comboBox_BrowserSelect.Refresh();
        }
    }

}
