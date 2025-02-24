using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Application.Payloads
{
    public class UpdateInfoRequest
    {
        public class RequestNickname
        {
            public string Nickname { get; set; }
        }
    }
}
