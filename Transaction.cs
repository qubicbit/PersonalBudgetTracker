using System;
using static System.Net.Mime.MediaTypeNames;

namespace PersonalBudgetTracker
{
    // Transaction – representerar en enskild inkomst eller utgift
    public class Transaction
    {
        // 📌 Egenskaper för en transaktion
        public string Description { get; set; }   // Kort beskrivning av transaktionen
        public decimal Amount { get; set; }       // Belopp (positivt = inkomst, negativt = utgift)
        public string Category { get; set; }      // Kategori (t.ex. Mat, Hyra, Inkomst)
        public DateOnly Date { get; set; }        // Datum då transaktionen inträffade



    }
}
