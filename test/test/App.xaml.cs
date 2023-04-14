using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Settings1.Default.IsConnect)
            {
                StartupUri = new Uri("ConnectBD.xaml", UriKind.Relative);
                return;
            }
            StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }
    }
}
