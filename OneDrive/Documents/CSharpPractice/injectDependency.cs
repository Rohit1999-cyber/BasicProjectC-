using System;
class DependencyInjector
{
    private readonly IPayment _payment;
    
    public DependencyInjector(IPayment payment)
    {
        _payment = payment;
    }

    public void checkout()
    {
        _payment.Pay();
    }
}