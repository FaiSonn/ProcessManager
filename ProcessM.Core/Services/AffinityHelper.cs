using System;

namespace ProcessM.Core.Services
{
    public static class AffinityHelper
    {
        // Проверяет включено ли ядро
        public static bool IsCoreEnabled(IntPtr mask, int coreIndex)
        {
            long value = mask.ToInt64();

            return (value & (1L << coreIndex)) != 0;
        }

        // Создает маску из массива ядер
        public static IntPtr BuildMask(bool[] cores)
        {
            long mask = 0;

            for (int i = 0; i < cores.Length; i++)
            {
                if (cores[i])
                {
                    mask |= (1L << i);
                }
            }

            return new IntPtr(mask);
        }
    }
}