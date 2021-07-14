using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http.Headers;

namespace mars_rover_client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMarsRoverClient(this IServiceCollection services)
        {
            var httpClientBuilder = services.AddHttpClient<IMarsRoverClient, MarsRoverClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.nasa.gov/mars-photos/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            httpClientBuilder.AddPolicyHandler(retryPolicy);

            return services;
        }
    }
}
