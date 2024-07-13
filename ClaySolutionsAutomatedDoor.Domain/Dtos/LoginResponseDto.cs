namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }

        public int ExpiresInMinutes { get; set; }
        public string UserId { get; set; }
    }
}
