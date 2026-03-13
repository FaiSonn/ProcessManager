using System.Collections.Generic;
using System.Diagnostics;
using ProcessM.Core.Models;

namespace ProcessM.Core.Services
{
    public interface IProcessService
    {
        List<ProcessInfo> GetProcesses();

        void KillProcess(int id);

        void SetPriority(int id, ProcessPriorityClass priority);
    }
}