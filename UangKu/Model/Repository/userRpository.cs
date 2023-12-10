using System;

using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UangKu.Model.Context;
using UangKu.Model.Entity;

using System.Windows.Forms;

namespace UangKu.Model.Repository
{
    public class userRpository
    {
        private MySqlConnection _cnn;
        private User user;

        public userRpository(DbContext context)
        {
            _cnn = (context.ConnectionOpen());
        }

        // Validasi akun sudah terbuat atau belum
        public bool DaftarValidasi(string username)
        {
            bool valid = false;
            try
            {
                string sql = "SELECT username FROM users WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            valid = true;
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Get User and Pass Error: {0}", ex.Message);
            }
            return valid;
        }

        // daftar akun
        public int signUp(User usrSIgnUp)
        {
            int result = 0;
            string sql = "INSERT INTO Users(name, username, email, password) VALUES (@name, @username, @email, @password)";


/*            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usrSIgnUp.Password, salt);*/

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@name", usrSIgnUp.Name);
                cmd.Parameters.AddWithValue("@username", usrSIgnUp.Username);
                cmd.Parameters.AddWithValue("@email", usrSIgnUp.Email);
                cmd.Parameters.AddWithValue("@password", usrSIgnUp.Password);

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

        // method untuk melakukan login
        public int Login(User usrLogin)
        {
            /*bool valid = false;*/
            int result = 0;

            string sql = "SELECT *FROM users WHERE username = @username AND password = @password";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@username", usrLogin.Username);
                cmd.Parameters.AddWithValue("@password", usrLogin.Password);


                try
                {
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch(Exception ex)
                {
                    return 0;
                }

 /*             MySqlDataReader dtr = cmd.ExecuteReader();
                if (dtr.Read())
                {
                    string encryPasss = dtr["password"].ToString();
                    bool validasiPass = BCrypt.Net.BCrypt.Verify(usrLogin.Password, encryPasss);     // validasi password yang ter ecryp

                    if (encryPasss)
                    {
                        try
                        {
                            valid = true;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);
                        }
                    }
                }*/
            }

            return result;
        }

        // cari username
        public string readName(string username)
        {
            string nama = null;

            string sql = "SELECT name FROM users WHERE username = @username";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader dtr = cmd.ExecuteReader();
                if (dtr.Read())
                {
                    nama = dtr["name"].ToString();
                }
            }
            return nama;
        }

        // cari user_id
        public string readUserId(string username)
        {
            string userId = null;

            string sql = "SELECT user_id FROM users WHERE username = @username";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader dtr = cmd.ExecuteReader();
                if (dtr.Read())
                {
                    userId = dtr["user_id"].ToString();
                }
            }

            return userId;
        }

        // tampilin user di profile
        public List<User> userData(int user_id)
        {
            List<User> list = new List<User>(); // Menggunakan tipe data User

            try
            {
                string sql = "SELECT * FROM users WHERE user_id = @user_id";

                using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
                {
                    cmd.Parameters.AddWithValue("@user_id", user_id);

                    using (MySqlDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            user = new User();
                            user.User_Id = user_id;
                            user.Name = dtr["name"].ToString();
                            user.Username = dtr["username"].ToString();
                            user.Email = dtr["email"].ToString();
                            user.Password = dtr["password"].ToString();
                            list.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Error: {0}", ex.Message);
            }

            return list;
        }

        // update data user
        public int updateAcc(User usr, int userId)
        {
            int result = 0;

            string sql = "UPDATE users SET name = @name, email = @email WHERE  user_id = @user_id";

            using (MySqlCommand cmd = new MySqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@name", usr.Name);
                cmd.Parameters.AddWithValue("@email", usr.Email);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Error: {0}", ex.Message);
                }
            }

             return result;
        }

        // hapus account
        public int deleteAcc(int userId)
        {
            int result = 0;

            string sql = "DELETE FROM users WHERE user_id = @user_id";

            using (MySqlCommand cmd = new MySqlCommand (sql, _cnn))
            {
                cmd.Parameters.AddWithValue ("@user_id", userId);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Error: {0}", ex.Message);

                }
            }

            return result;
        }

    }
}


