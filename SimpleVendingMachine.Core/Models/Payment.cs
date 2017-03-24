using System;

namespace SimpleVendingMachine.Core.Models
{
    public class Payment
    {
        public Payment()
        {
            Amount = 0m;
            Currency = CurrencyEnum.GBP;
        }

        public Payment(decimal amount, CurrencyEnum currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public CurrencyEnum Currency { get; }

        public override string ToString()
        {
            return String.Format("{0}{1}", Amount, Currency);
        }

        #region Equals

        public override bool Equals(object obj)
        {
            var payment = obj as Payment;
            if (payment == null)
                return false;

            if (this.Currency == payment.Currency && this.Amount == payment.Amount)
                return true;

            return false;

        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode() + Currency.GetHashCode();
        }

        #endregion

        #region Operator overloading
        public static bool operator >(Payment p1, Payment p2)
        {
            if (p1.Currency == p2.Currency)
                return p1.Amount > p2.Amount;
            else
                throw new Exception(
                    "Currently, cannot compare payments of different currency. This can be implemented using a payment converter");
        }

        public static bool operator <(Payment p1, Payment p2)
        {
            if (p1.Currency == p2.Currency)
                return p1.Amount < p2.Amount;
            else
                throw new Exception(
                    "Currently, cannot compare payments of different currency. This can be implemented using a payment converter");
        }

        public static Payment operator -(Payment p1, Payment p2)
        {
            if (p1.Currency == p2.Currency)
                return new Payment(p1.Amount - p2.Amount, p1.Currency);
            else
                throw new Exception(
                    "Currently, cannot subtract payments of different currency. This can be implemented using a payment converter");
        }

        public static Payment operator +(Payment p1, Payment p2)
        {
            if (p1.Currency == p2.Currency)
                return new Payment(p1.Amount + p2.Amount, p1.Currency);
            else
                throw new Exception(
                    "Currently, cannot add payments of different currency. This can be implemented using a payment converter");
        }
        #endregion
    }
}