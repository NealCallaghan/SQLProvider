//namespace CSharp.Data.Sql.Tests.Schema.Provider
//{
//    using System.Data;
//    using FluentAssertions;
//    using Moq;
//    using NUnit.Framework;

//    using static Sql.Schema.Provider.SqlServer.SqlServerSchemaProvider;

//    [TestFixture]
//    public class SqlServerSchemaProviderTests
//    {
//        [Test]
//        public void GetTableList_CallsDisposeAction()
//        {
//            var mockAdapter = new Mock<IDataAdapter>();

//            mockAdapter.Setup(x => x.Fill(It.IsAny<DataSet>())).Callback((DataSet x) =>
//            {
//                x.Tables.Add("Table");
//                x.Tables[0].Columns.Add(new DataColumn("name"));

//                var row = x.Tables[0].NewRow();
//                row[0] = "Test";

//                x.Tables[0].Rows.Add(row);
//            });

//            var actionWasCalled = false;
//            void TestAction() => actionWasCalled = true;

//            GetTableList(mockAdapter.Object, TestAction, "name");
//            actionWasCalled.Should().BeTrue();
//        }

//        [Test]
//        public void GetTableList_ReturnsCorrectNumberOfItems_WhenGivenRows()
//        {
//            var mockAdapter = new Mock<IDataAdapter>();

//            mockAdapter.Setup(x => x.Fill(It.IsAny<DataSet>())).Callback((DataSet x) =>
//            {
//                x.Tables.Add("Table");
//                x.Tables[0].Columns.Add(new DataColumn("name"));

//                var row = x.Tables[0].NewRow();
//                row[0] = "Test";

//                x.Tables[0].Rows.Add(row);

//                var row2 = x.Tables[0].NewRow();
//                row2[0] = "Test";

//                x.Tables[0].Rows.Add(row2);
//            });

//            var result = GetTableList(mockAdapter.Object, () => { }, "name");
//            result.Should().HaveCount(2);
//        }

//        [Test]
//        public void GetTableList_ReturnsNoItems_WhenNoRows()
//        {
//            var mockAdapter = new Mock<IDataAdapter>();

//            mockAdapter.Setup(x => x.Fill(It.IsAny<DataSet>())).Callback((DataSet x) =>
//            {
//                x.Tables.Add("Table");
//                x.Tables[0].Columns.Add(new DataColumn("name"));
//            });

//            var result = GetTableList(mockAdapter.Object, () => { }, "name");
//            result.Should().HaveCount(0);
//        }

//        [Test]
//        public void GetTable_ReturnsSchemaTableWithSingleColumn_WhenOneColumnGiven()
//        {
//            var mockAdapter = new Mock<ISchemaDataAdapter>();

//            mockAdapter.Setup(x => x.FillSchemaDataTable(It.IsAny<DataTable>(), It.IsAny<SchemaType>())).Callback((DataTable x, SchemaType _) =>
//            {
//                x.Columns.Add(new DataColumn("SomeName", typeof(int)));
//            });

//            var result = GetTable(mockAdapter.Object, "tableName");
//            result.Columns.Length.Should().Be(1);
//        }

//        [Test]
//        public void GetTable_ReturnsSchemaTableWithCorrectName_WhenOneColumnGiven()
//        {
//            var mockAdapter = new Mock<ISchemaDataAdapter>();

//            mockAdapter.Setup(x => x.FillSchemaDataTable(It.IsAny<DataTable>(), It.IsAny<SchemaType>())).Callback((DataTable x, SchemaType _) =>
//            {
//                x.Columns.Add(new DataColumn("SomeName", typeof(int)));
//            });

//            var result = GetTable(mockAdapter.Object, "tableName");
//            result.TableName.Should().Be("tableName");
//        }

//        [Test]
//        public void GetTable_ReturnsSchemaTableWithTwoColumns_WhenOneColumnIsPrimaryKeyAndOneIsNormal()
//        {
//            var mockAdapter = new Mock<ISchemaDataAdapter>();

//            mockAdapter.Setup(x => x.FillSchemaDataTable(It.IsAny<DataTable>(), It.IsAny<SchemaType>())).Callback((DataTable x, SchemaType _) =>
//            {

//                var primaryCol = new DataColumn("PrimaryKeyColumn", typeof(int));
//                x.Columns.Add(primaryCol);
//                x.PrimaryKey = new[] { primaryCol };

//                x.Columns.Add(new DataColumn("SomeName", typeof(int)));
//            });

//            var result = GetTable(mockAdapter.Object, "tableName");
//            result.Columns.Length.Should().Be(2);
//        }
//    }
//}