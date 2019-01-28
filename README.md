# Moyasar.Net

[![Build Status](https://travis-ci.org/aliio/moyasar-dotnet.svg?branch=master)](https://travis-ci.org/aliio/moyasar-dotnet)

Moyasar's .NET Client Library


# Target Frameworks

This library targets following frameworks:

1. .Net Standard 2.0
2. .Net Framework 4.0


# Installation

If you are using `dotnet` command line tools you can add the library to
your project using the following command

```bash
dotnet add package moyasar
```

Or if you are using Nuget Package Manager

```powershell
PM> Install-Package moyasar
```


# Manual Installation

`Please note that this installation method is not recommended`

To Install the library manually please download the last release from
the releases section and reference it in your project.


# Usage

### Setup

Make sure to set the API key before proceeding

```csharp
MoyasarService.ApiKey = "YouKeyHere";
```

### Payment

To create a new Payment use the following:

```csharp
try
{
    var payment = Payment.Create(new PaymentInfo
    {
        Amount = 200,
        Currency = "SAR",
        Description = "Colombia, Narino Sandona, Medium Roast",
        Source = new CreditCardSource()
        {
            Name = "John Doe",
            Number = "4111111111111111",
            Cvc = 141,
            Month = 3,
            Year = 2021,
        },
        CallbackUrl = "http://www.example.com/payment_succeeded/"
    });
}
catch (ValidationException)
{
}
catch (NetworkException)
{
}
catch (ApiException)
{
}
```

To create a payment for Sadad, use the following for `Source`:
```csharp
new SadadSource()
{
    UserName = "johndoe123"
};
```

`Make sure you always try to catch the exceptions above`

To fetch a payment from Moyasar, use the following:

```csharp
Payment.Fetch("Payment-Id");
```

To refund a payment, one must have a Payment instance `somePayment` then
invoke the following:

```csharp
somePayment.Refund();
```

To update your payment, change `Description` property on that payment,
then invoke `Update`:

```csharp
somePayment.Description = "Colombia, Narino Sandona, Medium Roast (Special)";
somePayment.Update();
```

To list or search for payments at Moyasar, do the following:

```csharp
var result = Payment.List();
```

or

```csharp
var result = Payment.List(new SearchQuery
{
    Id = "SomeId",
    Source = "creditcard OR sadad",
    Status = "some status",
    Page = 2,
    CreatedAfter = DateTime.Now.AddDays(-5),
    CreatedBefore = DateTime.Now
});
```

All `SearchQuery` parameters are optional, use what is needed

### Invoice

Use `Invoice` class with the same methods as `Payment` class, except
for the following:

To create an invoice for example:

```csharp
var invoice = Invoice.Create(new InvoiceInfo
{
    Amount = 7000,
    Currency = "SAR",
    Description = "A 70 SAR invoice just because",
    ExpiredAt = DateTime.Now.AddDays(3),
    CallbackUrl = "http://www.example.com/invoice_callback"
});
```

To Cancel an Invoice:

```csharp
someInvoice.Cancel();
```

For more details, please refer to the official documentation: https://moyasar.com/docs/api/

# Testing

To run the tests use the following command

```bash
dotnet test
```