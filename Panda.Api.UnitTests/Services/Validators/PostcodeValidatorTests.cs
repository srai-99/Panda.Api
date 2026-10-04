using FluentAssertions;
using Panda.Application.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Panda.Api.UnitTests.Services.Validators
{
    [TestFixture]
    public class PostcodeValidatorTests
    {
        [Test]
        public void Normalise_ShouldThrow_WhenPostcodeIsEmpty()
        {
            // Arrange
            var input = "   ";

            // Act
            Action act = () => PostcodeValidator.Normalise(input);

            // Assert
            act.Should()
                .Throw<ValidationException>()
                .WithMessage("Postcode is required.");
        }

        [Test]
        public void Normalise_ShouldThrow_WhenPostcodeIsInvalid()
        {
            // Arrange
            var input = "INVALID";

            // Act
            Action act = () => PostcodeValidator.Normalise(input);

            // Assert
            act.Should()
                .Throw<ValidationException>()
                .WithMessage("Invalid UK postcode.");
        }
    }
}