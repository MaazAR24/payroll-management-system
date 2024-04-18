namespace PayrollManagementSystem.Services.Authentication
{
    public interface IAuthService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        string GenerateToken();
    }
}
