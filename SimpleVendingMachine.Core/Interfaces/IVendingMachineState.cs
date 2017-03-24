using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVendingMachine.Core.Interfaces
{
    public interface IVendingMachineState
    {
        void Select(ItemType item);
        void InsertPayment(IPaymentProvider provider, string pin);
        void Vend(IPaymentProvider provider);
    }

}
