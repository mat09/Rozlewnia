using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;


namespace Rozlewnia
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
            return cmd.ExecuteReader();
        }



        /* returned values:
         *  0 - no users exist
         *  1 - admin
         *  2 - stockman storage
         *  3 - stockman booting
         */
        public int login(String login, String pass)
        {
            MySqlDataReader result = query("SELECT who FROM USERS WHERE login='" + login + "' and password='" + pass + "'");
            int r = 0;
            if (result.Read())
                r = result.GetInt16(0);
            return r;

        }

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

        static private User user;
        static private Boolean logIN = false;
        static private int who;

        public User User
        {
            get
            {
                if (user == null && logIN)
                {
                    switch (who)
                    {
                        case 1: user = new Admin(); break;
                        case 2: user = new StockmanStorage(); break;
                        case 3: user = new StockmanBooting(); break;
                    }
                }
                return user;
            }
        }


        static public void login(String login, String password)
        {
            DataBase.Instance.connect();
            who = DataBase.Instance.login(login, password);
            if (who != 0)
            {
                logIN = true;
                // zapisanie do tabeli sesja 
            }
        }
        static public void logout()
        {
            if (logIN)
            {
                logIN = false;
                who = 0;
                user = null;
                //zapis do tableli sesja
            }
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





    class MENU
    {
        private int what;
        private int i;

        public MENU()
        {
            i = 0;
        }

        public void show()
        {
            Console.Clear();
            Console.WriteLine("===== MENU ====" + i + "==\n BUTLE [1]\nKoniec [0]\n");
            what = Convert.ToInt16(Console.ReadLine());
            i++;
            switch (what)
            {
                case 0: break;
                case 1: showButle(); break;
                default: show(); break;
            }
        }

        public void showButle()
        {
            Console.Clear();
            Console.WriteLine("======= BUTLE ======\n[1] Wyswietl Butle\n[2] Dodaj Butle\n[0] Wstecz");
            what = Convert.ToInt16(Console.ReadLine());
            switch (what)
            {
                case 1: break;
                case 0: show(); break;
            }
        }

    }





    class Program
    {
        static void Main(string[] args)
        {
            User user = new User();
            user.login("admin", "haslo");
            Console.ReadLine();
        }
    }

}

