using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Book.Dto;
using RadencyLibraryInfrastructure.Persistence;
using System.Linq.Dynamic.Core;

namespace RadencyLibrary.CQRS.Book.Queries.GetAllBooks
{
    public class GetAllBookQuery : IRequest<IEnumerable<BookDto>>
    {
        public string? Order { get; set; }
    }

    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, IEnumerable<BookDto>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        public GetAllBookQueryHandler(
            LibraryDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BookDto>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            var queryBooks = _context.Books
                .Include(x => x.Reviews)
                .Include(x => x.Ratings)
                .AsQueryable();

            var orderedQueryBooks = request.Order != null ? queryBooks.OrderBy(request.Order.ToString()) : queryBooks;

            return await orderedQueryBooks.ProjectTo<BookDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
