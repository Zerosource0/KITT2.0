using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KITT.Facade.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using Serilog;

namespace KITT.Facade.Initialization
{
    public class DatabaseTableHandler : IDatabaseTableHandler
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["KITTDB"].ConnectionString;
        private IDbConnection _connection = null;
        private BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        //TODO: Handle tables that exist, but are incorrect.

        /// <summary>
        /// Verifies that the database has tables that represent the classes that implement the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interface">The interface</param>
        /// <returns>void</returns>
        public void CompareAndVerifyTablesInDatabase(Type @interface)
        {

            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";

            using (_connection = new SqlConnection(_connectionString))
            {
                var tables = _connection.Query<DatabaseTable>(query).ToList();

                if (!tables.Any())
                {
                    Log.Logger.Debug("The database is empty.");
                    CreateOrModifyTablesInDatabase(@interface, null);
                    return;
                }

                var deviantTables = AnalyzeTables(@interface, out var numberOfTypes);

                if (deviantTables.All(x => x.TableExistsInDb))
                {
                    Log.Logger.Debug("Tables in database match all classes implementing " + @interface.Name);
                    return;
                }

                Log.Logger.Debug("Tables in database deviates from classes implementing " + @interface.Name);
                Log.Logger.Debug("Number of classes implementing " + @interface.Name + ": " + numberOfTypes + ", Deviant types from database: " + deviantTables.Count);

                CreateOrModifyTablesInDatabase(@interface, deviantTables);
            }
        }

        private List<DeviantTable> AnalyzeTables(Type type, out int numberOfTypes)
        {
            Log.Logger.Debug("Comparing table in database using baseclass: " + type.Name);

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);
            numberOfTypes = types.Count();

            List<DeviantTable> deviantTables = new List<DeviantTable>();
            Log.Logger.Debug("----------------------------------------------------");
            foreach (Type t in types)
            {
                var deviants = CompareTableToObject(t);
                if (deviants != null)
                {
                    deviantTables.Add(deviants);
                }
                Log.Logger.Debug("----------------------------------------------------");
            }
            Log.Logger.Debug("Done comparing table with types extending " + type.Name);
            return deviantTables;
        }


        /// <summary>
        /// Compares name and members of type t to table with corresponding name in database.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>Returns a DeviantTalbe that represents whats missing in the db, and if the table exists</returns>
        private DeviantTable CompareTableToObject(Type t)
        {
            string query =
                "SELECT TABLE_CATALOG, TABLE_NAME, COLUMN_NAME, DATA_TYPE, ORDINAL_POSITION " +
                "FROM KITT.INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME = N'" + t.Name + "'";
            //Get all columns from table with name of t
            List<DatabaseColumn> columns = _connection.Query<DatabaseColumn>(query).ToList();

            //check if database had such table
            if (!columns.Any())
            {
                Log.Logger.Debug("The database does not contain table with name " + t.Name);
                return MissingTable(t);
            }

            Log.Logger.Debug("Comparing type " + t.Name + " to database equivalent");
            Dictionary<string, Type> properties = t.GetProperties(bindingFlags).ToDictionary(x => x.Name, y => y.PropertyType);

            IDictionary<string, Type> deviantColumns = new Dictionary<string, Type>();
            IDictionary<string, bool> deviantExistingColums = new Dictionary<string, bool>();
            foreach (var property in properties)
            {

                var column = columns.FirstOrDefault(x => ConvertIdName(x.COLUMN_NAME).Equals(property.Key));
                if (column != null)
                {
                    if (ConvertToStandardType(column.DATA_TYPE) == property.Value)
                    {
                        Log.Logger.Debug("Table " + t.Name + " has column " + ConvertIdName(column.COLUMN_NAME) + ", with correct type: " + ConvertToStandardType(column.DATA_TYPE));
                    }
                    else
                    {
                        Log.Logger.Debug("Table " + t.Name + " has column " + ConvertIdName(column.COLUMN_NAME) + ", with wrong type: " + ConvertToStandardType(column.DATA_TYPE) + " should be " + property.Value);
                        deviantColumns.Add(property);
                        deviantExistingColums.Add(property.Key, true);
                    }
                }
                else
                {
                    Log.Logger.Debug("Table " + t.Name + " is missing column " + property.Key);
                    deviantColumns.Add(property);
                }

            }

            //After this loop, we know that all the all the colums are set to be created - but what about the columns that doesn't match the classes?

            if (!deviantExistingColums.Any() && !deviantColumns.Any())
            {
                Log.Logger.Debug("Table " + t.Name + " is a match");
            }

            var deviant = new DeviantTable
            {
                TableExistsInDb = true,
                DeviantColumns = deviantColumns,
                ExistingColums = deviantExistingColums,
                Name = t.Name
            };
            return deviant;
        }

        private string ConvertIdName(string name)
        {
            if (name.EndsWith("id", StringComparison.OrdinalIgnoreCase))
            {
                return "Id";
            }
            return name;
        }

        private Type ConvertToStandardType(string sqlType)
        {
            switch (sqlType)
            {
                case "nchar":
                    return typeof(string);
                case "int":
                    return typeof(int);
                case "uniqueidentifier":
                    return typeof(Guid);
                default:
                    return null;
            }

        }

        private DeviantTable MissingTable(Type t)
        {
            Dictionary<string, Type> propertyTypes = t.GetProperties(bindingFlags).ToDictionary(x => x.Name, y => y.PropertyType);
            var table = new DeviantTable
            {
                Name = t.Name,
                DeviantColumns = propertyTypes,
                TableExistsInDb = false
            };
            return table;
        }


        public void CreateOrModifyTablesInDatabase(Type type, List<DeviantTable> tables)
        {

            var hello = "";

            return;
            //throw new NotImplementedException();
        }

    }
}
