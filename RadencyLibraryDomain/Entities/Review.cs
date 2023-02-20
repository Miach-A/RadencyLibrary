namespace RadencyLibraryDomain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public Book Book { get; set; } = null!;
        public int BookId { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
