using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.Common.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void SeedLibrary(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                if (context == null) return;

                var books = new List<Book>
                {
                new() {Id = 1, Author = "Tolkien", Title = "The Lord of the Rings ", Genre = "Fantasy"},
                new() {Id = 2, Author = "George Martin", Title = "A Song of Ice and Fire", Genre = "Fantasy"},
                new() {Id = 3, Author = "Susanna Clarke", Title = "Jonathan Strange & Mr Norrell", Genre = "Fantasy"},
                new() {Id = 4, Author = "Neil Gaiman", Title = "American Gods" , Genre = "Fantasy"},
                new() {Id = 5, Author = "Robin Hobb", Title = "Assassin’s Apprentice", Genre = "Fantasy"}
                };

                var rand = new Random();

                var id = 1;
                var ratings = new List<Rating>();
                books.ForEach(book =>
                {
                    foreach (var item in Enumerable.Range(1, 40))
                    {
                        ratings.Add(new()
                        {
                            Id = id,
                            BookId = book.Id,
                            Score = (short)rand.Next(1, 5)
                        });
                        id++;
                    }
                });

                id = 1;
                var reviews = new List<Review>();
                books.ForEach(book =>
                {
                    foreach (var item in Enumerable.Range(1, 20))
                    {
                        reviews.Add(new()
                        {
                            Id = id,
                            BookId = book.Id,
                            Reviewer = string.Concat("Reviewer #", item),
                            Message = string.Concat("Message #", item)
                        });
                        id++;
                    }
                });

                context.Books.AddRange(books);
                context.Ratings.AddRange(ratings);
                context.Reviews.AddRange(reviews);
                context.SaveChanges();
            }
        }
    }
}
