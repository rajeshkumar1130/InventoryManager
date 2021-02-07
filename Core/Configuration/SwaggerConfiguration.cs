using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace InventoryManager.API.Core.Configuration
{
    public static class SwaggerConfigurationExtension
    {
        public static IServiceCollection SwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InventoryManager", Version = "v1" });
                c.OperationFilter<AddRequiredHeaderParameter>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}
