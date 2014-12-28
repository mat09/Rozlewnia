using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class UserAdd_EditDialog : Window
    {
        private ConfigurationData data;
        private bool add;
        public UserAdd_EditDialog(bool ADD)
        {
            add = ADD;
            data = new ConfigurationData();
            if (add)
            {
                data.ActStrButton = "Dodaj użytkownika";
                data.Who = 1;
            }
            else
            {
                data.ActStrButton = "Zapisz użytkownika";
            }
            InitializeComponent();
            this.DataContext = data;
        }

       private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if ( (MyName.Length>0) && ( Surname.Length>0)  && (Login.Length>0) && (Password.Length>0) ){
                if (DataBase.Instance.call_user("null", Who, Login.Trim(), Password.Trim(), MyName.Trim(), Surname.Trim()))
                {
                    if (add)
                    MessageBox.Show("Dodano uzytkownika");
                    else
                    MessageBox.Show("Zapisano uzytkownika");
                    this.DialogResult = true;
                }
                else
                {
                    msg.Content = "Błąd - wprowadziłeś niepoprawne dane";
                    msg.Visibility = Visibility.Visible;
                }
            }
            else
            {
                msg.Content = "Uzupełnij wszystkie pola";
                msg.Visibility = Visibility.Visible;
            }
        }

        public String MyName
        {
            get { return name.Text; }
        }
        public String Surname
        {
            get { return surname.Text; }
        }
        public String Login
        {
            get { return login.Text; }
        }
        public String Password
        {
            get { return password.Text;}
        }
        public int Who
        {
            get { return data.Who; }
        }


    }
    public class whoToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                             object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            int integer = (int)value;
            if (integer == int.Parse(parameter.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return parameter;
        }
    }

    public class ConfigurationData : INotifyPropertyChanged
    {
        int who;
        String actStrButton;

        public String ActStrButton
        {
            get { return actStrButton; }
            set { actStrButton = value; OnPropertyChanged("ActStrButton"); }
        }

        public int Who
        {
            get { return who; }
            set { who = value; OnPropertyChanged("Who"); }
        }

        //below is the boilerplate code supporting PropertyChanged events:
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
