using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using KITT.Models.DatabaseModels;
using Microsoft.AspNet.Identity;

namespace KITT.Identity
{
    public class CustomUserStore : IUserStore<User>, IUserLoginStore<User>, IUserPasswordStore<User>, IUserSecurityStampStore<User>
    {

        private readonly string connectionString;

        public CustomUserStore()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["SqlConn"].ConnectionString;
        }
        public void Dispose()
        {

        }

        #region IUserStore

        public Task CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                user.UserId = Guid.NewGuid();
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute(
                        "insert into dbo.User(Id, UserName, PasswordHash, SecurityStamp, FirstName, LastName) " +
                        "values (@Id, @UserName, @PasswordHash, @SecurityStamp, @FirstName, @LastName)",
                        user);
            });

        }

        public Task DeleteAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("delete from User where UserId = @userId", new { user.Id });
            });
        }



        public Task<User> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            Guid parsedUserId;
            if (!Guid.TryParse(userId, out parsedUserId))
                throw new ArgumentOutOfRangeException("userId", string.Format("'{0}' is not a valid GUID.", new { userId }));

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return connection.Query<User>("select * from User where UserId = @userId", new { userId = parsedUserId }).SingleOrDefault();
            });
        }

        public Task<User> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return connection.Query<User>("select * from User where lower(UserName) = lower(@userName)", new { userName }).SingleOrDefault();
            });
        }

        public Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("update User set UserName = @userName, PasswordHash = @passwordHash, SecurityStamp = @securityStamp, FirstName = @FirstName, LastName = @LastName where Id = @Id", user);
            });
        }

        #endregion

        #region IUserLoginStore

        public virtual Task AddLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("insert into ExternalLogins(ExternalLoginId, Id, LoginProvider, ProviderKey) values(@externalLoginId, @userId, @loginProvider, @providerKey)",
                        new { externalLoginId = Guid.NewGuid(), userId = user.Id, loginProvider = login.LoginProvider, providerKey = login.ProviderKey });
            });
        }

        public virtual Task<User> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return connection.Query<User>("select u.* from User u inner join ExternalLogins l on l.Id = u.Id where l.LoginProvider = @loginProvider and l.ProviderKey = @providerKey",
                        login).SingleOrDefault();
            });
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return (IList<UserLoginInfo>)connection.Query<UserLoginInfo>("select LoginProvider, ProviderKey from ExternalLogins where UserId = @Id", new { user.Id }).ToList();
            });
        }

        public virtual Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("delete from ExternalLogins where Id = @Id and LoginProvider = @loginProvider and ProviderKey = @providerKey",
                        new { user.Id, login.LoginProvider, login.ProviderKey });
            });
        }
        #endregion

        #region IUserPasswordStore
        public virtual Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public virtual Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        #endregion

        #region IUserSecurityStampStore
        public virtual Task<string> GetSecurityStampAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetSecurityStampAsync(User user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        #endregion

    }
}