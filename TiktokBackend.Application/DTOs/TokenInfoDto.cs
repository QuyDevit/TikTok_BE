using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Application.DTOs
{
    public class TokenInfoDto
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}
