using ChromeDroid_TabMan.DTOs;
using ChromeDroid_TabMan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeDroid_TabMan.Auxiliary
{
    internal static class WinFormsUtils
    {
        public static void InitializeOrResetBrowserListComboBox(ComboBox browserSelect, List<BrowserComboItem> itemLists_OptionalParam = null) //note, c# passes objects by reference.
        {
            browserSelect.DataSource = null;
            List<BrowserComboItem> comboItemsAlignedList;
            if (itemLists_OptionalParam == null)
                comboItemsAlignedList = BrowserDiscoveryUtils.GetDefaultBrowserComboItems();
            else
                comboItemsAlignedList = itemLists_OptionalParam;
            browserSelect.DataSource = comboItemsAlignedList;

            browserSelect.DisplayMember = nameof(BrowserComboItem.Name);
            browserSelect.ValueMember = nameof(BrowserComboItem.BrowserDetails);
            browserSelect.SelectedItem = browserSelect.Items[0];
            browserSelect.SelectedIndex = 0;

            browserSelect.Refresh();
        }

        public static string GetADBPathDialog(bool resetFilePathToC = false)
        {
            var filePath = string.Empty;
            bool IsCancelled = false;
            while (filePath == string.Empty && !IsCancelled)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if(resetFilePathToC)
                        openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "ADB Executable (adb.exe)|adb.exe|All Executables (*.exe)|*.exe";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() != DialogResult.OK || openFileDialog.FileName == string.Empty)
                    {
                        if (DialogResult.Cancel == MessageBox.Show("No file selected.", "Notice", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation))
                        {
                            IsCancelled = true;
                            return "-1";
                        }
                        continue;
                    }

                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            MessageBox.Show("Selected executable: " + filePath, "ADB Executable Selected!" + filePath, MessageBoxButtons.OK);

            return filePath;
        }

        public static void InitializeOrResetDataTableForTabsDataGridView(ref DataTable dataTable, ref DataGridView dataGridView)
        {
            dataTable.Columns.Clear();
            dataTable.Columns.Add("TabNum", typeof(int));
            dataTable.Columns.Add("Title");
            dataTable.Columns.Add("Base URL");
            dataTable.Columns.Add("URL");
            dataGridView.DataSource = dataTable;

            for(int i=0; i< dataGridView.Columns.Count;i++)
            {
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dataGridView.Columns[0].FillWeight = 8;
            dataGridView.Columns[1].FillWeight = 35;
            dataGridView.Columns[2].FillWeight = 17;
            dataGridView.Columns[3].FillWeight = 40;

            dataGridView.Refresh();
        }

        public static void FillDataGridView(IEnumerable<TabInf> tabsToAdd, ref DataTable dataTable, ref DataGridView dataGridView)
        {
            dataTable.Rows.Clear();

            foreach (TabInf tab in tabsToAdd)
            {
                DataRow dr = dataTable.NewRow();
                dr["TabNum"] = tab.TabNum;
                dr["Title"] = tab.LastKnownTitle;
                dr["Base URL"] = tab.BaseWebsite;
                dr["URL"] = tab.URL;
                dataTable.Rows.Add(dr);
            }

            //dataGridView.DataSource = dataTable;

            dataGridView.Refresh();
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
        public static void FillTreeView(ITabsContainer tabsContainer, ref TreeView tabListTree)
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
    }
}
