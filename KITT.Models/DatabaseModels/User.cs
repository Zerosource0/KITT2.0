using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace KITT.Models.DatabaseModels
{
    public class User : IUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public string Id
        {
            get { return UserId.ToString(); }
        }
    }
}
