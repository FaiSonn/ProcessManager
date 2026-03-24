using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ProcessM.Core.Models;
using ProcessM.Core.Services;
using System.Diagnostics;
using System;

namespace ProcessM.Core.ViewModels
{
    public class MainViewModel
    {
        private readonly ProcessService _service; // конкретный тип нужен для BuildProcessTree

        public ObservableCollection<ProcessInfo> Processes { get; set; }
        public ObservableCollection<ProcessInfo> ProcessTree { get; set; }
        public ProcessInfo SelectedProcess { get; set; }

        public MainViewModel(ProcessService service) // конкретный тип
        {
            _service = service;
            Processes = new ObservableCollection<ProcessInfo>();
            ProcessTree = new ObservableCollection<ProcessInfo>();
        }

        public async Task RefreshAsync()
        {
            var processList = await Task.Run(() => _service.GetProcesses());
            var tree = _service.BuildProcessTree(processList);

            Processes.Clear();
            ProcessTree.Clear();

            foreach (var proc in processList)
                Processes.Add(proc);

            foreach (var root in tree)
                ProcessTree.Add(root);
        }

        public void KillSelected()
        {
            if (SelectedProcess == null) return;
            int id = SelectedProcess.Id;
            _service.KillProcess(id);
            var item = Processes.FirstOrDefault(x => x.Id == id);
            if (item != null) Processes.Remove(item);
        }

        public void ChangePriority(ProcessPriorityClass priority)
        {
            if (SelectedProcess == null) return;
            _service.SetPriority(SelectedProcess.Id, priority);
        }

        public void SetAffinity(bool[] cores)
        {
            if (SelectedProcess == null) return;
            var mask = AffinityHelper.BuildMask(cores);
            _service.SetAffinity(SelectedProcess.Id, mask);
        }
    }
}