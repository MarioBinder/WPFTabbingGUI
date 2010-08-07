using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WPFTabbingGUI.Views;


namespace WPFTabbingGUI
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow mainView = new MainWindow();           
            mainView.Show();
        }
    }
}
