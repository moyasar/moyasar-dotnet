using System;
using Moyasar;

namespace MoyasarEasyCli
{
    class Program
    {
        static void Main(string[] args)
        {
            MoyasarService.ApiKey = "sk_test_xNXaMTEADmd8n4WjfFuXHKtKY8ogogzRfnhSg2pG";
            
            var exit = false;
            while (!exit)
            {   
                ClearPrintOutWelcomeDetails();
                exit = PrintOutMainMenu();
            }
        }

        public static void ClearPrintOutWelcomeDetails()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Moyasar Easy Cli!");
            Console.WriteLine("This tool was written to show examples on how to use the library.");
            Console.WriteLine("By Ali Alhoshaiyan (alim.sa) 2019");
            
            if (Moyasar.MoyasarService.ApiKey != null)
            {
                Console.WriteLine($"Moyasar API Key: {Moyasar.MoyasarService.ApiKey}");
            }
        }

        private static bool PrintOutMainMenu()
        {
            Console.WriteLine();
            PrintOutMainMenuOptions();
            Console.WriteLine();
            Console.Write("Please choose an option: ");

            var option = Console.ReadLine();
            try
            {
                if (option != null) return ProcessMainMenuOption(option);
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

        private static void PrintOutMainMenuOptions()
        {
            Console.WriteLine("[1] Set Api Key");
            Console.WriteLine("[2] Payments");
            Console.WriteLine("[3] Invoices");
            Console.WriteLine("[0] Exit");
        }

        private static bool ProcessMainMenuOption(string option)
        {
            switch (option)
            {
                case "0":
                    return true; // Exit
                case "1":
                    SetApiKey();
                    break;
                case "2":
                    PaymentProcessor.InitiatePaymentProcessor();
                    break;
                case "3":
                    InvoiceProcessor.PrintOutInvoiceMenu();
                    break;
                default:
                    throw new Exception();
            }

            return false;
        }

        private static void SetApiKey()
        {
            ClearPrintOutWelcomeDetails();
            Console.WriteLine();
            
            Console.Write("Enter API key: ");
            var key = Console.ReadLine()?.Trim();
            if (String.IsNullOrEmpty(key)) key = null;

            Moyasar.MoyasarService.ApiKey = key;
        }
    }
}