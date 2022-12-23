using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class AddressDto
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public Addresstype Addresstype { get; set; }
        public Countrytype Countrytype { get; set; }
    }

    public class AddressUpdationDto : AddressDto
    {
        public Guid Id { get; set; }
    }

    public class AddressToReturnDto : AddressUpdationDto { }
}
