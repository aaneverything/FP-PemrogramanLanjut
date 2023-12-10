using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UangKu.Model.Entity
{
    public class TransactionHistory
    {
        public int User_id { get; set; }
        public int TransactionHistory_id { get; set; }
        public int Transaction_id { get; set; }
        public string Nama_History { get; set; }
        public DateTime Transaction_date { get; set; }
        public int Transaction_amount { get; set; }
    }
}
