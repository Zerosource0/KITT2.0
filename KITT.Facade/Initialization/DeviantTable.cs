using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Facade.Initialization
{
    public class DeviantTable
    {
        public bool TableExistsInDb;
        public string Name { get; set; }
        public IDictionary<string, Type> DeviantColumns { get; set; }
        public IDictionary<string, bool> ExistingColums { get; set; }
    }
}
