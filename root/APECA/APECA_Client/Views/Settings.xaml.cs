using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APECA_Client.Models;

namespace APECA_Client.Views
{
    public partial class Settings : Page
    {
        public SettingsModel model;
        public Settings()
        {
            InitializeComponent();

            model = new SettingsModel();
            
            saveSettingsButton.IsDefault = true;

            //im not really sure whats going on here but it works now
            Properties.Settings.Default.SettingsLoaded += (sender, e) => { populate(); };
            populate();
        }

        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            model.saveSettings(userNameInput.Text, serverIPInput.Text, keyInput.Text);
        }
        private void populate()
        {
            if(Properties.Settings.Default != null && Properties.Settings.Default.Config != null)
            {
                userNameInput.Text = Properties.Settings.Default.Config.userName;
                serverIPInput.Text = Properties.Settings.Default.Config.serverIP;
                keyInput.Text = Properties.Settings.Default.Config.getKeyString();
            }
        }
    }
}
