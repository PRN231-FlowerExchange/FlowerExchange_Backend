using Application.PostFlower.Commands.ActiveServiceCommand;
using Application.PostFlower.Commands.AddServiceToPostCommand;
using Application.PostFlower.DTOs;
using Application.PostFlower.Queries.GetPostService;
using Application.PostFlower.Queries.GetService;
using Application.Weather.Queries.GetWeather;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceController : APIControllerBase
    {
        [HttpGet(Name = "get-all")]
        public async Task<List<ServiceViewDTO>> GetAll()
        {
            return await Mediator.Send(new GetServiceQuery());
        }

        [HttpPost(Name = "get-service")]
        public async Task<List<PostService>> GetPostServiceByPostId([FromBody]GetPostServiceByPostIdQuery query)
        {
            return await Mediator.Send(query);
        }

        // AddPostToService endpoint should have a unique name
        [HttpPut("add-service-to-post")]
        public async Task<IActionResult> AddServiceToPost([FromBody] AddServiceToPostCommand command)
        {
            try {
                var response = await Mediator.Send(command);
                return Ok(response);
            }
            catch(NotFoundException ex)
            {
                return StatusCode(200, ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("active-service-to-post")]
        public async Task<IActionResult> ActiveService([FromBody] ActiveServiceCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(200, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
