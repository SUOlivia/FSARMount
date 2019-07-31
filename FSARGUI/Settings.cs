using System;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace FSARGUI
{
    public partial class Settings : Form
    {
        public static string SettingsPath = "settings.ini";
        public string XMK2MIDPath = "";
        public string MUB2MIDPath = "";
        public string MID2MUBPath = "";
        public string TEPath = "";

        bool Modified = false;

        FileIniDataParser SettingsParser = new FileIniDataParser();
        IniData SettingsData;

        public Settings()
        {
            InitializeComponent();

            if(File.Exists(SettingsPath))
                ReadSettings();
            else
                InitSettings();
        }

        private void SelectPath(object sender, EventArgs e)
        {
            var ClickedButton = (Button) sender;
            using(var SettingsOpener = new OpenFileDialog())
            {
                SettingsOpener.Filter = "Windows executable (*.exe) |*.exe";
                SettingsOpener.FilterIndex = 1;

                if(SettingsOpener.ShowDialog() == DialogResult.OK)
                {
                    switch(ClickedButton.Name)
                    {
                        case "XMK2MIDButton":
                            XMK2MIDPath = SettingsOpener.FileName;
                            XMK2MIDTextBox.Text = SettingsOpener.FileName;
                            break;
                        case "MUB2MIDButton":
                            MUB2MIDPath = SettingsOpener.FileName;
                            MUB2MIDTextBox.Text = SettingsOpener.FileName;
                            break;
                        case "MID2MUBButton":
                            MID2MUBPath = SettingsOpener.FileName;
                            MID2MUBTextBox.Text = SettingsOpener.FileName;
                            break;
                        case "TEButton":
                            TEPath = SettingsOpener.FileName;
                            TETextBox.Text = SettingsOpener.FileName;
                            break;
                    }
                    Modified = true;
                }
            }
        }

        public void ReadSettings()
        {
            SettingsData = SettingsParser.ReadFile(SettingsPath);

            XMK2MIDPath = SettingsData["Paths"]["XMK2MIDPath"];
            MUB2MIDPath = SettingsData["Paths"]["MUB2MIDPath"];
            MID2MUBPath = SettingsData["Paths"]["MID2MUBPath"];
            TEPath = SettingsData["Paths"]["TEPath"];
            
            XMK2MIDTextBox.Text = XMK2MIDPath;
            MUB2MIDTextBox.Text = MUB2MIDPath;
            MID2MUBTextBox.Text = MID2MUBPath;
            TETextBox.Text = TEPath;
        }

        public void WriteSettings()
        {
            SettingsData["Paths"]["XMK2MIDPath"] = XMK2MIDPath;
            SettingsData["Paths"]["MUB2MIDPath"] = MUB2MIDPath;
            SettingsData["Paths"]["MID2MUBPath"] = MID2MUBPath;
            SettingsData["Paths"]["TEPath"] = TEPath;

            SettingsParser.WriteFile(SettingsPath, SettingsData);
        }

        public void InitSettings()
        {
            SettingsData = new IniData();

            SettingsData.Sections.AddSection("Paths");
            SettingsData["Paths"].AddKey("XMK2MIDPath");
            SettingsData["Paths"].AddKey("MUB2MIDPath");
            SettingsData["Paths"].AddKey("MID2MUBPath");
            SettingsData["Paths"].AddKey("TEPath");

            WriteSettings();
        }

        private void EndDialog(object sender, EventArgs e)
        {
            if(XMK2MIDTextBox.Modified || MUB2MIDTextBox.Modified || MID2MUBTextBox.Modified || TETextBox.Modified)
            {
                Modified = true;

                XMK2MIDPath = XMK2MIDTextBox.Text;
                MUB2MIDPath = MUB2MIDTextBox.Text;
                MID2MUBPath = MID2MUBTextBox.Text;
                TEPath = TETextBox.Text;
            }

            if(Modified)
            {
                if(MessageBox.Show("The settings were modified, do you want to save them?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    WriteSettings();
                Modified = false;
            }
            this.Close();
        }
    }
}
