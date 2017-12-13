using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Facade.Interfaces
{
    public interface IFacade
    {
        List<T> DataTableQuery<T>(T returnType, DatabaseOperationEnum operation);
        List<T> SingleQuery<T>(T returnType, DatabaseOperationEnum operation);
    }

    public enum DatabaseOperationEnum
    {
        Get,
        Put,
        Delete
    }
}
