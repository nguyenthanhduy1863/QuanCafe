using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2.DTO;

namespace WinFormsApp2.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { if(instance==null) instance = new TableDAO(); return TableDAO.instance; }
            private set{ TableDAO.instance = value; }
        }

        public static int TableWidth = 100;
        public static int TableHeight = 100;

        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExcuteQuery("exec dbo.usp_gettablelist");

            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

    }
}
