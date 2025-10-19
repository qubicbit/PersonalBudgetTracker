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
        public static Transaction GetTransactionInput()
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

            // Skapar och returnerar ett nytt Transaction-objekt
            Transaction transaction = new Transaction(description, amount, category, date);

            Console.WriteLine("\nTransaktion tillagd!\n");
            return transaction;
        }

        // Tar bort en transaktion baserat på användarens valda index
        public static bool DeleteTransactionByIndex(BudgetManager budgetManager)
        {
            // hämtar alla tillgänliga tranaktioner
            var transactions = budgetManager.Transactions;

            // visar alla transaktioner
            budgetManager.ShowTransactions(transactions);

            // Hämtar transaktionen som användaren vill ta bort via index
            var transaction = GetTransactionByIndex(budgetManager);

            // Om indexet är ogiltigt, avbryt
            if (transaction == null)
            {
                Console.WriteLine("Ogiltigt index.");
                return false;
            }

            // Försöker ta bort transaktionen från listan
            bool removed = budgetManager.DeleteTransaction(transaction);

            // Bekräftar om borttagningen lyckades
            Console.WriteLine(removed ? "\nTransaktion borttagen.\n" : "\nKunde inte ta bort transaktionen.\n");
            return removed;
        }


        public static bool CheckTransactionsIndex(BudgetManager budgetManager, out int index)

        {   // läs in index från användare, kolla om 3 villkor uppfylls, om så index ut, med true eller false.
            return int.TryParse(Console.ReadLine(), out index)  // läser in index från användaren, converterar till int, kollar om det är en int
                && index >= 0                                   // index som läses in större än lika med 0
                && index < budgetManager.Transactions.Count;    // indexet som läses in måste vara mindre än antal transaktioner, alltså att en transaktion existerar

        }


        // Hämtar en transaktion från listan baserat på användarens indexinmatning
        public static Transaction? GetTransactionByIndex(BudgetManager budgetManager) // transaction kan vara null
        {
            // Kontrollera om det finns några transaktioner i listan
            if (budgetManager.Transactions.Count == 0)
                return null;

            // Be användaren ange ett index
            Console.Write("\nAnge index: ");

            // Försök läsa och validera indexet, finns transaktionen ?
            int index;
            bool isValidIndex = CheckTransactionsIndex(budgetManager, out index); // om finns, true. index ut

            // Om indexet är giltigt, returnera motsvarande transaktion
            if (isValidIndex)
            {
                return budgetManager.Transactions[index];
            }

            // Om indexet är ogiltigt, returnera null
            return null;
        }


        // Visar detaljerad information om en vald transaktion
        public static void ShowTransactionInfo(BudgetManager budgetManager)
        {
            Console.WriteLine("\nVälj en transaktion att visa mer information om:");

            // hämtar alla tillgänliga tranaktioner
            var transactions = budgetManager.Transactions;

            // visar alla transaktioner
            budgetManager.ShowTransactions(transactions);

            // hämtar en transaktion via metoden GetTransactionByIndex som användaren valt via index. 
            var transaction = GetTransactionByIndex(budgetManager);

            if (transaction != null) // om transaktionen inte är tom/"" 
                transaction.ShowInfo(); // visa detalj om en transaktion
            else
                Console.WriteLine("\nOgiltigt index. Ingen transaktion kunde visas.\n");
        }

        // Loopar visning av transaktioner per kategori tills användaren väljer att avsluta
        public static void ShowTransactionsBySelectedCategoryLoop(BudgetManager budgetManager)
        {
            while (true)
            {
                ShowTransactionsBySelectedCategory(budgetManager); // visar meny för kategori och hanterar logiken via index om en kategori väljs

                Console.Write("Vill du visa en annan kategori? (j/n): ");
                string askYesOrNo = Console.ReadLine().ToLower().Trim();

                // Bryt loopen om användaren inte svarar "j"
                if (!askYesOrNo.Equals("j", StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }

        // Visar transaktioner för en vald kategori, sorterade efter datum, 
        public static void ShowTransactionsBySelectedCategory(BudgetManager budgetManager)
        {
            // Hämtar och visar tillgängliga kategorier
            var categories = MenuHelper.ShowCategoryMenu(budgetManager); // skapar en lokal variabel categories. hämtar en lista över alla kategorier i transactions, kopierar in i categories

            // Om inga kategorier finns, avsluta metoden
            if (categories.Count == 0) // längden på listan
            {
                Console.WriteLine("\nInga kategorier tillgängliga.\n");
                return; // tillbaka till metoden
            }

            Console.Write("\nVälj en kategori (index): "); // frågar user om en index

            // om indexet för kategoring upfyller alla tre villkoren då körs blocket
            // läser in string från användaren,
            string input = Console.ReadLine();

            int index;

            //konverterar det till int med int.TryParse().
            //om konverteringen lyckas, dvs om användaren skriver ett giltig string tal, retunerar int.TryPare() ett true och sätter värdet på index.
            //om misslyckas, tex. en tom sträng/bokstäver eller annat då returen false och värdet på index blir 0.
            bool isValidIndex = int.TryParse(input, out index);

            if (isValidIndex                     //se om konverteringen lyckades 
                && index >= 0                   // användare matar in ett index som ska vara större än och lika med 0. giltig index som ska finnas i transaktionslistan 
                && index < categories.Count)    // indexet måste vara mindre än antal kategorier. Dvs, kategorier måste finnas 
            {
                string selectedCategory = categories[index]; // den valda katergorin, exempel Mat.


                // skapar en lokal variabel. med alla transaktioner tillgängliga, behövs för filtreringen.
                var transactions = budgetManager.Transactions;

                //--------------------filtrera och sortera--------------

                //steg 1.Filter(Where()): alla transaktioner med avseende på den valda category i en transaktion(t)
                //behövs ingen tom lista här.where() går söker direkt i listan transactions.
                var transactionsByCategory = transactions.Where(t => t.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)) // Jämför strängarna utan att bry sig om versaler/gemener, Skiftlägesokänslig
                                                         .ToList(); // skapar och sparar alla de transaktioner som uppfyller villkoret selectedCategory i listan transactionsByCategory


                //steg 2.Sortering: Ordna den valda filtrerade kategorin(filteredByCategory) så att de sorteras i stigande datum också i varje transaktion(t)
                var transactionsByCategorySortedByDate = transactionsByCategory.OrderBy(t => t.Date).ToList();


                // kopierar in namnet i en ny variable:
                var filtered = transactionsByCategorySortedByDate;

                Console.WriteLine($"\nTransaktioner i kategori \"{selectedCategory}\":\n");

                // visa transaktioner för vald kategori och sorterade efter datum 
                budgetManager.ShowTransactions(filtered);


                //summera belopp efter vald kategory
                decimal totalSumCategory = 0;
                foreach (var transaction in filtered)
                {
                    totalSumCategory += transaction.Amount;
                }

                // Visar totalsumma för vald kategori
                Console.WriteLine($"\nTotalt belopp: {totalSumCategory:N2} kr\n");
            }
            else
            {
                Console.WriteLine("\nOgiltigt val.");
            }
        }


        // Visar alla transaktioner sorterade efter kategori och datum, med totalsummor, Samma princip som ovan
        public static void ShowTransactionsGroupedByCategory(BudgetManager budgetManager)
        {
            // för att kunna sortera behöver anropa transactions
            var transactions = budgetManager.Transactions;

            // sorterade efter kategori och datum
            var grouped = transactions.OrderBy(t => t.Category)      // sorterar hela listan efter kategori
                 .ThenBy(t => t.Date)                               // sorterar inom varje kategorin efter datum
                 .GroupBy(t => t.Category);                         // grupperar transaktioner (delar upp) efter kategori, ej sortering, retur på en sekvens av grupper.
                                                                    // varje grupp har key: kategori och en uppsättning av object. Typen för grouped:  IEnumerable<IGrouping<string, Transaction>> grouped


            var totalsPerCategory = new Dictionary<string, decimal>(); // en ny Dictionary för att kunna spara totalGroup, som ska printas ut senare

            foreach (var group in grouped) // loop över alla gruppkategori som finns i grouped  
            {
                decimal totalGroup = 0; // nollställer summan för varje gruppkategori

                Console.WriteLine($"Kategori: {group.Key}"); // skriver ut kategorin i varje grupp.

                budgetManager.ShowTransactions(group.ToList()); // visar grupper av transaktioner sorterade. Skickas till en lista för att metoden tar emot en lista

                // summera total belopp för varje grupp
                //decimal totalGroup = group.Sum(t => t.Amount); // alternativ, istället för loop
                foreach (var transaction in group)
                {
                    totalGroup += transaction.Amount; // summan för varje gruppkategori 
                }

                // skriver ut summan för varje gruppkategori
                Console.WriteLine($"    Totalt för {group.Key}: {totalGroup:N2} kr\n");

                // sparar summan för varje group i totalsPerCategory som är en Dictionary. Används senare för att printa ut.
                totalsPerCategory[group.Key] = totalGroup; 
            }

            Console.WriteLine("Sammanställning per kategori:\n");

            foreach (var s in totalsPerCategory.OrderByDescending(s => s.Value)) // sorterar så att största värdet kommer först, och minskar sedan
            {
                string text = $"- {s.Key}: {s.Value:N2} kr"; // skapar en text sträng

                MenuHelper.PrintColoredAmount(s.Value, text); // printar ut varje gruppkategori med summan
            }

            // Nettosumma för alla transaktioner 
            Console.WriteLine($"\n🧮 Nettosaldo: {totalsPerCategory.Values.Sum():N2} kr"); // netto, inkomst-utgifter




        }



    }
}
