using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E.AutoTable;
using E.Common.Serialization;
using E.Repository.System;
using E.Repository.System.Models;
using FreeSql;
using NUnit.Framework;
using Serialize.Linq.Extensions;
using Serialize.Linq.Serializers;

namespace E.Tester
{
    public class SerializeExpressionTester
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SerializeTest()
        {
            Expression<Func<DbConfig, bool>> expr = config => config.IsEnable;
            //var expressionJson = expr.ToString();
            var expressionNode = expr.ToExpressionNode();

            var e = expressionNode.ToBooleanExpression<DbConfig>();

            Console.WriteLine(expressionNode);

            Console.WriteLine(e.ToString());
        }

        [Test]
        public async Task DbConfigAutoTableTest()
        {
            IFreeSql CreateFreeSql()
            {
                var newFreeSql = new FreeSqlBuilder().UseConnectionString(DataType.MySql, "Server=216.24.255.173;Database=e;User ID=E;Password=nRK6twEdWfXLFtxM")
                    .UseAutoSyncStructure(true)
                    .Build();
                newFreeSql.UseJsonMap();
                return newFreeSql;
            }

            var freeSql = CreateFreeSql();
            var systemDbContext = new SystemDbContext(freeSql);
            var autoTable = new AutoTable<DbConfig>(systemDbContext);
            Expression<Func<DbConfig, bool>> expr = config => config.IsEnable;
            var expressionNode = expr.ToExpressionNode();

            Expression<Func<DbConfig, long>> orderExpression = config => config.Id;
            var orderExpressionNode = orderExpression.ToExpressionNode();
            var result = await autoTable.PagesAsync<long>(expressionNode, orderExpressionNode, true, 0, 10);
            var json = result.Serialize();
            Console.WriteLine(json);
        }
    }
}