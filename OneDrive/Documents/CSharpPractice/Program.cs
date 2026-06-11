using System;
class Program
{
    public static void Main()
    {
        // Payment payment;

        // payment = new CreditCardPayment();
        // payment.Pay();

        // payment = new UpiPayment();
        // payment.Pay();

        // payment = new CashPayment();
        // payment.Pay();

        IPayment payment = new CreditCardPayment();
        var injector = new DependencyInjector(payment);
        injector.checkout();

        payment = new UpiPayment();
        injector = new DependencyInjector(payment);
        injector.checkout();

        payment = new CashPayment();
        injector = new DependencyInjector(payment);
        injector.checkout();
    }
}