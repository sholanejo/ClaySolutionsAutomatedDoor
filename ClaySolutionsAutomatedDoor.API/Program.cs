using ClaySolutionsAutomatedDoor.API.Extensions;
using ClaySolutionsAutomatedDoor.Application.Common.Extensions;
using ClaySolutionsAutomatedDoor.Application.Middlewares;
using ClaySolutionsAutomatedDoor.Infrastructure.Extensions;
using Serilog;

namespace ClaySolutionsAutomatedDoor.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                SerilogService.AddSerilogLogging();
                builder.Host.UseSerilog();
                // Add services to the container.
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddSwaggerGenExt();
                builder.Services.AddApplicationServices();
                builder.Services.AddInfrastructureServices(builder.Configuration);
                builder.Services.AddApiServices(builder);

                var app = builder.Build();
                app.UseMiddleware<ErrorHandlingMiddleware>();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    await SeedDbData.SetupDatabase(app);
                    app.UseSwagger();
                    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clay Solutions Automated Door"); });
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseCors("corsPolicy");

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured during application startup");
            }

        }
    }
}
