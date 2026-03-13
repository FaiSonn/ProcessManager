using Xunit;
using ProcessM.Core.Services;
using System;

namespace ProcessManager.Tests.UtilityTests
{
    public class AffinityHelperTests
    {
        [Fact]
        public void BuildMask_ShouldCreateCorrectMask()
        {
            bool[] cores = { true, false, true };

            var mask = AffinityHelper.BuildMask(cores);

            Assert.Equal(new IntPtr(5), mask);
        }

        [Fact]
        public void IsCoreEnabled_ShouldReturnTrue()
        {
            var mask = new IntPtr(5);

            var result = AffinityHelper.IsCoreEnabled(mask, 0);

            Assert.True(result);
        }

        [Fact]
        public void IsCoreEnabled_ShouldReturnFalse()
        {
            var mask = new IntPtr(5);

            var result = AffinityHelper.IsCoreEnabled(mask, 1);

            Assert.False(result);
        }
    }
}