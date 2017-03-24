namespace SimpleVendingMachine.Core
{
    public interface IPaymentValidator
    {
        bool IsValidPin(string pin);
    }
}