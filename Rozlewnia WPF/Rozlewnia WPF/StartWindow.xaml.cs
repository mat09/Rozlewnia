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

    //   Pattern:Singleton
    class DataBase
    {

        static private DataBase instance;
        private MySqlConnection sqlCon;
        private MySqlConnectionStringBuilder strCon;

        private DataBase()
        {

            strCon = new MySqlConnectionStringBuilder();
            strCon.Server = "localhost";
            strCon.UserID = "root";
            strCon.Password = "hasloRozlewnia";
            strCon.Database = "rozlewnia";
            sqlCon = new MySqlConnection(strCon.ToString());
            connect();
        }

        static public DataBase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataBase();
                }
                return instance;
            }
        }
        public void connect()
        {

            try
            {
                    sqlCon.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }

        }
        public void close()
        {
            try
            {
                sqlCon.Close();
                instance = null;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
        }

        private MySqlDataReader query(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            MySqlDataReader resu=null;
            try 
            {
                resu = cmd.ExecuteReader();
            }
            catch (MySqlException ex){
                Console.WriteLine("Error:" + ex.ToString());
            }
            return resu;
        }



        /* returned values:
         *  0 - no users exist
         *  1 - admin
         *  2 - stockman storage
         *  3 - stockman booting
         */
        
        public Iterator<Bootle> InitBootle() // DONE
        {
            MySqlDataReader result = query("SELECT * FROM BOOTLE;");
            Iterator<Bootle> list = new Iterator<Bootle>();

            while (result.Read())
            {
                list.add(new Bootle(result.GetInt16("id_bootle"), result.GetInt16("id_client")));
            }

            return list;
        }
        public Iterator<Client> InitClient() // TO DO
        {
            MySqlDataReader result = query("SELECT * FROM CLIENT;");
            Iterator<Client> list = new Iterator<Client>();

            while (result.Read())
            {
                list.add(new Client());
            }
            return list;
        }
        public Iterator<Transporter> InitTransporter()// TO DO
        {
            MySqlDataReader result = query("SELECT * FROM TRANSPORTER;");
            Iterator<Transporter> list = new Iterator<Transporter>();

            while (result.Read())
            {
                list.add(new Transporter());
            }

            return list;
        }

        public Dictionary<String, object> login(String login, String pass)
        {
            MySqlDataReader result = query("SELECT * FROM USERS WHERE login='" + login + "' and password='" + pass + "'");
            Dictionary<String, object> dict = new Dictionary<String, object>();
            dict.Add("who", 0);

            if (result.Read())
            {
                dict.Add("id_user", result.GetInt16(0));
                dict["who"] = result.GetInt16(1);
                dict.Add("login", result.GetString(2));
                dict.Add("name", result.GetString(3));
                dict.Add("surname", result.GetString(4));
            }
            result.Close();
            return dict;

        }
        internal void insertSession(int  id_user)
        {
            MySqlDataReader result = query("SELECT id_session FROM SESSION WHERE id_user=" + id_user+" AND logout is NULL");
            Boolean flaga = true;
            while (result.Read())
            {
                if(flaga)
                {
                    flaga= false;
                    int id_session = result.GetInt16(0);
                    result.Close();
                    query("UPDATE SESSION  SET logout=current_timestamp() WHERE id_session=" + id_session);
                }
                else 
                {
                    // tzn ze istnieja niezamkniete sesje uzytkownika 
                    // metoda ktora zamyka niezakmniete sesje uzytkownika
                }
                
            }
            if (flaga)
            {
                result.Close();
                query("INSERT INTO SESSION VALUES (NULL,"+id_user+",login=current_timestamp() , NULL);");
            }

        }
    }

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
    //  Pattern : Singleton
    class User
    {

        private Boolean interfaceLock = false;
        private Dictionary<String, object> data;

        static private User instance;
        static private Boolean logIN = false;
        static private int who;
        

        static public User Instance
        {
            get
            {
                if (instance == null )
                {
                    if (logIN)
                    {
                        switch (who)
                        {
                            case 1: instance = new Admin(); break;
                            case 2: instance = new StockmanStorage(); break;
                            case 3: instance = new StockmanBooting(); break;
                            default: break;
                        }
                    }
                    else
                        instance = new User();
                }
                return instance;
            }
        }


        public void login(String login, String password)
        {
            logout();
            data = DataBase.Instance.login(login, password);
            if ( Convert.ToInt16(data["who"]) != 0)
            {
                logIN = true;
                who = Convert.ToInt16(data["who"]);
                DataBase.Instance.insertSession(Convert.ToInt16(data["id_user"]));
            }
        }
        public void logout()
        {
            if (logIN)
            {
                logIN = false;
                who = 0;
                instance = null;
                DataBase.Instance.insertSession(Convert.ToInt16(data["id_user"]));
                data = null;
            }
        }
        public int tellMeWho()
        {
            return who;  
        }
        public void interfaceBlock() { }


        private class Stockman : User
        {
        }

        private class StockmanStorage : Stockman
        {
        }

        private class StockmanBooting : Stockman
        {
        }
        private class Admin : User
        {
            public Admin() { }
        }
    }



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                MessageBox.Show("login to :" + loginBox.Text + "   haslo:" + passwordBox.Password);
            }
        }




    }
}
