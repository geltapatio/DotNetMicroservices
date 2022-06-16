using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviour;
using System.Reflection;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); // it required the nuget package AutoMapper.Extensions.Microsoft.DepedendyInjection (Profile)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // it required the nuget package FluentValidation.DependencyInjecttionExtensions (AbstractValidator)
            services.AddMediatR(Assembly.GetExecutingAssembly()); // it required the nuget package MediatR.Extensions.Microsoft.DependencyInjection (IRequest and IRequestHandler)
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}
