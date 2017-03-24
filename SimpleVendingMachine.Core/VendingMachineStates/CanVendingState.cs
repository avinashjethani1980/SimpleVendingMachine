using System;
using SimpleVendingMachine.Core.Interfaces;
using SimpleVendingMachine.Core.Models;

namespace SimpleVendingMachine.Core.VendingMachineStates
{
    internal class CanVendingState : IVendingMachineState
    {
        private SimpleVendingMachine _vendingMachine;

        public CanVendingState(SimpleVendingMachine _vendingMachine)
        {
            this._vendingMachine = _vendingMachine;
        }

        public void Select(ItemType item)
        {
            throw new Exception("Currently vending an item");
        }

        public void InsertPayment(IPaymentProvider provider, string pin)
        {
            throw new Exception("Currently vending an item");
        }

        public void Vend(IPaymentProvider provider)
        {
            if (provider.UpdateCardBalance(new Payment(0.50m, CurrencyEnum.GBP)))
                ChangeState();
            else
            {
                throw new Exception("Card balance too low or pin invalid");
            }
        }

        private void ChangeState()
        {
            _vendingMachine.State = new ItemSelectedState(_vendingMachine);
        }
    }
}