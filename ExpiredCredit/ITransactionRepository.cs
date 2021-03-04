using System;
using System.Collections.Generic;

namespace ExpiredCredit
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);

        Transaction FirstOrDefault(Func<Transaction, bool> predicate);

        List<Transaction> GetList();
        List<Transaction> GetList(int userId);
        List<Transaction> GetList(Func<Transaction, bool> predicate);
    }
}
