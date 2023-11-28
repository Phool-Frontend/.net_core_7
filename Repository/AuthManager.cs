using AutoMapper;
using ga.Contracts;
using ga.Data;
using ga.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace ga.Repository
{
    public class AuthManager : IAuthManager
    {
        private IMapper _mapper;
        private UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager) {
            this._mapper = mapper;
            this._userManager = userManager;
        }
        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }

    }
}












