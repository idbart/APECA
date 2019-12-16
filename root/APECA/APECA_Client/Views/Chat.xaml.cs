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
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Page
    {
        public ChatModel model = new ChatModel();
        public Chat()
        {
            InitializeComponent();

            sendMessageButton.IsDefault = true;
            messagesView.ItemsSource = model.messagesToDisplay;
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(chatMessageInput.Text) != true)
            {
                model.sendMessage(chatMessageInput.Text);
            }
        }
    }
}
