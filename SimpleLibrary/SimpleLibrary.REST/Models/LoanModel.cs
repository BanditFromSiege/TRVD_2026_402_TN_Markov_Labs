namespace SimpleLibrary.REST.Models
{
    public class LoanCreateModel
    {
        public int UserId { get; set; }

        public int BookId { get; set; }

        public DateTime DueDate { get; set; }
    }

    public class LoanUpdateModel
    {
        public DateTime? ReturnDate { get; set; }
    }

    public class LoanResponseModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}