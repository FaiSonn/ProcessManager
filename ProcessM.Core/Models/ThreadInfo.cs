using System;
using System.Diagnostics;

namespace ProcessM.Core.Models
{
    public class ThreadInfo
    {
        public int Id { get; set; }

        public ThreadPriorityLevel Priority { get; set; }

        public ThreadState State { get; set; }

        public TimeSpan CpuTime { get; set; }
    }
}