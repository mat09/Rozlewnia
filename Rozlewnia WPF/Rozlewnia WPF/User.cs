using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozlewnia_WPF
{
    /*  Pattern : Singleton
         *  0 - no users exist
         *  1 - admin
         *  2 - stockman storage
         *  3 - stockman booting
    */
    class User
    {

        private Boolean interfaceLock = false;
        private Dictionary<String, object> data;
        private int giveBootleCount;

        static private User instance;
        static private Boolean logIN;
        static private int who;


        static public User Instance
        {
            get
            {
                if (instance == null)
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
        private User()
        {
            logIN = false;
        }

        // 1 - true
        // 0 - false
        // -1 - juz jest ktos zalogowany
        public int login(String login, String password)
        {
            if (!logIN)
            {
                data = DataBase.Instance.login(login, password);
                who = Convert.ToInt16(data["who"]);
                if (who != 0)
                {
                    giveBootleCount = 0;
                    logIN = true;
                    DataBase.Instance.insertSession(Convert.ToInt16(data["id_user"]),true);
                    return 1;
                }
                else return 0;
            }
            else
            {
                return -1;
            }
        }
        public void logout()
        {
            if (logIN)
            {
                logIN = false;
                who = 0;
                instance = null;
                DataBase.Instance.insertSession(Convert.ToInt16(data["id_user"]),false);
                data = null;
                giveBootleCount = 0;
            }
        }
        public int tellMeWho()
        {
            return who;
        }
        public void interfaceBlock() { }
        public void addBootleCount()
        {
            giveBootleCount++;
        }

        public class Stockman : User
        {
            public void addBootleCount()
            {
                giveBootleCount++;
            }
        }

        public class StockmanStorage : Stockman
        {
        }

        public class StockmanBooting : Stockman
        {
        }
        public class Admin : User
        {
            public Admin() { }
        }
    }


}
