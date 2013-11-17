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
        private readonly int _timeoutMilliseconds;
        private readonly string _testFile;
        private readonly string[] _dependencies;

        public QUnitDataAttribute(string testFile, params string[] dependencies)
            : this(5000, testFile, dependencies)
        {
        }

        public QUnitDataAttribute(int timeoutMilliseconds, string testFile, params string[] dependencies)
        {
            _timeoutMilliseconds = timeoutMilliseconds;
            _testFile = testFile;
            _dependencies = dependencies;
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var arguments = GetArguments();
            var json = GetProcessOutput(arguments);

            ResultData results;

            try
            {
                results = JsonConvert.DeserializeObject<ResultData>(json);
            }
            catch
            {
                throw new Exception(json);    
            }

            var context = new QUnitResultContext(
                results.QUnitResult.Total,
                results.QUnitResult.Passed,
                results.QUnitResult.Failed,
                results.QUnitResult.Runtime,
                results.Logs);

            return results.TestResults
                .Select(p => new object[] {new QUnitTest(p.Key, p.Value.Success, p.Value.Message, context)})
                .ToArray();
        }

        private string GetArguments()
        {
            var timeout = _timeoutMilliseconds.ToString();
            var argumentsList = new List<string>
            {
                @"xunit.phantomq\xunit.phantomq.js",
                "xunit.phantomq.html",
                timeout,
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

        private class ResultData
        {
            public QUnitResult QUnitResult { get; set; }
            public IDictionary<string, TestResult> TestResults { get; set; }
            public IList<string> Logs { get; set; }
        }

        private class QUnitResult
        {
            public int Total { get; set; }
            public int Passed { get; set; }
            public int Failed { get; set; }
            public int Runtime { get; set; }
        }

        private class TestResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}