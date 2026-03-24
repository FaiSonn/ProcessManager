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
            // 1. Настройка Mock-сервиса
            var mock = new Mock<IProcessService>();

            // 2. Инициализация ViewModel
            var vm = new MainViewModel(mock.Object);

            // 3. Создание тестового узла (ProcessTreeNode)
            var node = new ProcessTreeNode
            {
                Id = 10,
                Name = "TestProcess",
                Memory = 1024,
                Priority = System.Diagnostics.ProcessPriorityClass.Normal
            };

            // 4. Присвоение (теперь не вызовет NotImplementedException)
            vm.SelectedProcess = node;

            // 5. Выполнение метода
            vm.KillSelected();

            // 6. Проверка, что метод KillProcess был вызван именно с Id = 10
            mock.Verify(s => s.KillProcess(10), Times.Once);
        }
    }
}