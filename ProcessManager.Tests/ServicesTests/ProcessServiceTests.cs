using Xunit;
using ProcessM.Core.Services;

namespace ProcessManager.Tests.ServicesTests
{
    public class ProcessServiceTests
    {
        [Fact]
        public void GetProcesses_ShouldReturnList()
        {
            var service = new ProcessService();

            var result = service.GetProcesses();

            Assert.NotNull(result);
        }
    }
}