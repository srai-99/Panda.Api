using FluentAssertions;
using Panda.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Panda.Api.UnitTests.Services.Validators
{
    [TestFixture]
    public class NhsNumberValidatorTests
    {
        [Test]
        public void IsValid_ShouldReturnTrue_ForValidNhsNumber()
        {
            // Arrange
            var nhs = "1373645350";

            // Act
            var result = NhsNumberValidator.IsValid(nhs);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenContainsNonDigits()
        {
            // Arrange
            var nhs = "ABC1234567";

            // Act
            var result = NhsNumberValidator.IsValid(nhs);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenLengthIsNotTen()
        {
            // Arrange
            var nhs = "12345";

            // Act
            var result = NhsNumberValidator.IsValid(nhs);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenChecksumIsInvalid()
        {
            // Arrange
            var nhs = "1373645351"; // wrong last digit

            // Act
            var result = NhsNumberValidator.IsValid(nhs);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void EnsureValid_ShouldThrow_WhenInvalid()
        {
            // Arrange
            var nhs = "1234567890";

            // Act
            Action act = () => NhsNumberValidator.EnsureValid(nhs);

            // Assert
            act.Should()
                .Throw<ValidationException>()
                .WithMessage("Invalid NHS number.");
        }

        [Test]
        public void EnsureValid_ShouldNotThrow_WhenValid()
        {
            // Arrange
            var nhs = "1373645350";

            // Act
            Action act = () => NhsNumberValidator.EnsureValid(nhs);

            // Assert
            act.Should().NotThrow();
        }
    }
}