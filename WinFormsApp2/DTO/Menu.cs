﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.DTO
{
    public class Menu
    {

        public Menu(string foodName, int count, float price, float totalPriice = 0)
        {
            this.foodName = foodName;
            this.count = count;
            this.price = price;
            this.totalPrice = totalPriice;
        }
        public Menu(DataRow row)
        {
            this.foodName = row["Name"].ToString();
            this.count = (int)row["count"];
            this.price = (float)Convert.ToDouble(row["price"].ToString());
            this.totalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }

        private float totalPrice;

        private float price;

        private int count;

        private string foodName;

        public string FoodName { get => foodName; set => foodName = value; }
        public int Count { get => count; set => count = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
