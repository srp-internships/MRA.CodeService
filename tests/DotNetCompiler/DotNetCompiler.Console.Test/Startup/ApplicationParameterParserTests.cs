using DotNetCompiler.Console.Startup;
using NUnit.Framework;
using FluentAssertions;
using Newtonsoft.Json;
using Core.Exceptions;
using Application.DotNetCodeAnalyzer.Commands;
using System.Text;
using System;

namespace DotNetCompiler.Console.Test.Startup
{
    public class ApplicationParameterParserTests : TestBase
    {
        [Test]
        public void ApplicationTryParse_ShouldReturnFalse_WhenArgumentsLenght_IsNotEqualToOne()
        {
            var applicationParameter = new ApplicationParameterParser();
            var parsed = applicationParameter.TryParse(new string[] { "1", "2" }, out _);

            parsed.Should().BeFalse();
        }

        [Test]
        public void ApplicationTryParse_ShouldReturnFalse_StringWasNotInBase64Format()
        {
            var applicationParameter = new ApplicationParameterParser();
            var validJson = GetValidJson();
            var parsed = applicationParameter.TryParse(new string[] { validJson }, out _);

            parsed.Should().BeFalse();
        }

        [Test]
        public void ApplicationTryParse_ShouldReturnFalse_StringWasNotDeserialized()
        {
            var applicationParameter = new ApplicationParameterParser();
            string inValidJsonText = Convert.ToBase64String(Encoding.UTF8.GetBytes("I am not JSON"));
            var parsed = applicationParameter.TryParse(new string[] { inValidJsonText }, out _);

            parsed.Should().BeFalse();
        }

        [Test]
        public void ApplicationTryParse_ShouldReturnTrue_WhenArgumentsAreCorrect()
        {
            var validJson = GetValidJson();
            var jsonToBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(validJson));
            var applicationParameter = new ApplicationParameterParser();
            var parsed = applicationParameter.TryParse(new string[] { jsonToBase64 }, out DotNetAnalyzeCodeCommand parsedParamter);

            parsed.Should().BeTrue();
            parsedParamter.Should().NotBeNull();
            parsedParamter.DotNetVersionInfo.Language.Should().Be(Constants.CSHARP_LANGUAGE);
        }

        string GetValidJson()
        {
            var dotNetVersion = GetCSharpNet8VersionDTO();
            var parameter = new DotNetAnalyzeCodeCommand() { DotNetVersionInfo = dotNetVersion };
            return JsonConvert.SerializeObject(parameter);
        }
    }
}
