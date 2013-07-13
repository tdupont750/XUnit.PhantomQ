using Xunit;

namespace XUnit.PhantomQ
{
    public class QUnitTest
    {
        public QUnitTest(string name, bool success)
        {
            Name = name;
            Success = success;
        }

        public string Name { get; private set; }
        public bool Success { get; private set; }

        public void AssertSuccess()
        {
            Assert.True(Success);
        }

        public void AssertSuccess(string message)
        {
            Assert.True(Success, message);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}