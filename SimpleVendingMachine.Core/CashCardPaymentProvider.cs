using SimpleVendingMachine.Core.Models;

namespace SimpleVendingMachine.Core
{
    /// <summary>
    /// Cash Card Payment Provider. Currently only supports GBP currencies
    /// </summary>
    public class CashCardPaymentProvider : IPaymentProvider
    {
        private readonly CashCard _card;
        public CashCardPaymentProvider(CashCard card)
        {
            _card = card;
        }

        public Payment MinimumBalance
        {
            get { return _card.MinimumLimit; }
        }

        public Payment CardBalance
        {
            get { return _card.CurrentBalance; }
        }

        public bool IsValid { get; private set; }

        public bool HasBalance()
        {
            return CardBalance > MinimumBalance;
        }

        public bool IsValidPin(string pin)
        {
            IsValid = _card.IsValid(pin);
            return IsValid;
        }


        /// <summary>
        /// Card balance is updated whenever a can is bought. 
        /// </summary>
        /// <returns></returns>
        public bool UpdateCardBalance(Payment payment)
        {
            if (HasBalance() && IsValid)
            {
                _card.UpdateCardBalance(payment);
                return true;
            }

            return false;
        }
    }
}