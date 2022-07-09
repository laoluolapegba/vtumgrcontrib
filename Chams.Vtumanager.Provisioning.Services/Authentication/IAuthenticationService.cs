using Chams.Vtumanager.Provisioning.Entities;

namespace Chams.Vtumanager.Provisioning.Services.Authentication
{
    public interface IAuthenticationService
    {
        User Authenticate(UserLogin userLogin);
        public string GenerateToken(User userModel);
    }
}