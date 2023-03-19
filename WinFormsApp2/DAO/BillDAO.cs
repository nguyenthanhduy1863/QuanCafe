using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2.DTO;

namespace WinFormsApp2.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }

        public int GetUnCheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExcuteQuery("select * from dbo.Bill where idTable = " + id + " and status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return - 1;
        }

        public void CheckOut(int id,int discount, float totalPrice)
        {
            string query = "update Bill set dateCheckOut = GETDATE(), status =1, "+"discount="+discount+", totalPrice = "+totalPrice+"  where id = " +id;
            DataProvider.Instance.ExcuteNonQuery(query);
        }


        public void InsertBill(int id)
        {
            DataProvider.Instance.ExcuteNonQuery("exec usp_InsertBill @idTable", new object[] {id});
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            DataTable dataTable = DataProvider.Instance.ExcuteQuery("usp_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
            return dataTable;
        }

        public int GetMaxIDBill()
        {

            try
            {
               return (int)DataProvider.Instance.ExcuteScalar("select max(id) from bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
