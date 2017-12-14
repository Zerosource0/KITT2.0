using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Models.DatabaseModels
{
    class Translation
    {
        public string TranslationValue { get; set; }
        public Guid FkKeyId { get; set; }
        public Guid FkAreaId { get; set; }
        public Guid FkLocaleId { get; set; }
    }
}
