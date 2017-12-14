using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Models.DatabaseModels.BaseClass;

namespace KITT.Models.DatabaseModels
{
    public class BrandLocale : DatabaseModel
    {
        public int BrandLocaleId { get; set; }
        public int FkBrandId { get; set; }
        public int FkLocaleId { get; set; }
    }
}
