using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestDto
{
    public class AddressBookCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmailDto> Emails { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public List<PhoneDto> Phones { get; set; }
    }
}
