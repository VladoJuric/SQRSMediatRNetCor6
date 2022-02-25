using Application.Administrator.RequestDtos;
using Application.Common.Interfaces;
using Application.Dtos;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Application.Common.Services
{
    public class PoreznaApiService : IPoreznaApiService
    {
        private readonly IConfiguration configuration;
        public PoreznaApiService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<PoreznaDto>> GetPorez(CancellationToken cancellationToken) 
        { 
            var options = new RestClientOptions(configuration["Porezna:PoreznaApi"])
            {
                ThrowOnAnyError = true,
                Timeout = 1000
            };
            var client = new RestClient(options);

            var restResponse = new List<PoreznaDto>();

            var restRequest = new RestRequest()
                .AddQueryParameter("foo", "bar")
                .AddJsonBody(new RequestPoreznaDto());

            Task.Run(async () =>
            {
                restResponse = await client.GetAsync<List<PoreznaDto>>(restRequest, cancellationToken);
            }).Wait();

            return restResponse;
        }

    }
}
