using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjekatStudent.Entities;
using ProjekatStudent.Repos;
using BCrypt.Net;
using ProjekatStudent.Models;
using AutoMapper;

namespace ProjekatStudent.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                if (users.Count() == 0)
                    return NotFound("There are no user to show.");

                return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto user)
        {
            try
            {
                var result = await _userRepository.GetUserByUsernameAsync(user.Username);
                if (result != null)
                    return NotFound("A user with the given username already exists.");

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var result1 = await _userRepository.CreateUserAsync(_mapper.Map<User>(user));
                return Ok(result1);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{username}")]
        public async Task<ActionResult<User>> DeleteUser(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound("A user with the given username does not exists.");

                await _userRepository.DeleteUserAsync(user);
                return Ok("The user was successfully deleted.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
