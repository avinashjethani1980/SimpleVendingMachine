using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleVendingMachine.Core.Interfaces;

namespace SimpleVendingMachine.Core.VendingMachineStates
{
    public class CardInsertedState : IVendingMachineState
    {
        private SimpleVendingMachine _vendingMachine;

        public CardInsertedState(SimpleVendingMachine vendingMachine)
        {
            this._vendingMachine = vendingMachine;
        }

        public void InsertPayment(IPaymentProvider provider, string pin)
        {
            if (!provider.HasBalance())
                throw new Exception(String.Format("Cannot vend if balance is lower than {0}", provider.MinimumBalance));

            if (!provider.IsValidPin(pin))
                throw new Exception("Invalid Pin provided");
            
            ChangeState();
        }

        public void Select(ItemType item)
        {
            throw new Exception("Item has already been selected");
        }

        public void Vend(IPaymentProvider provider)
        {
            throw new Exception("Item has already been selected");
        }

        private void ChangeState()
        {
            _vendingMachine.State = new CanVendingState(_vendingMachine);
        }
    }
}
