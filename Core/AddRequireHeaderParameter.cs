using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InventoryManager.API.Core
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            HashSet<string> action = new HashSet<string>();
            action.Add("RegisterUser");
            action.Add("Login");
            action.Add("Logout");
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (!action.Contains(descriptor.ActionName))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "access token",
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString("Bearer ")
                    },
                    Required = false
                });
            }
        }
    }
}
