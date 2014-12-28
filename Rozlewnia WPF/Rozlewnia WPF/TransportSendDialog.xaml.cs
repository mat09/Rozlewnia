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
    /// Interaction logic for TransportSendDialog.xaml
    /// </summary>
    public partial class TransportSendDialog : Window
    {
        static int rowAmmountTransporter = 4;
        Transport trans;

        public TransportSendDialog()
        {
            trans = null;
            InitializeComponent();
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            dt2.CanUserAddRows = false;
            dt2.CanUserDeleteRows = false;
            dt2.IsReadOnly = true;
            dt.ItemsSource = DataBase.Instance.showTransport(1);
            this.DataContext = this;
        }

        private void AddBootleToTransport_Clik(object sender, RoutedEventArgs e)
        {
            trans = dt.SelectedItem as Transport;
            if (trans != null)
            {
                BootleAddToSendDialog dialog = new BootleAddToSendDialog(trans.Id_transport);
                if (dialog.ShowDialog() == true)
                {
                    dt2.ItemsSource = DataBase.Instance.searchBootle(null, null, null, 2, int.Parse(trans.Id_transport));
                    MessageBox.Show("Dodano butle.");
                }
            }
            

        }

        private void SendTransport_Click(object sender, RoutedEventArgs e)
        {
            dt2.SelectAll();
            List<searchBootleClass> bootle = dt2.SelectedItems.OfType<searchBootleClass>().ToList();

            trans = dt.SelectedItem as Transport;
            if (trans != null)
            {
                if (bootle != null)
                {
                    if (dt.SelectedCells.Count == rowAmmountTransporter)
                    {
                        if (DataBase.Instance.changeTransport(bootle, null, 3, trans.Id_transport,false))
                        {
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Błąd");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Zaznacz tylko jedną firmę transportową.");
                    }
                }
                else
                {
                    MessageBox.Show("Błąd. W transporcie nie ma butli do wysłania");
                }
            }
            else
            {
                MessageBox.Show("Nie zaznaczono firmy transportowej.");
            }
        }

        private void dt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            trans = dt.SelectedItem as Transport;
            if(trans!=null)
            {
                dt2.ItemsSource = DataBase.Instance.searchBootle(null, null, null, 2, int.Parse(trans.Id_transport));
            }
        }
    }
}
