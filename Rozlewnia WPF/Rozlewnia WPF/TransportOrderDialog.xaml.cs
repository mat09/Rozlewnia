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
    /// Interaction logic for TransportOrderDialog.xaml
    /// </summary>
    public partial class TransportOrderDialog : Window
    {
        static int rowAmmountTransporter = 8;
        
        private Transporter trans;

        private String name;
        private String phone;
        public String NName
        {
            get { return name; }
            set { name = value; }
        }
        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public TransportOrderDialog()
        {
            NName = "";
            Phone = "";
            InitializeComponent();
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            dt2.CanUserAddRows = false;
            dt2.CanUserDeleteRows = false;
            dt2.IsReadOnly = true;
            this.DataContext = this;
            dt2.ItemsSource = DataBase.Instance.searchBootle(null,null,null, 1,-1);
            dt.ItemsSource = DataBase.Instance.showTransporter(NName, Phone);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.ItemsSource = DataBase.Instance.showTransporter(NName, Phone);
        }
        private void MarkAll_Click(object sender, RoutedEventArgs e)
        {
            dt2.SelectAll();
        }
        private void UnmarkAll_Click(object sender, RoutedEventArgs e)
        {
            dt2.UnselectAll();
        }

        private void OrderTransport_Click(object sender, RoutedEventArgs e)
        {
            List<searchBootleClass> bootle = dt2.SelectedItems.OfType<searchBootleClass>().ToList();
            trans = (Transporter)dt.SelectedItem;
            if (trans != null)
            {
                if( bootle!= null)
                {
                    if (dt.SelectedCells.Count == rowAmmountTransporter)
                    {
                        if (DataBase.Instance.changeTransport(bootle, trans.Id_transporter, 2, null,false,null))
                        {
                            this.DialogResult = true;
                            MessageBox.Show("Zamówiono transport.");
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
                    MessageBox.Show("Nie zaznaczono butli.");
                }
            }
            else
            {
                MessageBox.Show("Nie zaznaczono firmy transportowej.");
            }
        }

        
    }
}
