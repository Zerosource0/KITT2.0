using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Models.DatabaseModels
{
    public class UserLocales
    {
        public int UserLocaleId { get; set; }
        public int FkUserId { get; set; }
        public int FkLocaleId { get; set; }
    }
}
