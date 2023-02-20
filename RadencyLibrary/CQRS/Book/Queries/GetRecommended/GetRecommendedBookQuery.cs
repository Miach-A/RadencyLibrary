using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Book.Dto;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.Book.Queries.GetRecommended
{
    public class GetRecommendedBookQuery : IRequest<IEnumerable<BookDto>>
    {
        public string? Genre { get; set; }
    }

    public class GetAllBookQueryHandler : IRequestHandler<GetRecommendedBookQuery, IEnumerable<BookDto>>
    {
        private readonly LibraryDbContext _context;
        public GetAllBookQueryHandler(
            LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookDto>> Handle(GetRecommendedBookQuery request, CancellationToken cancellationToken)
        {
            var queryBooks = _context.Books
                .Where(x => request.Genre == null ? true : x.Genre == request.Genre)
                .Include(x => x.Reviews)
                .Include(x => x.Ratings)
                .Select(x => new BookDto()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Rating = x.Ratings.Count() > 0 ? Convert.ToDecimal(x.Ratings.Average(y => y.Score)) : 0,
                    ReviwsNumber = x.Reviews.Count()
                })
                .Where(x => x.ReviwsNumber > 10)
                .OrderByDescending(x => x.Rating)
                .Take(10);

            return await queryBooks.ToListAsync(cancellationToken);
        }
    }
}
