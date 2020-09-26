using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class StateManagerTests
    {
        [Theory(DisplayName = "SMN-001: GetValue should return default value if key not present.")]
        [InlineData(StateScope.Session)]
        [InlineData(StateScope.User)]
        public void SM001(StateScope scope)
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(scope))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            var value = stateManager.GetValue(scope, "test", 42);

            // assert
            value.Should().Be(42);
        }

        [Theory(DisplayName = "SMN-002: Value set with SetValue should be returned with GetValue.")]
        [InlineData(StateScope.Session)]
        [InlineData(StateScope.User)]
        public void SM002(StateScope scope)
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(scope))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);
            stateManager.SetValue(scope, "test", 123);

            // act
            var value = stateManager.GetValue(scope, "test", 42);

            // assert
            value.Should().Be(123);
        }

        [Theory(DisplayName = "SMN-003: HasKeyFor should return false if no value set for key.")]
        [InlineData(StateScope.Session)]
        [InlineData(StateScope.User)]
        public void SM003(StateScope scope)
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(scope))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            var result = stateManager.HasValueFor(scope, "test");

            // assert
            result.Should().BeFalse();
        }

        [Theory(DisplayName = "SMN-004: HasKeyFor should return true if value set for key.")]
        [InlineData(StateScope.Session)]
        [InlineData(StateScope.User)]
        public void SM004(StateScope scope)
        {
            // arrange
            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(scope))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager(sessionManagerMock.Object);

            stateManager.SetValue(scope, "test", 123);

            // act
            var result = stateManager.HasValueFor(scope, "test");

            // assert
            result.Should().BeTrue();
        }

        [Theory(DisplayName = "SMN-005: StateManager should load state.")]
        [InlineData(StateScope.Session)]
        [InlineData(StateScope.User)]
        public void SM005(StateScope scope)
        {
            // arrange
            var storedSessionState = new Dictionary<string, object>()
            {
                { "one", "value one" },
                { "two", 2 }
            };

            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(scope))
                .Returns(storedSessionState);

            // act
            var stateManager = new StateManager(sessionManagerMock.Object);


            // assert
            sessionManagerMock.Verify(m => m.Load(scope), Times.Once);
            stateManager.GetValue<string>(scope, "one").Should().Be("value one");
            stateManager.GetValue<int>(scope, "two").Should().Be(2);
        }

        [Theory(DisplayName = "SMN-006: StateManager should save state.")]
        [InlineData(StateScope.Session)]
        [InlineData(StateScope.User)]
        public void SM006(StateScope scope)
        {
            // arrange
            var storedSessionState = new Dictionary<string, object>()
            {
                { "one", "value one" },
                { "two", 2 }
            };

            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(scope))
                .Returns(storedSessionState);

             IDictionary<string, object> persistedState = null;

            sessionManagerMock.Setup(m => m.Save(scope, It.IsAny<IDictionary<string, object>>()))
                .Callback<StateScope, IDictionary<string, object>>((scope, state) => persistedState = state);

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            stateManager.SetValue(scope, "One", "Value One");
            stateManager.Save();

            // assert
            sessionManagerMock.Verify(m => m.Save(scope, It.IsAny<IDictionary<string, object>>()), Times.Once);

            persistedState.Should().NotBeNull();
            persistedState.Should().ContainKey("One");
            persistedState["One"].Should().Be("Value One");
        }


        [Fact(DisplayName = "SMN-007: StateManager should keep state of different scope separate.")]
        public void SM007()
        {
            // arrange
            var storedSessionState = new Dictionary<string, object>()
            {
                { "one", "session" },
                { "two", 2 }
            };

            var storedUserState = new Dictionary<string, object>()
            {
                { "one", "user" },
                { "three", 3 }
            };

            var sessionManagerMock = new Mock<ISessionManager>();
            sessionManagerMock.Setup(m => m.Load(StateScope.Session))
                .Returns(storedSessionState);

            sessionManagerMock.Setup(m => m.Load(StateScope.User))
                .Returns(storedUserState);

            IDictionary<string, object> persistedSessionState = null;
            IDictionary<string, object> persistedUserState = null;

            sessionManagerMock.Setup(m => m.Save(StateScope.Session, It.IsAny<IDictionary<string, object>>()))
                .Callback<StateScope, IDictionary<string, object>>((scope, state) => persistedSessionState = state);

            sessionManagerMock.Setup(m => m.Save(StateScope.User, It.IsAny<IDictionary<string, object>>()))
                .Callback<StateScope, IDictionary<string, object>>((scope, state) => persistedUserState = state);

            var stateManager = new StateManager(sessionManagerMock.Object);

            // act
            stateManager.SetValue(StateScope.Session, "ten", "SessionTen");
            stateManager.SetValue(StateScope.User, "ten", "UserTen");
            stateManager.Save();

            // assert
            sessionManagerMock.Verify(m => m.Save(StateScope.Session, It.IsAny<IDictionary<string, object>>()), Times.Once);
            sessionManagerMock.Verify(m => m.Save(StateScope.User, It.IsAny<IDictionary<string, object>>()), Times.Once);

            persistedSessionState.Should().NotBeNull();
            persistedSessionState["one"].Should().Be("session");
            persistedSessionState["two"].Should().Be(2);
            persistedSessionState.ContainsKey("three").Should().BeFalse();
            persistedSessionState["ten"].Should().Be("SessionTen");

            persistedUserState.Should().NotBeNull();
            persistedUserState["one"].Should().Be("user");
            persistedUserState.ContainsKey("two").Should().BeFalse();
            persistedUserState["three"].Should().Be(3);
            persistedUserState["ten"].Should().Be("UserTen");
        }

    }
}
