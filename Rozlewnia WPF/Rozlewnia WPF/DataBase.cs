using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Rozlewnia_WPF
{
    /*   Pattern:Singleton
    
     *      SESSION - ERROR
     *          1 = uzytkownik nie zakonczyl poprawnie sesji
     * 
     * 
     */
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

       private bool static_query(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error:" + ex.ToString());
                return false;
            }
            return true;
        }


        /* returned values:
         *  0 - no users exist
         *  1 - admin
         *  2 - stockman storage
         *  3 - stockman booting
         */

      /*  public Iterator<Bootle> InitBootle() // DONE
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
        */
        public Dictionary<String, object> login(String login, String pass)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM USERS WHERE login='" + login + "' and password='" + pass + "'", sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
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
            result.Dispose();
            result.Close();
            return dict;

        }
        internal void insertSession(int id_user , bool IN)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT id_session FROM SESSION WHERE id_user=" + id_user + " AND logout is NULL", sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            Boolean flaga = true;
            List<int> list = new List<int>();

            while (result.Read())
            {
                list.Add(result.GetInt16(0));
            }
            result.Dispose();
            result.Close();
            list.Reverse();
            int i=0;
            while ( i < list.Count)
            {
                if (IN)
                {
                    static_query("UPDATE SESSION  SET error=1 , logout=current_timestamp() WHERE id_session=" + list[i]);
                }
                else if (flaga)
                {
                    flaga = false;
                    static_query("UPDATE SESSION  SET logout=current_timestamp() WHERE id_session=" + list[i]);
                }
                else
                {
                    static_query("UPDATE SESSION  SET error=1 , logout=current_timestamp() WHERE id_session=" + list[i]);
                }
                i++;
            }
            if (IN)
            {
                static_query("INSERT INTO SESSION VALUES (NULL," + id_user + ",current_timestamp() , NULL , 0 );");
            }

        }
    }

}
