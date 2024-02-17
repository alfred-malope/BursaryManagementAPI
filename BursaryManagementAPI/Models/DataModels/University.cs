﻿using System.Dynamic;

namespace BursaryManagementAPI.Models.DataModels
{
    public class University
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProvinceID { get; set; }

        public University(string name, int provinceID)
        {
            Name = name;
            ProvinceID = provinceID;
        }
        public University(int _id, string _name, int _provinceID)
        {
            ID = _id;
            Name = _name;
            ProvinceID = _provinceID;

        }

        public int GetID() => ID;
        public string GetName() => Name;
        public int GetProvinceID() => ProvinceID;



    }
}
