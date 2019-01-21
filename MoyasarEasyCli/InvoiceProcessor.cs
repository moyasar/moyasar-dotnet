using System;
using Moyasar.Exceptions;
using Moyasar.Models;
using Moyasar.Services;

namespace MoyasarEasyCli
{
    public static class InvoiceProcessor
    {
        public static void InitiateInvoiceProcessor()
        {
            while (!PrintOutInvoiceMenu()) { }
        }
        
        public static bool PrintOutInvoiceMenu()
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            Console.WriteLine("[1] Create Invoice");
            Console.WriteLine("[2] Fetch Invoice");
            Console.WriteLine("[3] List Invoices");
            Console.WriteLine("[0] Go Back");
            Console.WriteLine();
            Console.Write("Please choose an option: ");

            var option = Console.ReadLine();
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

        private static bool ProcessInvoiceMenuOption(string option)
        {
            switch (option)
            {
                case "0":
                    return true;
                case "1":
                    CreateInvoice();
                    break;
                case "2":
                    FetchInvoice();
                    break;
                case "3":
                    ListInvoices();
                    break;
            }
            
            return false;
        }

        private static void CreateInvoice()
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
                    Console.WriteLine("Invalid value!");
                }
            }

            Console.Write("Currency [default=SAR]: ");
            var currency = Console.ReadLine();
            if (string.IsNullOrEmpty(currency)) currency = "SAR";

            Console.Write("Description [optional]: ");
            var desc = Console.ReadLine();
            if (string.IsNullOrEmpty(desc)) desc = null;
            
            Console.Write("Callback URL: ");
            var callback = Console.ReadLine();
            
            // Read and parse date
            
            Invoice invoice;
            try
            {
                invoice = Invoice.Create(new InvoiceInfo()
                {
                    Amount = amount,
                    Currency = currency,
                    Description = desc,
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

            PrintOutInvoice(invoice);
        }

        private static void FetchInvoice()
        {
            throw new NotImplementedException();
        }

        private static void ListInvoices()
        {
            throw new NotImplementedException();
        }
        
        private static void PrintOutInvoice(Invoice payment)
        {
            throw new NotImplementedException();
        }
    }
}