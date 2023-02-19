namespace RadencyLibrary.CQRS.Book.Queries.GetAllBooks
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public decimal ReviwsNumber { get; set; }

    }
}