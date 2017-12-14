using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KITT.Facade.Interfaces
{
    public interface IDatabaseSchemaHandler
    {
        void VerifySchema(Type @interface);
        void CreateSchema(Type type, List<Type> types);
    }
}
