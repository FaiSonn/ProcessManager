using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessM.Core.Models
{
    public class ProcessInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProcessPriorityClass Priority { get; set; }

        public long MemoryUsage { get; set; }

        public int ThreadCount { get; set; }

        public TimeSpan CpuTime { get; set; }

        public List<ThreadInfo> Threads { get; set; } = new List<ThreadInfo>();

        public IntPtr AffinityMask { get; set; }
    }
}