using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.DTO
{
    public class Food
    {
        public Food(int id, string name, int categoryID, float price)
        {
            this.iD = id;
            this.name = name;
            this.categoryID = categoryID;
            this.price = price;
        }

        public Food(DataRow row)
        {
            this.iD = (int)row["id"];
            this.name = row["name"].ToString();
            this.categoryID = (int)row["idcategory"];
            this.price = (float)Convert.ToDouble(row["price"].ToString());
        }

        private float price;
        private int categoryID;
        private string name;
        private int iD;

        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public float Price { get => price; set => price = value; }
    }
}
