namespace PersonalBudgetTracker
{
    // Statistic – ansvarar för att beräkna och visa statistik över transaktioner
    public class Statistic
    {
        //ShowStatistics – skriver ut en sammanfattning av transaktionsdata
        public static void ShowStatistics(BudgetManager budgetManager)
        {
            // skapar en lokal lista som innehåller alla transaktioner
            var transactions = budgetManager.Transactions;

            // Antal transaktioner
            int count = transactions.Count;
            decimal totalIncome = 0;
            decimal totalExpense = 0;

            //Summerar alla inkomster(positiva belopp)
            foreach (var transaction in transactions)
            {
                if (transaction.Amount > 0) 
                {
                    totalIncome += transaction.Amount;
                }
                if (transaction.Amount < 0)
                {
                    totalExpense += transaction.Amount;
                }
            }
            decimal totalsaldo = totalIncome + totalExpense;

            // Skriver ut statistiköversikt i konsolen
            Console.WriteLine("\n📊 Statistiköversikt:\n");
            Console.WriteLine($"🧾 Antal transaktioner: {count}");
            Console.WriteLine($"💰 Total inkomst: {totalIncome:N2} kr");
            Console.WriteLine($"💸 Total utgift: {Math.Abs(totalExpense):N2} kr");
            Console.WriteLine($"🧮 Nettosaldo: {totalsaldo:N2} kr"); // netto, inkomst-utgifter
        }
    }
}
