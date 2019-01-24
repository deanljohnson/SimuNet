using Xunit;
using SimuNet;
using System;

namespace SimuNetTests
{
    public class MemoryTests
    {
        [Fact]
        public void ConstructorThrowsTests()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Memory(-1));
        }

        [Fact]
        public void OutOfBoundsGetThrowsException()
        {
            var mem = new Memory(8);
            Assert.Throws<InvalidOperationException>(() => mem[-1]);
            Assert.Throws<InvalidOperationException>(() => mem[8]);
        }

        [Fact]
        public void OutOfBoundsSetThrowsException()
        {
            var mem = new Memory(8);
            Assert.Throws<InvalidOperationException>(() => mem[-1] = 5);
            Assert.Throws<InvalidOperationException>(() => mem[8] = 5);
        }

        [Fact]
        public void IndexerTests()
        {
            var mem = new Memory(8);
            for (int i = 0; i < 8; i++)
            {
                mem[i] = i;
                Assert.Equal(i, mem[i]);
            }
        }

        [Fact]
        public void AddAndRemoveMemoryMapThrows()
        {
            var mem = new Memory(8);
            Assert.Throws<ArgumentNullException>(() => mem.AddMemoryMap(null));
            Assert.Throws<InvalidOperationException>(() => mem.AddMemoryMap(new MemoryMap(0, 4)));
            Assert.Throws<ArgumentNullException>(() => mem.RemoveMemoryMap(null));
        }

        [Fact]
        public void MemoryMapIndexingTest()
        {
            var mem = new Memory(8);
            var map = new MemoryMap(16, 8);
            mem.AddMemoryMap(map);

            int[] mapData = new int[8];
            map.MemoryGet += (addr) => mapData[addr - 16];
            map.MemorySet += (addr, val) => mapData[addr - 16] = val;

            for (int i = 16; i < 24; i++)
            {
                mem[i] = i;
                Assert.Equal(i, mapData[i - 16]);
                Assert.Equal(i, mem[i]);
            }

            Assert.Throws<InvalidOperationException>(() => mem[8]);
            Assert.Throws<InvalidOperationException>(() => mem[8] = 8);
            Assert.Throws<InvalidOperationException>(() => mem[24]);
            Assert.Throws<InvalidOperationException>(() => mem[24] = 24);
            mem.RemoveMemoryMap(map);
            Assert.Throws<InvalidOperationException>(() => mem[8]);
            Assert.Throws<InvalidOperationException>(() => mem[8] = 8);
            Assert.Throws<InvalidOperationException>(() => mem[16]);
            Assert.Throws<InvalidOperationException>(() => mem[16] = 16);
        }
    }
}