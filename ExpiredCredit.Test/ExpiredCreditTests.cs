using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ExpiredCredit.Test
{
    [TestClass]
    public class ExpiredCreditTests
    {
        // Credit is applied and still applied.
        [TestMethod]
        public void ActiveCreditTest()
        {
            // Arrange
            var repository = new TransactionRepositoryStub();
            var service = new TransactionService(repository);
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 01), Amount = 100, Type = TransactionType.Credit });

            // Act
            service.ExpireCredit(1, new DateTime(2000, 01, 02));

            // Assert
            var balance = service.GetBalance(1);
            Assert.AreEqual(100m, balance);
        }

        // The credit is expired.
        [TestMethod]
        public void ExpiredCreditTest()
        {
            // Arrange
            var repository = new TransactionRepositoryStub();
            var service = new TransactionService(repository);
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 01), Amount = 100, Type = TransactionType.Credit });

            // Act
            service.ExpireCredit(1, new DateTime(2001, 01, 01));

            // Assert
            var balance = service.GetBalance(1);
            Assert.AreEqual(0m, balance);
        }

        // Some credit is expired, some is still active.
        [TestMethod]
        public void ExpiredAndActiveCreditTest()
        {
            // Arrange
            var repository = new TransactionRepositoryStub();
            var service = new TransactionService(repository);
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 01), Amount = 100, Type = TransactionType.Credit });
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 02), Amount = 100, Type = TransactionType.Credit });

            // Act
            service.ExpireCredit(1, new DateTime(2001, 01, 01));

            // Assert
            var balance = service.GetBalance(1);
            Assert.AreEqual(100m, balance);
        }

        // 100 credit, used 50, need to expire 50
        [TestMethod]
        public void ExpiredPartiallyUsedCreditTest()
        {
            // Arrange
            var repository = new TransactionRepositoryStub();
            var service = new TransactionService(repository);
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 01), Amount = 100, Type = TransactionType.Credit });
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 02), Amount = -50, Type = TransactionType.Debit });

            // Act
            service.ExpireCredit(1, new DateTime(2001, 01, 01));

            // Assert
            var balance = service.GetBalance(1);
            Assert.AreEqual(0m, balance);
        }

        // 100 credit that is expired, 100 credit that isn't expired, used 50, need to expire 50
        [TestMethod]
        public void ExpiredAndActivePartiallyUsedCreditTest()
        {
            // Arrange
            var repository = new TransactionRepositoryStub();
            var service = new TransactionService(repository);
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 01), Amount = 100, Type = TransactionType.Credit });
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 02), Amount = 100, Type = TransactionType.Credit });
            service.Add(new Transaction() { UserId = 1, DateCreated = new DateTime(2000, 01, 02), Amount = -50, Type = TransactionType.Debit });

            // Act
            service.ExpireCredit(1, new DateTime(2001, 01, 01));

            // Assert
            var balance = service.GetBalance(1);
            Assert.AreEqual(100m, balance);
        }
    }
}
