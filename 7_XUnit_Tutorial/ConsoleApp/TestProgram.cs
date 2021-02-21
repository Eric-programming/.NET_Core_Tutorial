using Xunit;

namespace ConsoleApp
{

    public class TestProgram
    {
        // [Fact]
        // public void PassEvenTest()
        // {
        //     Assert.Equal(true, Program.isEven(2));
        // }
        // [Fact]
        // public void FailEvenTest()
        // {
        //     Assert.NotEqual(true, Program.isEven(3));
        // }


        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void EvenTheory(int num)
        {
            Assert.True(Program.isEven(num));
        }
    }
}