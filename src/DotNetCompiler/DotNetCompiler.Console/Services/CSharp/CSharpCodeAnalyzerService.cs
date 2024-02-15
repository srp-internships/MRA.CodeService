using System.Reflection;
using Core.Extensions;
using System.Text;
using Application.DotNetCodeAnalyzer.DTO;
using Domain.Entities;
using Core.Exceptions;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System;
using System.Linq;
using DotNetCompiler.Console.Services.DotNetCompiler;
using Application.DotNetCodeAnalyzer.Commands;
using DotNetCompiler.Console.Repositories;
using DotNetCompiler.Console.Strategies;
using System.Threading.Tasks;
using System.Collections;

namespace DotNetCompiler.Console.Services.CSharp
{
    internal class CSharpCodeAnalyzerService : IDotNetCodeAnalyzerConsoleService
    {
        const string DotNetVersion = Constants.DOTNET_EIGHT_VERSION;
        const string Language = Constants.CSHARP_LANGUAGE;

        public DotNetVersionInfo DotNetVersionInfo { get; }
        IDotNetCompilerService _dotNetCompilerService;
        public CSharpCodeAnalyzerService(IDotNetFrameworkProvider dotNetFrameworkProvider, IDotNetCompilerResolver dotNetCompilerFactory)
        {
            DotNetVersionInfo = dotNetFrameworkProvider.GetDotNetVersion(DotNetVersion, Language);
            _dotNetCompilerService = dotNetCompilerFactory.GetDotNetCompilerService(DotNetVersionInfo);
        }

        public Task<CodeAnalyzeOutput> AnalyzeCodeAsync(DotNetAnalyzeCodeCommand codeAnalyzerParameter)
        {
            return Task.Run(() => AnalyzeCode(codeAnalyzerParameter));
        }

        CodeAnalyzeOutput AnalyzeCode(DotNetAnalyzeCodeCommand codeAnalyzerParameter)
        {
            bool succeded;
            StringBuilder errorStringBuilder = new();
            var compileOutput = _dotNetCompilerService.CompileTexts(codeAnalyzerParameter.Codes);
            if (compileOutput.CompileSucceeded)
            {
                succeded = true;
                var allClasses = GetTestClassesFromAssembly(compileOutput.Assembly);
                foreach (var classType in allClasses)
                {
                    var testClassInstanse = Activator.CreateInstance(classType);
                    foreach (var method in GetTestMethodsFromClassType(classType))
                    {
                        if (!RunTest(method, testClassInstanse, out string assertionFailedError))
                        {
                            succeded = false;
                            errorStringBuilder.AppendLine(assertionFailedError);
                            errorStringBuilder.AppendLine();
                            errorStringBuilder.AppendLine();
                        }
                    }
                }
            }
            else
            {
                succeded = false;
                errorStringBuilder.AppendLine("Code was not compiled");
                errorStringBuilder.AppendLine(compileOutput.Errors);
            }
            return new CodeAnalyzeOutput { Errors = errorStringBuilder.ToString(), Success = succeded };
        }

        bool RunTest(MethodInfo testMethod, object testClassInstanse, out string assertionFailedError)
        {
            assertionFailedError = string.Empty;
            using (new TestExecutionContext.IsolatedContext())
            {
                try
                {
                    var parameterCount = testMethod.GetParameters().Length;
                    if (parameterCount > 0)
                    {
                        foreach (var item in testMethod.CustomAttributes.Where(s => s.ConstructorArguments.Count == 1).Select(s => s.ConstructorArguments[0].Value))
                        {
                            if (item is IEnumerable<CustomAttributeTypedArgument> methodParams)
                            {
                                testMethod.Invoke(testClassInstanse, methodParams.Select(s => s.Value).ToArray());
                            }
                        }
                        foreach (var item in testMethod.CustomAttributes.Where(s => s.ConstructorArguments.Count > 1).Select(s => s.ConstructorArguments))
                        {
                            testMethod.Invoke(testClassInstanse, item.Select(s => s.Value).ToArray());
                        }
                        return true;
                    }
                    testMethod.Invoke(testClassInstanse, null);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.TryGetInnerMessageOfException(Constants.SUCCESS_EXCEPTION, out _))
                        return true;
                    ex.TryGetInnerMessageOfException(Constants.ASSERT_EXCEPTION, out assertionFailedError);
                    return false;
                }
            }
        }

        IEnumerable<Type> GetTestClassesFromAssembly(Assembly assembly)
        {
            return assembly.GetTypes().Where(s => GetTestMethodsFromClassType(s).Any());
        }

        IEnumerable<MethodInfo> GetTestMethodsFromClassType(Type classType)
        {
            return classType.GetMethods().Where(s => s.CustomAttributes.Any(s => s.AttributeType.Name == Constants.TEST_ATTRIBUTE || s.AttributeType.Name == Constants.TEST_CASE_ATTRIBUTE));
        }
    }
}
