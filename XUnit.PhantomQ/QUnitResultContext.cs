using System.Collections.Generic;

namespace XUnit.PhantomQ
{
    public class QUnitResultContext
    {
        public QUnitResultContext(int total, int passed, int failed, int runtime, IList<string> logs)
        {
            Total = total;
            Passed = passed;
            Failed = failed;
            Runtime = runtime;
            Logs = logs;
        }

        public int Total { get; private set; }
        public int Passed { get; private set; }
        public int Failed { get; private set; }
        public int Runtime { get; private set; }
        public IList<string> Logs { get; private set; }
    }
}