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

        internal bool call_user(string id , int who ,string p1, string p2, string p3, string p4)
        {
            return static_query("CALL user(" + id + "," + who + ",'" + p1 + "','" + p2 + "','" + p3 + "','" + p4 + "')");
        }
        internal int  call_client(  string id_client ,string name,string surname, string city,  string street,string house_number,string flat_number,string post_code , string phone_number)
        {
            //System.Windows.MessageBox.Show("CALL client(" + id_client + ",'" + name + "','" + surname + "','" + city + "','" + street + "'," + house_number + "," + flat_number + ",'" + post_code + "','" + phone_number + "')");
            MySqlCommand cmd = new MySqlCommand("CALL client(" + id_client + ",'" + name + "','" + surname + "','" + city + "','" + street + "','" + house_number + "','" + flat_number + "','" + post_code + "','" + phone_number + "')", sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            int id = 0;
            if (result.Read())
                id = result.GetInt16(0);
            result.Close();
            return id; 
        }
        internal bool call_attachBootle(String ID, int id_client)
        {
            return static_query("CALL attachBootle("+id_client+","+ID+")");
        }
        internal bool call_statusBootle(string ID, int status)
        {
            MySqlCommand cmd = new MySqlCommand("CALL statusBootle(" + ID + "," + status + ")", sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            bool ret = false;
            if (result.Read())
            {
                System.Windows.MessageBox.Show("a:" + result.GetInt16(0) + "  a2:" + result.GetInt16(1));
                if (result.GetInt16(0) == 1 && result.GetInt16(1)==1 )
                    ret=true;
            }
            result.Close();
            return ret;
        }
        internal bool call_deleteBootle(string p)
        {
            System.Windows.MessageBox.Show("a:" + p);

            MySqlCommand cmd = new MySqlCommand("CALL deleteBootle(" + p + ")", sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            bool ret = false;
            if (result.Read())
            {
                System.Windows.MessageBox.Show("a:" + result.GetInt16(0) + "  a2:" + result.GetInt16(1));
                if (result.GetInt16(0) == 1 && result.GetInt16(1) == 0)
                    ret = true;
            }
            result.Close();
            return ret;
        }
        internal int  call_transporter(string id_transporter, string name, string city, string street, string house_number, string flat_number, string post_code, string phone_number)
        {
            //System.Windows.MessageBox.Show("CALL client(" + id_client + ",'" + name + "','" + surname + "','" + city + "','" + street + "'," + house_number + "," + flat_number + ",'" + post_code + "','" + phone_number + "')");
            MySqlCommand cmd = new MySqlCommand("CALL transporter(" + id_transporter + ",'" + name + "','" + city + "','" + street + "','" + house_number + "','" + flat_number + "','" + post_code + "','" + phone_number + "')", sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            int id = 0;
            if (result.Read())
                id = result.GetInt16(0);
            result.Close();
            return id;
        }
        
        /* metoda sluzy do dodawani transportu(gdy user zamowi transpor)
             *                  wtedy :
             *                          doda nowy wiersz w tabeli transport
             *                          zmieni status wszytskich butli jakie wybrano na 2
             *                          wstawi wszystkie butle do tabeli IN_TRANS (powiarze butle z konkretnym transportem)
             *                  aby dodac transport trzeba wpisac w wywolaniu id_trans=null bo wpis transport dopiero powstanie na podstawie wybranego transportera
         *              lub do edycji trasnpostu ( gdy user np kliknie wyslij transport)
             *                  wtedy:
             *                          doda do transportu data_start
             *                          zmieni status butli powiazanych z transportem na 3
             *                  aby ja wywolac trzeba wpiszac w wywolaniu id_trasnporter=null bo nie interesuje nas transporter tylko transport
         *              lub do dodawania butli czekajacych na transport do transportu
             *                  wtedy:
             *                          zmienia status butli na 2
             *                          dodaje bulte do in_tran
             *                  aby ja wywolac trzeba ustawic w wywolaniu add_bootle=true
         */
        internal bool changeTransport(List<searchBootleClass> bootle, String id_transporter, int status, String id_trans,bool add_bootle,String data)
        {

            if (id_trans == null && id_transporter != null && add_bootle == false && data == null)//dodawanie nowego transportu
            {
                MySqlTransaction transaction = null;
                MySqlCommand cmd = null;
                int id=new int();

                try
                {
                    MySqlDataReader result;
                    transaction = sqlCon.BeginTransaction();
                    cmd = new MySqlCommand();
                    cmd.Connection = sqlCon;
                    cmd.Transaction = transaction;


                    cmd.CommandText = "INSERT INTO TRANSPORT VALUES (null," + id_transporter + ",1,null,null,now())";
                    cmd.ExecuteNonQuery();
                   

                    cmd.CommandText = "SELECT last_insert_id()";
                    result = cmd.ExecuteReader();
                    if (result.Read())
                        id = result.GetInt16(0);
                    result.Close();
                    foreach (searchBootleClass bo in bootle)
                    {
                        cmd.CommandText = "UPDATE BOOTLE SET status=2 WHERE ID="+bo.ID;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO IN_TRANS VALUES(" + bo.ID + "," + id + ")";
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (MySqlException ex)
                {
                    try
                    {
                        transaction.Rollback(); 

                    }
                    catch (MySqlException ex1)
                    {
                        System.Windows.MessageBox.Show("Error1: "+ ex1.ToString());
                    }

                    System.Windows.MessageBox.Show("Error2: "+ ex.ToString());
                    return false;
                }
            }
            if( id_trans != null && id_transporter==null && add_bootle==false && data!=null )  // wysylanie i odnieranie transportu
            {
                MySqlTransaction transaction = null;
                MySqlCommand cmd = null;
                
                try
                {
                    transaction = sqlCon.BeginTransaction();
                    cmd = new MySqlCommand();
                    cmd.Connection = sqlCon;
                    cmd.Transaction = transaction;


                    cmd.CommandText = "UPDATE TRANSPORT SET "+data+"=now() WHERE id_trans=" + id_trans;
                    cmd.ExecuteNonQuery();
                    foreach (searchBootleClass bo in bootle)
                    {
                        cmd.CommandText = "UPDATE BOOTLE SET status="+status+" WHERE ID=" + bo.ID;
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (MySqlException ex)
                {
                    try
                    {
                        transaction.Rollback();

                    }
                    catch (MySqlException ex1)
                    {
                        System.Windows.MessageBox.Show("Error1: " + ex1.ToString());
                    }

                    System.Windows.MessageBox.Show("Error2: " + ex.ToString());
                    return false;
                }
            }
            if (id_trans != null && id_transporter == null && add_bootle == true && data == null) // dodawnanie butli do zamowionego transportu
            {
                MySqlTransaction transaction = null;
                MySqlCommand cmd = null;
                
                try
                {
                    transaction = sqlCon.BeginTransaction();
                    cmd = new MySqlCommand();
                    cmd.Connection = sqlCon;
                    cmd.Transaction = transaction;

                    foreach (searchBootleClass bo in bootle)
                    {
                        cmd.CommandText = "UPDATE BOOTLE SET status=2 WHERE ID=" + bo.ID;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO IN_TRANS VALUES(" + bo.ID + "," + id_trans + ")";
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (MySqlException ex)
                {
                    try
                    {
                        transaction.Rollback();

                    }
                    catch (MySqlException ex1)
                    {
                        System.Windows.MessageBox.Show("Error1: " + ex1.ToString());
                    }

                    System.Windows.MessageBox.Show("Error2: " + ex.ToString());
                    return false;
                }
            }
            return false;
        }


        internal List<searchBootleClass> searchBootle(String name, String surname, String ID,int status,int id_trans)
        {
            List <searchBootleClass> list = new List<searchBootleClass>();
            String query=null;
            if (id_trans == -1) 
            {
                query = "SELECT ID , name ,surname FROM BOOTLE, CLIENT WHERE client.id_client = bootle.id_client ";
                if (name != "" && name != null) query += " AND client.name='" + name + "'";
                if (surname != "" && surname != null) query += " AND client.surname='" + surname + "'";
                if (ID != "" && ID != null) query += " AND bootle.ID=" + ID;
                if (status != -1) query += " AND bootle.status=" + status;
            }
            else
            {
                query="SELECT DISTINCT BOOTLE.ID ,CLIENT.name,CLIENT.surname FROM BOOTLE,CLIENT,TRANSPORT,IN_TRANS WHERE BOOTLE.ID=IN_TRANS.ID AND IN_TRANS.id_trans ="+id_trans+" and BOOTLE.id_client=CLIENT.id_client";
            }

            
            System.Windows.MessageBox.Show(query);
            
            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                list.Add(new searchBootleClass() { ID = result.GetInt32(0), Name = result.GetString(1), Surname = result.GetString(2) });
            }
            result.Close();
            return list; 
        }
        internal List<Client> showClient(String name,String surname)
        {
            List<Client> list = new List<Client>();
            String query = "SELECT client.id_client , name ,surname , state , house_number , flat_number , city , post_code , phone_number FROM CONTACT, CLIENT WHERE client.id_contact = contact.id_contact ";
            if (name != "" && name != null) query += " AND client.name='" + name + "'";
            if (surname != "" && surname != null) query += " AND client.surname='" + surname + "'";
            
            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                
                    list.Add(new Client()
                    {
                        Id_client = result.GetString(0),
                        NName = result.GetString(1),
                        Surname = result.GetString(2),
                        State = result.GetString(3),
                        House_number = result.GetString(4),
                        Flat_number = result.GetString(5),
                        City = result.GetString(6),
                        Post_code = result.GetString(7),
                        Phone_number = result.GetString(8)
                    }
                    );
            }
            result.Close();
            return list;
        }
        internal List<editBootleClass> editBootle(string ID)
        {
            List<editBootleClass> list = new List<editBootleClass>();
            String query = "SELECT ID , status FROM BOOTLE WHERE id_client ="+ID;
            
            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {

                list.Add(new editBootleClass()
                {
                    Status = result.GetString(1),
                    ID = result.GetString(0)
                }
                );
            }
            result.Close();
            return list;
        }
        internal List<Transporter> showTransporter(String Name , String Phone)
        {
            List<Transporter> list = new List<Transporter>();
            String query = "SELECT id_transporter , name  , state , house_number , flat_number , city , post_code , phone_number FROM CONTACT, TRANSPORTER WHERE transporter.id_contact = contact.id_contact ";
            if (Name != "" && Phone != null) query += " AND transporter.name='" + Name + "'";
            if (Phone != "" && Phone != null) query += " AND contact.phone_number='" + Phone + "'";

            System.Windows.MessageBox.Show(query);
            
            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {

                list.Add(new Transporter()
                {
                    Id_transporter = result.GetString(0),
                    NName = result.GetString(1),
                    State = result.GetString(2),
                    House_number = result.GetString(3),
                    Flat_number = result.GetString(4),
                    City = result.GetString(5),
                    Post_code = result.GetString(6),
                    Phone_number = result.GetString(7)
                }
                );
            }
            result.Close();
            return list;

        }
        internal List<Transport> showTransport(int type)
        {
            List<Transport> list = new List<Transport>();
            String query = null;
            if(type == 1)
                query = "SELECT transport.id_trans ,transporter.name,contact.phone_number,transport.data_ordered FROM TRANSPORT,TRANSPORTER,CONTACT WHERE TRANSPORT.id_transporter=TRANSPORTER.id_transporter AND data_start is null AND contact.id_contact=transporter.id_contact AND transport.type=1";
            if(type == 2 )
                query = "SELECT transport.id_trans ,transporter.name,contact.phone_number,transport.data_start FROM TRANSPORT,TRANSPORTER,CONTACT WHERE TRANSPORT.id_transporter=TRANSPORTER.id_transporter AND data_end is null AND data_start is not null AND contact.id_contact=transporter.id_contact AND transport.type=2";


            MySqlCommand cmd = new MySqlCommand(query, sqlCon);
            MySqlDataReader result = cmd.ExecuteReader();
            if(type == 1)
            while (result.Read())
            {
                list.Add(new Transport()
                {
                    Id_transport = result.GetString(0),
                    NName = result.GetString(1),
                    Phone_number = result.GetString(2),
                    Data_ordered= result.GetString(3)
                }
                );
            }
            else if (type == 2)
            {
                while (result.Read())
                {
                    list.Add(new Transport()
                    {
                        Id_transport = result.GetString(0),
                        NName = result.GetString(1),
                        Phone_number = result.GetString(2),
                        Data_start = result.GetString(3)
                    }
                    );
                }
            }
            result.Close();
            return list;
        }
    }
}
