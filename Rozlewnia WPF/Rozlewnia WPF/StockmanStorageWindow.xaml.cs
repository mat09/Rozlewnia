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
using System.Windows.Shapes;

namespace Rozlewnia_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StockmanStorageWindow : Window
    {
        public StockmanStorageWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            User.Instance.logout();
            Application.Current.Shutdown();
            
        }

        private void GetBootleButton_Click(Object sender, RoutedEventArgs e)
        {

        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            InputUserDialog dialog = new InputUserDialog(true);
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("imie:" + dialog.MyName + " who " + dialog.Who);
            }


        }
    }
}
