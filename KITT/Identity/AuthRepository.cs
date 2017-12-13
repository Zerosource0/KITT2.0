using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using KITTBackend.HelperClasses;
using KITTBackend.Models.ImplementedModels;
using Microsoft.AspNet.Identity;

namespace KITTBackend.Identity
{
    public class AuthRepository : IDisposable
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private UserManager<UsersModel> _userManager;

        public AuthRepository()
        {
            _userManager = new UserManager<UsersModel>(new CustomUserStore());
            _userManager.UserValidator = new UserValidator<UsersModel>(_userManager) {AllowOnlyAlphanumericUserNames = false};
        }

        public async Task<IdentityResult> RegisterUser(UsersModel usersModel, string unhashedPassword)
        {
            //log.Debug("AuthRepository.RegisterUser: " + usersModel.UserName + " " + unhashedPassword);
            var result = await _userManager.CreateAsync(usersModel, unhashedPassword);

            return result;
        }

        public async Task<UsersModel> FindUser(string userName, string password)
        {

            //log.Debug("AuthRepository.Find: " + userName + " " + password);
            UsersModel user = await _userManager.FindAsync(userName, password);

            return user;
        }


        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}