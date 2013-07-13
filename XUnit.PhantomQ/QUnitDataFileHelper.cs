using System;
using System.Configuration;
using System.IO;

namespace XUnit.PhantomQ
{
    internal static class QUnitDataFileHelper
    {
        private const string WorkingDirectoryKey = "PhantomQ.WorkingDirectory";
        private const string PhantomJsExePathKey = "PhantomQ.PhantomJsExePath";
        
        private static string _workingDirectory;
        public static string WorkingDirectory
        {
            get
            {
                if (_workingDirectory == null)
                {
                    _workingDirectory = ConfigurationManager.AppSettings[WorkingDirectoryKey];

                    if (String.IsNullOrWhiteSpace(_workingDirectory))
                        _workingDirectory = GetWorkingDirectory();
                }

                if (String.IsNullOrWhiteSpace(_workingDirectory))
                    throw new InvalidOperationException("Unable to locate WorkingDirectory");

                return _workingDirectory;
            }
        }

        private static string GetWorkingDirectory()
        {
            const string binDirectoryName = "bin";

            var directory = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(directory);

            while (true)
            {
                if (directoryInfo.Name == binDirectoryName)
                    return directoryInfo.Parent == null
                        ? String.Empty 
                        : directoryInfo.Parent.FullName;

                if (directoryInfo.Parent == null)
                    break;

                directoryInfo = directoryInfo.Parent;
            }

            return String.Empty;
        }

        private static string _phantomJsExePath;
        public static string PhantomJsExePath
        {
            get
            {
                if (_phantomJsExePath == null)
                {
                    _phantomJsExePath = ConfigurationManager.AppSettings[PhantomJsExePathKey];

                    if (String.IsNullOrWhiteSpace(_phantomJsExePath))
                        _phantomJsExePath = FindPhantomJsExePath();
                }

                if (String.IsNullOrWhiteSpace(_phantomJsExePath))
                    throw new InvalidOperationException("Unable to locate PhantomJs.exe directory");

                return _phantomJsExePath;
            }
        }

        private static string FindPhantomJsExePath()
        {
            const string packagesDirectoryPattern = "packages";
            const string phantomJsExeDirPattern = "phantomjs.exe*";
            const string phantomJsExeFile = @"tools\phantomjs\phantomjs.exe";

            var directory = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(directory);

            while (true)
            {
                var packageDirectories = directoryInfo.GetDirectories(packagesDirectoryPattern, SearchOption.TopDirectoryOnly);
                if (packageDirectories.Length == 1)
                {
                    var exeDirectories = packageDirectories[0].GetDirectories(phantomJsExeDirPattern, SearchOption.TopDirectoryOnly);
                    if (exeDirectories.Length == 1)
                    {
                        var phantomJsExePath = Path.Combine(exeDirectories[0].FullName, phantomJsExeFile);
                        if (File.Exists(phantomJsExePath))
                            return phantomJsExePath;
                    }
                }

                if (directoryInfo.Parent == null)
                    break;

                directoryInfo = directoryInfo.Parent;
            }

            return String.Empty;
        }
    }
}
