using System;

// class Payment
// {
//     public virtual void Pay()
//     {
//         Console.WriteLine("Processing payment");
//     }
// }

public class CreditCardPayment : IPayment
{
    public void Pay()
    {
        Console.WriteLine("Payment done using Credit Card");
    }
}

public class UpiPayment : IPayment
{
    public void Pay()
    {
        Console.WriteLine("Payment done using UPI");
    }
}

public class CashPayment : IPayment
{
    public void Pay()
    {
        Console.WriteLine("Payment done using Cash");
    }
}

