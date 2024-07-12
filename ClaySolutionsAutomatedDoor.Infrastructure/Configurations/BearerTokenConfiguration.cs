namespace ClaySolutionsAutomatedDoor.Infrastructure.Configurations
{
    public class BearerTokenConfiguration
    {
        public static string SectionName = "BearerTokenConfiguration";
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
    }
}
