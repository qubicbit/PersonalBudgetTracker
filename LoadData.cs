namespace PersonalBudgetTracker
{
    // LoadData – ansvarar för att ladda in testdata till BudgetManager
    public static class LoadData
    {
        // TransactionsData – fyller BudgetManager med fördefinierade transaktioner
        public static bool TransactionsData(BudgetManager budgetManager)
        {
            // Skapar en lista med exempeltransaktioner (inkomster och utgifter)
            var Transactions = new List<Transaction>
            {
                new Transaction("Lön", 22000, "Inkomst", new DateOnly(2025, 10, 1)),
                new Transaction("Lägenhet", -8200, "Boende", new DateOnly(2025, 10, 2)),
                new Transaction("Elräkning", -950, "Boende", new DateOnly(2025, 10, 3)),
                new Transaction("ICA Maxi", -450, "Mat", new DateOnly(2025, 10, 4)),
                new Transaction("Hemköp", -320, "Mat", new DateOnly(2025, 10, 5)),
                new Transaction("SL månadskort", -970, "Transport", new DateOnly(2025, 10, 6)),
                new Transaction("Bensin", -700, "Transport", new DateOnly(2025, 10, 7)),
                new Transaction("Spotify", -109, "Underhållning", new DateOnly(2025, 10, 8)),
                new Transaction("Netflix", -159, "Underhållning", new DateOnly(2025, 10, 9)),
                new Transaction("Frilansuppdrag", 4800, "Inkomst", new DateOnly(2025, 10, 10)),
                new Transaction("Middag ute", -550, "Mat", new DateOnly(2025, 10, 11)),
                new Transaction("Gymkort", -299, "Hälsa", new DateOnly(2025, 10, 12)),
                new Transaction("Apotek", -220, "Hälsa", new DateOnly(2025, 10, 13)),
                new Transaction("Bonus", 3000, "Inkomst", new DateOnly(2025, 10, 14)),
                new Transaction("Vattenräkning", -400, "Boende", new DateOnly(2025, 10, 15))
            };

            
            // slumpa om listan, för att datumen är sorterade på den ursprunliga listan
            var shuffledTransaction= Transactions.OrderBy(t => Guid.NewGuid()).ToList();

            // Lägger till varje transaktion i BudgetManager
            foreach (var t in shuffledTransaction)
            {
                budgetManager.AddTransaction(t);
            }

            Console.WriteLine("Laddar in testdata...");

            return true; // Bekräftar att data har laddats
        }

    }
}
