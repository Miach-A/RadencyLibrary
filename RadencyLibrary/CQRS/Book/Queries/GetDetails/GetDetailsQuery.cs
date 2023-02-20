using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Book.Dto;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.Book.Queries.GetDetails
{
    public class GetBookDetailsQuery : IRequest<BookDetailsDto>
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
            return await _context.Books
                .Where(x => x.Id == request.Id)
                .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
