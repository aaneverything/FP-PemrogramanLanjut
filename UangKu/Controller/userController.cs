using System;

using System.Collections.Generic;
using System.Windows.Forms;
using UangKu.Model.Context;
using UangKu.Model.Entity;
using UangKu.Model.Repository;

namespace UangKu.Controller
{
    public class userController
    {
        private userRpository _repository;

        public bool usernameValidasi(string username)
        {
            bool valid = false;
            using (DbContext context = new DbContext())
            {
                _repository = new userRpository(context);
                valid = _repository.DaftarValidasi(username);
            }

            return valid;
        }

        public int SignUp(User user)
        {
            bool valid = usernameValidasi(user.Username);

            int result = 0;

            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                MessageBox.Show("Datamu Masi Belum Lengkap", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return result;
            }

            if (valid != true)
            {

                using (DbContext context = new DbContext())
                {
                    _repository = new userRpository(context);
                    result = _repository.signUp(user);
                }
            }
            else
            {
                MessageBox.Show("Username anda udah ada yang make", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return result;
            }

            if (result > 0)
            {
                MessageBox.Show("Data berhasil disimpan !", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return result;
        }

        public int Login(User user)
        {
            int result = 0;
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                MessageBox.Show("Isi datanya yang bener !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            using (DbContext context = new DbContext())
            {
                _repository = new userRpository(context);
                result = _repository.Login(user);
            }

            return result;
        }

        public string getName(string username)
        {
            string nama = null;

            using (DbContext context = new DbContext())
            {
                _repository = new userRpository(context);
                nama = _repository.readName(username);
            }

            return nama;
        }

        public string getUserId(string username)
        {
            string userId = null;

            using (DbContext context = new DbContext())
            {
                _repository = new userRpository(context);
                userId = _repository.readUserId(username);
            }

            return userId;
        }

        public List<User> userData(int userId)
        {
            List<User> list = new List<User>();

            using (DbContext context = new DbContext())
            {
                _repository = new userRpository(context);
                list = _repository.userData(userId);
            }
            return list;

        }

        public int updateData(User usr, int userId)
        {
            int result = 0;

            using (DbContext context = new DbContext())
            {
                _repository = new userRpository(context);
                result = _repository.updateAcc(usr, userId);
            }

            if (result > 0)
            {
                MessageBox.Show("Data berhasil diupdate !", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Data gagal diupdate !!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return result;   
        }

        public int deleteAccount(int userId)
        {
            int result = 0;

            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin dihapus ? ", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(konfirmasi == DialogResult.Yes)
            {
                using (DbContext context = new DbContext()) 
                { 
                    _repository = new userRpository(context);
                    result = _repository.deleteAcc(userId);
                }

                MessageBox.Show("Account berhasil dihapus", "Konfirmasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return result;
        }
    }
}
