﻿using Core.Application.Rules;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
            return services;
        }

        public static IServiceCollection AddSubClassesOfType(this IServiceCollection services, Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();

            foreach (var item in types)
                if (addWithLifeCycle is null)
                    services.AddScoped(item);
                else
                    addWithLifeCycle(services, type);

            return services;
        }
    }
}
