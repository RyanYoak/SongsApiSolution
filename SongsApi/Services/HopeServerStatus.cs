using SongsApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsApi.Services
{
    public class HopeServerStatus : IProviderServerStatus
    {
        public GetStatusResponse GetMyStatus()
        {
            return new GetStatusResponse
            {
                message = "Everthing is Operational",
                lastChecked = DateTime.Now
            };
        }
    }
}
