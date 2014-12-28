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
        
        private String name;
        private String surname;
        private String state;
        private String flat_number;
        private String house_number;
        private String post_code1;
        private String post_code2;
        private String city;
        private String phone_number;
        
        private String id;


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
                Name = "";
                Surname = "";
                City = "";
                State = "";
                Post_code1 = "";
                Post_code2 = "";
                Flat_number = "";
                House_number = "";
                Phone_number = "";
                id = "null";
            }
            else
            {
                actStrButton = "Zapisz dane klienta";
            }
            this.DataContext = this;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if ((NName.Length > 0) && (Surname.Length > 0) & ( City.Length>0)  && (State.Length > 0) && (Postcode_ALL.Length == 6) && House_number.Length>0  && Phone_number.Length>0 )
            {
                String flat="null";
                if (Flat_number.Length>0)
                    flat = Filter.sql(Flat_number);
                // result to jest id zmienionego lub utworzonego clienta procedura client, jesli result == 0 to znaczy ze jest juz taki client (imie i nazwisko)
                int result = DataBase.Instance.call_client(id, Filter.sql(NName),Filter.sql(Surname),Filter.sql(City),Filter.sql_with_space(State),Filter.sql(House_number),flat,Filter.sql(Postcode_ALL),Filter.sql(Phone_number));
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
                    MSG.Visibility = Visibility.Visible;
                    MSG.Content = "Błedne dane";
                }
            }
            else
            {
                MSG.Visibility = Visibility.Visible;
                MSG.Content = "Uzupełnij wszystkie pola";
            }
        }


        public String ID
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        public String NName
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
                return this.surname;
            }
            set
            {
                surname = value;
            }
        }
        public String City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        public String State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        public String Post_code1
        {
            get
            {
                return post_code1;
            }
            set
            {
                post_code1 = value;
            }
        }
        public String Post_code2
        {
            get
            {
                return post_code2;
            }
            set
            {
                post_code2 = value;
            }
        }
        public String Postcode_ALL
        {
            get
            {
                return post_code1 + "-" + post_code2;
            }
        }
        public String Flat_number
        {
            get
            {
                return flat_number;
            }
            set
            {
                flat_number = value;
            }
        }
        public String House_number
        {
            get
            {
                return house_number;
            }
            set
            {
                house_number = value;
            }
        }
        public String Phone_number
        {
            set
            {
                phone_number = value;
            }
            get
            {
                return phone_number;
            }
        }
    
    }
}
