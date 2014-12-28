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
        
        private void showClient(object sender, RoutedEventArgs e)
        {
            showClientDialog dialog = new showClientDialog();
            dialog.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TransporterManageDialog dialog = new TransporterManageDialog();
            dialog.ShowDialog();
        }

        private void showTransportOrder_Click(object sender, RoutedEventArgs e)
        {
            TransportOrderDialog dialog = new TransportOrderDialog();
            dialog.ShowDialog();
        }

       
        private void showSendTransport_click(object sender, RoutedEventArgs e)
        {
            TransportSendDialog dialog = new TransportSendDialog();
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Transport wysłano.");
            }
        }

        private void showGiveBootle_Clisk(object sender, RoutedEventArgs e)
        {
            BootleGet_GiveDialog dialog = new BootleGet_GiveDialog(false);
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Wydano butle klientowi");
            }
        }

        private void showGetBootle_Click(object sender, RoutedEventArgs e)
        {
            BootleGet_GiveDialog dialog = new BootleGet_GiveDialog(true);
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Odebrano butle do napełnienia");
            }
        }

        private void showTransporterManager_Click(object sender, RoutedEventArgs e)
        {
            TransporterManageDialog dialog = new TransporterManageDialog();
            dialog.ShowDialog();    
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            UserAdd_EditDialog dialog = new UserAdd_EditDialog(true);
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Dodano uzytkownika");
            }
        }
    }
}
