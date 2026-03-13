using Xunit;
using Moq;
using ProcessM.Core.Services;
using ProcessM.Core.ViewModels;
using ProcessM.Core.Models;

namespace ProcessManager.Tests.ViewModelTests
{
    public class MainViewModelTests
    {
        [Fact]
        public void KillSelected_ShouldCallService()
        {
            var mock = new Mock<IProcessService>();

            var vm = new MainViewModel(mock.Object);

            vm.SelectedProcess = new ProcessTreeNode
            {
                Id = 10
            };

            vm.KillSelected();

            mock.Verify(s => s.KillProcess(10), Times.Once);
        }
    }
}