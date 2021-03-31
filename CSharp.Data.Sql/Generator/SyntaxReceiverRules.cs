namespace CSharp.Data.Sql.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Errors;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Util.Func;

    using static Common.ConnectionSetAttribute;
    public static class SyntaxReceiverRules
    {
        public static Result<ClassDeclarationSyntax> ClassIsSubClassOfMarker(
            ClassDeclarationSyntax classDeclarationSyntax) =>
            classDeclarationSyntax
                .BaseList
                ?.Types
                .Any(x => x.Type.GetText().ToString().Trim() == nameof(SqlDataProvider)) ?? false
                ? Success<ClassDeclarationSyntax>.Succeed(classDeclarationSyntax)
                : Failure<ClassDeclarationSyntax, UnknownSyntax>.Fail(new UnknownSyntax());

        public static Result<ClassDeclarationSyntax> ClassIsPartialClass(
            ClassDeclarationSyntax classDeclarationSyntax) =>
            classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                ? Success<ClassDeclarationSyntax>.Succeed(classDeclarationSyntax)
                : Failure<ClassDeclarationSyntax, SyntaxError>.Fail(new SyntaxError("Must be a partial class"));

        public static Result<ClassDeclarationSyntax> ClassIsWithoutConstructorWithStringArgument(
            ClassDeclarationSyntax classDeclarationSyntax) =>
            classDeclarationSyntax.ChildNodes()
                    .FirstOrDefault(x => x.IsKind(SyntaxKind.ConstructorDeclaration))
                is ConstructorDeclarationSyntax constructorSyntax
            && constructorSyntax.ParameterList.Parameters.Count == 1
            && constructorSyntax.ParameterList.Parameters.First().Type?.GetFirstToken().Text == "string"
                ? Failure<ClassDeclarationSyntax, SyntaxError>.Fail(
                    new SyntaxError("Class must not have a constructor with a single string argument"))
                : Success<ClassDeclarationSyntax>.Succeed(classDeclarationSyntax);

        public static Result<(ClassMetaData, SyntaxList<AttributeListSyntax>)> ClassHasANamespace(
            ClassDeclarationSyntax classDeclarationSyntax)
        {
            var (foundNamespace, nameSpaceName) = GetClassNamespace(classDeclarationSyntax);
            return foundNamespace
                ? Success<(ClassMetaData, SyntaxList<AttributeListSyntax>)>.Succeed(
                    (new ClassMetaData(nameSpaceName, classDeclarationSyntax.Identifier.Text),
                        classDeclarationSyntax.AttributeLists))
                : Failure<(ClassMetaData, SyntaxList<AttributeListSyntax>), SyntaxError>.Fail(
                    new SyntaxError("Class must be declared within a namespace"));
        }

        public static IReadOnlyCollection<Result<ConnectionMetadata>> ClassAttributesAreValid(
            (ClassMetaData classMetaData, SyntaxList<AttributeListSyntax> syntaxList) tuple)
        {
            var (classMetaData, syntaxList) = tuple;
            var connectionSet = nameof(ConnectionSetAttribute).Replace(nameof(Attribute), string.Empty);

            var attributes = syntaxList
                .SelectMany(x => x.Attributes)
                .Where(x => x.Name.ToString().StartsWith(connectionSet))
                .ToList();

            if (attributes.Any(x => x.ArgumentList?.Arguments is null))
                return new[]
                {
                    Failure<ConnectionMetadata, SyntaxError>.Fail(
                        new SyntaxError($"There are no properties for the {nameof(ConnectionSetAttribute)}"))
                };

            var connectionMetadataResults =
                attributes
                    .Select(x => GetAttributeProperty(x.ArgumentList.Arguments))
                    .Select(CollectAttributeProperties)
                    .Select(x => CollectConnectionMetaData(x, classMetaData))
                    .ToList();

            return connectionMetadataResults;
        }

        static (Result<string> connectionString, Result<string> dbType) CollectAttributeProperties(Func<string, string, Result<string>> func)
        {
            var connectionStringResult = func(ConnectionStringName(),
                $"{nameof(ConnectionSetAttribute)} must have a connection string property");

            var dbTypeResult = func(DatabaseTypeName(),
                $"{nameof(ConnectionSetAttribute)} must have a database type property");

            return (connectionStringResult, dbTypeResult);
        }

        private static Result<ConnectionMetadata> CollectConnectionMetaData(
            (Result<string> connectionString, Result<string> dbType) tuple, ClassMetaData metaData) =>
            tuple switch
            {
                (Success<string> connectionString, Success<string> dbType) when Enum.TryParse<DatabaseType>(dbType.Value, out var databaseType) =>
                   Success<ConnectionMetadata>.Succeed(new ConnectionMetadata(metaData, connectionString.Value, databaseType)),
                (Success<string> _, Success<string> dbType) => Failure<ConnectionMetadata, SyntaxError>.Fail(new SyntaxError($"Unknown Database type {dbType.Value}")),
                (Failure<string, SyntaxError> failure, Success<string> _) => Failure<ConnectionMetadata, SyntaxError>.Fail(failure.ResultError),
                (Success<string>, Failure<string, SyntaxError> failure) => Failure<ConnectionMetadata, SyntaxError>.Fail(failure.ResultError),
                (Failure<string,SyntaxError> f1, Failure<string, SyntaxError> f2) => Failure<ConnectionMetadata, SyntaxErrorConcatenated>.Fail(new SyntaxErrorConcatenated(f1.ResultError, f2.ResultError)),
                _ => Failure<ConnectionMetadata, UnknownSyntax>.Fail(new UnknownSyntax()),
            };

        private static Func<string, string, Result<string>> GetAttributeProperty(
            SeparatedSyntaxList<AttributeArgumentSyntax> syntaxList) =>
            (propertyName, errorMessage) =>
            {
                var attributeArgument = syntaxList.FirstOrDefault(x => x?.NameEquals?.Name.Identifier.Text == propertyName);
                var propertyText = attributeArgument?.Expression.GetLastToken().ValueText;
                return propertyText is not null
                    ? Success<string>.Succeed(propertyText)
                    : Failure<string, SyntaxError>.Fail(new SyntaxError(errorMessage));
            };

        private static (bool foundNamespace, string nameSpaceName) GetClassNamespace(SyntaxNode classDeclarationSyntax)
        {
            var nameSpace = GetAllParentNodes(classDeclarationSyntax)
                .FirstOrDefault(x => x is NamespaceDeclarationSyntax) as NamespaceDeclarationSyntax;

            return (nameSpace?.Name != null, nameSpace?.Name.GetFirstToken().ValueText);

            static IList<SyntaxNode> GetAllParentNodes(SyntaxNode syntaxNode)
            {
                var result = new List<SyntaxNode>();

                var node = syntaxNode?.Parent;
                while (node != null)
                {
                    result.Add(node);
                    node = node?.Parent;
                }

                return result;
            }
        }
    }
}