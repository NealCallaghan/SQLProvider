namespace CSharp.Data.Sql.Tests.Schema
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using Sql.Schema;
    
    using static Sql.Schema.ColumnUtilities;

    [TestFixture]
    public class ColumnTests
    {
        [TestCaseSource(nameof(GetTypesForPropertyText))]
        public void ProvidePropertyTextFromColumn_ShouldReturnCorrectPropertyText_GivenType(Type type)
        {
            var testColumn = new Column("column", new ClrType(type), false, false);

            var result = ProvidePropertyTextFromColumn(testColumn);

            var expected = $@"
        [System.Data.Linq.Mapping.Column]
        public {type.FullName} column {{ get; set; }}
";
            result.Should().Be(expected);
        }

        private static IEnumerable<Type> GetTypesForPropertyText() =>
            new[]
            {
                typeof(int),
                typeof(bool),
                typeof(DateTime),
                typeof(string),
            };

        [TestCaseSource(nameof(GetColumnNameText))]
        public void ProvidePropertyTextFromColumn_ShouldReturnCorrectColumnName_GivenName(string name)
        {
            var type = typeof(int);
            var testColumn = new Column(name, new ClrType(type), false, false);

            var result = ProvidePropertyTextFromColumn(testColumn);

            var expected = $@"
        [System.Data.Linq.Mapping.Column]
        public {type.FullName} {name} {{ get; set; }}
";
            result.Should().Be(expected);
        }

        private static IEnumerable<string> GetColumnNameText() =>
            new[]
            {
                "MyColumn",
                "SomeColumn",
                "column_,"
            };

        [Test]
        public void ProvidePropertyTextFromColumn_ShouldReturnCorrectColumnAttribute_IsNotPrimaryKey()
        {
            var type = typeof(int);
            var testColumn = new Column("column", new ClrType(type), false, false);

            var result = ProvidePropertyTextFromColumn(testColumn);

            var expected = $@"
        [System.Data.Linq.Mapping.Column]
        public {type.FullName} column {{ get; set; }}
";
            result.Should().Be(expected);
        }

        [Test]
        public void ProvidePropertyTextFromColumn_ShouldReturnCorrectColumnAttribute_IsPrimaryKey()
        {
            var type = typeof(int);
            var testColumn = new Column("column", new ClrType(type), true, false);

            var result = ProvidePropertyTextFromColumn(testColumn);

            var expected = $@"
        [System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
        public {type.FullName} column {{ get; set; }}
";
            result.Should().Be(expected);
        }
    }
}