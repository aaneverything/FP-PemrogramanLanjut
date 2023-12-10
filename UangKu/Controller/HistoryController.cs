using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UangKu.Model.Repository;
using UangKu.Model.Context;
using UangKu.Model.Entity;
using UangKu.View;

namespace UangKu.Controller
{
    public class HistoryController
    {
        private HistoryRepository historyRepository;
        public HistoryController(DbContext context)
        {
            historyRepository = new HistoryRepository(context);
        }

        /*public int CreateTransaction(TransactionHistory history, int userId)
        {
            return historyRepository.Create(history, userId);
        }

        public List<TransactionHistory> GetTransactionHistory(int userId)
        {
            return historyRepository.readHistory(userId);
        }

        public List<TransactionHistory> SearchTransactionHistory(string nama)
        {
            return historyRepository.ReadByNama(nama);
        }

        internal List<TransactionHistory> GetTransactionHistory(object userId)
        {
            throw new NotImplementedException();
        }*/

        public List<TransactionHistory> ReadByNama(string nama)
        {
            // membuat objek collection
            List<TransactionHistory> list = new List<TransactionHistory>();

            // membuat objek context menggunakan blok using
            using (DbContext context = new DbContext())
            {
                // membuat objek dari class repository
                historyRepository = new HistoryRepository(context);

                // panggil method ReadByNama yang ada di dalam class repository
                list = historyRepository.ReadByNama(nama);
            }

            return list;
        }

        /// <summary>
        /// Method untuk menampilkan semua data 
        /// </summary>
        /// <returns></returns>
        public List<TransactionHistory> readHistory(int user_id)
        {
            // membuat objek collection
            List<TransactionHistory> list = new List<TransactionHistory>();

            // membuat objek context menggunakan blok using
            using (DbContext context = new DbContext())
            {
                historyRepository = new HistoryRepository(context);
                list = historyRepository.readHistory(user_id);
            }

            return list;
        }
        // method menambahkan data transaksi history
        public int Create(TransactionHistory history, int userId)
        {
            int result = 0;

            using (DbContext context = new DbContext())
            {
                historyRepository = new HistoryRepository(context);
                result = historyRepository.Create(history, userId);
            }

            return result;
        }

    }
}
