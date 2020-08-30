using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class StateManagerTests
    {

        [Fact(DisplayName ="SMN-001: ApplicationName should return name specified during construction.")]
        public void SM001()
        {
            // arrange
            var stateManager = new StateManager("MyApplication");

            // act
            var applicationName = stateManager.ApplicationName;

            // assert
            applicationName.Should().Be("MyApplication");
        }

        [Fact(DisplayName = "SMN-002: GetValue should return default value if key not present.")]
        public void SM002()
        {
            // arrange
            var stateManager = new StateManager("MyApplication");

            // act
            var value = stateManager.GetValue("test", 42);

            // assert
            value.Should().Be(42);
        }

        [Fact(DisplayName = "SMN-003: Value set with SetValue should be returned with GetValue.")]
        public void SM003()
        {
            // arrange
            var stateManager = new StateManager("MyApplication");
            stateManager.SetValue("test", 123);

            // act
            var value = stateManager.GetValue("test", 42);

            // assert
            value.Should().Be(123);
        }

        [Fact(DisplayName = "SMN-004: HasKeyFor should return false if no value set for key.")]
        public void SM004()
        {
            // arrange
            var stateManager = new StateManager("MyApplication");

            // act
            var result = stateManager.HasValueFor("test");

            // assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "SMN-005: HasKeyFor should return true if value set for key.")]
        public void SM005()
        {
            // arrange
            var stateManager = new StateManager("MyApplication");
            stateManager.SetValue("test", 123);

            // act
            var result = stateManager.HasValueFor("test");

            // assert
            result.Should().BeTrue();
        }
    }
}
