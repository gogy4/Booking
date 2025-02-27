using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

public class SwaggerFileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            if (parameter.Schema?.Type == "string" && parameter.Schema?.Format == "binary")
            {
                parameter.Description = "File to upload";
                parameter.Schema.Type = "string";
                parameter.Schema.Format = "binary";
            }
        }
    }
}