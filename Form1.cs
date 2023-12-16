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
        DataTable dataTable = new DataTable();
        ITabsContainer tabsContainer =null;
        
        public MainForm()
        {

            InitializeComponent();

            WinFormsUtils.InitializeOrResetDataTableForTabsDataGridView(ref dataTable, ref dataGridView1);
            WinFormsUtils.InitializeOrResetBrowserListComboBox(comboBox_BrowserSelect);
            //var tabsList = TabsList.GetInstance();
            //FillMyTreeView(tabsList);
            //tabsList.ExportToHTML();
            //tabsList.ExportToNetscapeBookmarksHTML();
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
                AdbPath = WinFormsUtils.GetADBPathDialog();
                if (AdbPath == "-1")
                {
                    AdbPath = string.Empty;
                    return;
                }
            }
            //string adbPath = "C:\\Program Files (x86)\\Minimal ADB and Fastboot\\adb.exe";
            //IAdbConnector adbConnector = new StaticSocketNameChromiumAdbConnector(adbPath, ConfigHelper.ADB.Chrome_PackageName, ConfigHelper.ADB.Chrome_ForwardParameter_Remote);
            IAdbConnector adbConnector; //= new DynamicSocketNamePidAtEndChromiumAdbConnector(adbPath, ConfigHelper.ADB.Edge_PackageName, ConfigHelper.ADB.EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd);
            BrowserInfo browserDetails = (BrowserInfo) comboBox_BrowserSelect.SelectedValue;//.SelectedItem).BrowserDetails;
            if (browserDetails.PackageName == null && browserDetails.Socket.IsSocketNameComplete)
                adbConnector = new DiscoveredSocketOnlyAdbConnector(AdbPath, browserDetails.Socket.SocketConnectionStr);
            else if (browserDetails.Socket.IsSocketNameComplete)
                adbConnector = new StaticSocketNameChromiumAdbConnector(AdbPath, browserDetails.PackageName, browserDetails.Socket.SocketConnectionStr);
            else
                adbConnector = new DynamicSocketNamePidAtEndChromiumAdbConnector(AdbPath, browserDetails.PackageName, browserDetails.Socket.SocketConnectionStr);

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

            dataTable.Clear();
            dataGridView1.Refresh();
            tabListTree.Nodes.Clear();
            tabListTree.Refresh();

            ITabsImporter tabsImporter = new JsonToTabsImporter(jsonLocation);
            tabsContainer = tabsImporter.Import();

            WinFormsUtils.FillDataGridView(tabsContainer.AllTabInfs, ref dataTable, ref dataGridView1);
            WinFormsUtils.FillTreeView(tabsContainer, ref tabListTree);


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

        private async void button_FixOrDiscoverDevToolsSockets_Click_Async(object sender, EventArgs e)//open your targeted browser on your android phone to make sure its devtools socket is open.
        {
            MessageBox.Show("This feature will discover/rediscover and verify DevTools sockets available on your device. \nMake sure your browser of interest is open on your device and your device is connected.\n If one of the listed browsers isn't being verified, it may be listed below as a separate search result.", "READ ME!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (this.AdbPath == null || this.AdbPath.Length == 0)
            {
                AdbPath = WinFormsUtils.GetADBPathDialog();
                if (AdbPath == "-1")
                {
                    AdbPath = string.Empty;
                    return;
                }
            }

            var devToolsSocketsFound = BrowserDiscoveryUtils.GetDevToolsSockets(AdbPath);

            var newComboItemsList = BrowserDiscoveryUtils.GetDefaultBrowserComboItems();
            var adbConn = AdbConnection.ConnectAndOrGetInstance(AdbPath);
            await BrowserDiscoveryUtils.VerifyExistingSocketsAsync(newComboItemsList, devToolsSocketsFound, adbConn);
            BrowserDiscoveryUtils.DiscoverNewSocketsAndOrFixKnownBrowsersWithUnexpectedSocketNames(newComboItemsList, devToolsSocketsFound, AdbPath);
            

            WinFormsUtils.InitializeOrResetBrowserListComboBox(comboBox_BrowserSelect, newComboItemsList);
        }
    }

}
