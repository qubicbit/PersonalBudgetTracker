using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;

namespace PersonalBudgetTracker
{
    // Helper – innehåller metoder för att hantera inmatning, visning och borttagning av transaktioner
    public static class Helper
    {
        // Skapar en ny transaktion genom att fråga användaren om beskrivning, belopp, kategori och datum
        public static Transaction GetInput()
        {
            Console.Write("\nBeskrivning: ");
            string description = Console.ReadLine();

            // Loop som säkerställer att användaren matar in ett giltigt decimalvärde
            decimal amount;
            while (true)
            {
                Console.Write("Belopp (positivt för inkomst, negativt för utgift): ");
                if (decimal.TryParse(Console.ReadLine(), out amount))
                    break;

                Console.WriteLine("Ogiltigt belopp. Ange ett numeriskt värde.");
            }

            Console.Write("Kategori: ");
            string category = Console.ReadLine();

            Console.Write("Datum (ÅÅÅÅ-MM-DD): ");
            DateOnly date = DateOnly.Parse(Console.ReadLine());

            Console.WriteLine("\nTransaktion tillagd!\n");

            // Skapar och returnerar ett nytt Transaction-objekt
            return new Transaction(description, amount, category, date);

        }

    }
}
