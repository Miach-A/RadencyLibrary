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

    public class GetBookDetailsQueryHandler : QueryHandler, IRequestHandler<GetBookDetailsQuery, Response<BookDetailsDto, ValidationFailure>>
    {
        private readonly Response<BookDetailsDto, ValidationFailure> _responce;
        public GetBookDetailsQueryHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<BookDetailsDto, ValidationFailure> responce) : base(context, mapper)
        {
            _responce = responce;
        }
        public async Task<Response<BookDetailsDto, ValidationFailure>> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            _responce.Result = await _context.Books.AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (_responce.Result == null)
            {
                _responce.Validated = false;
                _responce.Errors.Add(new ValidationFailure("id", "id not exist"));
            }

            return _responce;
        }
    }
}
