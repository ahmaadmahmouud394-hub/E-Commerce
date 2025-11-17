namespace E_Commerce.Models.DTO
{
    // This defines the JSON body for the /api/auth/login endpoint
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}