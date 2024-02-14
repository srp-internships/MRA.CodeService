using Domain.Entities;
using Core.Extensions;
using Core.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetCompiler.Console.Repositories
{
    internal class DotNetFrameworkProvider : IDotNetFrameworkProvider
    {
        IDirectoryProvider _directoryProvider;
        public DotNetFrameworkProvider(IDirectoryProvider directoryProvider)
        {
            _directoryProvider = directoryProvider;
        }

        List<DotNetVersionInfo> _allVersions;
        List<DotNetVersionInfo> AllVersions
        {
            get
            {
                if (_allVersions == null)
                    _allVersions = ReadVersionsFromFolder();
                return _allVersions;
            }
        }

        List<DotNetVersionInfo> ReadVersionsFromFolder()
        {
            List<DotNetVersionInfo> versions = new List<DotNetVersionInfo>();
            foreach (var language in GetLanguages())
            {
                var versionsPath = Path.Combine(_directoryProvider.GetLanguagePath(language), "Versions");
                var allVersions = Directory.GetDirectories(versionsPath);
                foreach (var version in allVersions)
                {
                    var name = version.GetDirectoryName();
                    DotNetVersionInfo dotNetVersionInfo = new();
                    dotNetVersionInfo.Language = language;
                    dotNetVersionInfo.Version = name;
                    SetMetaDataAndUsings(dotNetVersionInfo, version);
                    versions.Add(dotNetVersionInfo);
                }
            }
            return versions;
        }

        void SetMetaDataAndUsings(DotNetVersionInfo dotNetVersionInfo, string versionPath)
        {
            var defaultUsingsPath = Path.Combine(versionPath, Constants.DEFAULT_USING_PATH_NAME);
            dotNetVersionInfo.DefaultUsings = File.ReadAllLines(defaultUsingsPath);

            var metaDataPath = Path.Combine(versionPath, Constants.META_DATA_PATH_NAME);
            dotNetVersionInfo.AvailableMetaData = Directory.GetFiles(metaDataPath).ToList();
        }

        public DotNetVersionInfo GetDotNetVersion(string version, string language)
        {
            return AllVersions.FirstOrDefault(x => x.Version == version && x.Language == language);
        }

        public ICollection<DotNetVersionInfo> GetDotNetVersions()
        {
            return AllVersions;
        }

        List<string> _languages;
        List<string> Languages
        {
            get
            {
                if (_languages == null)
                    _languages = Directory.GetDirectories(_directoryProvider.GetLanguagesPath()).Select(s => s.GetDirectoryName()).ToList();
                return _languages;
            }
        }

        public ICollection<string> GetLanguages()
        {
            return Languages;
        }
    }
}
