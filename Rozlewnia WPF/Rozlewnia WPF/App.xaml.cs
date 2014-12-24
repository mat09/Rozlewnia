﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Rozlewnia_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
                        MessageBox.Show("Przechwycono niewyłapany wyjątek: " + e.Exception.Message, "Niewyłapany wyjątek", MessageBoxButton.OK, MessageBoxImage.Warning);
                        e.Handled = true;
                
        }

    }
}
