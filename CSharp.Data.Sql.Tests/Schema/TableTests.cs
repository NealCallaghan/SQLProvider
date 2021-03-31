namespace CSharp.Data.Sql.Tests.Schema
{
    using CSharp.Data.Sql.Schema;
    using FluentAssertions;
    using NUnit.Framework;
    using System;

    using static Sql.Schema.TableUtilities;

    [TestFixture]
    public class TableTests
    {
        [Test]
        public void ProvideDataClassTextFromTable_ShouldReturnCorrectDataClass_WhenEmptyColumns()
        {
            var testTable = new Table("table", new Column []{ } );

            var result = ProvideDataClassTextFromTable(testTable);

            var expected =$@"
    [System.Data.Linq.Mapping.Table(Name = ""table"")]
    public class table
    {{
    }}";
            result.Should().Be(expected);
        }

        [Test]
        public void ProvideDataClassTextFromTable_ShouldReturnCorrectDataClassWithCorrectName_NameGiven()
        {
            var tableName = "MyTable";
            var testTable = new Table(tableName, new Column[] { });

            var result = ProvideDataClassTextFromTable(testTable);

            var expected = $@"
    [System.Data.Linq.Mapping.Table(Name = ""{tableName}"")]
    public class {tableName}
    {{
    }}";
            result.Should().Be(expected);
        }

        [Test]
        public void ProvideDataClassTextFromTable_ShouldReturnCorrectDataClassWithColumnsAndName_NameAndColumnGiven()
        {
            var tableName = "MyTable";
            var intType = typeof(int);
            var dateTimeType = typeof(DateTime);
            var boolType = typeof(bool);
            var stringType = typeof(string);
            Column[] columns = 
            {
                new ("column1", new ClrType(intType), true, false),
                new ("column2", new ClrType(dateTimeType), false, false),
                new ("column3", new ClrType(boolType), false, false),
                new ("column4", new ClrType(stringType), false, false),
            };
            var testTable = new Table(tableName, columns);

            var result = ProvideDataClassTextFromTable(testTable);

            var expected = $@"
    [System.Data.Linq.Mapping.Table(Name = ""{tableName}"")]
    public class {tableName}
    {{
        [System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
        public {intType.FullName} column1 {{ get; set; }}

        [System.Data.Linq.Mapping.Column]
        public {dateTimeType.FullName} column2 {{ get; set; }}

        [System.Data.Linq.Mapping.Column]
        public {boolType.FullName} column3 {{ get; set; }}

        [System.Data.Linq.Mapping.Column]
        public {stringType.FullName} column4 {{ get; set; }}

    }}";
            result.Should().Be(expected);
        }
    }
}