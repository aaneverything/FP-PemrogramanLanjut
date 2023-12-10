using System;
using System.Collections.Generic;

namespace UangKu.Model.Entity
{
    public class Transaction
    {
        public int Transaction_id { get; set; } 
        public int User_id { get; set; }
        public string Transaction_category { get; set; }    
        public string Transaction_date { get; set; }
        public int Transaction_amount { get; set; }
        public string Transaciton_name { get; set; }
        public string Id_method { get; set; }
        public string Nama_Method {  get; set; }

    }
}
