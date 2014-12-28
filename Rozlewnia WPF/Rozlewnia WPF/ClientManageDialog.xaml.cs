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
    /// Interaction logic for showClientDialog.xaml
    /// </summary>
    public partial class showClientDialog : Window
    {
        static int rowAmmount = 9;
        Client client;
        private String name;
        private String surname;

        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public String Surname
        {
            get
            {
                return surname;

            }
            set
            {
                surname = value;
            }
        }

        public showClientDialog()
        {
            InitializeComponent();
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            this.DataContext = this;
            dt.ItemsSource = DataBase.Instance.showClient(Name, Surname);
        }

        private void EditClient(object sender, RoutedEventArgs e)
        {
            client = (Client)dt.SelectedItem;
            if (client != null)
            {
                if (dt.SelectedCells.Count == rowAmmount)
                {
                    InputClientDialog dialog = new InputClientDialog(false);
                    dialog.NName = client.NName;
                    dialog.Surname = client.Surname;
                    dialog.State = client.State;
                    dialog.Post_code1 = ""+client.Post_code[0]+client.Post_code[1];
                    dialog.Post_code2 = "" + client.Post_code[3] + client.Post_code[4] + client.Post_code[5];
                    dialog.Flat_number = client.Flat_number;
                    dialog.House_number = client.House_number;
                    dialog.ID = client.Id_client;
                    dialog.City = client.City;
                    dialog.Phone_number = client.Phone_number;
                    if (dialog.ShowDialog() == true)
                    {
                        dt.ItemsSource = DataBase.Instance.showClient(Name, Surname);
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

        private void AddClient(object sender, RoutedEventArgs e)
        {
            InputClientDialog dialog = new InputClientDialog(true);
            if (dialog.ShowDialog() == true)
            {
                dt.ItemsSource = DataBase.Instance.showClient(Name, Surname);
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.ItemsSource = DataBase.Instance.showClient(Name,Surname);
        }

        private void EditBootle(object sender, RoutedEventArgs e)
        {
            client = (Client)dt.SelectedItem;
            if (client != null)
            {
                if (dt.SelectedCells.Count == rowAmmount)
                {
                    editBootleDialog dialog = new editBootleDialog(client.Id_client);
                    if (dialog.ShowDialog() == true)
                    {
                        dt.ItemsSource = DataBase.Instance.showClient(Name, Surname);
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
    }
}
