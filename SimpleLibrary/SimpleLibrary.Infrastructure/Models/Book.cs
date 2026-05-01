using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrary.Infrastructure.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? Genre { get; set; } = null!;
        public string? Isbn { get; set; } = null!;

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
