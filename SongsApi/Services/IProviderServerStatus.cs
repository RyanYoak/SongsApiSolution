using SongsApi.Controllers;

namespace SongsApi
{
    public interface IProviderServerStatus
    {
        GetStatusResponse GetMyStatus();
    }
}