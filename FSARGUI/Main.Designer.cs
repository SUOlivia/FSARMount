namespace FSARGUI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenuItem = new System.Windows.Forms.MenuItem();
            this.OpenMenuItem = new System.Windows.Forms.MenuItem();
            this.CloseMenuItem = new System.Windows.Forms.MenuItem();
            this.XafMenuItem = new System.Windows.Forms.MenuItem();
            this.EditMenuItem = new System.Windows.Forms.MenuItem();
            this.SettingsMenuItem = new System.Windows.Forms.MenuItem();
            this.AboutMenuItem = new System.Windows.Forms.MenuItem();
            this.CatLoad = new System.Windows.Forms.PictureBox();
            this.OpenedArchives = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.FileInfoGrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.CatLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenuItem,
            this.EditMenuItem,
            this.AboutMenuItem});
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.Index = 0;
            this.FileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenMenuItem,
            this.CloseMenuItem,
            this.XafMenuItem});
            this.FileMenuItem.Text = "File";
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Index = 0;
            this.OpenMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.OpenMenuItem.Text = "Open";
            this.OpenMenuItem.Click += new System.EventHandler(this.OpenFile);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Index = 1;
            this.CloseMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
            this.CloseMenuItem.Text = "Close";
            this.CloseMenuItem.Click += new System.EventHandler(this.CloseTab);
            // 
            // XafMenuItem
            // 
            this.XafMenuItem.Enabled = false;
            this.XafMenuItem.Index = 2;
            this.XafMenuItem.Text = "Extract all files";
            this.XafMenuItem.Click += new System.EventHandler(this.ExtractAllFiles);
            // 
            // EditMenuItem
            // 
            this.EditMenuItem.Index = 1;
            this.EditMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SettingsMenuItem});
            this.EditMenuItem.Text = "Edit";
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.Index = 0;
            this.SettingsMenuItem.Text = "Settings";
            this.SettingsMenuItem.Click += new System.EventHandler(this.OpenDialog);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Index = 2;
            this.AboutMenuItem.Text = "About";
            this.AboutMenuItem.Click += new System.EventHandler(this.OpenDialog);
            // 
            // CatLoad
            // 
            this.CatLoad.BackColor = System.Drawing.SystemColors.Control;
            this.CatLoad.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CatLoad.Image = global::FSARGUI.Properties.Resources.loadw;
            this.CatLoad.Location = new System.Drawing.Point(0, 0);
            this.CatLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CatLoad.Name = "CatLoad";
            this.CatLoad.Size = new System.Drawing.Size(128, 128);
            this.CatLoad.TabIndex = 0;
            this.CatLoad.TabStop = false;
            this.CatLoad.Visible = false;
            // 
            // OpenedArchives
            // 
            this.OpenedArchives.Font = new System.Drawing.Font("Nintendo DS BIOS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenedArchives.Location = new System.Drawing.Point(13, 0);
            this.OpenedArchives.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OpenedArchives.Name = "OpenedArchives";
            this.OpenedArchives.SelectedIndex = 0;
            this.OpenedArchives.Size = new System.Drawing.Size(483, 320);
            this.OpenedArchives.TabIndex = 0;
            this.OpenedArchives.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(0, 0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(200, 100);
            this.tabPage3.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.LineColor = System.Drawing.Color.Empty;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(200, 100);
            this.tabPage1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(200, 100);
            this.tabPage2.TabIndex = 0;
            // 
            // FileInfoGrid
            // 
            this.FileInfoGrid.Location = new System.Drawing.Point(503, 0);
            this.FileInfoGrid.Name = "FileInfoGrid";
            this.FileInfoGrid.Size = new System.Drawing.Size(161, 320);
            this.FileInfoGrid.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 334);
            this.Controls.Add(this.FileInfoGrid);
            this.Controls.Add(this.CatLoad);
            this.Controls.Add(this.OpenedArchives);
            this.Font = new System.Drawing.Font("Noto Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Menu = this.mainMenu1;
            this.Name = "Main";
            this.Text = "FSARGUI";
            this.ResizeEnd += new System.EventHandler(this.Main_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.Main_ResizeEnd);
            this.Resize += new System.EventHandler(this.Main_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.CatLoad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem FileMenuItem;
        private System.Windows.Forms.MenuItem OpenMenuItem;
        private System.Windows.Forms.MenuItem EditMenuItem;
        private System.Windows.Forms.MenuItem AboutMenuItem;
        private System.Windows.Forms.PictureBox CatLoad;
        private System.Windows.Forms.TabControl OpenedArchives;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuItem CloseMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuItem XafMenuItem;
        public System.Windows.Forms.PropertyGrid FileInfoGrid;
        private System.Windows.Forms.MenuItem SettingsMenuItem;
    }
}

