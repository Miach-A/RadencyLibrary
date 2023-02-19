using Microsoft.AspNetCore.Mvc;

namespace RadencyLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        //private ISender _mediator = null!;
        //public ApiControllerBase(ISender mediator)
        //{
        //    _mediator = mediator;
        //}

        //protected ISender Mediator => _mediator;
    }

}
