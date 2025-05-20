using TaskAssignment.Api.Shared;

namespace TaskAssignment.Api.Configuration
{
    public static class CorsConfig
    {
        public readonly static string AppCorsPolicy = "AppCorsPolicy";
        public readonly static string[] AllowedHeaders = new string[] { "Authorization", "Content-Type", "Access-Control-Allow-Origin" };
        public readonly static string[] AllowedMethods = new string[] { "GET", "POST", "PUT", "DELETE", "HEAD", "PATCH" };

        public static void AddAppCors(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            var clientAddress = configuration.GetSection("CorsSettings:ClientAddress").Value;

            if (environment.IsNotRestricted())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AppCorsPolicy, builder => builder
                                .WithOrigins(clientAddress)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .SetIsOriginAllowed(t => true)
                                .AllowCredentials());

                    options.DefaultPolicyName = AppCorsPolicy;
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AppCorsPolicy, builder => builder
                                .WithOrigins(clientAddress)
                                .WithMethods(AllowedMethods)
                                .WithHeaders(AllowedHeaders)
                                .AllowCredentials());

                    options.DefaultPolicyName = AppCorsPolicy;
                });

            }
        }
    }
}
