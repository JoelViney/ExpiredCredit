using System;
using System.Linq;

namespace ExpiredCredit
{
    public class TransactionService
    {
        private readonly ITransactionRepository _repository;

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public void Add(Transaction transaction)
        {
            _repository.Add(transaction);
        }

        public decimal GetBalance(int userId)
        {
            var list = _repository.GetList(userId);

            return list.Sum(x => x.Amount);
        }

        public void ExpireCredit(int userId, DateTime dateTime)
        {
            var lastYear = dateTime.AddYears(-1);

            var transactions = _repository.GetList(userId);
            var lastYearsCredit = transactions.Where(x => x.DateCreated > lastYear && x.UserId == userId && x.Type == TransactionType.Credit).ToList();
            var expiredCreditList = transactions.Where(x => x.DateCreated <= lastYear && x.UserId == userId).OrderByDescending(o => o.DateCreated).ToList();

            decimal totalBalance = transactions.Sum(x => x.Amount);
            decimal totalLastYearsCredit = lastYearsCredit.Sum(x => x.Amount);
            decimal expiredBalance = totalBalance - totalLastYearsCredit;

            while (expiredBalance > 0)
            {
                foreach (var expiredCredit in expiredCreditList)
                {
                    var amount = expiredCredit.Amount;
                    if (expiredBalance < amount)
                    {
                        amount = expiredBalance;
                    }

                    var transaction = new Transaction()
                    {
                        UserId = expiredCredit.UserId,
                        DateCreated = dateTime,
                        Type = TransactionType.ExpiredCredit,
                        Amount = 0 - amount
                    };

                    this._repository.Add(transaction);

                    expiredBalance -= amount;
                }            
            }
        }
    }
}
