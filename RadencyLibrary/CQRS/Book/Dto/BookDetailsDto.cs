namespace RadencyLibrary.CQRS.Book.Dto
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int ReviwsNumber { get; set; }

    }
}