using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Facade
{
    public class DatabaseColumn
    {
        public string TABLE_CATALOG { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string ORDINAL_POSITION { get; set; }
    }
}
