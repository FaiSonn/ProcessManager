using ProcessM.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

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
                        Priority = SafePriority(p),
                        ParentProcessId = GetParentProcessId(p.Id)  // ← добавлено
                    });
                }
                catch { }
            }

            return list;
        }

        public void KillProcess(int id)
        {
            try
            {
                var process = Process.GetProcessById(id);
                process.Kill();
                process.WaitForExit();
            }
            catch { }
        }

        public void SetPriority(int id, ProcessPriorityClass priority)
        {
            try
            {
                Process.GetProcessById(id).PriorityClass = priority;
            }
            catch { }
        }

        public void SetAffinity(int processId, IntPtr mask)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                process.ProcessorAffinity = mask;
            }
            catch { }
        }

        private ProcessPriorityClass SafePriority(Process p)
        {
            try { return p.PriorityClass; }
            catch { return ProcessPriorityClass.Normal; }
        }

        // ← C# 7.3: using() со скобками, не через using var
        private static int GetParentProcessId(int pid)
        {
            try
            {
                using (var query = new ManagementObjectSearcher(
                    "SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = " + pid))
                {
                    foreach (ManagementObject obj in query.Get())
                        return Convert.ToInt32(obj["ParentProcessId"]);
                }
            }
            catch { }
            return 0;
        }

        // ← новый метод — строит дерево из плоского списка
        public List<ProcessInfo> BuildProcessTree(List<ProcessInfo> flatList)
        {
            var dict = new Dictionary<int, ProcessInfo>();
            foreach (var p in flatList)
            {
                p.Children.Clear();
                dict[p.Id] = p;
            }

            var roots = new List<ProcessInfo>();
            foreach (var p in flatList)
            {
                ProcessInfo parent;
                if (p.ParentProcessId != p.Id && dict.TryGetValue(p.ParentProcessId, out parent))
                    parent.Children.Add(p);
                else
                    roots.Add(p);
            }

            return roots;
        }
    }
}