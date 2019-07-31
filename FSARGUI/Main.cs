using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSARLib;

namespace FSARGUI
{
    
    public partial class Main : Form
    {
        public const string Version = "(Not versionned yet)";

        public readonly decimal TabWidthRatio;
        public readonly decimal TabHeightRatio;
        
        List<OpenedArchive> Tabs = new List<OpenedArchive>();
        OpenedArchive[] OpenedFiles;
        Size CurSize;

        AboutBox ADialog = new AboutBox();
        Settings SDialog = new Settings();

        public Main()
        {
            InitializeComponent();
            CurSize = this.ClientSize;
            
            Bitmap picImage = new Bitmap(CatLoad.Image);
            CatLoad.Location = new Point((CatLoad.Parent.ClientSize.Width / 2) - (picImage.Width / 2),
                                        (CatLoad.Parent.ClientSize.Height / 2) - (picImage.Height / 2));
            CatLoad.Refresh();

            TabWidthRatio = (decimal) OpenedArchives.Size.Width / (decimal) CurSize.Width;
            TabHeightRatio = (decimal) OpenedArchives.Size.Height / (decimal) CurSize.Height;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            using(var FAROpen = new OpenFileDialog())
            {
                CatLoad.Visible = true;

                FAROpen.Filter = "Far archives (*.far) |*.far";
                FAROpen.FilterIndex = 1;

                if (FAROpen.ShowDialog() == DialogResult.OK)
                {
                    if(OpenedFiles != null) Tabs = OpenedFiles.ToList();
                    Tabs.Add(new OpenedArchive());
                    OpenedFiles = Tabs.ToArray();
                    int CurInt = OpenedFiles.Length - 1;

                    OpenedFiles[CurInt].FilePath = FAROpen.FileName;
                    OpenedFiles[CurInt].FileName = Path.GetFileName(FAROpen.FileName);

                    try
                    {
                        ParseFarArchive(OpenedFiles[CurInt], FAROpen.FileName);
                    }    
                    catch(Exception error)
                    {
                        MessageBox.Show(error.Message);
                        CatLoad.Visible = false;
                        return;
                    }

                    OpenedFiles[CurInt].Tab = new TabPage();
                    NewTab(OpenedFiles[CurInt], OpenedFiles[CurInt].FileName);

                    OpenedFiles[CurInt].tree.NodeMouseClick += new TreeNodeMouseClickEventHandler(ShowNodeInfo);
                    OpenedFiles[CurInt].tree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(ShowNodeInfo);

                }
                CatLoad.Visible = false;
            }
        }

        private void ParseFarArchive(OpenedArchive CurArchive, string FarPath)
        {
            CurArchive.Archive = new FSARArchive();
            CurArchive.Data = File.ReadAllBytes(FarPath);
            Byte[] Header = new Byte[0x20];
            Byte[] FileTable;

            // Read header
            FSARHelper.fastCopyBlock(CurArchive.Data, 0, Header, 0, Header.Length);
            CurArchive.Archive.Header = FSARRead.ParseHeader(Header);

            // Parse the file headers
            FileTable = new Byte[CurArchive.Archive.Header.FileTableEnd];
            FSARHelper.fastCopyBlock(CurArchive.Data, 0x20, FileTable, 0, FileTable.Length);
            CurArchive.FileEntries = FSARRead.GetFileEntries(FileTable, CurArchive.Archive.Header.FileTableObjects);
        }
        private void OpenFile(Byte[] Data, string OutPath)
        {
            CatLoad.Visible = true;

            if(OpenedFiles != null) Tabs = OpenedFiles.ToList();
            Tabs.Add(new OpenedArchive());
            OpenedFiles = Tabs.ToArray();
            int CurInt = OpenedFiles.Length - 1;

            OpenedFiles[CurInt].FilePath = OutPath;
            OpenedFiles[CurInt].FileName = Path.GetFileName(OutPath);
            OpenedFiles[CurInt].Data = Data;

            try
            {
                ParseFarArchive(OpenedFiles[CurInt]);
            }    
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
                CatLoad.Visible = false;
                return;
            }

            OpenedFiles[CurInt].Tab = new TabPage();
            NewTab(OpenedFiles[CurInt], OpenedFiles[CurInt].FileName);

            OpenedFiles[CurInt].tree.NodeMouseClick += new TreeNodeMouseClickEventHandler(ShowNodeInfo);
            OpenedFiles[CurInt].tree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(ShowNodeInfo);

                
            CatLoad.Visible = false;
        }

        private void ParseFarArchive(OpenedArchive CurArchive)
        {
            CurArchive.Archive = new FSARArchive();
            Byte[] Header = new Byte[0x20];
            Byte[] FileTable;

            // Read header
            FSARHelper.fastCopyBlock(CurArchive.Data, 0, Header, 0, Header.Length);
            CurArchive.Archive.Header = FSARRead.ParseHeader(Header);

            // Parse the file headers
            FileTable = new Byte[CurArchive.Archive.Header.FileTableEnd];
            FSARHelper.fastCopyBlock(CurArchive.Data, 0x20, FileTable, 0, FileTable.Length);
            CurArchive.FileEntries = FSARRead.GetFileEntries(FileTable, CurArchive.Archive.Header.FileTableObjects);
        }

