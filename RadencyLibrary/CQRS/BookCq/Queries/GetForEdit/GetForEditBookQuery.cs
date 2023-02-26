using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Base;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibraryInfrastructure.Persistence;
using System.Linq.Dynamic.Core;

namespace RadencyLibrary.CQRS.BookCq.Queries.GetForEdit
{
    public record GetForEditBookQuery : IRequest<Response<BookEditDto, ValidationFailure>>
    {
        public GetForEditBookQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class GetForEditBookQueryHandler : QueryHandler, IRequestHandler<GetForEditBookQuery, Response<BookEditDto, ValidationFailure>>
    {
        private readonly Response<BookEditDto, ValidationFailure> _responce;

        public GetForEditBookQueryHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<BookEditDto, ValidationFailure> responce) : base(context, mapper)
        {
            _responce = responce;
        }
        public async Task<Response<BookEditDto, ValidationFailure>> Handle(GetForEditBookQuery request, CancellationToken cancellationToken)
        {
            _responce.Result = await _context.Books.AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ProjectTo<BookEditDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (_responce.Result == null)
            {
                _responce.Validated = false;
                _responce.Errors.Add(new ValidationFailure("id", "id not exist"));
            }

            return _responce;
        }
    }
}
