using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APECA_Client.Scripts;
using APECA_Shared_Library;
using System.Windows;
using APECA_Client.Properties;

namespace APECA_Client.Models
{
    public class SettingsModel
    {
        public void saveSettings(string username, string serverip, string enckey)
        {
            if (Settings.Default.Config == null)
            {
                Settings.Default.Config = new SettingsConfig();
            }

            if (Settings.Default.Config.setKey(enckey) == false)
            {
                MessageBox.Show("Encryption key is invalid");
                return;
            }
            else
            {
                Settings.Default.Config.userName = username;
                Settings.Default.Config.serverIP = serverip;
                Settings.Default.Save();
                MessageBox.Show("Settings Saved");
            }
        }
    }
}
