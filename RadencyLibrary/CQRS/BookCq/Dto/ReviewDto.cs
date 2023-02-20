namespace RadencyLibrary.CQRS.BookCq.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
