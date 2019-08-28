using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Activation;
using OTCHET;
using System.Threading;



namespace Client
{
    
    public class Client : Form
    {
        public DateTime Click_time;
       Interface1 obj;
        public MonthCalendar cal;
        private Button Prihod, Uhod, But1, But2, But3, But4;         // Кнопки
        public TextBox txt;
        public ComboBox cmb, cmb1;
        Form2 ent_f;
        ILease lease;
        public bool is_adm=false;


        public Client()
        {
            Create();
            Click_time = DateTime.Now;
        }

        public void Create()
        {
            
           
            Color z = new Color();              //цвет кнопки 
            z = Color.Wheat;


            this.ClientSize = new System.Drawing.Size(1000, 500);
            Prihod = new Button();                   // Создание кнопки
            Prihod.Location = new Point(20, 20);     // Расположение кнопки
            Prihod.Size = new Size(150, 25);         // Размер кнопки
            Prihod.Name = "Prihod";                   // Имя кнопки
            Prihod.Text = "Арендовать";                   // Текст на кнопки
            Prihod.Click += new EventHandler(OnBut);
            Prihod.BackColor = z;
            this.Controls.Add(Prihod);               // Добавление кнопки в окно клиента

            Uhod = new Button();                   // Создание кнопки
            Uhod.Location = new Point(20, 50);     // Расположение кнопки
            Uhod.Size = new Size(150, 25);         // Размер кнопки
            Uhod.Name = "Uhod";                   // Имя кнопки
            Uhod.Text = "Купить";                   // Текст на кнопки
            Uhod.Click += new EventHandler(OnBut);
            Uhod.BackColor = z;
            this.Controls.Add(Uhod);               // Добавление кнопки в окно клиента


            But1 = new Button();                   // Создание кнопки
            But1.Location = new Point(20, 150);     // Расположение кнопки
            But1.Size = new Size(200, 50);         // Размер кнопки
            But1.Name = "But1";                   // Имя кнопки
            But1.Text = "TCP";  // Текст на кнопки
            But1.Click += new EventHandler(Connection1);
            But1.BackColor = z;
            this.Controls.Add(But1);               // Добавление кнопки в окно клиента

            But2 = new Button();                   // Создание кнопки
            But2.Location = new Point(240, 150);     // Расположение кнопки
            But2.Size = new Size(200, 50);         // Размер кнопки
            But2.Name = "But2";                   // Имя кнопки
            But2.Text = "HTTP (КОНФ. ФАЙЛ)";  // Текст на кнопки
            But2.Click += new EventHandler(Connection2);
            But2.BackColor = z;
            this.Controls.Add(But2);               // Добавление кнопки в окно клиента

            But3 = new Button();                   // Создание кнопки
            But3.Location = new Point(460, 150);     // Расположение кнопки
            But3.Size = new Size(200, 50);         // Размер кнопки
            But3.Name = "But3";                   // Имя кнопки
            But3.Text = "Войти как администратор";  // Текст на кнопки
            But3.Click += new EventHandler(Admin_ent);
            But3.BackColor = z;
            this.Controls.Add(But3);               // Добавление кнопки в окно клиента

            if (is_adm)
            {
                But4 = new Button();                   // Создание кнопки
                But4.Location = new Point(20, 75);     // Расположение кнопки
                But4.Size = new Size(150, 25);         // Размер кнопки
                But4.Name = "Add";                   // Имя кнопки
                But4.Text = "Добаваить сервер";                   // Текст на кнопки
                But4.Click += new EventHandler(OnBut);
                But4.BackColor = z;
                this.Controls.Add(But4);
            }

            txt = new TextBox();
            txt.Location = new Point(250, 50);     // Расположение
            txt.Size = new Size(250, 600);         // Размер 
            txt.Name = "Txt";                   // Имя кнопки
            this.Controls.Add(txt);

            cal = new MonthCalendar();
            cal.Location = new Point(600, 300);
            cal.Name = "Calendar";
            Controls.Add(cal);

            cmb = new ComboBox();
            cmb.Location = new Point(250, 100);
            cmb.Size = new Size(250, 600);
            cmb.Name = "combo";
            this.cmb.Items.AddRange(new object[]
            {
                "igrovoi",
                "Web server",
                "Application server"
            });
            this.Controls.Add(cmb);

           
            this.Click += new System.EventHandler(Click_on);

        }

