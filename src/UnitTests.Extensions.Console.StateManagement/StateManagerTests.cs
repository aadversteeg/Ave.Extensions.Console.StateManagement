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
    }
}
