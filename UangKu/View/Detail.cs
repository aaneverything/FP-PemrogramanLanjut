using System;
using System.Collections.Generic;
using System.Windows.Forms;

using UangKu.Model.Entity;
using UangKu.Controller;
using UangKu.Model.Context;

namespace UangKu.View
{
    public partial class Detail : Form
    {
        int user = Login.getUserId;
        private transactionController controller;
        public List<Transaction> listTransaction = new List<Transaction>();
        private TransactionHistory history;
        private HistoryController contHistory;
        static int userId = Login.getUserId;

        public Detail()
        {
            InitializeComponent();
            controller = new transactionController();

            lblUser.Text = Login.getName;
            lblUser1.Text = Login.getName;
            Date.Value = DateTime.Now;
            lblIncome.Text = $"Rp. {controller.readIncome(user)};";
            lblOutcome.Text = $"Rp. {controller.readOutcome(user)};";
            lblBalence.Text = $"Rp. {controller.readBalance(user)};";

            InisialisasiTransaksi();
            LoadDataTransaksi();
        }


        private void InisialisasiTransaksi()
        {
            lvwTransaction.View = System.Windows.Forms.View.Details;
            lvwTransaction.FullRowSelect = true;
            lvwTransaction.GridLines = true;

            lvwTransaction.Columns.Add("No", 40, HorizontalAlignment.Center);
          //  lvwTransaction.Columns.Add("Transactionn Id", 110, HorizontalAlignment.Center);//
            lvwTransaction.Columns.Add("Metode", 150, HorizontalAlignment.Center);
            lvwTransaction.Columns.Add("Transaction Category", 165, HorizontalAlignment.Center);
            lvwTransaction.Columns.Add("Date", 200, HorizontalAlignment.Center);
            lvwTransaction.Columns.Add("Amounnt", 150, HorizontalAlignment.Center);
            lvwTransaction.Columns.Add("Description", 250, HorizontalAlignment.Center);
        }

        private void LoadDataTransaksi()
        {
            lvwTransaction.Items.Clear();

            // Ensure that controller.read(user) returns the expected data
            listTransaction = controller.read(user);

            foreach (var transaction in listTransaction)
            {
                var noUrut = lvwTransaction.Items.Count + 1;

                var item = new ListViewItem(noUrut.ToString());
               // item.SubItems.Add(transaction.Transaction_id.ToString());//
                string namaMethod = transaction.Nama_Method.ToString();
                item.SubItems.Add(namaMethod);
                item.SubItems.Add(transaction.Transaction_category.ToString());
                item.SubItems.Add(transaction.Transaction_date);
                item.SubItems.Add(transaction.Transaction_amount.ToString());
                item.SubItems.Add(transaction.Transaciton_name);
              
                lvwTransaction.Items.Add(item);
            }
            lvwTransaction.Refresh();
        }



        // ini masi rusak hehehehe
        private void HandleCreateData(Transaction transaction)
        {
            LoadDataTransaksi();
        }

        private void HandleUpdateeData(Transaction transaction)
        {
            LoadDataTransaksi();
        }

        private void Detail_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard home = new Dashboard();  
            home.ShowDialog();

