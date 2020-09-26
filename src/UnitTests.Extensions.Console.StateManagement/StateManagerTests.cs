using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class StateManagerTests
    {
        [Fact(DisplayName = "SMN-001: GetValue should return default value if key not present.")]
        public void SM001()
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            var value = stateManager.GetValue(StateScope.Session, "test", 42);

            // assert
            value.Should().Be(42);
        }

        [Fact(DisplayName = "SMN-002: Value set with SetValue should be returned with GetValue.")]
        public void SM002()
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);
            stateManager.SetValue(StateScope.Session, "test", 123);

            // act
            var value = stateManager.GetValue(StateScope.Session, "test", 42);

            // assert
            value.Should().Be(123);
        }

        [Fact(DisplayName = "SMN-003: HasKeyFor should return false if no value set for key.")]
        public void SM003()
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            var result = stateManager.HasValueFor(StateScope.Session, "test");

            // assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "SMN-004: HasKeyFor should return true if value set for key.")]
        public void SM004()
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);

            stateManager.SetValue(StateScope.Session, "test", 123);

            // act
            var result = stateManager.HasValueFor(StateScope.Session, "test");

            // assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "SMN-005: StateManager should load state using provided session key.")]
        public void SM005()
        {
            // arrange
            var storedSessionState = new Dictionary<string, object>()
            {
                { "one", "value one" },
                { "two", 2 }
            };

            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(storedSessionState);

            // act
            var stateManager = new StateManager(sessionManagerMock.Object);


            // assert
            sessionManagerMock.Verify(m => m.Load(StateScope.Session), Times.Once);
            stateManager.GetValue<string>(StateScope.Session, "one").Should().Be("value one");
            stateManager.GetValue<int>(StateScope.Session, "two").Should().Be(2);
        }

        [Fact(DisplayName = "SMN-006: StateManager should save state using provided session key.")]
        public void SM006()
        {
            // arrange
            var storedSessionState = new Dictionary<string, object>()
            {
                { "one", "value one" },
                { "two", 2 }
            };

            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(storedSessionState);

             IDictionary<string, object> persistedState = null;

            sessionManagerMock.Setup(m => m.Save(StateScope.Session, It.IsAny<IDictionary<string, object>>()))
                .Callback<StateScope, IDictionary<string, object>>((scope, state) => persistedState = state);

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            stateManager.SetValue(StateScope.Session, "One", "Value One");
            stateManager.Save();

            // assert
            sessionManagerMock.Verify(m => m.Save(StateScope.Session, It.IsAny<IDictionary<string, object>>()), Times.Once);

            persistedState.Should().NotBeNull();
            persistedState.Should().ContainKey("One");
            persistedState["One"].Should().Be("Value One");
        }
    }
}
