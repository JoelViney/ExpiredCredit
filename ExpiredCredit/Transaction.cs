using System;

namespace ExpiredCredit
{
    public enum TransactionType
    {
        Credit,
        Debit,
        ExpiredCredit
    }

    public class Transaction
    {
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
