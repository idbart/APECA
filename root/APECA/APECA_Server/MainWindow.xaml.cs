﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APECA_Server.Scripts;
using APECA_Server.Models;
using APECA_Shared_Library;

namespace APECA_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowModel model = new MainWindowModel();

        public MainWindow()
        {
            InitializeComponent();

            connectedUsersViewHolder.ItemsSource = model.getClients();
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            setListenIPButton.IsEnabled = true;
            setListenIPInput.IsEnabled = true;

            Thread visualUpdater = new Thread(updateVisuals);
            visualUpdater.Start();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            bool didStart = model.startServer();

            if(didStart)
            {
                startButton.IsEnabled = false;
                stopButton.IsEnabled = true;
                setListenIPButton.IsEnabled = false;
                setListenIPInput.Text = model.getListenIP();
                setListenIPInput.IsEnabled = false;
            }
            else
            {
                return;
            }
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            model.stopServer();

            stopButton.IsEnabled = false;
            startButton.IsEnabled = true;
            setListenIPButton.IsEnabled = false;
            setListenIPInput.IsEnabled = true;
        }

        private void setListenIPButton_Click(object sender, RoutedEventArgs e)
        {
            model.setListenIP(setListenIPInput.Text);
            setListenIPButton.IsEnabled = false;
        }
        private void setListenIPInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(setListenIPInput.Text == model.getListenIP())
            {
                setListenIPButton.IsEnabled = false;
            }
            else
            {
                setListenIPButton.IsEnabled = true;
            }
        }

        public void updateVisuals()
        {   
            while (true)
            { 
                Thread.Sleep(1000);
            }
        }
    }
}
