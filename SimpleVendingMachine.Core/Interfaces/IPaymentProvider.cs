using SimpleVendingMachine.Core.Models;

namespace SimpleVendingMachine.Core
{
    public interface IPaymentProvider
    {
        Payment MinimumBalance { get;}
        Payment CardBalance { get;}
        bool HasBalance();
        bool IsValidPin(string pin);
        bool UpdateCardBalance(Payment payment);
    }
}