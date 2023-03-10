using AutoMapper;
using Contract;
using Contract.Response;
using Entities.Dto;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace AddressBookAssignment.Controllers
{
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        private readonly ILog _log;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="userService">Communication between respository and controller</param>
        /// <param name="mapper">used to map dto</param>
        /// <param name="logger">Used to log the data</param>
        /// <returns></returns>
        public UserController(IUserService userService, 
            IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _log = LogManager.GetLogger(typeof(UserController));
        }

        //Authentication and user creation controller

        /// <summary>
        /// Method to do user authentication
        /// </summary>
        /// <param name="user">user login credential data</param>
        /// <returns>JSON Web Token</returns>
        [HttpPost]
        [Route("api/user/auth")]
        public IActionResult AuthUser([FromBody] UserDto user)
        {


            _log.Info("Get Authorization");
            if (!ModelState.IsValid)
            {
                _log.Error("Invalid login details used.");
                return BadRequest("Enter valid user data");
            }

            try
            {
                var response = _userService.AuthUser(user);

                if(response.AccessToken == null && response.TokenType == null) 
                {
                    return Unauthorized();
                }
                _log.Info(user.UserName + " user logged in.");

                var token = new Token(response.AccessToken, response.TokenType);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _log.Error("Something went wrong", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Method to create user
        /// </summary>
        /// <param name="user">user data to be created</param>
        /// <returns>user data with Id</returns>
        [HttpPost]
        [Route("api/user/register")]
        public IActionResult CreateUser([FromBody] UserCreationDto user)
        {
            if (!ModelState.IsValid)
            {
                _log.Error("Invalid user details used.");
                return BadRequest("Enter valid user data");
            }
            var response = _userService.CreateUser(user);

            try
            {
                _log.Info("User created with username: " + user.UserName);
                var userToReturn = _mapper.Map<UserReturnDto>(response.user);
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                _log.Error("Something went wrong", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Method to user data
        /// </summary>
        /// <param name="Id">user Id</param>
        /// <returns>user data with id</returns>
        [HttpGet]
        [Authorize]
        [Route("api/user/{Id}")]
        public IActionResult GetUser(Guid Id)
        {
            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Error("Trying to access user data with not a valid user ID with user Id: " + tokenUserId);
                return BadRequest("Not a valid user ID");
            }

            var response = _userService.GetUserById(Id, tokenUserId);

            try
            {
                _log.Info("User with ID: " + Id + ", viewed the data.");
                var user = _mapper.Map<UserReturnDto>(response.user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                _log.Error("Something went wrong", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Method to update user data
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <param name="userData">user data to be updated</param>
        /// <returns>user data with Id</returns>
        [HttpPut]
        [Authorize]
        [Route("api/user/{Id}")]
        public IActionResult UpdateUser(Guid Id, [FromBody] UserUpdationDto userData)
        {
            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Error("Trying to update user data with not a valid user id by user: " + tokenUserId);
                return BadRequest("Not a valid user ID.");
            }

            var response = _userService.UpdateUser(Id, tokenUserId, userData);

            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }

            var user = _mapper.Map<UserReturnDto>(response.user);

            return Ok(user);
        }

        /// <summary>
        /// Method to delete an User
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <returns>no content</returns>
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public IActionResult DeleteUser(Guid Id)
        {

            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Error("Trying to delete user data with not a valid user ID by user Id: " + tokenUserId);
                return BadRequest("Not a valid user ID.");
            }


            var response = _userService.DeleteUser(Id, tokenUserId);

            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }

            _log.Info($"User with ID: {response.user.Id}, deleted successfully.");

            return NoContent();
        }
    }
}
