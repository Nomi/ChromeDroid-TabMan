using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeDroid_TabMan
{
    public partial class Form1 : Form
    {
        TabsList tabbys = new TabsList();// new TabsList(false);
        public Form1()
        {
            //ImportUtilities.StartChromeAndroidJsonListServer();
            ImportUtilities.DownloadTabListJSON();
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("lastknownTitle");
            dt.Columns.Add("url");
            dt.Columns.Add("BaseURL");
            foreach (TabInf tab in tabbys.tabs)
            {
                //this.listView1.Items.Add(tab.url);
                DataRow dr = dt.NewRow();
                dr["lastknownTitle"] = tab.lastKnownTitle;
                dr["url"] = tab.url;
                dr["BaseURL"] = tab.baseWebsite;
                dt.Rows.Add(dr);
            }
            this.dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            FillMyTreeView(tabbys);
            //tabbys.ExportToHTML();
            tabbys.ExportToNetscapeBookmarksHTML();
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
            List<TabInf> gtl = new List<TabInf>(tabs.tabs);

            gtl.Sort();
            tabListTree.BeginUpdate();
            List<string> basur = new List<string>(tabs.baseURLs);
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
            int googleLinksCount = tabListTree.Nodes[basur.IndexOf("www.google.com")].Nodes.Count;
            //the above shows correct count but TreeListView doesn't work?
        }
    }

}