        private void OpenDialog(object sender, EventArgs e)
        {
            switch(((MenuItem) sender).Text)
            {
                case "About":
                    ADialog = new AboutBox();
                    ADialog.ShowDialog();
                    ADialog.Dispose();
                    break;
                case "Settings":
                    SDialog = new Settings();
                    SDialog.ShowDialog();
                    SDialog.Dispose();
                    break;
            }
        }

        private void NewTab(OpenedArchive CurTab, string name)
        {
            var NewPage = CurTab.Tab;

            OpenedArchives.Controls.Add(NewPage);
            NewPage.Location = new System.Drawing.Point(4, 22);
            // NewPage.Name = name;
            NewPage.Padding = new System.Windows.Forms.Padding(3);
            NewPage.Size = new System.Drawing.Size(604, 412);
            NewPage.TabIndex = OpenedArchives.TabCount;
            NewPage.Text = name;
            NewPage.UseVisualStyleBackColor = true;
            NewPage.Font = OpenedArchives.Font;
            NewPage.Controls.Add(CurTab.tree);
            CurTab.Tab = NewPage;

            NodeStuff.GenTreeNodes(CurTab);
            CurTab.tree.Size = NewPage.Size;
            CurTab.tree.BackColor = System.Drawing.SystemColors.Control;
            CurTab.tree.Font = OpenedArchives.Font;

            OpenedArchives.TabIndex = NewPage.TabIndex;
            if(!OpenedArchives.Visible) OpenedArchives.Visible = true;
            if(!XafMenuItem.Enabled) XafMenuItem.Enabled = true;
        }

        private void CloseTab(object sender, EventArgs e)
        {
            if(OpenedArchives.TabCount > 0)
            {
                OpenedFiles[OpenedArchives.TabIndex - 1].Tab.Dispose();
                OpenedFiles[OpenedArchives.TabIndex - 1] = new OpenedArchive();
                if(OpenedArchives.TabCount == 0)
                {
                    OpenedArchives.Visible = false;
                    XafMenuItem.Enabled = false;
                }    
            }

        }

        private void Main_ResizeEnd(object sender, EventArgs e)
        {
            if(CurSize.Height > 0 && CurSize.Width > 0 &&
               OpenedArchives.Parent.ClientSize.Height > 0 && OpenedArchives.Parent.ClientSize.Width > 0)
            {
                OpenedArchives.Height = (int) Math.Round(TabHeightRatio * (decimal) OpenedArchives.Parent.ClientSize.Height);
                OpenedArchives.Width = (int) Math.Round(TabWidthRatio * (decimal) OpenedArchives.Parent.ClientSize.Width);
            }
            OpenedArchives.Refresh();

            if(OpenedFiles != null && OpenedFiles.Length > 0)
                foreach (var Openedfile in OpenedFiles)
                {
                    Openedfile.Tab.Size = OpenedArchives.Size;
                    Openedfile.Tab.Refresh();
                
                    Openedfile.tree.Size = Openedfile.Tab.Size;
                    Openedfile.tree.Refresh();
                }

            Bitmap picImage = new Bitmap(CatLoad.Image);
            CatLoad.Location = new Point((CatLoad.Parent.ClientSize.Width / 2) - (picImage.Width / 2),
                                        (CatLoad.Parent.ClientSize.Height / 2) - (picImage.Height / 2));
            CatLoad.Refresh();

            CurSize = OpenedArchives.Parent.ClientSize;
        }

        private void ShowNodeInfo(object sender, TreeNodeMouseClickEventArgs e)
        {
           if(e.Button == MouseButtons.Right)
           {
                //Open context menu
           }

           if(e.Button == MouseButtons.Left && e.Clicks == 2 && e.Node.LastNode == null)
            {
                MessageBox.Show($"You double clicked {e.Node.Text}");

                if(e.Node.Text.EndsWith(".far", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach(var File in OpenedFiles[OpenedArchives.TabIndex - 1].FileEntries)
                    {
                        if(e.Node.Text == File.FileName)
                        {
                            OpenFile(File.GetFile(OpenedFiles[OpenedArchives.TabIndex - 1].Data).UncompressedData, Path.GetDirectoryName(OpenedFiles[OpenedArchives.TabIndex - 1].FilePath) + File.FileName);
                            break;
                        }
                    }
                }
                else if(e.Node.Text.EndsWith(".xmk", StringComparison.InvariantCultureIgnoreCase) || Name.EndsWith(".fsgmub", StringComparison.InvariantCultureIgnoreCase))
                {
                    //Extract to midi
                }
                else if(e.Node.Text.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    //Open file in text editor
                }
            }

            FileInfoGrid.Text = $"Titties {e.Node.FullPath}";
        }

        private void ExtractAllFiles(object sender, EventArgs e)
        {
            
        }
    }

    public class OpenedArchive
    {
        public TabPage Tab;
        public FSARArchive Archive;
        public FSARFileEntryInfo[] FileEntries;
        public Byte[] Data;
        public string FileName;
        public string FilePath;
        public string OpenedPath = "\\";
        public TreeView tree = new TreeView();
    }
}
