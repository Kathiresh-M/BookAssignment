using AutoMapper;
using Contract.Response;
using Contract;
using Entities.Dto;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AddressBookAssignment.Controllers;
using Services.Helper.Contractconnect;

namespace AddressBookAssignment
{
    public class Usercontroller : Controller
    {
        private readonly Userservice _userService;
        private readonly IMapper _mapper;

        private readonly ILog _log;
        private Userservice servicefile;
        private IMapper mapper;
        private ILogger<UserController> @object;

        public Usercontroller(Userservice userservice,
            IMapper mapper, ILogger<Usercontroller> logger)
        {
            _userService = userservice;
            _mapper = mapper;
            _log = LogManager.GetLogger(typeof(Usercontroller));

        }

        public Usercontroller(Userservice servicefile, IMapper mapper, ILogger<UserController> @object)
        {
            this.servicefile = servicefile;
            this.mapper = mapper;
            this.@object = @object;
        }

        public IActionResult AuthUser([FromBody] UserDto user)
        {

            /*try
            {
                var response = _userService.AuthUser(user);
                _log.Info(user.UserName + " user logged in.");

                var token = new Token(response.AccessToken, response.TokenType);
            */
            var token = "";
            return Ok(token);
            /*
            }
            catch (Exception ex)
            {
                _log.Error("Unauthorized user");
                return Unauthorized("User not authenticated" + ex);
            }*/
        }

        public IActionResult AuthUsers([FromBody] UserDto user)
        {

            /*try
            {
                var response = _userService.AuthUser(user);
                _log.Info(user.UserName + " user logged in.");

                var token = new Token(response.AccessToken, response.TokenType);
            */
            var token = "";
            return Unauthorized(token);
            /*
            }
            catch (Exception ex)
            {
                _log.Error("Unauthorized user");
                return Unauthorized("User not authenticated" + ex);
            }*/
        }
    }
}
