namespace CSharp.Data.Sql.Tests.Schema
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using Sql.Schema;

    [TestFixture]
    public class ClrTypeTests
    {
        [TestCaseSource(nameof(SystemTypes))]
        public void CreateClrTypeWithInteger_ShouldReturnTypeFullName((Type type, string typeName) testData)
        {
            var (type, typeName) = testData;
            new ClrType(type).TypeName.Should().Be(typeName);
        }

        private static IEnumerable<(Type, string)> SystemTypes() => 
            new[]
            {
                (typeof(int), "System.Int32"),
                (typeof(DateTime), "System.DateTime"),
                (typeof(string), "System.String"),
                (typeof(bool), "System.Boolean"),
            };
    }
}