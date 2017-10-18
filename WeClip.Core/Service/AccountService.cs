using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Repository;

namespace WeClip.Core.Service
{
    public class AccountService
    {
        private static AccountRepository repo = new AccountRepository();

        public async Task<Token> Login(LoginModel model)
        {
            return await repo.Login(model);
        }

        public async Task<JsonResult> CreateAccount(RegistrationModel model, string url)
        {
            return await repo.CreateAccount(model, url);
        }


        public async Task<UserProfile> GetUserProfile(string url)
        {
            return await repo.GetUserProfile(url);
        }

        public async Task<JsonResult> UpdateDeviceID(UserDeviceInfo info, string url)
        {
            return await repo.UpdateDeviceID(info, url);
        }

        public async Task<JsonResult> NotificationSetting(string url)
        {
            return await repo.NotificationSetting(url);
        }

        public async Task<JsonResult> SetProfile(UserProfile model, string url)
        {
            return await repo.SetProfile(model, url);
        }
    }
}
