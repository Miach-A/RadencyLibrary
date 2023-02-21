using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Base;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Queries.GetRecommended
{
    public record GetRecommendedBookQuery : IRequest<Response<IEnumerable<BookDto>, ValidationFailure>>//IEnumerable<BookDto>>
    {
        public string? Genre { get; set; }
    }

    public class GetAllBookQueryHandler : IRequestHandler<GetRecommendedBookQuery, Response<IEnumerable<BookDto>, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Response<IEnumerable<BookDto>, ValidationFailure> _response;

        public GetAllBookQueryHandler(
            IMapper mapper,
            LibraryDbContext context,
            Response<IEnumerable<BookDto>, ValidationFailure> response)
        {
            _context = context;
            _mapper = mapper;
            _response = response;
        }
        public async Task<Response<IEnumerable<BookDto>, ValidationFailure>> Handle(GetRecommendedBookQuery request, CancellationToken cancellationToken)
        {
            _response.Result = await _context.Books.AsNoTracking()
                .Where(x => request.Genre == null ? true : x.Genre.ToLower() == request.Genre.ToLower())
                .Include(x => x.Reviews)
                .Include(x => x.Ratings)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .Where(x => x.ReviewsNumber > 10)
                .OrderByDescending(x => x.Rating)
                .Take(10)
                .ToListAsync(cancellationToken);

            return _response;
        }
    }
}
