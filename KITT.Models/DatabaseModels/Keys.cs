using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Models.DatabaseModels.BaseClass;

namespace KITT.Models.DatabaseModels
{
    class Keys : DatabaseModel
    {
        public string KeysName { get; set; }
        public Guid FkBrandsId { get; set; }
        public Guid FkAreasId { get; set; } 
    }
}
