using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Rhino.Mocks;
using SimpleVendingMachine.Core.Models;
using SimpleVendingMachine.Core.VendingMachineStates;

namespace SimpleVendingMachine.Core.Tests
{
    /// <summary>
    /// Tests for the Simple Vending Machine class. I have developed the vending machine using 
    /// TDD i.e. using the RED GREEN REFACTOR methodlogy i.e. 
    ///        Red: Create a test and make it fail. 
    ///        Green: Make the test pass by writing minimal code. 
    ///        Refactor: Refactor the code to improve the design while ensuring that all tests still pass.
    /// All tests are written in the AAA syntax i.e. Arrange Act Assert
    /// </summary>
    [TestFixture]
    public class SimpleVendingMachineTests
    {
        /// <summary>
        /// Requirement 1: Cant vend more than 25 cans
        /// </summary>
        [Test]
        public void SimpleVendingMachine_OnRequestToVendOfMoreThan25Cans_ThrowsException()
        {
            var paymentProvider = MockRepository.GenerateMock<IPaymentProvider>();
            paymentProvider.Expect(x => x.HasBalance()).Return(true);
            paymentProvider.Expect(x => x.IsValidPin(Arg<string>.Is.NotNull)).Return(true);
            paymentProvider.Expect(x => x.UpdateCardBalance(Arg<Payment>.Is.Anything)).Return(true);
            SimpleVendingMachine machine = new SimpleVendingMachine();
            for (int i = 0; i < 25; i++)
            {
                machine.SelectItem(ItemType.SoftDrink);
                machine.Vend(paymentProvider, "12345");
            }
            Assert.Throws<Exception>(() => machine.SelectItem(ItemType.SoftDrink));
        }

        /// <summary>
        /// Requirement 2 : Cant vend if less than 50p on the card 
        /// </summary>
        [Test]
        public void SimpleVendingMachine_OnRequestToVendWithLessThan50ponCard_ThrowsException()
        {
            var paymentProvider = MockRepository.GenerateMock<IPaymentProvider>();
            paymentProvider.Expect(x => x.HasBalance()).Return(false);
            SimpleVendingMachine machine = new SimpleVendingMachine();
            Assert.Throws<Exception>(() => machine.Vend(paymentProvider, "12345"));
        }


        /// <summary>
        /// Requirement 2 : Cant vend if PIN supplied is invalid
        /// </summary>
        [Test]
        public void SimpleVendingMachine_OnRequestToVendWithInvalidPin_ThrowsException()
        {
            var paymentProvider = MockRepository.GenerateMock<IPaymentProvider>();
            paymentProvider.Expect(
                    //x => x.HasBalance(Arg<Payment>.Matches(y => y.Amount < .50m && y.Currency == CurrencyEnum.GBP)))
                    x => x.HasBalance())
                .Return(true);
            paymentProvider.Expect(x => x.IsValidPin(Arg<string>.Is.NotNull)).Return(false);
            SimpleVendingMachine machine = new SimpleVendingMachine();
            Assert.Throws<Exception>(() => machine.Vend(paymentProvider, "12345"));
        }

        /// <summary>
        /// Requirement 3. Cash card balance should be update when a can is bought. 
        /// This test will ensure that the UpdateCardBalance is called when a can is bought. 
        /// The actual calculation of the card balance is delegated as per SRP to payment provider.
        /// </summary>
        [Test]
        public void SimpleVendingMachine_OnVend_CallsUpdatesCardBalance()
        {
            var paymentProvider = MockRepository.GenerateMock<IPaymentProvider>();
            paymentProvider.Expect(x => x.HasBalance()).Return(true);
            paymentProvider.Expect(x => x.IsValidPin(Arg<string>.Is.NotNull)).Return(true);
            paymentProvider.Expect(x => x.UpdateCardBalance(Arg<Payment>.Is.NotNull)).Return(true);
            SimpleVendingMachine machine = new SimpleVendingMachine();
            machine.State = new CardInsertedState(machine);
            machine.Vend(paymentProvider, "12345");

            paymentProvider.VerifyAllExpectations();
        }


        /// <summary>
        /// Requirement 3. Cash card balance should be update when a can is bought. 
        /// This test ensures that the card balance is updated when the update card balance is called on payment provider
        /// </summary> 
        [Test]
        public void PaymentProvider_OnUpdateCardBalance_UpdatesCardBalance()
        {
            IPaymentProvider paymentProvider =
                new CashCardPaymentProvider(
                    new CashCard(new Account("HSBC", "Avinash Jethani", new Payment(1m, CurrencyEnum.GBP)),
                                "12345"));
            var isValid = paymentProvider.IsValidPin("12345");
            var updatesCardBalance = paymentProvider.UpdateCardBalance(new Payment(0.5m, CurrencyEnum.GBP));

            Assert.That(isValid, Is.EqualTo(true));
            Assert.That(updatesCardBalance, Is.EqualTo(true));
            Assert.That(paymentProvider.CardBalance, Is.EqualTo(new Payment(0.5m, CurrencyEnum.GBP)));
        }

        [Test]
        public void SimpleVendingMachine_OnMultipleCashCardsLinkedtoSingleAccount_UpdatesAccountSuccessfully()
        {
            var account = new Account("HSBC", "Avinash Jethani", new Payment(1000m, CurrencyEnum.GBP));
            var cashCard1 = new CashCard(account, "12345");
            var cashCard2 = new CashCard(account, "67890");
            IPaymentProvider paymentProvider1 = new CashCardPaymentProvider(cashCard1);
            IPaymentProvider paymentProvider2 = new CashCardPaymentProvider(cashCard2);

            SimpleVendingMachine machine = new SimpleVendingMachine();
            machine.SelectItem(ItemType.SoftDrink);
            machine.Vend(paymentProvider1, "12345");
            
            machine.SelectItem(ItemType.SoftDrink);
            machine.Vend(paymentProvider2, "67890");

            Assert.That(cashCard1.CurrentBalance, Is.EqualTo(new Payment(999m, CurrencyEnum.GBP)));
            Assert.That(cashCard2.CurrentBalance, Is.EqualTo(new Payment(999m, CurrencyEnum.GBP)));
            Assert.That(account.CurrentBalance, Is.EqualTo(new Payment(999m, CurrencyEnum.GBP)));
        }

    }


}
