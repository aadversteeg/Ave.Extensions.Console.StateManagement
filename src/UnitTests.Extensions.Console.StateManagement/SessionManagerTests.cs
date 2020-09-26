using System.Linq;
using AutoFixture;
using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class SessionManagerTests
    {
        [Fact(DisplayName = "SM-001: Sessionkey should be derived from parent process id.")]
        public void SM001()
        {
            // arrange
            var fixture = new Fixture();

            var parentProcessId = fixture.Create<int>();

            var processIdProviderMock = new Mock<IProcessIdProvider>();
            processIdProviderMock.Setup(m => m.ParentProcessId)
                .Returns(parentProcessId);

            processIdProviderMock.Setup(m => m.AllProcessIds)
                .Returns(fixture.CreateMany<int>().ToList());

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.StoredSessions)
                .Returns(fixture.CreateMany<int>().Select(pid => pid.ToString().PadLeft(10, '0')).ToList());

            var sessionManager = new SessionManager(sessionStorageMock.Object, processIdProviderMock.Object);

            // act
            var sessionKey = sessionManager.Key;

            // assert
            var expectedKey = parentProcessId.ToString().PadLeft(10, '0');

            sessionKey.Should().Be(expectedKey);
        }

        [Fact(DisplayName = "SM-002: Session manager should remove all stale sessions.")]
        public void SM002()
        {
            // arrange
            var fixture = new Fixture();

            var allProcessIds = fixture
                .CreateMany<int>()
                .ToList();

            var processIdProviderMock = new Mock<IProcessIdProvider>();
            processIdProviderMock.Setup(m => m.AllProcessIds)
                .Returns(allProcessIds);

            var staleProcessId = fixture.Create<int>();

            var sessionStorageMock = new Mock<ISessionStorage>();
            sessionStorageMock.Setup(m => m.StoredSessions)
                .Returns( allProcessIds
                    .Concat(new[] { staleProcessId })
                    .Select(pid => pid.ToString().PadLeft(10, '0'))
                    .ToList());

            // act
            var sessionManager = new SessionManager(sessionStorageMock.Object, processIdProviderMock.Object);

            // assert
            var expectedKey = staleProcessId.ToString().PadLeft(10, '0');
            sessionStorageMock.Verify(m => m.Delete(expectedKey));
        }
    }
}
