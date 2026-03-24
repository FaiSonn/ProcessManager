using ProcessM.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessM.Core.Services
{
    public interface IProcessService
    {
        List<ProcessInfo> GetProcesses();

        void KillProcess(int id);

        void SetAffinity(int id, IntPtr mask);

        void SetPriority(int id, ProcessPriorityClass priority);
    }
}