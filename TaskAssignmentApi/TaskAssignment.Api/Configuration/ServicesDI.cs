using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using TaskAssignment.Api.Shared;
using TaskAssignment.Application.Interfaces;
using TaskAssignment.Application.Interfaces.Settings;
using TaskAssignment.Domain.Exceptions;
using TaskAssignment.Infrastructure.CqrsDispatcherPipelineBehaviors;
using TaskAssignment.Infrastructure.Database;
using TaskAssignment.Infrastructure.Shared;

namespace TaskAssignment.Api.Configuration
{
    public static class ServicesDI
    {
        public static void RegisterTaskAssignmentServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            RegisterApi(builder);
            RegisterApplication(services);
            RegisterInfrastructure(services);
        }

        private static void RegisterApi(WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var environment = builder.Environment;
            var configuration = builder.Configuration;

            services.AddControllers();
            services.AddAppCors(environment, configuration);
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void RegisterApplication(IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetAssembly(typeof(IUnitOfWork));

            services.AddMediatR(applicationAssembly);
            services.AddScopedServices(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        }

        private static void RegisterDomain(IServiceCollection services)
        {
            var domainAssembly = Assembly.GetAssembly(typeof(UserException));

            services.AddScopedServices(domainAssembly);
            services.AddValidatorsFromAssembly(domainAssembly);

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        }

        private static void RegisterInfrastructure(IServiceCollection services)
        {
            var infraAssembly = Assembly.GetAssembly(typeof(UnitOfWork));

            services.AddDbContext<TaskAssignmentDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScopedRepositories(infraAssembly);

            // attention: MediatR pipeline - the order matters
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorsExecutionPipelineBehavior<,>));

            services.AddScopedServices(infraAssembly);

            services.AddScoped<IHandlerContext, HandlerContext>();
        }
    }
}
