using Core.Exceptions;
using Domain.Entities;
using DotNetCompiler.Console.Repositories;
using DotNetCompiler.Console.Services.DotNetCompiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DotNetCompiler.Console.Services.CSharp
{
    internal class CSharpCodeCompilerService : IDotNetCompilerService
    {
        const string DotNetVersion = Constants.DOTNET_EIGHT_VERSION;
        const string Language = Constants.CSHARP_LANGUAGE;
        public DotNetVersionInfo DotNetVersionInfo { get; }

        public CSharpCodeCompilerService(IDotNetFrameworkProvider dotNetFrameworkProvider)
        {
            DotNetVersionInfo = dotNetFrameworkProvider.GetDotNetVersion(DotNetVersion, Language);
        }

        public CompileOutput CompileTexts(IEnumerable<string> codes)
        {
            try
            {
                var syntaxTrees = BuildSyntaxTree(codes);
                string assemblyName = Path.GetRandomFileName();
                CSharpCompilation compilation = CSharpCompilation.Create(
                    assemblyName,
                    syntaxTrees: syntaxTrees,
                    references: GetMetadataReferences(DotNetVersionInfo.AvailableMetaData),
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithPlatform(Platform.AnyCpu)
                    .WithUsings(DotNetVersionInfo.DefaultUsings));

                if (TryCompile(compilation, out Assembly assembly, out string errors))
                {
                    return new CompileOutput { Assembly = assembly, CompileSucceeded = true, Errors = errors };
                }
                else
                {
                    return new CompileOutput { Errors = errors, CompileSucceeded = false };
                }
            }
            catch (Exception ex)
            {
                return new CompileOutput { Errors = ex.Message, CompileSucceeded = false };
            }
        }

        bool TryCompile(CSharpCompilation compilation, out Assembly assembly, out string errors)
        {
            assembly = null;
            errors = null;
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    StringBuilder errorBuilder = new();
                    foreach (var item in failures)
                    {
                        errorBuilder.AppendLine(item.GetMessage());
                    }
                    errors = errorBuilder.ToString();
                    return false;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    return true;
                }
            }
        }

        IEnumerable<MetadataReference> GetMetadataReferences(IEnumerable<string> files)
        {
            List<MetadataReference> metadataReferences = new();
            foreach (var file in files)
            {
                string filePath = file;
                metadataReferences.Add(MetadataReference.CreateFromFile(filePath));
            }
            return metadataReferences;
        }

        List<SyntaxTree> BuildSyntaxTree(IEnumerable<string> codes)
        {
            List<SyntaxTree> syntaxTrees = new();
            foreach (var code in codes)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
                syntaxTrees.Add(syntaxTree);
            }
            return syntaxTrees;
        }
    }
}
