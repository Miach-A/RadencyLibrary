using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Base;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibraryInfrastructure.Persistence;
using System.Linq.Dynamic.Core;

namespace RadencyLibrary.CQRS.BookCq.Queries.GetAll
{
    public record GetAllBookQuery : IRequest<Response<IEnumerable<BookDto>, ValidationFailure>>
    {
        public string? Order { get; set; }
    }

    public class GetAllBookQueryHandler : QueryHandler, IRequestHandler<GetAllBookQuery, Response<IEnumerable<BookDto>, ValidationFailure>>
    {
        private readonly Response<IEnumerable<BookDto>, ValidationFailure> _responce;

        public GetAllBookQueryHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<IEnumerable<BookDto>, ValidationFailure> responce) : base(context, mapper)
        {
            _responce = responce;
        }
        public async Task<Response<IEnumerable<BookDto>, ValidationFailure>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            var queryBooks = _context.Books.AsNoTracking()
                .Include(x => x.Reviews)
                .Include(x => x.Ratings)
                .AsQueryable();

            var orderedQueryBooks = request.Order != null ? queryBooks.OrderBy(request.Order.ToString()) : queryBooks;

            _responce.Result = await orderedQueryBooks.ProjectTo<BookDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return _responce;
        }
    }
}
