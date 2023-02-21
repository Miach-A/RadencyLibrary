using AutoMapper;
using FluentValidation.Results;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.Base
{
    public abstract class CommandHandler
    {
        protected readonly LibraryDbContext _context;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        public CommandHandler(
            LibraryDbContext context,
            ILogger logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        protected void UpdateError(string message, IValidatedResponse<ValidationFailure> response, object request)
        {
            response.Validated = false;
            response.Errors.Add(new ValidationFailure("id", message));
            var requestName = request.GetType().Name;
            _logger.LogError("Radency library request: Update database Exeption for Request {Name} {@Request}", requestName, request);
        }
    }
}
