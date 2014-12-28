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
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Rozlewnia_WPF
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            //InitializeComponent();
            Window wi = new StockmanStorageWindow(); wi.Show(); this.Close();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if( loginBox.Text.Length==0 || passwordBox.Password.Length==0 )
            {
                msg.Content = "Uzupełnij wszystkie pola";    
                msg.Visibility = Visibility.Visible;
            }
            else
            {
                if (User.Instance.login(loginBox.Text, passwordBox.Password)==1)
                {
                    switch (User.Instance.tellMeWho())
                    {
                        case 1: break; // admin
                        case 2: Window wi = new StockmanStorageWindow(); wi.Show(); this.Close(); break; // STORAGE
                        case 3: break; // booting
                        default: break;
                    }
                }
                else
                {
                    msg.Content = "Błedny login lub hasło";
                    msg.Visibility = Visibility.Visible;
                }
            }

        }

      
    }
}
