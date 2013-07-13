using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Xunit.Extensions;

namespace XUnit.PhantomQ
{
    public class QUnitDataAttribute : DataAttribute
    {
        private readonly string _testFile;
        private readonly string[] _dependencies;

        public QUnitDataAttribute(string testFile, params string[] dependencies)
        {
            _testFile = testFile;
            _dependencies = dependencies;
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var arguments = GetArguments();
            var json = GetProcessOutput(arguments);
            var results = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);

            return results
                .Select(p => new object[]
                {
                    new QUnitTest(p.Key, p.Value)
                })
                .ToArray();
        }

        private string GetArguments()
        {
            var argumentsList = new List<string>
            {
                @"xunit.phantomq\xunit.phantomq.server.js",
                "xunit.phantomq.html",
                _testFile
            };

            if (_dependencies != null)
                argumentsList.AddRange(_dependencies);

            return String.Join(" ", argumentsList);
        }

        private static string GetProcessOutput(string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                FileName = QUnitDataFileHelper.PhantomJsExePath,
                WorkingDirectory = QUnitDataFileHelper.WorkingDirectory,
                Arguments = arguments
            };

            string output, error;
            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                process.WaitForExit(5000);

                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }

            if (!String.IsNullOrWhiteSpace(error))
                Trace.WriteLine(error);

            return output.Trim();
        }
    }
}