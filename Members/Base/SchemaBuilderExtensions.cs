using Dapper;
using System.Linq;
using YesSql;
using YesSql.Sql;
using YesSql.Sql.Schema;

namespace Members.Base
{
    public static class SchemaBuilderExtensions
    {
        public static void ExecuteSql(this ISchemaBuilder schemaBuilder,string sql)
        {
            var interpreter = (ICommandInterpreter)schemaBuilder.GetType().GetField("_commandInterpreter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(schemaBuilder);
           var rawsql=interpreter.CreateSql(new SqlStatementCommand(sql));
            schemaBuilder.Connection.Execute(rawsql.FirstOrDefault(),null,schemaBuilder.Transaction);
        }
    }
}
