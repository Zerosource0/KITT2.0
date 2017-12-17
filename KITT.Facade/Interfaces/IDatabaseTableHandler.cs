using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Facade.Initialization;

namespace KITT.Facade.Interfaces
{
    public interface IDatabaseTableHandler
    {
        void CompareAndVerifyTablesInDatabase(Type @interface);
        void CreateOrModifyTablesInDatabase(Type type, List<DeviantTable> tables);
    }
}
