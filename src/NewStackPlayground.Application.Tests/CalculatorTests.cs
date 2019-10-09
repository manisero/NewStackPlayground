using FluentAssertions;
using Xunit;

namespace NewStackPlayground.Application.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void add_1_2___3()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(1, 2);

            // Assert
            result.Should().Be(3);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 2, 4)]
        public void adds(int a, int b, int expectedResult)
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(a, b);
            
            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
