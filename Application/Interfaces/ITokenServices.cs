namespace Application.Interfaces
{
    public interface ITokenServices
    {
        string GetToken(string userId, string userName, string email, IList<string> roles);
    }
}
