using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleVendingMachine.Core.Models;

namespace SimpleVendingMachine.Core.Tests
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void Account_OnDepositAmount_DepositsAmountSuccessfully()
        {
            Account a = new Account("HSBC", "Avinash Jethani",
                                        new Payment(10000m, CurrencyEnum.GBP));
            Thread t1 = new Thread(() => DepositManyPayments(a));
            Thread t2 = new Thread(() => DepositManyPayments(a));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Assert.That(a.CurrentBalance, Is.EqualTo(new Payment(14000m, CurrencyEnum.GBP)));
        }

        private void DepositManyPayments(Account account)
        {
            for (int i = 0; i < 100; i++)
            {
                account.Deposit(new Payment(20, CurrencyEnum.GBP));
            }
        }

        [Test]
        public void Account_OnWithDrawAmountAtTheSameTime_WithdrawsAmountSuccessfully()
        {
            Account a = new Account("HSBC", "Avinash Jethani",
                                        new Payment(10000m, CurrencyEnum.GBP));
            Thread t1 = new Thread(() => WithdrawManyPayments(a));
            Thread t2 = new Thread(() => WithdrawManyPayments(a));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Assert.That(a.CurrentBalance, Is.EqualTo(new Payment(6000m, CurrencyEnum.GBP)));

        }

        private void WithdrawManyPayments(Account account)
        {
            for (int i = 0; i < 100; i++)
            {
                account.Withdraw(new Payment(20, CurrencyEnum.GBP));
            }
        }
    }

}
