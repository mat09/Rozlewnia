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
    public partial class TransporterManageDialog : Window
    {
        static int rowAmmount = 8;
        private Transporter trans;
        private String name;

        public String NName
        {
            get { return name; }
            set { name = value; }
        }
        private String phone;

        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }


        public TransporterManageDialog()
        {
            NName = "";
            Phone = "";
            InitializeComponent();
            this.DataContext = this;
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            dt.ItemsSource = DataBase.Instance.showTransporter(Name, Phone);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            trans = (Transporter)dt.SelectedItem;
            if (trans != null)
            {
                if (dt.SelectedCells.Count == rowAmmount)
                {
                    InputTransporterDialog dialog = new InputTransporterDialog(false);
                    dialog.NName = trans.NName;
                    dialog.State = trans.State;
                    dialog.Post_code1 = "" + trans.Post_code[0] + trans.Post_code[1];
                    dialog.Post_code2 = "" + trans.Post_code[3] + trans.Post_code[4] + trans.Post_code[5];
                    dialog.Flat_number = trans.Flat_number;
                    dialog.House_number = trans.House_number;
                    dialog.ID = trans.Id_transporter;
                    dialog.City = trans.City;
                    dialog.Phone_number = trans.Phone_number;
                    if (dialog.ShowDialog() == true)
                    {
                        dt.ItemsSource = DataBase.Instance.showTransporter(Name, Phone);
                        MessageBox.Show("Zapisano dane firmy transportowej");
                    }
                }
                else
                {
                    MessageBox.Show("Zaznaczono więcej niż jeden wiersz.");
                }
            }
            else
            {
                MessageBox.Show("Nie zaznaczono żadnego wiersza.");
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            InputTransporterDialog dialog = new InputTransporterDialog(true);
            if (dialog.ShowDialog() == true)
            {
                dt.ItemsSource = DataBase.Instance.showTransporter(Name, Phone);
                MessageBox.Show("Dodano nową firmę transportową");
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.ItemsSource = DataBase.Instance.showTransporter(NName, Phone);
        }
    }
}
