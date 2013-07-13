using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit.Extensions;

namespace XUnit.PhantomQ.WebTests
{
    public class JavaScriptTests
    {
        [Theory, QUnitData("TestScripts/MiscTests.js")]
        public void MiscTests(QUnitTest test)
        {
            test.AssertSuccess();
        }

        [Theory, QUnitData("TestScripts/AddTenTests.js", "Scripts/AddTen.js")]
        public void ReturnFiveTests(QUnitTest test)
        {
            test.AssertSuccess();
        }
    }
}