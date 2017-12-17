using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Facade.Initialization;

namespace KITT.Facade
{
    public class SqlTranslator
    {
        private readonly string _schema = ConfigurationManager.AppSettings["DatabaseSchema"];

        #region create and update table
        
        /*
        CREATE TABLE dbo.Products  
           (
            ProductID int PRIMARY KEY NOT NULL,  
            ProductName varchar(25) NOT NULL,  
            Price money NULL,  
            ProductDescription text NULL
            )  
        GO  
        */

        public StringBuilder CreateTable(DeviantTable table)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE " + _schema + "." + table.Name);
            sb.AppendLine("(");
            foreach (KeyValuePair<string, Type> column in table.DeviantColumns)
            {
                
                
            }
            return null;
        }

        /*
        ALTER TABLE table_name
        ADD column_name datatype; 
        */

        /*
        ALTER TABLE table_name
        ALTER COLUMN column_name datatype;
        */

        /*
        ALTER TABLE table_name
        DROP COLUMN column_name;
        */

        #endregion

        /*
        INSERT dbo.Products (ProductID, ProductName, Price, ProductDescription)  
        VALUES (1, 'Clamp', 12.48, 'Workbench clamp')  
        GO 
        */


        public string ConvertToSqlType(string tableName, KeyValuePair<string, Type> column)
        {

            string name;
            string type;

            if (column.Value == typeof(string))
            {
                
            }
            if (column.Value == typeof(int))
            {

            }
            if (column.Value == typeof(bool))
            {

            }
            if (column.Value == typeof(Guid))
            {
                if (column.Key.Equals("Id", StringComparison.OrdinalIgnoreCase) )
                {
                    return tableName + column.Key + " " + "uniqueidentifier PRIMARY KEY NOT NULL CLUSTERED";
                }
                else if(column.Key.StartsWith("Fk", StringComparison.OrdinalIgnoreCase))
                {
                    return column.Key + " " + "uniqueidentifier FORIEGN KEY NOT NULL CLUSTERED";
                    /*
                     CONSTRAINT fk_inv_product_id
                     FOREIGN KEY (product_id)
                     REFERENCES products (product_id)
                     */
                }

                return "uniqueidentifier";
            }

            return null;
            //case "nchar":
            //    return typeof(string);
            //case "int":
            //    return typeof(int);
            //case "uniqueidentifier":
            //    return typeof(Guid);
            //default:
            //    return null;

        }
    }
}
