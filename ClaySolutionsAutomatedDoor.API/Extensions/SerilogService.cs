using Serilog;

namespace ClaySolutionsAutomatedDoor.API.Extensions
{
    public class SerilogService
    {
        public static void AddSerilogLogging()
        {
            //get configuration settings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //initialize logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }
    }
}
