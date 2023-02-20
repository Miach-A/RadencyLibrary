using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Queries.GetRecommended
{
    public record GetRecommendedBookQuery : IRequest<IEnumerable<BookDto>>
    {
        public string? Genre { get; set; }
    }

    public class GetAllBookQueryHandler : IRequestHandler<GetRecommendedBookQuery, IEnumerable<BookDto>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public GetAllBookQueryHandler(
            IMapper mapper,
            LibraryDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BookDto>> Handle(GetRecommendedBookQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books.AsNoTracking()
                .Where(x => request.Genre == null ? true : x.Genre.ToLower() == request.Genre.ToLower())
                .Include(x => x.Reviews)
                .Include(x => x.Ratings)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .Where(x => x.ReviewsNumber > 10)
                .OrderByDescending(x => x.Rating)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
    }
}
