using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
           DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Sets the global culture to hebrew to avoid errors
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("he-IL");
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("he-IL");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("he-IL");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("he-IL");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            sender.GetType();
            MessageBox.Show(e.Exception.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
