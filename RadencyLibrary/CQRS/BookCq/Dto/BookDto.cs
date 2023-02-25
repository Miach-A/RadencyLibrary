namespace RadencyLibrary.CQRS.BookCq.Dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string Cover { get; set; } = string.Empty;
        public int ReviewsNumber { get; set; }

    }
}