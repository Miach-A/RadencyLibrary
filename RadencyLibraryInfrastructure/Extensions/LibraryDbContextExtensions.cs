using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibraryInfrastructure.Extensions
{
    public static class LibraryDbContextExtension
    {
        public static void SeedLibrary(this LibraryDbContext context)
        {
            var books = new Book[]
            {
                new() {Id = 1, Author = "Tolkien", Title = "The Lord of the Rings ", Genre = "Fantasy"},
                new() {Id = 2, Author = "George Martin", Title = "A Song of Ice and Fire", Genre = "Fantasy"},
                new() {Id = 3, Author = "Susanna Clarke", Title = "Jonathan Strange & Mr Norrell", Genre = "Fantasy"},
                new() {Id = 4, Author = "Neil Gaiman", Title = "American Gods" , Genre = "Fantasy"},
                new() {Id = 5, Author = "Robin Hobb", Title = "Assassin’s Apprentice", Genre = "Fantasy"}
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
