using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.DTO
{
    public class Account
    {
        public Account (string userName, int type, string password = null)
        {
            this.userName= userName;
            this.type = type;
            this.password = password;
        }

        public Account(DataRow row)
        {
            this.userName = row["userName"].ToString();
            this.type = (int)row["type"];
            this.password = row["password"].ToString();
        }

        private int type;

        private string password;

        private string userName;

        public string UserName { get => userName; set => userName = value; }
       public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }
    }
}
