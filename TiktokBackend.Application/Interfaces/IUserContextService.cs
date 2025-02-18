using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Application.Interfaces
{
    public interface IUserContextService
    {
        string GetIpAddress();
        string GetUserAgent();
    }
}
