using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Xsl;

namespace SimpleVendingMachine.Core.Models
{
    /// <summary>
    /// Represents a real world account. For simplicity, all properties are generally kept simple i.e. primitive types
    /// </summary>
    public class Account
    {
        private readonly object _syncRoot = new object();
        public Account(string bankName, string userName, Payment currentBalance)
        {
            AccountId = Guid.NewGuid(); // Should this really be a guid. Use a static class to generate the next id
            BankName = bankName;
            UserName = userName;
            CurrentBalance = currentBalance;
            ValidCards = new List<CashCard>();
        }

        public Account(string bankName, string userName) : this(bankName, userName, new Payment(0m, CurrencyEnum.GBP))
        {

        }

        /// <summary>
        /// Id of the account
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Bank name where the account resides
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// User name for the account holder
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Current balance of the account
        /// </summary>
        public Payment CurrentBalance { get; private set; }

        /// <summary>
        /// List of valid cards associated with this account
        /// </summary>
        public List<CashCard> ValidCards { get; private set; }

        /// <summary>
        /// Basic logic to add a cash card to the list of valid cards for the account
        /// </summary>
        /// <param name="card"></param>
        public void AddCashCard(CashCard card)
        {
            if (!ValidCards.Contains(card))
                ValidCards.Add(card);
        }

        /// <summary>
        /// Withdraws the amount in a thread safe manner. Can also be done using the interlocked class
        /// </summary>
        /// <param name="amount"></param>
        public void Withdraw(Payment amount)
        {
            if (amount > CurrentBalance)
                throw new Exception("Amount to be withdrawn cannot be greater than current balance");

            lock (_syncRoot)
            {
                CurrentBalance -= amount;
            }
            
        }

        /// <summary>
        /// Deposits the amount in a thread safe manner. Can also be done using the interlocked class
        /// </summary>
        /// <param name="amount"></param>
        public void Deposit(Payment amount)
        {
            lock (_syncRoot)
            {
                CurrentBalance += amount;
            }
        }

    }
}