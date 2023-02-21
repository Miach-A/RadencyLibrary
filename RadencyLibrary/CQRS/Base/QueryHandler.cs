using AutoMapper;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.Base
{
    public abstract class QueryHandler
    {
        protected readonly LibraryDbContext _context;
        protected readonly IMapper _mapper;
        public QueryHandler(
            LibraryDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
