using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Models.DatabaseModels
{
    public class UserBrand
    {
        public string FkUserId { get; set; }
        public string FkBrandId { get; set; }
    }
}
