using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WASMChat.CodeGenerators.Attributes;

namespace WASMChat.CodeGenerators.Generators;

[Generator]
public class ServiceAdderGenerator : ISourceGenerator
{
    private readonly StringBuilder _builder = ClearBuilder;

    public void Execute(GeneratorExecutionContext context)
    {
        INamedTypeSymbol attributeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(InjectScoped).FullName)!;
        
        var classesWithAttribute = context.Compilation.SyntaxTrees
            .SelectMany(st => st
                .GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(r => r.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Any(a => a.Name.GetText().ToString() == attributeSymbol.Name)));

        foreach (var @class in classesWithAttribute)
        {
            _builder.AppendLine($"public static void SayMyName(this {@class.Identifier.Text} obj)");
            _builder.AppendLine("\t=> Console.WriteLine(obj.GetType().FullName);");
        }
        SealBuilder();
            
        context.AddSource("ServiceAdder.g.cs", _builder.ToString());
    }
    
    public void Initialize(GeneratorInitializationContext context)
    { }

    private static StringBuilder ClearBuilder { get; } = new($$"""
    using System.CodeDom.Compiler;
    using System;

    namespace WASMChat.CodeGenerators.Generated;
    
    [GeneratedCode("{{nameof(ServiceAdderGenerator)}}", "0.1")]
    public static class GeneratedServiceAdder
    {
    """);

    private void SealBuilder() => _builder
        .Append("""
    }
    """);
}