using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.Common.Models;
using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Commands.Save
{
    public record SaveBookCommand : IRequest<SaveResult>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }

    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, SaveResult>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        public SaveBookCommandHandler(
            LibraryDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SaveResult?> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);

            if (book == null)
            {
                throw new Exception("map error");
            }
            _context.Books.Attach(book);

            var count = await _context.SaveChangesAsync();

            if (count == 0)
            {
                throw new DbUpdateConcurrencyException();
            }
            return new SaveResult() { Id = request.Id };
        }
    }
}
