using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Models.DatabaseModels.BaseClass;

namespace KITT.Models.DatabaseModels
{
    public class BrandsLocales : DatabaseModel
    {
        public int BrandsLocalesId { get; set; }
        public int FkBrandsId { get; set; }
        public int FkLocalesId { get; set; }
    }
}
