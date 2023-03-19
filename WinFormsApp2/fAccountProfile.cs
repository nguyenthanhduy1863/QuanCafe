using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp2.DAO;
using WinFormsApp2.DTO;

namespace WinFormsApp2
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txbUserName.Text = loginAccount.UserName;
        }
        void UpdateAccountInfo()
        {
            string password = txbPassword.Text;
            string newpass = txbNewPassword.Text;
            string reenterPass = txbReEnterPassword.Text;
            string userName = txbUserName.Text;
            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(userName, password, newpass))
                {
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    if (newpass == null || newpass == "")
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu mới!");
                    }
                    else
                    {
                        MessageBox.Show("Vui long nhập đúng mật khẩu!");
                    }
                }
            }
        }

        private void btnExitAccountProfile_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }
}
