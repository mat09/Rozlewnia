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
    /// Interaction logic for TransportGetDialog.xaml
    /// </summary>
    public partial class TransportGetDialog : Window
    {
        Transport trans;
        static int rowAmmountTransporter = 5;

        public TransportGetDialog()
        {
            InitializeComponent();
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            dt2.CanUserAddRows = false;
            dt2.CanUserDeleteRows = false;
            dt2.IsReadOnly = true;
            dt.ItemsSource = DataBase.Instance.showTransport(2);
            this.DataContext = this;
        
        }

        private void dt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            trans = dt.SelectedItem as Transport;
            if (trans != null)
            {
                dt2.ItemsSource = DataBase.Instance.searchBootle(null, null, null, 6, int.Parse(trans.Id_transport));
            }
        }

        private void GetTransport_Click(object sender, RoutedEventArgs e)
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
                        if (MessageBox.Show("Czy na pewno wszytkie butle sie zgadzają z listą transportową?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Stop) == MessageBoxResult.Yes)
                        {
                            if (DataBase.Instance.changeTransport(bootle, null, 7, trans.Id_transport, false, "data_end"))
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
                            MessageBox.Show("tu sie dorobi funkcjonalnosc");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Zaznacz tylko jedną firmę transportową.");
                    }
                }
                else
                {
                    MessageBox.Show("Błąd. W transporcie nie ma butli do odebrania");
                }
            }
            else
            {
                MessageBox.Show("Nie zaznaczono firmy transportowej.");
            }
        }
    }
}
