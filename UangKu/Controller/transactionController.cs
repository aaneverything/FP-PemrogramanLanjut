using System;
using System.Collections.Generic;

using UangKu.Model.Entity;
using UangKu.Model.Context;
using UangKu.Model.Repository;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UangKu.Controller
{

    public class transactionController
    {
        // memanggil repo
        private transactionRepository _repository;

        // method menambahkan data transaksi
        public int Create(Transaction transaksi, int user_id)
        {
            int result = 0;

            if (String.IsNullOrEmpty(transaksi.Transaction_category ) || String.IsNullOrEmpty(transaksi.Transaction_amount.ToString()) || String.IsNullOrEmpty(transaksi.Transaction_date) 
                || String.IsNullOrEmpty(transaksi.Transaciton_name))
            {
                MessageBox.Show("Isi dulu yang lengkap !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                result = _repository.Input(transaksi, user_id);
            }

            return result;
        }

        // method menghapus data transaksi
        public int Delete(int transactionId)
        {
            int result = 0;

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                result = _repository.transactionDelete(transactionId);
            }

            return result;
        }

        public int Update(Transaction transaction) 
        {
            int result = 0;

            if (transaction.Transaction_category != transaction.Transaction_category)
            {
                MessageBox.Show("Category tidak bisa diganti!!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            if (transaction.Transaction_date != transaction.Transaction_date)
            {
                MessageBox.Show("Hari tidak bisa diganti!!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            if (string.IsNullOrEmpty(transaction.Transaciton_name) || string.IsNullOrEmpty(transaction.Transaction_amount.ToString()))
            {
                MessageBox.Show("data harus diisi semua", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            // membuat objek context menggunakan blok using
            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                result = _repository.Update(transaction);
            }

            if (result > 0)
            {
                MessageBox.Show("Data berhasil diupdate !", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Data gagal diupdate !!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return result;
        }

        // method menampilkan data transaksi  berdasarkan user yang menggunakan
        public List<Transaction> read(int user_id)
        {
            List<Transaction> list= new List<Transaction>();

            using (DbContext context = new DbContext()) 
            {
                _repository = new transactionRepository(context);
                list = _repository.readTransaction(user_id);
            }
            return list;
        }

        // method membaca pengeluaran
        public string readOutcome(int user_id)
        {
            string outcome = null;

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                outcome = _repository.readOut(user_id);
            }

            return outcome;
        }

        // method membaca pemasukan
        public string readIncome(int user_id)
        {
            string income = null;

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                income = _repository.readIn(user_id);
            }

            return income;
        }

        // method menghitung jumlah pengeluaran dan pemasukan
        public int readBalance(int user_id)
        {
            int result = 0;

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                result = _repository.balance(user_id);
            }

            return result;
        }
        public List<Transaction> ReadByNama(string nama)
        {
            // membuat objek collection
            List<Transaction> list = new List<Transaction>();

            // membuat objek context menggunakan blok using
            using (DbContext context = new DbContext())
            {
                // membuat objek dari class repository
                _repository = new transactionRepository(context);

                // panggil method ReadByNama yang ada di dalam class repository
                list = _repository.ReadByNama(nama);
            }

            return list;
        }

        public List<Transaction> readIncomeOnly(int user_id)
        {
            List<Transaction> list = new List<Transaction>();

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                list = _repository.readIncomeOnly(user_id);
            }
            return list;
        }

        public List<Transaction> readOutcomeOnly(int user_id)
        {
            List<Transaction> list = new List<Transaction>();

            using (DbContext context = new DbContext())
            {
                _repository = new transactionRepository(context);
                list = _repository.readOutcomeOnly(user_id);
            }
            return list;
        }

    }
}
