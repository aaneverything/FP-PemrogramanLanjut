using System;

using System.Windows.Forms;
using UangKu.Model.Entity;
using UangKu.Controller;

namespace UangKu.View
{
    public partial class SIgnUp : Form
    {
        private userController _controller;
        public SIgnUp()
        {
            InitializeComponent();
            _controller = new userController();
            txtConPass.UseSystemPasswordChar = true;
            txtPass.UseSystemPasswordChar = true;
        }

        


        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();

            this.Close();
            loginForm.Show();

        }

        private void SIgnUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if(txtPass.Text == txtConPass.Text)
            {
                User user = new User();

                user.Name = txtName.Text;
                user.Username = txtUsername.Text;
                user.Email = txtEmail.Text;
                user.Password = txtPass.Text;

                int result = _controller.SignUp(user);
                bool valid = _controller.usernameValidasi(user.Username);

                if (result > 0)
                {
                    Login login = new Login();
                    login.ShowDialog();

                    Visible = false;
                }

                if(valid == true)
                {
                    txtUsername.Text = "";
                    txtUsername.Focus();
                }
            }
            else
            {
                MessageBox.Show("Password Tidak Sama!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information );
                txtConPass.Text = "";
                txtConPass.Focus();
            }
        }

        private void bunifuLabel12_Click(object sender, EventArgs e)
        {

        }
    }
}
