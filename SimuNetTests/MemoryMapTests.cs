using Xunit;
using SimuNet;
using System;

namespace SimuNetTests
{
    public class MemoryMapTests
    {
        [Fact]
        public void ConstructorThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MemoryMap(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MemoryMap(1, -1));
        }

        [Fact]
        public void ConstructorSetsProperties()
        {
            var map = new MemoryMap(10, 20);
            Assert.Equal(10, map.BaseAddress);
            Assert.Equal(20, map.Length);
        }

        [Fact]
        public void SetIsInvoked()
        {
            var map = new MemoryMap(10, 20);
            bool setInvoked = false;
            map.MemorySet += (addr, val) => setInvoked = true;
            map.Set(10, 5);
            Assert.True(setInvoked);
        }

        [Fact]
        public void SetWithNoSubscriptionDoesntThrow()
        {
            var map = new MemoryMap(10, 20);
            map.Set(10, 5);
        }

        [Fact]
        public void GetIsInvoked()
        {
            var map = new MemoryMap(10, 20);
            map.MemoryGet += (addr) => addr + 5;
            map.Get(10);
            Assert.Equal(15, map.Get(10));
        }

        [Fact]
        public void GetReturnsZeroWithNoSubscribers()
        {
            var map = new MemoryMap(10, 20);
            Assert.Equal(0, map.Get(10));
        }
    }
}