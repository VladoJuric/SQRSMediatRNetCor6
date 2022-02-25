using Application.Administrator.Commands;
using Application.Administrator.Queries;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AdministratorController : BaseApiController
    {
        [HttpGet("getUser")]
        public async Task<ActionResult<List<UserDto>>> GetUserDtoById(int userId)
        {
            return Ok(await Mediator.Send(new GetUserDtoByIdQuery() { UserId = userId }));
        }

        [HttpGet("poreznaUprava")]
        public async Task<ActionResult> GetPoreznaUprava()
        {
            return Ok(await Mediator.Send(new GetPoreznaUpravaQuery()));
        }

            [HttpPost("saveUser")]
        public async Task<ActionResult<int>> SaveAdminReassignAgent(SaveUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
