using Xunit.Extensions;

namespace XUnit.PhantomQ.Tests
{
    public class JavaScriptTests
    {
        [Theory, QUnitData("TestScripts/MiscTests.js")]
        public void MiscTests(QUnitTest test)
        {
            test.AssertSuccess();
        }

        [Theory, QUnitData("TestScripts/ReturnFiveTests.js", "Scripts/ReturnFive.js")]
        public void ReturnFiveTests(QUnitTest test)
        {
            test.AssertSuccess();
        }
    }
}
