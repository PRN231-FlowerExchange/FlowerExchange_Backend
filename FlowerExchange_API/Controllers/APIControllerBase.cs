using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class APIControllerBase : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
