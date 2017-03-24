using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleVendingMachine.Core.Interfaces;

namespace SimpleVendingMachine.Core.VendingMachineStates
{
    public class EmptyState : IVendingMachineState
    {
        public EmptyState(SimpleVendingMachine machine)
        {
            
        }
        public void InsertPayment(IPaymentProvider provider, string pin)
        {
            throw new NotImplementedException();
        }

        public void Select(ItemType item)
        {
            throw new NotImplementedException();
        }

        public void Vend(IPaymentProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
