using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ProcessM.Core.Models;
using ProcessM.Core.Services;

namespace ProcessM.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IProcessService _service;

        public ObservableCollection<ProcessTreeNode> Processes { get; }
            = new ObservableCollection<ProcessTreeNode>();

        private ProcessTreeNode _selectedProcess;

        public ProcessTreeNode SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() : this(new ProcessService()) { }

        public MainViewModel(IProcessService service)
        {
            _service = service;
        }

        public async Task RefreshAsync()
        {
            var processList = await Task.Run(() => _service.GetProcesses());

            var uiList = processList.Select(p => new ProcessTreeNode
            {
                Id = p.Id,
                Name = p.Name,
                Memory = p.MemoryUsage,
                Priority = p.Priority
            }).ToList();

            SyncCollection(uiList);
        }

        public void SyncCollection(
            System.Collections.Generic.List<ProcessTreeNode> newList)
        {
            foreach (var proc in newList)
            {
                var existing = Processes.FirstOrDefault(p => p.Id == proc.Id);

                if (existing == null)
                    Processes.Add(proc);
                else
                {
                    existing.Memory = proc.Memory;
                    existing.Name = proc.Name;
                    existing.Priority = proc.Priority;
                }
            }
        }

        public void KillSelected()
        {
            if (SelectedProcess != null)
                _service.KillProcess(SelectedProcess.Id);
        }

        public void ChangePriority(ProcessPriorityClass priority)
        {
            if (SelectedProcess != null)
                _service.SetPriority(SelectedProcess.Id, priority);
        }
    }
}