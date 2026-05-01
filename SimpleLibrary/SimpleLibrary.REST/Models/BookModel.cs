namespace SimpleLibrary.REST.Models
{
    public class BookCreateModel
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public int TotalCopies { get; set; }
    }

    public class BookUpdateModel
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public int TotalCopies { get; set; }
    }

    public class BookResponseModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public int AvailableCopies { get; set; }
    }
}