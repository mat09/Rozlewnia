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
    public partial class InputClientDialog : Window
    {
        private bool add;
        private string actStrButton;

        public string ActStrButton
        {
            get
            {
                return actStrButton;
            }
            set
            {
                actStrButton = value;
            }
        }

        public InputClientDialog(bool a)
        {
            add = a;
            InitializeComponent();

            if (add)
            {
                actStrButton = "Dodaj klienta";
            }
            else
            {
                actStrButton = "Zapisz dane klienta";
            }
            this.DataContext = this;
        
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if ((MyName.Length > 0) && (Surname.Length > 0) & ( City.Length>0)  && (Street.Length > 0) && (Postcode.Length > 0) && House_number.Length>0  && Phone_number.Length>0 )
            {
                String flat="null";
                if (Flat_number.Length>0)
                    flat = Filter.sql(Flat_number);
                // result to jest id zmienionego lub utworzonego clienta procedura client, jesli result == 0 to znaczy ze jest juz taki client (imie i nazwisko)
                int result = DataBase.Instance.call_client("null", Filter.sql(MyName),Filter.sql(Surname),Filter.sql(City),Filter.sql_with_space(Street),Filter.sql(House_number),flat,Filter.sql(Postcode),Filter.sql(Phone_number));
                if (result!=0)
                {
                    if (add)
                    {
                        this.DialogResult = true;
                            
                        if (MessageBox.Show("Chcesz przypisac butle do klienta?", "Komunikat", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            InputBootleDialog dialog = new InputBootleDialog(result);
                            if (dialog.ShowDialog() == true)
                            {
                                MessageBox.Show("Przypisano butle do klienta", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                        this.DialogResult = true;
                }
                else
                {
                    msg.Visibility = Visibility.Visible;
                    msg.Content = "Błedne dane";
                }
            }
            else
            {
                msg.Visibility = Visibility.Visible;
                msg.Content = "Uzupełnij wszystkie pola";
            }
        }



        public String MyName
        {
            get
            {
                return name.Text;
            }
        }
        public String Surname
        {
            get
            {
                return surname.Text;
            }
        }
        public String City
        {
            get
            {
                return city.Text;
            }
        }
        public String Street
        {
            get
            {
                return street.Text;
            }
        }
        public String Postcode
        {
            get
            {
                return post_code_1.Text + "-" + post_code_2.Text;
            }
        }
        public String Flat_number
        {
            get
            {
                return flat_number.Text;
            }
        }
        public String House_number
        {
            get
            {
                return house_number.Text;
            }
        }
        public String Phone_number
        {
            get
            {
                return phone.Text;
            }
        }
    
    }
}
