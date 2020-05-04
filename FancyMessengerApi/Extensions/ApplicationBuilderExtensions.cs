using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace FancyMessengerApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSwagger(this IApplicationBuilder instance)
        {
            // TODO
            // var commonOptions = instance.ApplicationServices.GetRequiredService<
            //     IOptions<CommonOptions>
            // >().Value;
            
            instance.UseSwagger(options => {
                options.PreSerializeFilters.Add((document, request) => {
                    // Translate routes to lower registry.
                    
                    var pathsInLower = document.Paths.ToDictionary(
                        p => p.Key.ToLowerInvariant(), p => p.Value
                    );
                    
                    document.Paths.Clear();

                    foreach (var pathInLower in pathsInLower)
                    {
                        document.Paths.Add(pathInLower.Key, pathInLower.Value);
                    }
                });
            });

            instance.UseSwaggerUI(options => {

                // Core.
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

                // Display.
                // TODO options.DocumentTitle = commonOptions.ServiceName;
                options.DefaultModelsExpandDepth(-1);

            });
        }
    }
}