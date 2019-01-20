using SimuNetAssembler;
using System;
using Xunit;

namespace SimuNetAssembler.Tests
{
    public class MacroAssemblerTests
    {
        [Fact]
        public void Constructor_Test()
        {
            var assem = new MacroAssembler();
            Assert.False(assem.InMacro);
        }

        [Fact]
        public void BeginMacroThrowsOnNestedMacro()
        {
            var assem = new MacroAssembler();
            assem.BeginMacro("#begin call $1", 5);
            Assert.Throws<InvalidOperationException>(() =>
                assem.BeginMacro("#begin call $1", 5));
        }

        [Theory]
        [InlineData("#begin call $x")]
        [InlineData("#begin call $-")]
        [InlineData("#begin call x")]
        [InlineData("#begin call 1")]
        [InlineData("#begin call $2")]
        public void BeginMacroThrowsOnInvalidArgumentSyntax(string source)
        {
            var assem = new MacroAssembler();
            Assert.Throws<InvalidOperationException>(() =>
                assem.BeginMacro(source, 0));
        }

        [Fact]
        public void InMacroIsTrueAfterBeginMacroCall()
        {
            var assem = new MacroAssembler();
            assem.BeginMacro("#begin call $1", 5);
            Assert.True(assem.InMacro);
        }

        [Fact]
        public void EndMacroThrowsIfNotInMacro()
        {
            var assem = new MacroAssembler();
            Assert.Throws<InvalidOperationException>(() =>
                assem.EndMacro(0));
        }

        [Fact]
        public void ZeroArgMacroIsCreatedCorrectly()
        {
            var assem = new MacroAssembler();
            assem.BeginMacro("#begin call", 1);
            assem.AddToMacroSource("addi PC 2 RA");
            assem.AddToMacroSource("jump method");
            assem.EndMacro(2);

            assem.TryGetMacro("call", out Macro macro);
            Assert.Equal("call", macro.Name);
            Assert.Equal(Array.Empty<string>(), macro.Arguments);
            Assert.Equal(new[] { "addi PC 2 RA", "jump method" }, macro.Instructions);
        }

        [Fact]
        public void TwoArgMacroIsCreatedCorrectly()
        {
            var assem = new MacroAssembler();
            assem.BeginMacro("#begin call $1 $2", 1);
            assem.AddToMacroSource("addi PC $2 RA");
            assem.AddToMacroSource("jump $1");
            assem.EndMacro(2);

            assem.TryGetMacro("call", out Macro macro);
            Assert.Equal("call", macro.Name);
            Assert.Equal(new[] { "$1", "$2" }, macro.Arguments);
            Assert.Equal(new[] { "addi PC $2 RA", "jump $1" }, macro.Instructions);
        }

        [Theory]
        [InlineData(true, "#begin")]
        [InlineData(true, "#begin test")]
        [InlineData(true, "#begin test $1")]
        [InlineData(false, "random")]
        [InlineData(false, "begin")]
        [InlineData(false, "#end")]
        public void IsBeginMacroTests(bool expected, string source)
        {
            var assem = new MacroAssembler();
            Assert.Equal(expected, assem.IsBeginMacro(source));
        }

        [Theory]
        [InlineData(true, "#end")]
        [InlineData(false, "#end $1")]
        [InlineData(false, "#begin")]
        [InlineData(false, "random")]
        public void IsEndMacroTests(bool expected, string source)
        {
            var assem = new MacroAssembler();
            Assert.Equal(expected, assem.IsEndMacro(source));
        }
    }
}