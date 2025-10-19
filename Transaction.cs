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


        // Konstruktor – initierar egenskaperna vid skapande av en ny transaktion
        public Transaction(string description, decimal amount, string category, DateOnly date)
        {
            Description = description;
            Amount = amount;
            Category = category;
            Date = date;
        }

        // Visar detaljerad information om transaktionen i konsolen
        public void ShowInfo()
        {
            Console.WriteLine("\nTransaktionsinformation:\n");
            Console.WriteLine($"Datum: {Date}");
            Console.WriteLine($"Beskrivning: {Description}");
            Console.WriteLine($"Kategori: {Category}");
            MenuHelper.PrintColoredAmount(Amount, $"Belopp: {Amount}");
        }

    }
}
