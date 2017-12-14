using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using KITT.Identity;
using Microsoft.AspNet.Identity;
using KITT.Models.DatabaseModels;
using KITTBackend.Identity;

namespace KITT.Identity
{
    public class AuthRepository : IDisposable
    {

        private UserManager<User> _userManager;

        public AuthRepository()
        {
            _userManager = new UserManager<User>(new CustomUserStore());
            _userManager.UserValidator = new UserValidator<User>(_userManager) {AllowOnlyAlphanumericUserNames = false};
        }

        public async Task<IdentityResult> RegisterUser(User usersModel, string unhashedPassword)
        {
            //log.Debug("AuthRepository.RegisterUser: " + usersModel.UserName + " " + unhashedPassword);
            var result = await _userManager.CreateAsync(usersModel, unhashedPassword);

            return result;
        }

        public async Task<User> FindUser(string userName, string password)
        {

            //log.Debug("AuthRepository.Find: " + userName + " " + password);
            User user = await _userManager.FindAsync(userName, password);

            return user;
        }


        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}