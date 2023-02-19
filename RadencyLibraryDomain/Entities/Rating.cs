namespace RadencyLibraryDomain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public Book Book { get; set; } = null!;
        public int BookId { get; set; }
        public short Score { get; set; }
    }
}