            Visible = false;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnInputData_Click(object sender, EventArgs e)
        { 
            InputData addTransaksi = new InputData("Tambah Data Transaksi", controller);
            addTransaksi.OnCreateData += HandleCreateData;
            addTransaksi.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvwTransaction.SelectedItems.Count > 0)
            {
                Transaction trs = listTransaction[lvwTransaction.SelectedIndices[0]];
                InputData updateData = new InputData("Update Data Transaksi", trs, controller);

                history = new TransactionHistory
                {
                    Transaction_id = trs.Transaction_id,
                    Nama_History = "Update",
                    Transaction_date = DateTime.Now,
                    Transaction_amount = trs.Transaction_amount // Mengisi nilai dari Transaction ke history

                };
                updateData.OnUpdateData += HandleUpdateeData;
                contHistory = new HistoryController(new DbContext()); // Inisialisasi contHistory
                contHistory.Create(history, userId);
                updateData.ShowDialog();
            }
            else
            {
                MessageBox.Show("Data belum dipilih", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvwTransaction.SelectedItems.Count > 0)
            {
                var konfirmasi = MessageBox.Show("Apakah data ini ingin dihapus ? ", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (konfirmasi == DialogResult.Yes)
                {
                    // ambil objek mhs yang mau dihapus dari collection
                    Transaction transaction = listTransaction[lvwTransaction.SelectedIndices[0]];

                    // Delete new history entry
                    history = new TransactionHistory
                    {
                        Transaction_id = transaction.Transaction_id,
                        Nama_History = "Delete",
                        Transaction_date = DateTime.Now, /*DateTime.Now.ToString("yyyy-MM-dd")*/ // Mengisi nilai dari Transaction ke history
                        Transaction_amount = transaction.Transaction_amount // Mengisi nilai dari Transaction ke history

                    };

                    int result = controller.Delete(transaction.Transaction_id);
                    contHistory = new HistoryController(new DbContext()); // Inisialisasi contHistory
                    contHistory.Create(history, userId);

                    if (result > 0) LoadDataTransaksi();
                }
            }
            else // data belum dipilih
            {
                MessageBox.Show("Data belum dipilih !!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuShadowPanel9_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile();
            profile.ShowDialog();

            Visible = true;
        }

        private void bunifuShadowPanel1_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void lvwTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            History history = new History();
            history.ShowDialog();

            Visible = false;
        }

        private void Date_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblUser1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearchDetail_Click(object sender, EventArgs e)
        {
            string nama = txtCariDetail.Text.ToString();

            lvwTransaction.Items.Clear();

            listTransaction = controller.ReadByNama(nama);

            foreach(var  transaction in listTransaction)
            {
                var noUrut = lvwTransaction.Items.Count + 1;
                var item = new ListViewItem(noUrut.ToString());
                string namaMethod = transaction.Nama_Method.ToString();
                item.SubItems.Add(namaMethod);
                item.SubItems.Add(transaction.Transaction_category.ToString());
                item.SubItems.Add(transaction.Transaction_date);
                item.SubItems.Add(transaction.Transaction_amount.ToString());
                item.SubItems.Add(transaction.Transaciton_name);

                lvwTransaction.Items.Add(item);
            }

        }

        //untuk membaca detail income pada panel income
        private void pnlIncome_Click(object sender, EventArgs e)
        {
            lvwTransaction.Items.Clear();

            // Ensure that controller.read(user) returns the expected data
            listTransaction = controller.readIncomeOnly(user);

            foreach (var transaction in listTransaction)
            {
                var noUrut = lvwTransaction.Items.Count + 1;

                var item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(transaction.Transaction_id.ToString());
                string namaMethod = transaction.Nama_Method.ToString();
                item.SubItems.Add(namaMethod);
                item.SubItems.Add(transaction.Transaction_category.ToString());
                item.SubItems.Add(transaction.Transaction_date);
                item.SubItems.Add(transaction.Transaction_amount.ToString());
                item.SubItems.Add(transaction.Transaciton_name);

                lvwTransaction.Items.Add(item);
            }
        }

        private void pnlOutcome_Click(object sender, EventArgs e)
        {
            lvwTransaction.Items.Clear();

            // Ensure that controller.read(user) returns the expected data
            listTransaction = controller.readOutcomeOnly(user);

            foreach (var transaction in listTransaction)
            {
                var noUrut = lvwTransaction.Items.Count + 1;

                var item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(transaction.Transaction_id.ToString());
                string namaMethod = transaction.Nama_Method.ToString();
                item.SubItems.Add(namaMethod);
                item.SubItems.Add(transaction.Transaction_category.ToString());
                item.SubItems.Add(transaction.Transaction_date);
                item.SubItems.Add(transaction.Transaction_amount.ToString());
                item.SubItems.Add(transaction.Transaciton_name);

                lvwTransaction.Items.Add(item);
            }

        }
    }
}
