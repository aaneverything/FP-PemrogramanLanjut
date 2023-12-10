using System;

using System.Windows.Forms;
using UangKu.Model.Entity;
using UangKu.Controller;

namespace UangKu.View
{
    public partial class Login : Form
    {
        private userController _controller;
        private User user;
        public static string getName;
        public static int getUserId;

        public Login()
        {
            InitializeComponent();
            _controller = new userController();
        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            user = new User();

            user.Username = txtUserLogin.Text;
            user.Password = txtPasswordLogin.Text;

            int result = _controller.Login(user);

            if(result > 0)
            {
                getName = _controller.getName(txtUserLogin.Text);
                getUserId = int.Parse(_controller.getUserId(txtUserLogin.Text));

                Dashboard userLogin = new Dashboard();
                userLogin.Show();

            }
            else
            {
                txtUserLogin.Text = "";
                txtPasswordLogin.Text = "";
                txtUserLogin.Focus();
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            SIgnUp createAcc = new SIgnUp();
            createAcc.ShowDialog();

            Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPasswordLogin_TextChanged(object sender, EventArgs e)
        {
            txtPasswordLogin.UseSystemPasswordChar = true;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}
