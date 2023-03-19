using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp2.DAO;
using WinFormsApp2.DTO;

namespace WinFormsApp2
{
    public partial class fTableManger : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public fTableManger(Account acc)
        {
            InitializeComponent();

            this.LoginAccount= acc;

            LoadTable();
            LoadCategory();
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
        }



        void LoadCategory()
        {
            List<Category> listCategory= CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource= listCategory;
            cbCategory.DisplayMember= "Name";
        }

        void LoadFoodListByCategory(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource= listFood;
            cbFood.DisplayMember= "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach(Table item in tableList)
            {
                Button btn = new Button() { Width=TableDAO.TableWidth, Height=TableDAO.TableHeight};

                btn.Text= item.Name + Environment.NewLine+item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }


                flpTable.Controls.Add(btn);

            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach(Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice+= item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            txbTotalPrice.Text= totalPrice.ToString("c", culture);
        }

        #endregion


        #region Events

        void btn_Click(object sender, EventArgs e)
        {
            int tableID=((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.ShowDialog();
        }


        private void adminToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = loginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_DeleteFood(object? sender, EventArgs e)
        {
            LoadFoodListByCategory((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_UpdateFood(object? sender, EventArgs e)
        {
            LoadFoodListByCategory((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void f_InsertFood(object? sender, EventArgs e)
        {
            LoadFoodListByCategory((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;


            LoadFoodListByCategory(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if(table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int foodID= (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value; 
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID);

            LoadTable();
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            double totalPrice = double.Parse(txbTotalPrice.Text, NumberStyles.Currency, new CultureInfo("vi-VN"));
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if(idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\n Tổng tiền - (Tổng tiền / 100) x Giảm giá = {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }

        #endregion   
    }
}
