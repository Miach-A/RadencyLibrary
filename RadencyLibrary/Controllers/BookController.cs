using MediatR;
using Microsoft.AspNetCore.Mvc;
using RadencyLibrary.CQRS.Book.Queries.GetAllBooks;

namespace RadencyLibrary.Controllers
{
    public class BookController : ApiControllerBase
    {
        public BookController(ISender mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] GetAllBookQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
