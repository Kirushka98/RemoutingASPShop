using OTCHET;
using System;
using System.Data.OleDb;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Text;


namespace Obj
{

    public delegate void Del(string s);

    public class UObj : MarshalByRefObject, Interface1
    {

        public event Del even;
        string type_serv;
        public int x = 0, y = 0, z = 0;
        public bool d = false, f = false, g = false;
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD.mdb;";
        private OleDbConnection myConnection;
        RSACryptoServiceProvider RSA;

        public void Connection() //подключение к БД
        {
            try { myConnection.Close(); }
            catch { }
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            //myConnection
        }
        //добавление нового сервера
        public bool Add_serv(string IP, string Type)
        {
            try
            {
                if (IP.Length < 13 || Type == null)
                    throw new Exception("ВВедите ip и тип правильно");
                connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD.mdb;";
                Connection();
                string query = "INSERT INTO Таблица1 (IP, Type) VALUES('" + IP + "','" + Type + "')";
                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, myConnection);
                // выполняем запрос к MS Access
                if (command.ExecuteNonQuery() == 0)
                    return false;
                else return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //поиск и запись арендованного сервера
        public string Prihod(string Type_serv, DateTime Exp_date)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD.mdb;";
            Connection();
            string IP;
            string query = "SELECT IP from Таблица1 WHERE Type = '" + Type_serv + "' AND Zanyat = 0";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                IP = reader.GetString(0);
                query = "Update Таблица1 SET Zanyat= 1, Buy_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', Expire_date='" + Exp_date.ToString("yyyy-MM-dd") + "'where IP ='" + IP + "'";
                command = new OleDbCommand(query, myConnection);
                command.ExecuteNonQuery();

                return IP;
            }
            else return null;
        }
        //Покупка (удаление) сервера
        public string Uhod(string Type_serv)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD.mdb;";
            Connection();
            string IP;
            string query = "SELECT IP from Таблица1 WHERE Type = '" + Type_serv + "' AND Zanyat = 0";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                IP = reader.GetString(0);
                query = "DELETE FROM Таблица1 where IP ='" + IP + "'";
                command = new OleDbCommand(query, myConnection);
                command.ExecuteNonQuery();

                return IP;
            }
            else return null;
        }
        public RSAParameters Key_send()
        {
            RSA = new RSACryptoServiceProvider();
            return RSA.ExportParameters(false);
        }
        public bool Enter_serv(byte[] Login, byte[] Pas)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD1.mdb;";
            Connection();
            string login, pas;
            try
            {


                login = Encoding.UTF8.GetString(RSA.Decrypt(Login, false));
                pas = Encoding.UTF8.GetString(RSA.Decrypt(Pas, false));
            }
            catch (ArgumentNullException)
            {

                Console.WriteLine("Encryption failed.");
                return false;
            }
            OleDbDataReader reader;
            try
            {
                OleDbCommand command = new OleDbCommand("SELECT Key from Таблица1 WHERE Login = '" + login + "'", myConnection);
                reader = command.ExecuteReader();
                if (!reader.HasRows)
                    return false;
                if (reader.Read())
                {
                    if (reader.GetString(0).Equals(pas))
                        return true;
                    else return false;
                }
                else return false;
            }
            catch
            {
                return false;
            }


        }
        public bool Enter_serv(string login, string pas)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD1.mdb;";
            Connection();            
            OleDbDataReader reader;
            try
            {
                OleDbCommand command = new OleDbCommand("SELECT Key from Таблица1 WHERE Login = '" + login + "'", myConnection);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetString(0).Equals(pas))
                        return true;
                    else return false;
                }
                else return false;
            }
            catch
            {
                return false;
            }

        }

        public int Check()
        {
            try
            {
                connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BD.mdb;";
                Connection();
                string query = "Update Таблица1 SET Zanyat= 0, Buy_date = null, Expire_date=null where Expire_date < date()";
                OleDbCommand command = new OleDbCommand(query, myConnection);
                return command.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
        }




        public override Object InitializeLifetimeService() //переопределяем время жизни каналов на 300 секунд
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(300);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(45);
            }
            return lease;
        }
    }
}
