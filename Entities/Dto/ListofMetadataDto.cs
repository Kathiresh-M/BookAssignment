using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class ListofMetadataDto
    {
        public List<Guid> Id { get; set; }
        List<string> Key { get; set; }
        List<string> Description { get; set; }
    }
}
