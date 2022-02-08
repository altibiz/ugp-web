using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YesSql;
using YesSql.Indexes;
using YesSql.Sql;

namespace Members.Base
{
    public static class YesSqlUtils
    {
        public static void ClearReduceIndexTable(this DbConnection connection, Type indexType, IConfiguration configuration, string collection = "")
        {
            var indexTable = configuration.TableNameConvention.GetIndexTable(indexType, collection);
            var documentTable = configuration.TableNameConvention.GetDocumentTable(collection);

            var bridgeTableName = indexTable + "_" + documentTable;

            try
            {
                connection.Execute($"DELETE FROM {configuration.SqlDialect.QuoteForTableName(configuration.TablePrefix + bridgeTableName)}");
                connection.Execute($"DELETE FROM {configuration.SqlDialect.QuoteForTableName(configuration.TablePrefix + indexTable)}");
            }
            catch
            {
            }
        }

        public static void ClearMapIndexTable(this DbConnection connection,Type indexType, IConfiguration configuration, string collection = "")
        {
            var indexTable = configuration.TableNameConvention.GetIndexTable(indexType, collection);

            try
            {
                connection.Execute($"DELETE FROM {configuration.SqlDialect.QuoteForTableName(configuration.TablePrefix + indexTable)}");
            }
            catch { }
        }

        public async static Task<IEnumerable<Document>> GetContentItems(this DbConnection conn,string contentItemType,IConfiguration configuration,string collection="")
        {
            var sqlBuilder = new SqlBuilder(configuration.TablePrefix, configuration.SqlDialect);
            sqlBuilder.Select();
            sqlBuilder.Selector("*");
            sqlBuilder.Table(configuration.TableNameConvention.GetDocumentTable(collection),"dd");
            sqlBuilder.WhereAnd(" dd.Type='OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions' ");
            if (!string.IsNullOrEmpty(contentItemType))
            {
                sqlBuilder.InnerJoin(configuration.TablePrefix + "ContentItemIndex", "cix", "DocumentId", "dd", "Id", "cix");
                sqlBuilder.WhereAnd("ContentType='" + contentItemType + "'");
            }
            return await conn.QueryAsync<Document>(sqlBuilder.ToSqlString());
        }

        private static IndexDescriptor GetDescriptor(ISession sess, IIndexProvider indexProvider)
        {
            MethodInfo getDesc = sess.GetType().GetMethod("GetDescriptors", BindingFlags.NonPublic | BindingFlags.Instance);
            var descs = getDesc.Invoke(sess, new object[] { indexProvider.ForType(), "" }) as IEnumerable<IndexDescriptor>;
            return descs.First();
        }

        public async static Task RefreshReduceIndex(this ISession templateSess, IIndexProvider indexProvider, string contentItemType="",string collection="")
        {
            //templateSess.Store.Configuration.Logger = new LoggerFactory(new[] { new DebugLoggerProvider() }).CreateLogger("YesSql");
            var store = await StoreFactory.CreateAndInitializeAsync(templateSess.Store.Configuration);
            using var sess = (Session)store.CreateSession();
            sess.RegisterIndexes(indexProvider);
            var desc = GetDescriptor(sess, indexProvider);
            var conn = await sess.CreateConnectionAsync();
            conn.ClearReduceIndexTable(desc.IndexType, templateSess.Store.Configuration);
            desc.Delete = (ndx, map) => ndx;//disable deletion for new stuff
            var docs = await conn.GetContentItems(contentItemType, store.Configuration, collection);
            var items = sess.Get<ContentItem>(docs.ToList(), collection);
            foreach (var itm in items)
            {
                sess.Save(itm);
            }
            await sess.SaveChangesAsync();
        }

        public async static Task RefreshMapIndex(this ISession templateSes, IIndexProvider indexProvider, string contentItemType="", string collection="")
        {
            var store = await StoreFactory.CreateAndInitializeAsync(templateSes.Store.Configuration);
            using var sess = (Session)store.CreateSession();
            sess.RegisterIndexes(indexProvider);
            var conn = await sess.CreateConnectionAsync();
            var desc = GetDescriptor(sess, indexProvider);
            //conn.ClearMapIndexTable(desc.IndexType, templateSes.Store.Configuration);
            var docs = await conn.GetContentItems(contentItemType, store.Configuration, collection);
            var items = sess.Get<ContentItem>(docs.ToList(), collection);
            foreach (var itm in items)
            {
                sess.Save(itm);
            }
            await sess.SaveChangesAsync();
        }
    }
}