        public void Connection1(object sender, EventArgs e)
        {
            try
            {  //полный уровень десериализации
                BinaryServerFormatterSinkProvider srvPrvd = new BinaryServerFormatterSinkProvider();
                srvPrvd.TypeFilterLevel = TypeFilterLevel.Full; //уровень диссериализации
                BinaryClientFormatterSinkProvider clntPrvd = new BinaryClientFormatterSinkProvider();
                Dictionary<string, string> proprt = new Dictionary<string, string>();
                proprt["port"] = "0";
                TcpChannel channel = new TcpChannel(proprt, clntPrvd, srvPrvd);
                ChannelServices.RegisterChannel(channel, false); //регистрация
                obj = (Interface1)Activator.GetObject(typeof(Interface1), "tcp://localhost:8086/UdOb");
               Sponsors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //http
        public void Connection2(object sender, EventArgs e)
        {
            try
            {
                
                RemotingConfiguration.Configure("C:\\Users\\Kirushka\\Desktop\\vaflin\\confin.config", false);
                obj = (Interface1)Activator.GetObject(typeof(Interface1), "http://localhost:8085/UdOb");
                Sponsors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Sponsors()
        {
            lease = (ILease)obj.GetLifetimeService(); //информация об аренде    
            MyClientSponsor sponsor = new MyClientSponsor(this); //создание спонсора
            lease.Register(sponsor); //регистрация спонсора в службе аренды
           
        }
        public void Admin_ent(object sender, EventArgs e)
        {
            ent_f = new Form2(obj,ref is_adm);
            ent_f.Show();
            ent_f.form_hide += (adm_form);

        }
        public void Click_on(object sender, EventArgs e)
        {
            Click_time = DateTime.Now;
        }
        void adm_form(bool Is_adm)
        {
            if (Is_adm)
                is_adm = true;
            ent_f.Close();

            But4 = new Button();                   // Создание кнопки
            But4.Location = new Point(20, 75);     // Расположение кнопки
            But4.Size = new Size(150, 25);         // Размер кнопки
            But4.Name = "Add";                   // Имя кнопки
            But4.Text = "Добавить сервер";                   // Текст на кнопки
            But4.Click += new EventHandler(OnBut);
            But4.BackColor = Color.Wheat;
            this.Controls.Add(But4);

        }
        public void OnBut(object sender, EventArgs e)  // Обработчик нажатия на кнопку
        {
            Button but = (Button)sender;
            try
            {
                switch (but.Name)                   // Выбор нажатия на кнопки
                {
                    case "Prihod":
                        txt.Text = obj.Prihod(Convert.ToString(cmb.SelectedItem), cal.SelectionEnd);
                        if (txt.Text == null) txt.Text = "сервера кончились, подождите";
                        break;
                    case "Uhod":
                        txt.Text = obj.Uhod(Convert.ToString(cmb.SelectedItem));
                                  if (txt.Text == null) txt.Text = "сервера кончились, подождите";
                        break;
                    case "Add":
                        string a = txt.Text;
                        string b = Convert.ToString(cmb.SelectedItem);
                        if (obj.Add_serv(a,b))
                            txt.Text = "Сервер добавлен";
                        else
                            txt.Text = "Ошибка";
                        break;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


       
    }











    public class MyClientSponsor : MarshalByRefObject, ISponsor
    {
        Client A;

        public MyClientSponsor(Client a)
        {
            A = a;
            Console.WriteLine("\nСпонсор создан ");
        }
        public TimeSpan Renewal(ILease lease) //продление жизни
        {
            if (( DateTime.Now - A.Click_time ) < TimeSpan.FromSeconds(30))
                return TimeSpan.FromSeconds(60);
            else
                MessageBox.Show(
       "Удаленный объект уничтожен",
       "Внимание!",
       MessageBoxButtons.OK,
       MessageBoxIcon.Error,
       MessageBoxDefaultButton.Button1,
       MessageBoxOptions.DefaultDesktopOnly);
            return TimeSpan.Zero;
            
        }


    }

   

}

