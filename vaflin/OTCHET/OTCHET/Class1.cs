using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;

namespace OTCHET
{
    public interface Interface1
    {
        string Prihod(string Type_serv, DateTime Exp_date);
        string Uhod(string Type_serv);
        bool Add_serv(string IP, string Type);
        RSAParameters Key_send();
        bool Enter_serv(byte[] Login, byte[] Pas);

        Object InitializeLifetimeService();
        Object GetLifetimeService();
    }
}
