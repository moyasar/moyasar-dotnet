using System;
using System.Linq;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Models;
using Moyasar.Services;

namespace MoyasarEasyCli
{
    public static class PaymentProcessor
    {
        public static void InitiatePaymentProcessor()
        {
            while (!PrintOutPaymentMenu()) { }
        }
        
        public static bool PrintOutPaymentMenu()
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            Console.WriteLine("[1] Create Payment");
            Console.WriteLine("[2] Fetch Payment");
            Console.WriteLine("[3] List Payments");
            Console.WriteLine("[0] Go Back");
            Console.WriteLine();
            Console.Write("Please choose an option: ");

            var option = Console.ReadKey().KeyChar.ToString()?.Trim().ToLower();
            try
            {
                if (option != null) return ProcessPaymentMenuOption(option);
                throw new Exception();
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("Error: Invalid option");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            return false;
        }

        private static bool ProcessPaymentMenuOption(string option)
        {
            switch (option)
            {
                case "0":
                    return true;
                case "1":
                    CreatePayment();
                    break;
                case "2":
                    FetchPayment();
                    break;
                case "3":
                    ListPayments();
                    break;
                default:
                    throw new Exception();
            }
            
            return false;
        }

        private static void FetchPayment()
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();

            var id = "";
            while (string.IsNullOrEmpty(id))
            {
                Console.Write("Payment Id: ");
                id = Console.ReadLine();
            }

