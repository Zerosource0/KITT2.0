using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Models.DatabaseModels.BaseClass;

namespace KITT.Models.DatabaseModels
{
    class Areas : DatabaseModel
    {

        public string AreasName { get; set; }
        public Guid FkBrandsId { get; set; }
        public Guid FkLocalesId { get; set; }

    }
}
