using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleVendingMachine.Core.Interfaces;

namespace SimpleVendingMachine.Core.VendingMachineStates
{
    public class ItemSelectedState : IVendingMachineState
    {
        private readonly SimpleVendingMachine _vendingMachine;

        public ItemSelectedState(SimpleVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
        }

        public void InsertPayment(IPaymentProvider provider, string pin)
        {
            throw new Exception("Please select an item first");
        }

        public void Select(ItemType item)
        {
            if (!_vendingMachine.IsEmpty())
            {
                ChangeState();
            }
            else throw new Exception("Vending machine cannot vend more cans");
        }

        public void Vend(IPaymentProvider provider)
        {
            throw new Exception("Please select an item first");
        }

        private void ChangeState()
        {
            _vendingMachine.State = new CardInsertedState(_vendingMachine);
        }
    }
}

