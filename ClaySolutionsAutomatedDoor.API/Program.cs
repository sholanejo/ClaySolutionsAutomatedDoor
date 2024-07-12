using ClaySolutionsAutomatedDoor.API.Extensions;
using ClaySolutionsAutomatedDoor.Application.Common.Extensions;
using ClaySolutionsAutomatedDoor.Application.Middlewares;
using ClaySolutionsAutomatedDoor.Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using Serilog;

namespace ClaySolutionsAutomatedDoor.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            var builder = WebApplication.CreateBuilder(args);
            SerilogService.AddSerilogLogging();
            builder.Host.UseSerilog();
            Log.Logger.Information("Application is starting...");
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
    }
}
