using System.Numerics;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalBudgetTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Aktiverar stöd för symboler i konsolen (t.ex. emojis)


            BudgetManager budgetManager = new(); // Skapar en instans av BudgetManager

            LoadData.TransactionsData(budgetManager); // Laddar in transaktionsdata till budgetManager

            MenuHelper.ClearConsole(); // Rensar konsolen

            bool EndProgram = false; // Flagga för att avsluta programmet

            while (!EndProgram) // Loopar tills användaren väljer att avsluta
            {
                MenuHelper.ShowMainMenu(); // Visar huvudmenyn

                EndProgram = switchBlock(budgetManager, EndProgram); // Kör menyval och uppdaterar avslutsflagga

                MenuHelper.ClearConsole(); // Rensar konsolen efter varje menyval
            }

        }

        private static bool switchBlock(BudgetManager budgetManager, bool EndProgram)
        {
            switch (MenuHelper.ReadMainMenuChoice()) // Läser användarens menyval
            {
                case "1":
                    var transaction = Helper.GetTransactionInput(); // Hämtar användarens inmatning som en transaktion
                    budgetManager.AddTransaction(transaction); // Lägger till transaktionen i listan
                    break;

                case "2":
                    var transactions = budgetManager.Transactions;
                    budgetManager.ShowTransactions(transactions); // Visar alla transaktioner
                    break;

                case "3":
                    decimal totalBalance = budgetManager.CalculateBalance();
                    Console.WriteLine($"\nTotal balans: {totalBalance:N2} kr"); // Visar total balans (inkomster - utgifter)
                    break;

                case "4":
                    Helper.DeleteTransactionByIndex(budgetManager); // Tar bort en transaktion baserat på index
                    break;

                case "5":
                    Helper.ShowTransactionInfo(budgetManager); // Visar detaljerad information om en specifik transaktion
                    break;

                case "6":
                    Helper.ShowTransactionsBySelectedCategoryLoop(budgetManager); // Visar transaktioner per kategori med färgkodning och möjlighet att välja flera kategorier
                    break;

                case "7":
                    Helper.ShowTransactionsGroupedByCategory(budgetManager); // Visar statistik: antal transaktioner, total inkomst, total utgift
                    break;

                case "8":
                    Statistic.ShowStatistics(budgetManager); // Visar statistik: antal transaktioner, total inkomst, total utgift
                    break;

                case "0":
                    Console.WriteLine("\nProgrammet avslutas.\n"); // Avslutar programmet
                    EndProgram = true;
                    break;

                default:
                    Console.WriteLine("\nOgiltigt val.\n"); // Felmeddelande vid ogiltigt menyval
                    break;
            }

            return EndProgram; // Returnerar om programmet ska avslutas
        }
    }
}

