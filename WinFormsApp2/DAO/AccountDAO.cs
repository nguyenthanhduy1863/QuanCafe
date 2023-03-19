using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2.DTO;

namespace WinFormsApp2.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance { 
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            set { AccountDAO.instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName, string password)
        {
            string query = "select * from dbo.Account where UserName = N'"+userName+"' and Password = N'"+password+"'";

            DataTable result = DataProvider.Instance.ExcuteQuery(query);

            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExcuteNonQuery("exec USP_UpdateAccount @userName , @password , @newPassword", new object[] {userName, pass, newPass});
            return result > 0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExcuteQuery("Select UserName, DisplayName, Type from Account");
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExcuteQuery("select * from Account where uSerName = '" + userName + "'");

            foreach(DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("insert Account (UserName, DisplayName, Type)values (N'{0}', N'{1}', {2})", name, displayName, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("update Account set DisplayName =N'{1}', Type = {2} where UserName = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string name)
        {
            string query = string.Format("update Account set password = N'0' where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
