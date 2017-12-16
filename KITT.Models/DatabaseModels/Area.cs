using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Models.DatabaseModels.BaseClass;

namespace KITT.Models.DatabaseModels
{
    class Area : DatabaseModel
    {

        public string AreaName { get; set; }
        public Guid FkBrandId { get; set; }
        public Guid FkLocaleId { get; set; }

    }
}
