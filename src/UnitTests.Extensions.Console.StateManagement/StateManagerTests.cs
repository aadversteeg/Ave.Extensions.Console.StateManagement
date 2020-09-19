using AutoFixture;
using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class StateManagerTests
    {

        [Fact(DisplayName = "SMN-001: ApplicationName should return name specified during construction.")]
        public void SM001()
        {
            // arrange
            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);

            // act
            var applicationName = stateManager.ApplicationName;

            // assert
            applicationName.Should().Be("MyApplication");
        }

        [Fact(DisplayName = "SMN-002: GetValue should return default value if key not present.")]
        public void SM002()
        {
            // arrange
            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);

            // act
            var value = stateManager.GetValue("test", 42);

            // assert
            value.Should().Be(42);
        }

        [Fact(DisplayName = "SMN-003: Value set with SetValue should be returned with GetValue.")]
        public void SM003()
        {
            // arrange

            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);
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
            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);

            // act
            var result = stateManager.HasValueFor("test");

            // assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "SMN-005: HasKeyFor should return true if value set for key.")]
        public void SM005()
        {
            // arrange
            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(new Dictionary<string, object>());

            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);

            stateManager.SetValue("test", 123);

            // act
            var result = stateManager.HasValueFor("test");

            // assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "SMN-006: StateManager should load state using provided session key.")]
        public void SM006()
        {
            // arrange
            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var storedSessionState = new Dictionary<string, object>()
            {
                { "one", "value one" },
                { "two", 2 }
            };

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(storedSessionState);

            // act
            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);

            // assert
            sessionStorageMock.Verify(m => m.Load(sessionKey), Times.Once);
            stateManager.GetValue<string>("one").Should().Be("value one");
            stateManager.GetValue<int>("two").Should().Be(2);
        }

        [Fact(DisplayName = "SMN-007: StateManager should save state using provided session key.")]
        public void SM007()
        {
            // arrange
            var fixture = new Fixture();

            var sessionKey = fixture.Create<string>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(m => m.Key).Returns(sessionKey);

            var storedSessionState = new Dictionary<string, object>()
            {
            };

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.Load(sessionKey))
                .Returns(storedSessionState);

            IDictionary<string, object> persistedState = null;

            sessionStorageMock.Setup(m => m.Save(sessionKey, It.IsAny<IDictionary<string, object>>()))
                .Callback<string, IDictionary<string, object>>((sessionKey, state) => persistedState = state);

            var stateManager = new StateManager("MyApplication", sessionMock.Object, sessionStorageMock.Object);

            // act
            stateManager.SetValue("One", "Value One");
            stateManager.Save();

            // assert
            sessionStorageMock.Verify(m => m.Save(sessionKey, It.IsAny<IDictionary<string, object>>()), Times.Once);

            persistedState.Should().NotBeNull();
            persistedState.Should().ContainKey("One");
            persistedState["One"].Should().Be("Value One");
        }
    }
}
