using DataAccessLayer.Models;
using DataAccessLayer.Models.MyInMemoryApi.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ServicesLayer.Contracts;

namespace SampleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("signup")]
        public async Task<ActionResult<ResultModel<User>>> SignUp(UserViewModel userViewModel)
        {
            var result = await _userService.SignUpAsync(userViewModel);

            if (result == null)
                return BadRequest("خطایی رخ داده است");

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<ActionResult<ResultModel<User>>> Login(UserViewModel userViewModel)
        {
            var result = await _userService.LoginAsync(userViewModel);

            if (result == null)
                return BadRequest("خطایی رخ داده است");

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

    }

}