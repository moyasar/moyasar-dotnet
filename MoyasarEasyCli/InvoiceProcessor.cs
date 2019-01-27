using System;
using System.Collections.Generic;
using System.Linq;
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

        private static bool ProcessPaymentMenuOption(string option)
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
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            
            var id = "";
            while (string.IsNullOrEmpty(id))
            {
                Console.Write("Invoice Id: ");
                id = Console.ReadLine();
            }

            Invoice invoice;
            try
            {
                invoice = Invoice.Fetch(id);
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

        private static void ListInvoices()
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            Console.WriteLine("Retrieving invoices");

            PaginationResult<Invoice> invoices;
            try
            {
                invoices = Invoice.List();
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
            
            if (!invoices.Items.Any())
            {
                Console.WriteLine("No invoices found");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                while (true)
                {
                    Program.ClearPrintOutWelcomeDetails();
                    Console.WriteLine();
                    
                    Console.WriteLine($"Page {invoices.CurrentPage.ToString()} of {invoices.TotalPages.ToString()}");

                    for (int i = 0; i < invoices.Items.Count; ++i)
                    {
                        Console.WriteLine($"[{i + 1}] {invoices.Items[i].Id}");
                    }
                    
                    Console.WriteLine("[0] Go Back");

                    if (invoices.NextPage.HasValue)
                    {
                        Console.WriteLine("[>] Next Page");
                    }
                    
                    if (invoices.PreviousPage.HasValue)
                    {
                        Console.WriteLine("[<] Previous Page");
                    }

                    var choice = "0";
                    while (true)
                    {
                        Console.WriteLine();
                        Console.Write("Please choose an option: ");
                        try
                        {
                            choice = Console.ReadLine();
                            break;
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (choice == "0") break;
                    if (choice == ">" && invoices.NextPage.HasValue)
                    {
                        invoices = invoices.GetNextPage();
                    }
                    else if(choice == "<" && invoices.PreviousPage.HasValue)
                    {
                        invoices = invoices.GetPreviousPage();
                    }
                    else if (int.Parse(choice) < 0 || int.Parse(choice) > invoices.Items.Count)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        PrintOutInvoice(invoices.Items[int.Parse(choice) - 1]);
                    }
                }
            }
        }
        
        private static void PrintOutInvoice(Invoice invoice)
        {
            Program.ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            
            Console.WriteLine($"Id: {invoice.Id}");
            Console.WriteLine($"Status: {invoice.Status}");
            Console.WriteLine($"Amount: {invoice.Amount.ToString()}");
            Console.WriteLine($"Formatted Amount: {invoice.FormattedAmount}");
            Console.WriteLine($"Currency: {invoice.Currency}");
            Console.WriteLine($"Description: {invoice.Description}");
            Console.WriteLine($"Logo Url: {invoice.LogoUrl}");
            Console.WriteLine($"Url: {invoice.Url}");
            Console.WriteLine($"Expired At: {invoice.ExpiredAt?.ToString("O")}");
            Console.WriteLine($"Created At: {invoice.CreatedAt?.ToString("O")}");
            Console.WriteLine($"Updated At: {invoice.UpdatedAt?.ToString("O")}");
            Console.WriteLine($"Callback Url: {invoice.CallbackUrl}");

            if (invoice.Payments != null && invoice.Payments.Any())
            {
                Console.WriteLine("Payments: [");
                invoice.Payments.ForEach(p => Console.WriteLine($"  '{p.Id}'"));
                Console.WriteLine("]");
            }
            
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}