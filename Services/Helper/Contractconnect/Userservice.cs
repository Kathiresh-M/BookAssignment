using Contract;
using Contract.IHelper;
using Contract.Response;
using Entities.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper.Contractconnect
{
    public class Userservice
    {
        private readonly IUserRepo _userRepo;
        private readonly IPassword _password;
        private object _jwtService;

        public Userservice(IUserRepo userRepo,
            IPassword passwordHasher)
        {
            _userRepo = userRepo;
            _password = passwordHasher;
        }

        public TokenResponse AuthUser(UserDto userData)
        {
            /*var user = _userRepo.GetUser(userData.UserName);

            if (user == null)
                return new TokenResponse(false, "User not authenticated", null, null);

            if (_password.PasswordMatches(userData.Password, user.Password))
                return new TokenResponse(true, "", _jwtService.GenerateSecurityToken(user), "bearer");

            return new TokenResponse(false, "User not authenticated", null, null);*/
            return new TokenResponse(true, "User not authenticated", null, null);

        }
    }
}
