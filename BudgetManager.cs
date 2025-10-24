using System.Transactions;
using PersonalBudgetTracker;

namespace PersonalBudgetTracker
{
    // BudgetManager – ansvarar för att hantera en lista av transaktioner och tillhörande logik
    public class BudgetManager
    {
        // Lista som innehåller alla transaktioner
        public List<Transaction> Transactions { get; set; }

        // lägger till en aktiv lista för att kunna skicka in godtyckligt lista, ex en filtrerad version av Transaktions lista
        // gör detta för att slippa skicka in List<Transaction> transactions som inparameter på metoderna.
        private List<Transaction> _activeList;


        // Konstruktor – initierar en tom lista av transaktioner
        public BudgetManager()
        {
            Transactions = new List<Transaction>();
            _activeList = new List<Transaction>();
        }

        // metod för att byta aktiv lista. Behöver det till filtrering
        public void SetActiveList(List<Transaction> list)
        {
            _activeList = list;
        }


        // Lägger till en ny transaktion i listan
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        // Skriver ut alla transaktioner med index, datum, beskrivning, kategori och belopp och färg
        public void ShowTransactions()
        {
            if (_activeList == null || _activeList.Count == 0) // tom eller inga transaktioner
            {
                Console.WriteLine("Inga transaktioner att visa.\n");
                return;
            }

            Console.WriteLine("\nTransaktioner:");
            int i = 0;
            foreach (var transaction in _activeList)
            {
                string text = $"[{i++}] {transaction.Date}: {transaction.Description}, {transaction.Category}, {transaction.Amount} kr";
                MenuHelper.PrintColoredAmount(transaction.Amount, text);
            }
        }

        // summerar alla belopp i listan Transactions
        public decimal CalculateBalance()
        {
            return _activeList.Sum(t => t.Amount);
        }


        // Tar bort en transaktion baserat på användarens valda index
        public bool DeleteTransaction()
        {
            bool removed = false;

            // visar alla transaktioner
            ShowTransactions();

            // Hämtar transaktionen som användaren vill ta bort via index
            var transaction = GetTransaction();

            if (transaction == null)
            {
                Console.WriteLine("\nIngen giltig transaktion hittades.\n");
                return false;

            }

            // Försöker ta bort transaktionen från listan
            removed = Transactions.Remove(transaction);

            // Bekräftar om borttagningen lyckades
            Console.WriteLine(removed ? "\nTransaktion borttagen.\n" : "\nKunde inte ta bort transaktionen.\n");
            return removed;
        }

        // Hämtar en transaktion från listan baserat på användarens indexinmatning
        public Transaction? GetTransaction() // transaction kan vara null
        {
            // Kontrollera om det finns några transaktioner i listan
            if (_activeList.Count == 0)
                return null;

            // hämta index
            int index = CheckIndex();

            // skicka ut transaction
            return _activeList[index];

        }

        // checkar index för typen List<Transaction>
        public int CheckIndex()
        {
            // loopar tills användare skriver rätt index
            while (true)
            {
                Console.Write("\nAnge index: ");
                bool isValidInput = int.TryParse(Console.ReadLine(), out int index);
                
                //Existerar en transaktion?
                if (isValidInput && index >= 0 && index < _activeList.Count)
                    return index;
                else
                    Console.WriteLine("Ogiltig inmatning.");
            }
        }

        // behöver ny metod Checkindex för kategori listan (List string), printar listan, och checkar användaren val av index
        public int CheckIndex(List<string> items)
        {
            Console.WriteLine("\nTillgängliga kategorier:\n");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"[{i}] {items[i]}");
            }

