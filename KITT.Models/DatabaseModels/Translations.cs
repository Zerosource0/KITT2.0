using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Models.DatabaseModels
{
    class Translations
    {
        public string TranslationsValue { get; set; }
        public Guid FkKeysId { get; set; }
        public Guid FkAreasId { get; set; }
        public Guid FkLocalesId { get; set; }
    }
}
