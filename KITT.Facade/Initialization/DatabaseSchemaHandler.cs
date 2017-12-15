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
    public class DatabaseSchemaHandler : IDatabaseSchemaHandler
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["KITTDB"].ConnectionString;
        private IDbConnection _connection = null;

        //TODO: Handle tables that exist, but are incorrect.

        /// <summary>
        /// Verifies that the database has tables that represent the classes that implement the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interface">The interface</param>
        /// <returns>void</returns>
        public void VerifySchema(Type @interface)
        {
            Log.Logger.Debug("Verifying Schema using interface: " + @interface.Name);
            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";

            using (_connection = new SqlConnection(_connectionString))
            {
                var tables = _connection.Query<DatabaseTable>(query).ToList();

                if (!tables.Any())
                {
                    //CreateSchema(@interface, null);
                    return;
                }

                var missingOrWrongTables = AnalyzeTables(@interface, out var numberOfTypes);
                Log.Logger.Debug("Number of classes implementing " + @interface.Name + ": " + numberOfTypes + ", Missing types from database: " + missingOrWrongTables.Count);

                if (!missingOrWrongTables.Any()) return;

                //CreateSchema(@interface, missingOrWrongTables.Count == numberOfTypes ? null : missingOrWrongTables);
                return;
            }

            return;
        }

        private List<Type> AnalyzeTables(Type type, out int numberOfTypes)
        {

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);
            numberOfTypes = types.Count();

            List<Type> missingTablesTypes = new List<Type>();

            foreach (Type t in types)
            {
                var memberType = CompareTableToObject(t);
                if (memberType != null)
                {
                    missingTablesTypes.Add(memberType);
                }
            }
            return missingTablesTypes;
        }


        /// <summary>
        /// Compares name and members of type t to table with corresponding name in database.
        /// </summary>
        /// <param name="t">The Type.</param>
        /// <returns>If they are equals, returns null; if not the type is returned.</returns>
        private Type CompareTableToObject(Type t)
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
                return t;
            }

            var bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            //Compare names
            IEnumerable<string> columNames = columns.Select(x => ConvertIdName(x.COLUMN_NAME));
            IEnumerable<string> propertyNames = t.GetProperties(bindingFlags).Select(x => x.Name);

            IEnumerable<string> differences = propertyNames.Except(columNames).Union(columNames.Except(propertyNames)).ToList();
            //Return type is any differences
            if (differences.Any())
            {
                return t;
            }

            //Compare types
            IEnumerable<Type> columnTypes = columns.Select(x => ConvertToStandardType(x.DATA_TYPE)); //turn this into a dictionary.
            Dictionary<string, Type> propertyTypes = t.GetProperties(bindingFlags).ToDictionary(x => x.Name, y => y.PropertyType);

            IEnumerable<Type> typeDifferences = propertyTypes.Select(x => x.Value).Except(columnTypes).Union(columnTypes.Except(propertyTypes.Select(x => x.Value)));
            differences = propertyTypes.Where(x => typeDifferences.Contains(x.Value)).Select(x => x.Key);
            //Return type is any differences
            if (differences.Any())
            {
                return t;
            }
            return null;
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

        public void CreateSchema(Type type, List<Type> tables)
        {
            throw new NotImplementedException();
        }
    }
}
