using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Models.DatabaseModels
{
    public class UsersLocales
    {
        public int UsersLocalesId { get; set; }
        public int FkUsersId { get; set; }
        public int FkLocalesId { get; set; }
    }
}
