using Microsoft.AspNetCore.Identity;

namespace BloggingCode.API.Repo.Interface
{
    public interface ITokenRepo
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
