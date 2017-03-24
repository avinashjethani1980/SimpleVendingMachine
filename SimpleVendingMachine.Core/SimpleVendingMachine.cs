using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SimpleVendingMachine.Core.Interfaces;
using SimpleVendingMachine.Core.VendingMachineStates;

namespace SimpleVendingMachine.Core
{
    public class SimpleVendingMachine
    {
        /// <summary>
        /// Maximum inventory of the vending machine
        /// </summary>
        private const int MaximumCapacity = 25;

        private int _runningTotal;
        public IVendingMachineState State { get; set; }

        /// <summary>
        /// Vending machine that dispenses cans 
        /// </summary>
        public SimpleVendingMachine()
        {
            State = new ItemSelectedState(this);
        }

        public bool IsEmpty()
        {
            if (_runningTotal >= MaximumCapacity)
                return true;
            return false;
        }

        public void SelectItem(ItemType itemType)
        {
            if (itemType == ItemType.SoftDrink)
                State.Select(itemType);
            else
                throw new Exception("Vending machine currently only supports soft drink cans");
        }

        public void Vend(IPaymentProvider paymentProvider, string pin)
        {
            State.InsertPayment(paymentProvider, pin);
            State.Vend(paymentProvider);
            _runningTotal++;
        }
    }
}
