using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SongsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsApiIntegrationTests
{
    public class BasicWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                // Services collection
                // ask for the service for the service that provides IProviderServerStatus
                var serviceDescription = services.Single(service =>
                service.ServiceType == typeof(IProviderServerStatus));
                // Then remove it
                services.Remove(serviceDescription);
                // Then replace it with a dummy
                services.AddScoped<IProviderServerStatus, DummySerbice>();
            });
        }
    }

    public class DummySerbice : IProviderServerStatus
    {
        public SongsApi.Controllers.GetStatusResponse GetMyStatus()
        {
            return new SongsApi.Controllers.GetStatusResponse
            {
                message = "Dummy says Howdy!",
                lastChecked = new DateTime(1969, 4, 20, 23, 59, 00)
            };
        }
    }
}
