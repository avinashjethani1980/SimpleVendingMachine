using System;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace SimpleVendingMachine.Core.Models
{
    /// <summary>
    /// Represents a cash card
    /// </summary>
    public class CashCard
    {
        public CashCard(Account account, string pin)
        {
            Account = account;
            PinNumber = pin;
            CardNumber = CardNumberGenerator.Create16DigitString();

            //Keep track of the cash cards associated with the account
            account.AddCashCard(this);

            MinimumLimit = new Payment(0.50m, CurrencyEnum.GBP);
        }

        private Account Account { get; set; }

        public Payment CurrentBalance
        {
            get
            {
                return Account.CurrentBalance;
            }
        }

        #region CardProperties

        public string CardNumber { get; set; }

        public string CvvNumber { get; set; }

        public string OwnerName { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Payment MinimumLimit { get; private set; }
        
        public string PinNumber { get; set; }

        #endregion

        public void UpdateCardBalance(Payment amount)
        {
            var tempAmount = CurrentBalance - amount;
            if (tempAmount < MinimumLimit)
                throw new Exception("Card balance cannot be less than Minimum Limit");
                        
            Account.Withdraw(amount);
        }

        public bool IsValid(string pin)
        {
            return String.Equals(pin, PinNumber, StringComparison.OrdinalIgnoreCase);
        }
    }
}