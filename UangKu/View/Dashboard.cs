using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using UangKu.Controller;
using LiveCharts.WinForms;
using LiveCharts.Definitions.Charts;
using System.Drawing;
using System.Windows.Media;

namespace UangKu.View
{
    public partial class Dashboard : Form
    {
        int user = Login.getUserId;
        private transactionController controller;
        private List<UangKu.Model.Entity.Transaction> transactions;
        
        public Dashboard()
        {
            
            InitializeComponent();
            cartesianChart1.Series.Add(new LineSeries
            {
                StrokeThickness = 4,
                StrokeDashArray = new System.Windows.Media.DoubleCollection(50),
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 195, 79)),
                LineSmoothness = 10,
                PointGeometrySize = 20,
                PointGeometry = null
            });
            controller = new transactionController();
            transactions = controller.read(user);

            lblUser.Text = Login.getName;
            lblUser1.Text = Login.getName;
            lblIncome.Text = $"Rp. {controller.readIncome(user)};";
            lblOutcome.Text = $"Rp. {controller.readOutcome(user)};";
            lblBelence.Text = $"Rp. {controller.readBalance(user)};";
            Date.Value = DateTime.Now;
            rdbBalanceHistory.Checked = true;
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        { 
            this.Dispose();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            Detail detail = new Detail();
            this.Close();
            detail.ShowDialog();

            Visible = false;
        }


       

        private void bunifuShadowPanel1_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile();
            this.Close();
            profile.ShowDialog();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            History history = new History();
            this.Close();
            history.ShowDialog();

            Visible = false;
        }

        private void Date_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rdbIncomeHistory_CheckedChanged(object sender, EventArgs e)
        {
                cartesianChart1.Series.Clear();
                cartesianChart1.Series.Add(new LineSeries
                {
                    StrokeThickness = 4,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(50),
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 187, 23)),
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 175, 225, 175)),
                    LineSmoothness = 10,
                    PointGeometrySize = 20,
                    PointGeometry = null
                });
                
                var values = transactions.Select(t => (double)t.Transaction_amount).ToList();
                string income = controller.readIncome(user);
                double incomeValue = double.Parse(income);
                values.Add(incomeValue);
                cartesianChart1.Series[0].Values = new ChartValues<double>(values);
        }

        private void rdbOutcomeHistory_CheckedChanged(object sender, EventArgs e)
        {
            
            cartesianChart1.Series.Clear();
            cartesianChart1.Series.Add(new LineSeries
            {
                StrokeThickness = 4,
                StrokeDashArray = new System.Windows.Media.DoubleCollection(50),
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 20, 60)),
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 248, 131, 121)),
                LineSmoothness = 10,
                PointGeometrySize = 20,
                PointGeometry = null
            });
            cartesianChart1.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 248, 131, 121));
            var values = transactions.Select(t => (double)t.Transaction_amount).ToList();
            string Outcome = controller.readOutcome(user);
            double OutcomeValue = double.Parse(Outcome);
            values.Add(OutcomeValue);
            cartesianChart1.Series[0].Values = new ChartValues<double>(values);
        }

        private void rdbBalanceHistory_CheckedChanged(object sender, EventArgs e)
        {
            cartesianChart1.Series.Clear();
            cartesianChart1.Series.Add(new LineSeries
            {
                StrokeThickness = 4,
                StrokeDashArray = new System.Windows.Media.DoubleCollection(50),
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 71, 171)),
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 100, 149, 237)),
                //Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 149, 237)),//
                LineSmoothness = 10,
                PointGeometrySize = 20,
                PointGeometry = null
            });
            cartesianChart1.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 100, 149, 237));
            var values = transactions.Select(t => (double)t.Transaction_amount).ToList();
            int Balance = controller.readBalance(user);
            double BalanceValue = (double)Balance;
            values.Add(BalanceValue);
            cartesianChart1.Series[0].Values = new ChartValues<double>(values);
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {

        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
