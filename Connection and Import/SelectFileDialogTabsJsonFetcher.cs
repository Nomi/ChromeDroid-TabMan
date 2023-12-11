using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal class SelectFileDialogTabsJsonFetcher : ITabsJsonFetcher
    {
#nullable enable
        public IAdbConnector? _adbConnector { get; } = null;
#nullable disable
        public SelectFileDialogTabsJsonFetcher() { }
        public string FetchTabsJson()
        {
            //Since JSON is pre-fetched, we only ever deal with getting its file path.

            string jsonPath = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "JSON and JSON.BAK files (*.json, *.json.bak)|*.json;*.json.bak|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    jsonPath = openFileDialog.FileName;
                }
            }

            if (jsonPath == null)
                throw new Exception("No file selected.");

            return jsonPath;
        }
    }
}
