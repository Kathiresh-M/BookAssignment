using Contract;
using Contract.IHelper;
using Contract.Response;
using Entities.Dto;
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
            return new TokenResponse(true, "User not authenticated", null, null);

        }
    }
}
