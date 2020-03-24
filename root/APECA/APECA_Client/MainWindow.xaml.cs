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
using APECA_Client.Scripts;
using APECA_Shared_Library;
using APECA_Client.Models;
using APECA_Client.Views;

namespace APECA_Client
{
    public partial class MainWindow : Window
    {
        MainWindowModel model;
        Chat chatView;
        Settings settingsView;

        public MainWindow()
        {
            InitializeComponent();

            model = new MainWindowModel();
            chatView = new Chat();
            settingsView = new Settings();

            setFrameToChat();

            connectedClientsDisplay.ItemsSource = model.clients;
        }

        private void chatButton_Click(object sender, RoutedEventArgs e)
        {
            setFrameToChat();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            setFrameToSettings();
        }

        private void setFrameToChat()
        {
            chatButton.IsEnabled = false;
            settingsButton.IsEnabled = true;

            largeFrame.Content = chatView;
        }
        private void setFrameToSettings()
        {
            settingsButton.IsEnabled = false;
            chatButton.IsEnabled = true;

            largeFrame.Content = settingsView;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.dissconnectFromServer();
        }
    }
}
