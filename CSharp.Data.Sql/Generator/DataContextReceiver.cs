namespace CSharp.Data.Sql.Generator
{
    using System.Collections.Generic;
    using Errors;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Util.Func;

    using static SyntaxReceiverRules;

    public sealed class DataContextReceiver : ISyntaxReceiver
    {
        public List<Result<ConnectionMetadata>> Candidates { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax) return;

            ClassIsSubClassOfMarker(classDeclarationSyntax)
                .Then(ClassIsPartialClass)
                .Then(ClassIsWithoutConstructorWithStringArgument)
                .Then(ClassHasANamespace)
                .OnSuccess(x =>
                    Candidates.AddRange(ClassAttributesAreValid(x)))
                .OnError((SyntaxError error) =>
                    Candidates.Add(Failure<ConnectionMetadata, SyntaxError>.Fail(error)))
                .OnError((UnknownSyntax _) => { });
        }
    }
}