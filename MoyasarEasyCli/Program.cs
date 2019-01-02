using System;

namespace MoyasarEasyCli
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {   
                ClearPrintOutWelcomeDetails();
                exit = PrintOutMainMenu();
            }
        }

        private static void ClearPrintOutWelcomeDetails()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Moyasar Easy Cli!");
            Console.WriteLine("This tool was written to show examples on how to use the library.");
            Console.WriteLine("By Ali Alhoshaiyan (alim.sa) 2019");
            
            if (Moyasar.Moyasar.ApiKey != null)
            {
                Console.WriteLine($"Moyasar API Key: {Moyasar.Moyasar.ApiKey}");
            }
        }

        private static bool PrintOutMainMenu()
        {
            Console.WriteLine();
            PrintOutMainMenuOptions();
            Console.WriteLine();
            Console.Write("Please choose an option: ");

            var option = Console.ReadKey().KeyChar.ToString()?.Trim().ToLower();
            try
            {
                if (option != null) return ProcessMainMenuOption(option);
                else
                {
                    throw new Exception();
                }
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
            Console.WriteLine("[2] Create Payment");
            Console.WriteLine("[3] Fetch Payment");
            Console.WriteLine("[4] List Payment");
            Console.WriteLine("[0] Exit");
        }

        private static bool ProcessMainMenuOption(string option)
        {
            if (option == "0")
            {
                return true; // Exit
            }
            else if(option == "1")
            {
                SetApiKey();
            }
            else
            {
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

            Moyasar.Moyasar.ApiKey = key;
        }
    }
}