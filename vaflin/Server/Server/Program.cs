using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using Obj;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters;




namespace Server
{
    class Server : Form
    {
        public delegate void AddMessageDelegate(string message, string s);
        UObj obj;
        private TextBox txt, txt2;
        public int x = 0, y = 0, z = 0;

        public Server()
        {
           
            obj = new UObj();
            

            //Создание канала
            BinaryServerFormatterSinkProvider srvPrvd = new BinaryServerFormatterSinkProvider();
            srvPrvd.TypeFilterLevel = TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clntPrvd = new BinaryClientFormatterSinkProvider();
            Dictionary<string, string> proprt = new Dictionary<string, string>();
            proprt["port"] = "8086";
            TcpChannel channel = new TcpChannel(proprt, clntPrvd, srvPrvd);

            //Регистация канала
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.Configure("C:\\Users\\Kirushka\\Desktop\\vaflin\\config.config", false);
            RemotingServices.Marshal(obj, "UdOb");  // задаем удаленный объект и слово через которое будет вестись связь
           

            Create();
            System.Threading.Timer timer = new System.Threading.Timer(tick, this, 0, 3600000);
        }
        void tick(object state)
        {
          
           obj.Check();

        }
        public void Create()
        { 
            Label lab = new Label();
            lab.Location = new Point(20, 10);
            lab.Size = new Size(1000, 25);
            lab.Text = "Сервер работает";
            this.Controls.Add(lab);

        }

        

        public void AddTxt(string message, string s)
        {
            switch (s)
            {
                case "1":
                    txt.Text = message;
                    break;
                case "2":
                    txt2.Text = message;
                    break;

            }
        }

       



        static class Programm
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Server());

            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Server
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Server";
           
            this.ResumeLayout(false);

        }

        private void Server_Load(object sender, EventArgs e)
        {
        }
    }
}
