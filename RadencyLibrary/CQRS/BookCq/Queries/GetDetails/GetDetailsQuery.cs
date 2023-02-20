using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Queries.GetDetails
{
    public record GetBookDetailsQuery : IRequest<BookDetailsDto>
    {
        public GetBookDetailsQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class GetBookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, BookDetailsDto?>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        public GetBookDetailsQueryHandler(
            LibraryDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BookDetailsDto?> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books.AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
