using ga.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace ga.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
    }
}