            Payment payment;
            try
            {
                payment = Payment.Fetch(id);
            }
            catch (NetworkException)
            {
                Console.WriteLine("Could not connect to Internet");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            catch (ApiException e)
            {
                Console.WriteLine($"Api Exception:\n{e}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            PrintOutPayment(payment);
        }
        
        private static void ListPayments()
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            Console.WriteLine("Retrieving payments");
            
            var payments = Payment.List();

            if (!payments.Items.Any())
            {
                Console.WriteLine("No payments found");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                while (true)
                {
                    Program.ClearPrintOutWelcomeDetails();
                    Console.WriteLine();

                    for (int i = 0; i < payments.Items.Count; ++i)
                    {
                        Console.WriteLine($"[{i + 1}] {payments.Items[i].Id}");
                    }
                    
                    Console.WriteLine("[0] Go Back");

                    var choice = 0;
                    while (true)
                    {
                        Console.WriteLine();
                        Console.Write("Please choose an option: ");
                        try
                        {
                            choice = int.Parse(Console.ReadLine());
                            break;
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (choice == 0) break;
                    if (choice < 0 || choice > payments.Items.Count)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        PrintOutPayment(payments.Items[choice - 1]);
                    }
                }
            }
        }

        private static void CreatePayment()
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();

            int amount; 
            while (true)
            {
                Console.Write("Amount: ");
                try
                {
                    amount = int.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Value!");
                }
            }

            Console.Write("Currency [default=SAR]: ");
            var currency = Console.ReadLine();
            if (string.IsNullOrEmpty(currency)) currency = "SAR";

            Console.Write("Description [optional]: ");
            var desc = Console.ReadLine();
            if (string.IsNullOrEmpty(desc)) desc = null;

            var source = GetPaymentSource();
            
            Console.Write("Callback URL: ");
            var callback = Console.ReadLine();
            
            Payment payment;
            try
            {
                payment = Payment.Create(new PaymentInfo
                {
                    Amount = amount,
                    Currency = currency,
                    Description = desc,
                    Source = source,
                    CallbackUrl = callback
                });
            }
            catch (ValidationException e)
            {
                Console.WriteLine("Incorrect Data:");
                e.FieldErrors.ForEach(err =>
                {
                    Console.WriteLine($"\t- {err.Field}: {err.Error}");
                });
                
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            catch (NetworkException)
            {
                Console.WriteLine("Could not connect to Internet");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            catch (ApiException e)
            {
                Console.WriteLine($"Api Exception:\n{e}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            PrintOutPayment(payment);
        }

        private static IPaymentSource GetPaymentSource()
        {   
            var choice = 1;
            while (true)
            {
                try
                {
                    Console.Write("Credit Card [1 default] or Sadad [2]: ");
                    var val = Console.ReadLine();
                    if (string.IsNullOrEmpty(val)) break;
                    
                    choice = int.Parse(val);
                    if (choice == 1 || choice == 2) break;
                    
                    Console.WriteLine("Invalid choice");
                }
                catch
                {
                    Console.WriteLine("Ex: Invalid choice");
                }
            }

            if (choice == 1)
            {
                Console.Write("Name: ");
                var name = Console.ReadLine();
                
                Console.Write("Number: ");
                var number = Console.ReadLine();
                
                int cvc; 
                while (true)
                {
                    Console.Write("Cvc: ");
                    try
                    {
                        cvc = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Value!");
                    }
                }
                
                int year; 
                while (true)
                {
                    Console.Write("Year: ");
                    try
                    {
                        year = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Value!");
                    }
                }
                
                int month; 
                while (true)
                {
                    Console.Write("Month: ");
                    try
                    {
                        month = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Value!");
                    }
                }

                return new CreditCardSource
                {
                    Name = name,
                    Number = number,
                    Cvc = cvc,
                    Year = year,
                    Month = month
                };
            }
            else
            {
                Console.WriteLine("Username: ");
                var username = Console.ReadLine();
                
                return new SadadSource
                {
                    UserName = username
                };
            }
        }
        
        private static void PrintOutPayment(Payment payment)
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            
            Console.WriteLine($"Id: {payment.Id}");
            Console.WriteLine($"Status: {payment.Status}");
            Console.WriteLine($"Amount: {payment.Amount.ToString()}");
            Console.WriteLine($"Formatted Amount: {payment.FormattedAmount}");
            Console.WriteLine($"Fee: {payment.Fee.ToString()}");
            Console.WriteLine($"Formatted Fee: {payment.FormattedFee}");
            
            if (payment.RefundedAt.HasValue)
            {
                Console.WriteLine($"Refunded Amount: {payment.RefundedAmount.ToString()}");
                Console.WriteLine($"Formatted Refunded Amount: {payment.FormattedRefundedAmount}");
                Console.WriteLine($"Refunded At: {payment.RefundedAt?.ToString("O")}");
            }
            
            Console.WriteLine($"Currency: {payment.Currency}");
            Console.WriteLine($"Description: {payment.Description}");

            if (payment.InvoiceId != null)
            {
                Console.WriteLine($"Invoice Id: {payment.InvoiceId}");
            }
            
            if (payment.Ip != null)
            {
                Console.WriteLine($"IP Address: {payment.Ip}");
            }
            
            if (payment.CallbackUrl != null)
            {
                Console.WriteLine($"Callback URL: {payment.CallbackUrl}");
            }
            
            if (payment.CreatedAt.HasValue)
            {
                Console.WriteLine($"Refunded At: {payment.CreatedAt?.ToString("O")}");
            }
            
            if (payment.UpdatedAt.HasValue)
            {
                Console.WriteLine($"Refunded At: {payment.UpdatedAt?.ToString("O")}");
            }

            Console.WriteLine("Payment Method:");
            ShowPaymentMethod(payment.Source);
            
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void ShowPaymentMethod(PaymentMethod paymentMethod)
        {
            switch (paymentMethod)
            {
                case CreditCard cc:
                    Console.WriteLine($"\t- Name: {cc.Name}");
                    Console.WriteLine($"\t- Number: {cc.Number}");
                    Console.WriteLine($"\t- Company: {cc.Company}");
                    Console.WriteLine($"\t- Message: {cc.Message}");
                    Console.WriteLine($"\t- Transaction URL: {cc.TransactionUrl}");
                    break;
                case Sadad sadad:
                    Console.WriteLine($"\t- UserName: {sadad.UserName}");
                    break;
            }
        }
    }
}