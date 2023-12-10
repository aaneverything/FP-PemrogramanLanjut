using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UangKu.Controller;
using UangKu.Model.Context;
using Bunifu.Framework.UI;
using UangKu.Model.Entity;
using UangKu.Model.Repository;
using Bunifu.UI.WinForms;
using System.Runtime.Remoting.Contexts;
using System;

namespace UangKu.View
{
    public partial class History : Form
    {
        int user = Login.getUserId;
        private HistoryController contrHistory;
        List<TransactionHistory> listOfHistory = new List<TransactionHistory>();


        public History()
        {
            InitializeComponent();
            contrHistory = new HistoryController(new DbContext());


            lblUser.Text = Login.getName;
            lblUser1.Text = Login.getName;
            Date.Value = DateTime.Now;



            InisialisasiHistory();
            LoadDataHistory();
        }

        private void InisialisasiHistory()
        {
            lsvHistory.View = System.Windows.Forms.View.Details;
            lsvHistory.FullRowSelect = true;
            lsvHistory.GridLines = true;

            lsvHistory.Columns.Add("No", 40, HorizontalAlignment.Center);
            //lsvHistory.Columns.Add("Transaksi ID",110, HorizontalAlignment.Center);
            lsvHistory.Columns.Add("History Name ", 330, HorizontalAlignment.Center);
            lsvHistory.Columns.Add("Date", 330, HorizontalAlignment.Center);
            lsvHistory.Columns.Add("Amount", 330, HorizontalAlignment.Center);
        }        

        private void LoadDataHistory()
        {
            try
            {
                lsvHistory.Items.Clear();
                listOfHistory = contrHistory.readHistory(user);

                string selectedFilter = filterHistory.SelectedItem?.ToString();
                listOfHistory = ApplyFilter(listOfHistory, selectedFilter);

                if (listOfHistory.Count == 0)
                {
                    MessageBox.Show("No history data found for the user.");
                }

                foreach (var history in listOfHistory)
                {
                    var noUrut = lsvHistory.Items.Count + 1;
                    var item = new ListViewItem(noUrut.ToString());
                    // item.SubItems.Add(history.Transaction_id.ToString());
                    item.SubItems.Add(history.Nama_History);
                    item.SubItems.Add(history.Transaction_date.ToString("dd/MM/yyyy"));
                    item.SubItems.Add(history.Transaction_amount.ToString());
                    // tampilkan data history ke listview
                    lsvHistory.Items.Add(item);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history data: {ex.Message}");
            }    
        }

        private List<TransactionHistory> ApplyFilter(List<TransactionHistory> historyList, string filterOption)
        {
            DateTime currentDate = DateTime.Now;

            switch (filterOption)
            {
                case "Today":
                    return historyList.Where(h => h.Transaction_date.Date == currentDate.Date).ToList();
                case "By Month":
                    return historyList.Where(h => h.Transaction_date.Month == currentDate.Month).ToList();
                case "By Year":
                    return historyList.Where(h => h.Transaction_date.Year == currentDate.Year).ToList();
                case "View All":
                    return historyList;
                default:
                    return historyList;
            }
        }




        private void btnDetail_Click(object sender, EventArgs e)
        {
            Detail detail = new Detail();
            detail.ShowDialog();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard home = new Dashboard();
            home.ShowDialog();

            Visible = false;
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile();
            profile.ShowDialog();

            Visible = true;
        }

        private void lsvHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearchHistory_Click(object sender, EventArgs e)
        {
            string nama = txtCariHistory.Text.ToString();

            lsvHistory.Items.Clear();

            listOfHistory = contrHistory.ReadByNama(nama);

            foreach (var history in listOfHistory)
            {
                var noUrut = lsvHistory.Items.Count + 1;
                var item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(history.Transaction_id.ToString());
                item.SubItems.Add(history.Nama_History);
                item.SubItems.Add(history.Transaction_date.ToString("dd/MM/yyyy"));
                item.SubItems.Add(history.Transaction_amount.ToString());
                //tampilkan data history ke listview
                lsvHistory.Items.Add(item);
            }
        }

        private void filterHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataHistory();
        }

        private void bunifuGradientPanel4_Click(object sender, EventArgs e)
        {

        }
    }
 }