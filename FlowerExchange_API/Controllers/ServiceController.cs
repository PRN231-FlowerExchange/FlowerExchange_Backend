using Application.PostFlower.DTOs;
using Application.PostFlower.Queries.GetPostService;
using Application.PostFlower.Queries.GetService;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/service/")]
    [ApiController]
    public class ServiceController : APIControllerBase
    {
        [HttpGet(Name = "get-all")]
        public async Task<List<ServiceViewDTO>> GetAll()
        {
            return await Mediator.Send(new GetServiceQuery());
        }

        [HttpPost(Name = "get-service")]
        public async Task<List<PostService>> GetPostServiceByPostId([FromBody] GetPostServiceByPostIdQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
