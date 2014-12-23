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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Rozlewnia_WPF
{

    class Transporter
    {
        public Transporter() { }
    }
    class Client
    {
        private String name;
        private String surname;
        private int id;
        private int id_butli;   // tu bedziee jakas kolekcja generyczna lub iterator lub jakis multiton....


        public Client() { }

    }
    //    Pattern::  Iterator
    class Iterator<X> where X : class, new()
    {

        private int count;
        private int current;

        public int Current
        {
            get { return current; }
            set { current = value; }
        }
        private List<X> list;

        public Iterator()
        {
            count = 0;
            current = 0;
            list = new List<X>();
        }
        public X last()
        {
            return list.Last();
        }
        public void add(X obj)
        {
            list.Add(obj);
            count++;
        }
        public X getId(int id)
        {
            if (id <= count)
                return list[id];
            else
                return null;
        }
        public X getCurrent()
        {
            return list[current];
        }
        public Boolean next()
        {
            if (current < count)
            {
                current++;
                return true;
            }
            else return false;
        }
        public void first()
        {
            current = 0;
        }
        public void remove(X obj)
        {
            list.Remove(obj);
            current = 0;
            count--;
        }
        public Boolean hasNext()
        {
            if (current < count)
                return true;
            else
                return false;
        }

    }
    class Bootle
    {
        private int id_bootle;
        private int id_client;
        private int flag;
        //id_bootle int(6)    id_client int(6) 

        public Bootle() { }
        public Bootle(int id_b, int id_c)
        {
            id_bootle = id_b;
            id_client = id_c;
        }

    }




   
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if( loginBox.Text.Length==0 || passwordBox.Password.Length==0 )
            {
                msg.Content = "Uzupełnij wszystkie pola";    
                msg.Visibility = Visibility.Visible;
            }
            else
            {
                if (User.Instance.login(loginBox.Text, passwordBox.Password)==1)
                {
                    switch (User.Instance.tellMeWho())
                    {
                        case 1: break; // admin
                        case 2: Window wi = new StockmanStorageWindow(); wi.Show(); this.Close(); break; // STORAGE
                        case 3: break; // booting
                        default: break;
                    }
                }
                else
                {
                    msg.Content = "Błedny login lub hasło";
                    msg.Visibility = Visibility.Visible;
                }
            }

        }

    }
}
