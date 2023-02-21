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

    public class RateBookCommandHandler : IRequestHandler<RateBookCommand, Response<bool, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Response<bool, ValidationFailure> _response;
        private readonly ILogger<SaveBookCommand> _logger;
        public RateBookCommandHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<bool, ValidationFailure> response,
            ILogger<SaveBookCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _response = response;
            _logger = logger;
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
                    UpdateError(string.Concat("Id = ", request.Id, " not save"), request);
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                UpdateError(ex.Message, request);
            }

            return _response;

        }

        private void UpdateError(string message, RateBookCommand request)
        {
            _response.Validated = false;
            _response.Errors.Add(new ValidationFailure("id", message));
            var requestName = request.GetType().Name;
            _logger.LogError("Radency library request: Update database Exeption for Request {Name} {@Request}", requestName, request);
        }
    }
}
