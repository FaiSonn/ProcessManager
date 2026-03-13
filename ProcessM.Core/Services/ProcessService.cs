using System.Collections.Generic;
using System.Diagnostics;
using ProcessM.Core.Models;

namespace ProcessM.Core.Services
{
    public class ProcessService : IProcessService
    {
        public List<ProcessInfo> GetProcesses()
        {
            var list = new List<ProcessInfo>();

            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    list.Add(new ProcessInfo
                    {
                        Id = p.Id,
                        Name = p.ProcessName,
                        MemoryUsage = p.WorkingSet64 / 1024 / 1024,
                        ThreadCount = p.Threads.Count,
                        CpuTime = p.TotalProcessorTime,
                        Priority = SafePriority(p)
                    });
                }
                catch { }
            }

            return list;
        }

        public void KillProcess(int id)
        {
            try { Process.GetProcessById(id).Kill(); }
            catch { }
        }

        public void SetPriority(int id, ProcessPriorityClass priority)
        {
            try { Process.GetProcessById(id).PriorityClass = priority; }
            catch { }
        }

        private ProcessPriorityClass SafePriority(Process p)
        {
            try { return p.PriorityClass; }
            catch { return ProcessPriorityClass.Normal; }
        }
    }
}