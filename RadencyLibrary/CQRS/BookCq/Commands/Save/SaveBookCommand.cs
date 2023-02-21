using AutoMapper;
using FluentValidation.Results;
using MediatR;
using RadencyLibrary.Common.Models;
using RadencyLibrary.CQRS.Base;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Commands.Save
{
    public record SaveBookCommand : IRequest<Response<SaveResult, ValidationFailure>>//SaveResult>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }

    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, Response<SaveResult, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Response<SaveResult, ValidationFailure> _response;
        public SaveBookCommandHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<SaveResult, ValidationFailure> response)
        {
            _context = context;
            _mapper = mapper;
            _response = response;
        }
        public async Task<Response<SaveResult, ValidationFailure>> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            //var book = _context.F

            //var book = _mapper.Map<Book>(request);

            //if (book == null)
            //{
            //    throw new Exception("map error");
            //}
            //var bookContext = _context.Books.Attach(book);
            //bookContext.State = EntityState.Detached;

            //var count = await _context.SaveChangesAsync();
            //if (count == 0)
            //{
            //    await _context.AddAsync(book);
            //    count = await _context.SaveChangesAsync();
            //}

            //if (count == 0)
            //{
            //    throw new DbUpdateConcurrencyException();
            //}
            return _response;//new SaveResult() { Id = request.Id };
        }
    }
}
