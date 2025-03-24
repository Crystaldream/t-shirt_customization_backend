using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SeventySevenDiamondsBackend.Filters {

    public class FileUploadFilter : IOperationFilter {

        public void Apply(OpenApiOperation operation, OperationFilterContext context) {

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor == null) 
                return;

            if (descriptor.ActionName == "UploadImage") {

                operation.RequestBody = new OpenApiRequestBody {

                    Content = new Dictionary<string, OpenApiMediaType> {

                        ["multipart/form-data"] = new OpenApiMediaType {
                            
                            Schema = new OpenApiSchema {

                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema> {
                                    ["File"] = new OpenApiSchema { Type = "string", Format = "binary" },
                                    ["ItemId"] = new OpenApiSchema { Type = "integer" },
                                    ["ColorId"] = new OpenApiSchema { Type = "integer" },
                                    ["FabricId"] = new OpenApiSchema { Type = "integer" }
                                },
                                Required = new HashSet<string> { "File", "ItemId", "ColorId", "FabricId" }

                            }

                        }

                    }

                };

            }

        }
        
    }
}