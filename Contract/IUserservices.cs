using Contract.Response;
using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IUserservices
    {
        TokenResponse AuthUser(UserDto userData);
    }
}