            // loopar tills användare skriver rätt index
            while (true)
            {
                Console.Write("\nAnge index: ");
                bool isValidInput = int.TryParse(Console.ReadLine(), out int index);

                if (isValidInput && index >= 0 && index < items.Count)
                    return index;

                Console.WriteLine("Ogiltig inmatning.");
            }
        }

        // Visar detaljerad information om en vald transaktion
        public void ShowTransactionInfo()
        {
            Console.WriteLine("\nVälj en transaktion att visa mer information om:");

            // visar alla transaktioner
            ShowTransactions();

            // hämtar en transaktion via metoden GetTransaction() som användaren valt via index. 
            var transaction = GetTransaction();

            if (transaction != null)     //kolla om transaktionen inte är tom/"" 
                transaction.ShowInfo(); // visa detalj om en transaktion
            else
                Console.WriteLine("\nIngen giltig transaktion hittades.\n");
        }


        // Loopar visning av transaktioner per kategori tills användaren väljer att avsluta
        public void TransactionsByCategoryLoop()
        {
            while (true)
            {
                // visar meny för kategori och hanterar logiken via index om en kategori väljs
                TransactionsByCategory(); 

                Console.Write("Vill du visa en annan kategori? (j/n): ");
                string askYesOrNo = Console.ReadLine().ToLower().Trim();

                // Bryt loopen om användaren inte svarar "j"
                if (!askYesOrNo.Equals("j", StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }

        // Visar transaktioner för en vald kategori, sorterade efter datum, 
        public void TransactionsByCategory()
        {
            // Hämtar och visar tillgängliga kategorier
            // skapar en lokal variabel categories. Hämtar en lista över alla kategorier i transactions, kopierar in i categories

            var categories = Transactions // sorterar transaktioner efter katergorier. 
            .Select(t => t.Category)      // hämta alla katergorier i transaktioner
            .Distinct()                   // Tar bort alla upprepade kategorier
            .OrderBy(c => c)              // sortera dem alfabetiskt
            .ToList();                    // skicka in till listan categories (List<string>)


            // Om inga kategorier finns, avsluta metoden
            if (categories.Count == 0) // längden på listan
            {
                Console.WriteLine("\nInga kategorier tillgängliga.\n");
                return; // tillbaka till metoden
            }

            Console.Write("\nVälj en kategori (index): ");

            // meny för kategori visas och index matas ut
            int index = CheckIndex(categories);

            // user har nu valt en kategori från meny
            string selectedCategory = categories[index];

            //--------------------filtrera och sortera--------------

            //steg 1. Filter(Where()): Alla transaktioner med avseende på den valda category i en transaktion(t)
            //behövs ingen tom lista här, where() går söker direkt i listan Transactions.
            var transactionsByCategory = Transactions.Where(t => t.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)) // Jämför strängarna utan att bry sig om versaler/gemener, Skiftlägesokänslig
                                                     .ToList(); // skapar och sparar alla de transaktioner som uppfyller villkoret selectedCategory i listan transactionsByCategory


            //steg 2.Sortering: Ordna den valda filtrerade kategorin(filteredByCategory) så att de sorteras i stigande datum också i varje transaktion(t)
            var transactionsByCategorySortedByDate = transactionsByCategory.OrderBy(t => t.Date).ToList();

            // kopierar in namnet i en ny variable:
            var filtered = transactionsByCategorySortedByDate;

            Console.WriteLine($"\nTransaktioner i kategori \"{selectedCategory}\":\n");

            // byter lista så att jag kan använda metoder
            SetActiveList(filtered);

            // visa transaktioner för vald kategori och sorterade efter datum 
            ShowTransactions();

            //summera belopp efter vald kategory
            decimal totalSumCategory = CalculateBalance();

            // Visar totalsumma för vald kategori
            Console.WriteLine($"\nTotalt belopp: {totalSumCategory:N2} kr\n");

            // återställ till original listan
            SetActiveList(Transactions);


        }


        //  Visar alla transaktioner grupperade efter kategori och sorterade efter datum, med totalsummor
        public void TransactionsGroupedByCategory()
        {
            // sorterade efter kategori och datum
            var grouped = Transactions.OrderBy(t => t.Category)      // Sorterar hela listan efter kategori
                 .ThenBy(t => t.Date)                                // Ordnar sedan stigande efter datum
                 .GroupBy(t => t.Category);                         //  grupperar (delar upp) efter kategori, ej sortering, retur på en sekvens av grupper.
                                                                    // varje grupp har key: kategori och en uppsättning av object. Typen för grouped:  IEnumerable<IGrouping<string, Transaction>> grouped


            // en ny Dictionary för att kunna spara totalGroup, som ska printas ut senare
            var totalsPerCategory = new Dictionary<string, decimal>();

            foreach (var group in grouped) // loop över alla gruppkategorier som finns i grouped  
            {
                decimal totalGroup = 0; // nollställer summan för varje gruppkategori

                Console.WriteLine($"Kategori: {group.Key}"); // skriver ut kategorin i varje grupp.

                // byter lista
                SetActiveList(group.ToList());

                // visar grupper av transaktioner sorterade. 
                ShowTransactions();

                // hämtar balance för varje gruppkategori
                totalGroup = CalculateBalance();

                // skriver ut summan för varje gruppkategori
                Console.WriteLine($"    Totalt för {group.Key}: {totalGroup:N2} kr\n");

                // sparar summan för varje group i totalsPerCategory som är en Dictionary. Används senare för att printa ut.
                totalsPerCategory[group.Key] = totalGroup;

                // tillbaka till ursprunliga listan för att grouped jobbar med Transactions
                SetActiveList(Transactions);
            }


            Console.WriteLine("Sammanställning per kategori:\n");

            foreach (var s in totalsPerCategory.OrderByDescending(s => s.Value)) // sorterar så att största värdet kommer först, och minskar sedan
            {
                string text = $"- {s.Key}: {s.Value:N2} kr"; // skapar en text sträng

                MenuHelper.PrintColoredAmount(s.Value, text); // printar ut varje gruppkategori med summan
            }

            // Nettosumma för alla transaktioner 
            Console.WriteLine($"\n🧮 Nettosaldo: {totalsPerCategory.Values.Sum():N2} kr"); // netto, inkomst-utgifter

            // tillbaka till ursprunliga listan
            SetActiveList(Transactions);

        }
        public void ShowStatistics()
        {
            // Antal transaktioner
            int count = Transactions.Count;

            //Summerar alla inkomster
            decimal totalIncome = Transactions.Sum(t => t.Amount > 0 ? t.Amount : 0);

            //Summerar alla utgifter
            decimal totalExpense = Transactions.Sum(t => t.Amount < 0 ? t.Amount : 0);

            // Netto balance för alla transaktioner
            decimal totalsaldo = CalculateBalance();

            // Skriver ut statistiköversikt i konsolen
            Console.WriteLine("\n📊 Statistiköversikt:\n");
            Console.WriteLine($"🧾 Antal transaktioner: {count}");
            Console.WriteLine($"💰 Total inkomst: {totalIncome:N2} kr");
            Console.WriteLine($"💸 Total utgift: {Math.Abs(totalExpense):N2} kr");
            Console.WriteLine($"🧮 Nettosaldo: {totalsaldo:N2} kr"); // netto, inkomst-utgifter
        }






    }
}
