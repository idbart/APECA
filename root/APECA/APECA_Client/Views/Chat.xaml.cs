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
    public partial class Chat : Page
    {
        public ChatModel model;
        public Chat()
        {
            InitializeComponent();

            model = new ChatModel();

            sendMessageButton.IsDefault = true;
            messagesView.ItemsSource = model.messagesToDisplay;

            model.messagesToDisplay.CollectionChanged += (e, args) => { updateMessageViewHolderScroll(); };
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(chatMessageInput.Text))
            {
                model.sendMessage(chatMessageInput.Text);

                chatMessageInput.Text = default(string); 
            }
        }
        private void updateMessageViewHolderScroll()
        {
            this.Dispatcher.Invoke(() => { messageViewHolder.ScrollToBottom(); });
        }
    }
}
