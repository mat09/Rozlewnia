using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Rozlewnia_WPF
{
    class Classes
    {
    }
    //dfgs
    class Filter
    {

        static public String sql(String str)
        {
            return str.Trim();
        }
        static public String sql_with_space(String str)
        {
            return str;
        }
        static public String sql_number(String str)
        {
            return str;
        }

    }

    class searchBootleClass : INotifyPropertyChanged
    {
        private String surname;
        public String Surname
        {
            get { return surname; }
            set { surname = value;
                  this.NotifyPropertyChanged("Surname");
            }
        }
        private String name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.NotifyPropertyChanged("Name");
            }
        }
        private int id;
        public int ID
        {
            get { return id; }
            set 
            { 
                id = value;
                this.NotifyPropertyChanged("ID");
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
    class editBootleClass
    {
        private String id;
        private String status;

        public String ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public String Status
        {
            get
            {
                switch (int.Parse(status))
                {
                    case 0 : return "u klienta";
	                case 1 : return "czeka na zamowienie transportu do rozlewni";
	                case 2 : return "czeka na transport do rozlewni";
	                case 3 : return "w transporcie do rozlewni";
	                case 4 : return "w rozlewni ,czeka na napelnienie";
	                case 5 : return "napelniona czeka na zamowienie trasnsport do magazynu";
	                case 6 : return "w transporcie do magazynu";
                    case 7 : return "czeka na odebranie przez klienta";
                }
                return null;
            }
            set
            {
                status = value;
            }
        }
    }

    class Transporter
    {
        private String id_transporter;
        public String Id_transporter
        {
            get { return id_transporter; }
            set { id_transporter = value; }
        }

        private String name;
        public String NName
        {
            get { return name; }
            set { name = value; }
        }

        private String state;
        public String State
        {
            get { return state; }
            set { state = value; }
        }

        private String house_number;
        public String House_number
        {
            get { return house_number; }
            set { house_number = value; }
        }

        private String flat_number;
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

        private String city;
        public String City
        {
            get { return city; }
            set { city = value; }
        }

        private String post_code;
        public String Post_code
        {
            get { return post_code; }
            set { post_code = value; }
        }

        private String phone_number;
        public String Phone_number
        {
            get { return phone_number; }
            set { phone_number = value; }
        }
        
    }
    class Client
    {
        private String id_client;
        public String Id_client
        {
            get { return id_client; }
            set { id_client = value; }
        }

        private String name;
        public String NName
        {
            get { return name; }
            set { name = value; }
        }
        
        private String surname;
        public String Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        
        private String state;
        public String State
{
  get { return state; }
  set { state = value; }
}

        private String house_number;
        public String House_number
{
  get { return house_number; }
  set { house_number = value; }
}

        private String flat_number;
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

        private String city;
        public String City
        {
          get { return city; }
          set { city = value; }
        }
       
        private String post_code;
        public String Post_code
        {
          get { return post_code; }
          set { post_code = value; }
        }

        private String phone_number;
        public String Phone_number
{
  get { return phone_number; }
  set { phone_number = value; }
}
        
    }
    class Transport
    {
        private String name;
        public String NName
        {
            get { return name; }
            set { name = value; }
        }

        private String data_ordered;
        public String Data_ordered
        {
            get
            {
                return data_ordered;
            }
            set
            {
                data_ordered = Data_ordered;
            }
        }

        private String data_start;

        public String Data_start
        {
            get { return data_start; }
            set { data_start = value; }
        }
        private String data_end;

        private String id_transport;
        public String Id_transport
{
  get { return id_transport; }
  set { id_transport = value; }
}

        private String phone_number;
        public String Phone_number
{
  get { return phone_number; }
  set { phone_number = value; }
}

        
    }

    // nieprzydatne poki co :|
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





}
