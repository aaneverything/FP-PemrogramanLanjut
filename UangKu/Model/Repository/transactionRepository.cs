using System;
using System.Collections.Generic;
using UangKu.Controller;
using MySql.Data.MySqlClient;
using UangKu.Model.Context;
using UangKu.Model.Entity;
using System.Windows.Forms;

namespace UangKu.Model.Repository
{
    public class transactionRepository
    {
        private MySqlConnection _cnn;
        private Transaction _transaction;

        public transactionRepository(DbContext context)
        {
            // membuka koneksi
            _cnn = context.ConnectionOpen();
        }

        // menambahkan transaksi
        public int Input(Transaction transaksi, int User_id)
        {
            int result = 0;
            string sql = "INSERT INTO transaction(User_id, Transaction_id, Transaction_category, Transaction_date, Transaction_amount, Transaction_name, Nama_Method) VALUES (@User_id, @Transaction_id, @Transaction_category, @Transaction_date, @Transaction_amount, @Transaction_name, @Nama_Method)";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@user_id", User_id);
                cmd.Parameters.AddWithValue("@Transaction_id", transaksi.Transaction_id);
                cmd.Parameters.AddWithValue("@transaction_category", transaksi.Transaction_category);
                cmd.Parameters.AddWithValue("@transaction_amount", transaksi.Transaction_amount);
                cmd.Parameters.AddWithValue("@transaction_date", transaksi.Transaction_date);
                cmd.Parameters.AddWithValue("@transaction_name", transaksi.Transaciton_name);
                cmd.Parameters.AddWithValue("@Nama_Method", transaksi.Nama_Method);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);
                }
            }
            return result;
        }


        // menghapus transaksi
        public int transactionDelete(int transaction_id)
        {

            int result = 0;
            string sql = "DELETE FROM transaction WHERE Transaction_id = @Transaction_id";



            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@transaction_id", transaction_id);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);

                }
            }

            return result;
        }

        // menampilkan detail transaksi
        public List<Transaction> readTransaction(int User_id)
        {
            List<Transaction> list = new List<Transaction>();
            try
            {
                string sql = "SELECT *FROM transaction WHERE User_id = @User_id";

                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@user_id", User_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _transaction = new Transaction();
                            _transaction.Transaction_id =int.Parse(reader["Transaction_id"].ToString());
                            _transaction.Transaction_category = reader["Transaction_category"].ToString();
                            _transaction.Nama_Method = reader["Nama_Method"].ToString();
                            _transaction.Transaction_date = reader["Transaction_date"].ToString();
                            _transaction.Transaction_amount = int.Parse(reader["Transaction_amount"].ToString());
                            _transaction.Transaciton_name = reader["Transaction_name"].ToString();
                            list.Add(_transaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);

            }

            return list;
        }

        // mengupdate transaksi 
        public int Update(Transaction transaction)
        {
            int result = 0;
            string sql = "UPDATE transaction SET Transaction_amount = @transaction_amount, Transaction_name = @transaction_name, Nama_Method =  @Nama_Method WHERE Transaction_id = @transaction_id";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@transaction_id", transaction.Transaction_id);
                cmd.Parameters.AddWithValue("@transaction_name", transaction.Transaciton_name);
                cmd.Parameters.AddWithValue("@transaction_amount", transaction.Transaction_amount);
                cmd.Parameters.AddWithValue("@Nama_Method", transaction.Nama_Method);


                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    return ex.ErrorCode;
                }
            }

            return result;
        }

        // baca jumlah dari outcome
        public string readOut(int userId)
        {
            string income = null;

            string sql = @"SELECT IFNULL(SUM(Transaction_amount), 0) AS total_Transaction_amount FROM transaction WHERE transaction_category = 'outcome' AND user_id = @user_id";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@user_id", userId);
                using (MySqlDataReader dtr = cmd.ExecuteReader())
                {
                    if (dtr.Read() && dtr["total_Transaction_amount"] != DBNull.Value)
                    {
                        income = dtr["total_Transaction_amount"].ToString();
                    }
                    else
                    {
                        income = "0";
                    }
                }
            }

            return income;
        }

        // baca jumlah dari income
        public string readIn(int User_Id)
        {
            string income = null;

            string sql = @"SELECT IFNULL(SUM(Transaction_amount), 0) AS total_Transaction_amount FROM transaction WHERE transaction_category = 'income' AND user_id = @user_id";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@user_id", User_Id);
                using (MySqlDataReader dtr = cmd.ExecuteReader())
                {
                    if (dtr.Read() && dtr["total_Transaction_amount"] != DBNull.Value)
                    {
                        income = dtr["total_Transaction_amount"].ToString();
                    }
                    else
                    {
                        income = "0";
                    }
                }
            }

            return income;
        }

        // baca total balande
        public int balance(int userId)
        {
            int result = 0;

            string sql = @"SELECT (SELECT IFNULL(SUM(Transaction_amount), 0) FROM transaction WHERE transaction_category = 'income' AND user_id = @user_id) -
                            (SELECT IFNULL(SUM(Transaction_amount), 0) FROM transaction WHERE transaction_category = 'outcome' AND user_id = @user_id) AS total_difference";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@user_id", userId);
                using (MySqlDataReader dtr = cmd.ExecuteReader())
                {
                    if (dtr.Read() && dtr["total_difference"] != DBNull.Value)
                    {
                        // Ambil nilai total_difference dari hasil kueri SQL
                        result = Convert.ToInt32(dtr["total_difference"]);
                    }
                    // Else block can be added for handling scenarios where there are no transactions for the specified categories
                }
            }

            return result;
        }

        // Method untuk menampilkan data mahasiwa berdasarkan pencarian nama
        public List<Transaction> ReadByNama(string nama)
        {
            // membuat objek collection untuk menampung objek Transaction
            List<Transaction> list = new List<Transaction>();

            try
            {
                // deklarasi perintah SQL
                string sql = @"select * 
                               from transaction 
                               where Transaction_name like @nama
                               order by Transaction_name";

                // membuat objek command menggunakan blok using
                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    // mendaftarkan parameter dan mengeset nilainya
                    cmd.Parameters.AddWithValue("@nama", string.Format("%{0}%", nama));

                    // membuat objek dtr (data reader) untuk menampung result set (hasil perintah SELECT)
                    using (MySqlDataReader dtr = cmd.ExecuteReader())
                    {
                        // panggil method Read untuk mendapatkan baris dari result set
                        while (dtr.Read())
                        {
                            _transaction = new Transaction();
                            _transaction.Transaction_id = int.Parse(dtr["Transaction_id"].ToString());
                            _transaction.Transaction_category = dtr["Transaction_category"].ToString();
                            _transaction.Nama_Method = dtr["Nama_Method"].ToString();
                            _transaction.Transaction_date = dtr["Transaction_date"].ToString();
                            _transaction.Transaction_amount = int.Parse(dtr["Transaction_amount"].ToString());
                            _transaction.Transaciton_name = dtr["Transaction_name"].ToString();
                            list.Add(_transaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("ReadByNama error: {0}", ex.Message);
            }

            return list;
        }

        public List<Transaction> readIncomeOnly(int User_id)
        {
            List<Transaction> list = new List<Transaction>();
            try
            {
                string sql = "SELECT * FROM transaction WHERE User_id = @User_id AND Transaction_category LIKE '%Income%'";

                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@user_id", User_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _transaction = new Transaction();
                            _transaction.Transaction_id = int.Parse(reader["Transaction_id"].ToString());
                            _transaction.Transaction_category = reader["Transaction_category"].ToString();
                            _transaction.Nama_Method = reader["Nama_Method"].ToString();
                            _transaction.Transaction_date = reader["Transaction_date"].ToString();
                            _transaction.Transaction_amount = int.Parse(reader["Transaction_amount"].ToString());
                            _transaction.Transaciton_name = reader["Transaction_name"].ToString();
                            list.Add(_transaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);

            }

            return list;
        }

        public List<Transaction> readOutcomeOnly(int User_id)
        {
            List<Transaction> list = new List<Transaction>();
            try
            {
                string sql = "SELECT * FROM transaction WHERE User_id = @User_id AND Transaction_category LIKE '%Outcome%'";

                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@user_id", User_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _transaction = new Transaction();
                            _transaction.Transaction_id = int.Parse(reader["Transaction_id"].ToString());
                            _transaction.Transaction_category = reader["Transaction_category"].ToString();
                            _transaction.Nama_Method = reader["Nama_Method"].ToString();
                            _transaction.Transaction_date = reader["Transaction_date"].ToString();
                            _transaction.Transaction_amount = int.Parse(reader["Transaction_amount"].ToString());
                            _transaction.Transaciton_name = reader["Transaction_name"].ToString();
                            list.Add(_transaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);

            }

            return list;
        }

    }
}    
