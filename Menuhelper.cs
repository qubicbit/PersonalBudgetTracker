namespace PersonalBudgetTracker
{
    // MenuHelper – ansvarar för menyer, konsolhantering och hjälptext
    public static class MenuHelper
    {
        // Visar huvudmenyn med alla tillgängliga alternativ
        public static void ShowMainMenu()
        {
            Console.WriteLine("----Personal Budget Tracker----");
            Console.WriteLine("1. Lägg till transaktion");
            Console.WriteLine("2. Visa alla transaktioner");
            Console.WriteLine("3. Visa total balans");
            Console.WriteLine("4. Ta bort transaktion");
            Console.WriteLine("5. Visa info för en specifik transaktion");
            Console.WriteLine("6. Visa transaktioner per kategori");
            Console.WriteLine("7. Visa statistik: antal transaktioner, total inkomst, total utgift");
            Console.WriteLine("0. Avsluta programmet");
            Console.Write("\nVälj ett alternativ (0-7): ");
        }

        // Visar en meny med unika kategorier från transaktionslistan
        public static List<string> ShowCategoryMenu(BudgetManager budgetManager)
        {

            var categories = budgetManager.Transactions // sorterar transaktioner efter katergorier. 
              .Select(t => t.Category)      // hämta alla katergorier i transaktioner
              .Distinct()                   // Tar bort alla upprepade kategorie
              .OrderBy(c => c)              // sortera dem alfabetiskt
              .ToList();                    // skicka in till listan categories



            Console.WriteLine("\nTillgängliga kategorier:\n");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"[{i}] {categories[i]}"); // printa ut kategorin i listan categories
            }

            return categories;
        }

        // Läser och returnerar användarens menyval
        public static string ReadMainMenuChoice()
        {
            return Console.ReadLine().Trim();
        }

        // Rensar konsolen efter att användaren tryckt Enter
        public static void ClearConsole()
        {
            Console.Write("\nTryck Enter för att fortsätta...");
            Console.ReadKey(true);
            Console.Clear();
        }


        // Skriver ut text med färg beroende på beloppets tecken
        public static void PrintColoredAmount(decimal amount, string text)
        {
            if (amount >= 0)
                Console.ForegroundColor = ConsoleColor.Green; // Inkomst = grön
            else
                Console.ForegroundColor = ConsoleColor.Red;   // Utgift = röd

            Console.WriteLine(text);
            Console.ResetColor(); // Återställ färg till standard
        }
    }
}
