
namespace ChromeDroid_TabMan
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabListTree = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPosList = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabTreeView = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.importAndProcessGroupbox = new System.Windows.Forms.GroupBox();
            this.button_ImportAndProcess = new System.Windows.Forms.Button();
            this.connectGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button_connectfetchjson = new System.Windows.Forms.Button();
            this.button_selectjson = new System.Windows.Forms.Button();
            this.button_exportListHTML = new System.Windows.Forms.Button();
            this.button_ExportAsBookmarks = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabControl1.SuspendLayout();
            this.tabPosList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabTreeView.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.importAndProcessGroupbox.SuspendLayout();
            this.connectGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabListTree
            // 
            this.tabListTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabListTree.Location = new System.Drawing.Point(3, 3);
            this.tabListTree.Name = "tabListTree";
            this.tabListTree.Size = new System.Drawing.Size(780, 292);
            this.tabListTree.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPosList);
            this.tabControl1.Controls.Add(this.tabTreeView);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(794, 331);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPosList
            // 
            this.tabPosList.Controls.Add(this.dataGridView1);
            this.tabPosList.Location = new System.Drawing.Point(4, 29);
            this.tabPosList.Name = "tabPosList";
            this.tabPosList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPosList.Size = new System.Drawing.Size(786, 298);
            this.tabPosList.TabIndex = 1;
            this.tabPosList.Text = "Tab List";
            this.tabPosList.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(780, 292);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabTreeView
            // 
            this.tabTreeView.Controls.Add(this.tabListTree);
            this.tabTreeView.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point);
            this.tabTreeView.ForeColor = System.Drawing.Color.Navy;
            this.tabTreeView.Location = new System.Drawing.Point(4, 29);
            this.tabTreeView.Name = "tabTreeView";
            this.tabTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tabTreeView.Size = new System.Drawing.Size(786, 298);
            this.tabTreeView.TabIndex = 0;
            this.tabTreeView.Text = "Tab Tree View";
            this.tabTreeView.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.importAndProcessGroupbox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.connectGroupBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 2;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(531, 340);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 107);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export Tabs";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter_1);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.button_ExportAsBookmarks, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.button_exportListHTML, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.linkLabel1, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(260, 81);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // importAndProcessGroupbox
            // 
            this.importAndProcessGroupbox.Controls.Add(this.button_ImportAndProcess);
            this.importAndProcessGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.importAndProcessGroupbox.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.importAndProcessGroupbox.ForeColor = System.Drawing.Color.White;
            this.importAndProcessGroupbox.Location = new System.Drawing.Point(267, 340);
            this.importAndProcessGroupbox.Name = "importAndProcessGroupbox";
            this.importAndProcessGroupbox.Size = new System.Drawing.Size(258, 107);
            this.importAndProcessGroupbox.TabIndex = 3;
            this.importAndProcessGroupbox.TabStop = false;
            this.importAndProcessGroupbox.Text = "Tabs Import and Processing";
            // 
            // button_ImportAndProcess
            // 
            this.button_ImportAndProcess.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_ImportAndProcess.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_ImportAndProcess.ForeColor = System.Drawing.Color.Black;
            this.button_ImportAndProcess.Location = new System.Drawing.Point(3, 23);
            this.button_ImportAndProcess.Name = "button_ImportAndProcess";
            this.button_ImportAndProcess.Size = new System.Drawing.Size(252, 29);
            this.button_ImportAndProcess.TabIndex = 0;
            this.button_ImportAndProcess.Text = "Import and Process";
            this.button_ImportAndProcess.UseVisualStyleBackColor = true;
            this.button_ImportAndProcess.Click += new System.EventHandler(this.button_ImportAndProcess_Click);
            // 
            // connectGroupBox
            // 
            this.connectGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.connectGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.connectGroupBox.ForeColor = System.Drawing.Color.Orange;
            this.connectGroupBox.Location = new System.Drawing.Point(3, 340);
            this.connectGroupBox.Name = "connectGroupBox";
            this.connectGroupBox.Size = new System.Drawing.Size(258, 107);
            this.connectGroupBox.TabIndex = 2;
            this.connectGroupBox.TabStop = false;
            this.connectGroupBox.Text = "Device Connection";
            this.connectGroupBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.button_connectfetchjson, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_selectjson, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(252, 81);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.AliceBlue;
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "--- OR ---";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_connectfetchjson
            // 
            this.button_connectfetchjson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_connectfetchjson.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_connectfetchjson.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button_connectfetchjson.Location = new System.Drawing.Point(3, 3);
            this.button_connectfetchjson.Name = "button_connectfetchjson";
            this.button_connectfetchjson.Size = new System.Drawing.Size(246, 26);
            this.button_connectfetchjson.TabIndex = 1;
            this.button_connectfetchjson.Text = "Connect and Fetch JSON";
            this.button_connectfetchjson.UseVisualStyleBackColor = true;
            this.button_connectfetchjson.Click += new System.EventHandler(this.button_connectfetchjson_Click);
            // 
            // button_selectjson
            // 
            this.button_selectjson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectjson.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_selectjson.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button_selectjson.Location = new System.Drawing.Point(3, 51);
            this.button_selectjson.Name = "button_selectjson";
            this.button_selectjson.Size = new System.Drawing.Size(246, 27);
            this.button_selectjson.TabIndex = 2;
            this.button_selectjson.Text = "Select pre-fetched JSON";
            this.button_selectjson.UseVisualStyleBackColor = true;
            this.button_selectjson.Click += new System.EventHandler(this.button_selectjson_Click);
            // 
            // button_exportListHTML
            // 
            this.button_exportListHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_exportListHTML.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_exportListHTML.ForeColor = System.Drawing.Color.Black;
            this.button_exportListHTML.Location = new System.Drawing.Point(3, 3);
            this.button_exportListHTML.Name = "button_exportListHTML";
            this.button_exportListHTML.Size = new System.Drawing.Size(254, 26);
            this.button_exportListHTML.TabIndex = 0;
            this.button_exportListHTML.Text = "Export as List (grouped)";
            this.button_exportListHTML.UseVisualStyleBackColor = true;
            this.button_exportListHTML.Click += new System.EventHandler(this.button_exportListHTML_Click);
            // 
            // button_ExportAsBookmarks
            // 
            this.button_ExportAsBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ExportAsBookmarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_ExportAsBookmarks.ForeColor = System.Drawing.Color.Black;
            this.button_ExportAsBookmarks.Location = new System.Drawing.Point(3, 35);
            this.button_ExportAsBookmarks.Name = "button_ExportAsBookmarks";
            this.button_ExportAsBookmarks.Size = new System.Drawing.Size(254, 26);
            this.button_ExportAsBookmarks.TabIndex = 1;
            this.button_ExportAsBookmarks.Text = "Export as Bookmarks";
            this.button_ExportAsBookmarks.UseVisualStyleBackColor = true;
            this.button_ExportAsBookmarks.Click += new System.EventHandler(this.button_ExportAsBookmarks_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI Semilight", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.linkLabel1.LinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel1.Location = new System.Drawing.Point(3, 64);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(254, 17);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "nomi.github.io";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "ChromeDroid TabMan";
            this.tabControl1.ResumeLayout(false);
            this.tabPosList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabTreeView.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.importAndProcessGroupbox.ResumeLayout(false);
            this.connectGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tabListTree;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTreeView;
        private System.Windows.Forms.TabPage tabPosList;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox connectGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox importAndProcessGroupbox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_connectfetchjson;
        private System.Windows.Forms.Button button_selectjson;
        private System.Windows.Forms.Button button_ImportAndProcess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button button_ExportAsBookmarks;
        private System.Windows.Forms.Button button_exportListHTML;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}

