using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestDto
{
    public class AddressBookUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmailUpdationDto> Emails { get; set; }
        public List<AddressUpdationDto> Addresses { get; set; }
        public List<PhoneUpdationDto> Phones { get; set; }
        public AssetIdDto Asset { get; set; }
    }
}
