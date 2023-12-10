using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UangKu.Model.Context;
using UangKu.Model.Entity;
using System.Data;
using System.Diagnostics;

namespace UangKu.Model.Repository
{
    public class HistoryRepository : IDisposable
    {
        private MySqlConnection _cnn;
        private TransactionHistory _history;


        public HistoryRepository(DbContext context)
        {
            _cnn = context.ConnectionOpen();
        }

        public int Create(TransactionHistory history, int userId)
        {
            int result = 0;
            string sql = "INSERT INTO transactionhistory(TransactionHistory_id,Transaction_id, Nama_History, User_id, Transaction_date, Transaction_amount) VALUES (@TransactionHistory_id,@Transaction_id, @Nama_History, @User_id, @Transaction_date, @Transaction_amount)"; 

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@user_id", userId);
               cmd.Parameters.AddWithValue("@TransactionHistory_id", history.TransactionHistory_id);
                cmd.Parameters.AddWithValue("@Nama_History", history.Nama_History);
               cmd.Parameters.AddWithValue("@Transaction_id", history.Transaction_id);
                cmd.Parameters.AddWithValue("@Transaction_date", history.Transaction_date);
                cmd.Parameters.AddWithValue("@Transaction_amount", history.Transaction_amount);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Create error: {ex.Message}");
                    throw;
                }
            }

            return result;
        }


        public List<TransactionHistory> readHistory(int user_id)
        {
            List<TransactionHistory> list = new List<TransactionHistory>();

            try
            {
                string sql = "SELECT Transaction_id, Nama_History, Transaction_date, Transaction_amount FROM transactionhistory WHERE user_id = @user_id";

                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@user_id", user_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _history = new TransactionHistory
                            {
                                // Ensure the property names match the selected column names in the SQL query
                                Transaction_id = int.Parse(reader["Transaction_id"].ToString()),
                                Nama_History = reader["Nama_History"].ToString(),
                                Transaction_date = reader.GetDateTime(reader.GetOrdinal("Transaction_date")),
                                Transaction_amount = int.Parse(reader["Transaction_amount"].ToString())
                            };

                            list.Add(_history);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"readHistory error: {ex.Message}");
                // Log the exception or handle it accordingly
                throw; // Consider handling the exception based on your application's requirements
            }

            return list;
        }


        public List<TransactionHistory> ReadByNama(string nama)
        {
            List<TransactionHistory> list = new List<TransactionHistory>();

            try
            {
                string sql = @"SELECT * FROM transactionhistory WHERE Nama_History LIKE @nama";

                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@nama", $"%{nama}%");

                    using (MySqlDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            _history = new TransactionHistory
                            {
                                Transaction_id = int.Parse(dtr["Transaction_id"].ToString()),
                                Nama_History = dtr["Nama_History"].ToString(),
                                Transaction_date = Convert.ToDateTime(dtr["Transaction_date"]),
                                Transaction_amount = int.Parse(dtr["Transaction_amount"].ToString())
                            };
                            list.Add(_history);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ReadByNama error: {ex.Message}");
                throw;
            }

            return list;
        }

        public void Dispose()
        {
            if (_cnn != null && _cnn.State != ConnectionState.Closed)
            {
                _cnn.Close();
                _cnn.Dispose();
            }
        }
    }

}
