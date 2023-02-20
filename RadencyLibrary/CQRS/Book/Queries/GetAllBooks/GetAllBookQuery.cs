using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.Book.Queries.GetAllBooks
{
    public class GetAllBookQuery : IRequest<IEnumerable<BookDto>>
    {
        public string? Order { get; set; }
    }

    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, IEnumerable<BookDto>>
    {
        private readonly LibraryDbContext _context;
        public GetAllBookQueryHandler(
            LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookDto>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books
                .Include(x => x.Reviews)
                .Include(x => x.Ratings)
                .OrderBy(x => request.Order == null ? string.Empty : request.Order.ToString())
                .Select(x => new BookDto()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Rating = x.Ratings.Count() > 0 ? Convert.ToDecimal(x.Ratings.Average(y => y.Score)) : 0,
                    ReviwsNumber = x.Reviews.Count()

                })
                .ToListAsync(cancellationToken);
        }
    }
}
