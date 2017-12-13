using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Facade.Interfaces;

namespace KITT.Facade
{
    class Facade : IFacade
    {
        public List<T> DataTableQuery<T>(T returnType, DatabaseOperationEnum operation)
        {
            throw new NotImplementedException();
        }

        public List<T> SingleQuery<T>(T returnType, DatabaseOperationEnum operation)
        {
            throw new NotImplementedException();
        }
    }
}
