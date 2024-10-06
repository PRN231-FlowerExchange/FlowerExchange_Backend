using Application.PostFlower.DTOs;
using Application.PostFlower.Queries.GetPostService;
using Application.PostFlower.Queries.GetService;
using Application.Weather.Queries.GetWeather;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/service/")]
    [ApiController]
    public class ServiceController : APIControllerBase
    {
        [HttpGet(Name = "all")]
        public async Task<List<ServiceViewDTO>> GetAll()
        {
            return await Mediator.Send(new GetServiceQuery());
        }

        [HttpPost(Name = "")]
        public async Task<List<PostService>> GetPostServiceByPostId([FromBody]GetPostServiceByPostIdQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
