using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Base;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Queries.GetDetails
{
    public record GetBookDetailsQuery : IRequest<Response<BookDetailsDto, ValidationFailure>>//BookDetailsDto>
    {
        public GetBookDetailsQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class GetBookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, Response<BookDetailsDto, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Response<BookDetailsDto, ValidationFailure> _responce;
        public GetBookDetailsQueryHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<BookDetailsDto, ValidationFailure> responce)
        {
            _context = context;
            _mapper = mapper;
            _responce = responce;
        }
        public async Task<Response<BookDetailsDto, ValidationFailure>> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            _responce.Result = await _context.Books.AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return _responce;
        }
    }
}
