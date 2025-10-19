using PersonalBudgetTracker;

namespace PersonalBudgetTracker
{
    // BudgetManager – ansvarar för att hantera en lista av transaktioner och tillhörande logik
    public class BudgetManager
    {
        // Lista som innehåller alla transaktioner
        public List<Transaction> Transactions { get; set; }

        // Konstruktor – initierar en tom lista av transaktioner
        public BudgetManager()
        {
            Transactions = new List<Transaction>();
        }

        // Lägger till en ny transaktion i listan
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        // Skriver ut alla transaktioner med index, datum, beskrivning, kategori och belopp ocg färg
        public void ShowTransactions(List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0) // tom eller inga transaktioner
            {
                Console.WriteLine("Inga transaktioner att visa.\n");
                return;
            }

           Console.WriteLine("\nTransaktioner:");
            int i = 0;
            foreach (var transaction in transactions)
            {
                string text = $"[{i++}] {transaction.Date}: {transaction.Description}, {transaction.Category}, {transaction.Amount} kr";
                MenuHelper.PrintColoredAmount(transaction.Amount, text);
            }
        }

        // Räknar ut total balans genom att summera alla belopp i Transactions
        public decimal CalculateBalance()
        {
            decimal totalBalance = 0;
            foreach (var transaction in Transactions)
            {
                totalBalance += transaction.Amount;
            }
            return totalBalance;
        }

        // Tar bort en specifik transaktion från listan
        public bool DeleteTransaction(Transaction transaction)
        {
            return Transactions.Remove(transaction);
        }

    }
}
