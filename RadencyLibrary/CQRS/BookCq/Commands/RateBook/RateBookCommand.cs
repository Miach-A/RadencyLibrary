using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Base;
using RadencyLibrary.CQRS.BookCq.Commands.Save;
using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;
using System.Text.Json.Serialization;

namespace RadencyLibrary.CQRS.BookCq.Commands.RateBook
{
    public record RateBookCommand : IRequest<Response<bool, ValidationFailure>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Score { get; set; }
    }

    public class RateBookCommandHandler : CommandHandler, IRequestHandler<RateBookCommand, Response<bool, ValidationFailure>>
    {
        private readonly Response<bool, ValidationFailure> _response;
        public RateBookCommandHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<bool, ValidationFailure> response,
            ILogger<SaveBookCommand> logger) : base(context, logger, mapper)
        {
            _response = response;
        }
        public async Task<Response<bool, ValidationFailure>> Handle(RateBookCommand request, CancellationToken cancellationToken)
        {
            var rating = _mapper.Map<Rating>(request);
            await _context.AddAsync(rating);

            try
            {
                var count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _response.Result = true;
                    return _response;
                }
                else
                {
                    UpdateError(string.Concat("Id = ", request.Id, " not save"), _response, request);
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                UpdateError(ex.Message, _response, request);
            }

            return _response;

        }

    }
}
