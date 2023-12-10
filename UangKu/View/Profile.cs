using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UangKu.Controller;    
using UangKu.Model.Entity;

namespace UangKu.View
{
    public partial class Profile : Form
    {
        // manggil controller
        private userController controller = new userController();
        private User usr = new User();
        List<User> userlist = new List<User>();
        int userId = Login.getUserId;

        public Profile()
        {
            InitializeComponent();
            userlist = controller.userData(userId);
            txtPassword.UseSystemPasswordChar = true;

            txtUsername.ReadOnly = true;
            txtPassword.ReadOnly = true;

            foreach (var usr in userlist)
            {
                txtName.Text = usr.Name;
                txtUsername.Text = usr.Username;
                txtEmail.Text = usr.Email;
                txtPassword.Text = usr.Password;
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda ingin keluar ? ", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (konfirmasi == DialogResult.Yes)
            {
                Login keluar = new Login();
                this.Close();
                keluar.ShowDialog();
            }
        }

       

        private void Profile_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            int hapus = controller.deleteAccount(userId);

            if (hapus > 0)
            {
                Login login = new Login();
                this.Close();
                login.ShowDialog();
            }
        }

        private void btnEditData_Click(object sender, EventArgs e)
        {

            var konfirmasi = MessageBox.Show("Apakah anda yakin akan mengedit data ini ? ", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (konfirmasi == DialogResult.Yes)
                {
                    // ambil objek mhs yang mau dihapus dari collection
                    User user =  new User { 
                        Name = txtName.Text,
                        Email = txtEmail.Text,
                    };

                controller.updateData(user, userId);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();  
        }
    }
}
