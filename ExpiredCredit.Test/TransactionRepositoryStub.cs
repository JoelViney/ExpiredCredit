using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpiredCredit.Test
{
    public class TransactionRepositoryStub : ITransactionRepository
    {
        private readonly List<Transaction> _list = new List<Transaction>();

        public void Add(Transaction transaction)
        {
            this._list.Add(transaction);
        }

        public List<Transaction> GetList()
        {
            return this._list;
        }

        public List<Transaction> GetList(int userId)
        {
            return this._list.Where(x => x.UserId == userId).ToList();
        }

        public List<Transaction> GetList(Func<Transaction, bool> predicate)
        {
            return _list.Where(predicate).ToList();
        }


        public Transaction FirstOrDefault(Func<Transaction, bool> predicate)
        {
            return _list.FirstOrDefault(predicate);
        }
    }
}
