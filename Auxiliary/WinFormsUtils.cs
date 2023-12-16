using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
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
    }
}
