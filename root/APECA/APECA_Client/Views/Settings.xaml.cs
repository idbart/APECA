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
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public SettingsModel model = new SettingsModel();
        public Settings()
        {
            InitializeComponent();
            
            saveSettingsButton.IsDefault = true;
            Properties.Settings.Default.SettingsLoaded += (sender, e) => { populate(); }; 
        }

        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            model.saveSettings(userNameInput.Text, serverIPInput.Text, keyInput.Text);
        }
        private void populate()
        {
            userNameInput.Text = Properties.Settings.Default.Config.userName;
            serverIPInput.Text = Properties.Settings.Default.Config.serverIP;
            keyInput.Text = Properties.Settings.Default.Config.getKey();
        }
    }
}
