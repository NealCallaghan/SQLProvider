namespace CSharp.Data.Sql.Tests.Generator
{
    using FluentAssertions;
    using NUnit.Framework;

    using static CSharp.Data.Sql.Generator.NamespaceWrapper;

    [TestFixture]
    public class NamespaceWrapperTests
    {
        [Test]
        public void WrapInNameSpace_WithNoCodeProvided_ReturnsNamespace()
        {
            var result = 
$@"namespace CSharp.Data.Sql
{{
    
}}";
            WrapInNameSpace(string.Empty).Should().Be(result);
        }

        [Test]
        public void WrapInNameSpace_WithClassCodeProvided_ReturnsNamespace()
        {
            var classCode =
                $@"
    public class Foo
    {{
        public string MyProp {{ get; }}
    }}
";
            var result =
                $@"namespace CSharp.Data.Sql
{{
    {classCode}
}}";
            WrapInNameSpace(classCode).Should().Be(result);
        }
    }
}