using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Stride.Domain.Config;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppDependencyResolverExtension
    {
        public static void ConfigureStrideServices(this IServiceCollection services, IConfiguration configuration) {
            SqlConfig sqlConfig = new SqlConfig();
            configuration.GetSection("SqlConfig").Bind(sqlConfig);
            services.AddSingleton(sqlConfig);
        }

        //let's use Polly if we need to send external HTTP Requests
        //if 404 found retry in 2 seconds for 5 times
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
