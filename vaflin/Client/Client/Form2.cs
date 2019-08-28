using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using OTCHET;

namespace Client
{
    public partial class Form2 : Form
    {
      
       bool aft;
       Interface1 obj;
        public delegate void Form_hide(bool aft);
        public event Form_hide form_hide;
        public Form2(Interface1 Obj,ref bool Aft)
        {
            InitializeComponent();
           
            obj = Obj;
            
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            byte[] Login, Pas;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {

                RSA.ImportParameters(obj.Key_send());
                Login = RSA.Encrypt(Encoding.UTF8.GetBytes(this.Login.Text), false);
                Pas = RSA.Encrypt(Encoding.UTF8.GetBytes(this.Pas.Text), false);
                // string a = obj.Enter_serv1(Login, Pas);
                if (obj.Enter_serv(Login, Pas))
                    form_hide(true);
                else
                {
                    MessageBox.Show(
    "Неверный логин или пароль",
    "Внимание!",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error,
    MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);
                    form_hide(false);
                    this.Hide();
                }
            }
        }
    }
    
}
