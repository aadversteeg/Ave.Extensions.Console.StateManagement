using AutoFixture;
using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class FileSessionStorageTests
    {
        [Fact(DisplayName = "FSS-001: Should check if file exists with correct path and return empty state because file does not exist.")]
        public void FSS001()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var expectedPath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();

            var directoryMock = new Mock<IDirectory>();

            var fileMock = new Mock<IFile>();
            fileMock
                .Setup(m => m.Exists(expectedPath))
                .Returns(false);

            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            var sessionState = storage.Load(sessionKey);

            // assert
            sessionState.Should().NotBeNull();
            sessionState.Count.Should().Be(0);

            fileMock.Verify(m => m.Exists(expectedPath), Times.Once);
        }

        [Fact(DisplayName = "FSS-002: Should load session state using correct path.")]
        public void FSS002()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var expectedPath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>()
            {
                { "one", 1 }
            };
            var serializedSessionState = sessionStateSerializer.Serialize(sessionState);

            var directoryMock = new Mock<IDirectory>();

            var fileMock = new Mock<IFile>();

            fileMock
                .Setup(m => m.Exists(expectedPath))
                .Returns(true);

            fileMock
                .Setup(m => m.ReadAllBytes(expectedPath))
                .Returns(serializedSessionState);

            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            var loadedSessionState = storage.Load(sessionKey);

            // assert
            loadedSessionState.Should().NotBeNull();
            loadedSessionState.Should().BeEquivalentTo(sessionState);

            fileMock.Verify(m => m.Exists(expectedPath), Times.Once);
            fileMock.Verify(m => m.ReadAllBytes(expectedPath), Times.Once);
        }

        [Fact(DisplayName = "FSS-003: Should save session state using correct path.")]
        public void FSS003()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var expectedPath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>()
            {
                { "one", 1 }
            };
            var serializedSessionState = sessionStateSerializer.Serialize(sessionState);

            var directoryMock = new Mock<IDirectory>();

            var fileMock = new Mock<IFile>();

            fileMock
                .Setup(m => m.Exists(expectedPath))
                .Returns(true);

            fileMock
                .Setup(m => m.WriteAllBytes(expectedPath, It.IsAny<byte[]>()))
                .Callback<string, byte[]>((sessionKey, bytes) => serializedSessionState = bytes);

            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            storage.Save(sessionKey, sessionState);

            // assert
            fileMock.Verify(m => m.WriteAllBytes(expectedPath, It.IsAny<byte[]>()), Times.Once);


            var expectedSessionState = new Dictionary<string, object>()
            {
                { "one", 1},
            };

            var deserializedSessionState = sessionStateSerializer.Deserialize(serializedSessionState);
            deserializedSessionState.Should().BeEquivalentTo(expectedSessionState);
        }

        [Fact(DisplayName = "FSS-004: Should create directory on save session state when directory does not exist.")]
        public void FSS004()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var expectedPath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>();

            var directoryMock = new Mock<IDirectory>();
            directoryMock.Setup(m => m.Exists(path))
                .Returns(false);

            var fileMock = new Mock<IFile>();
            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            storage.Save(sessionKey, sessionState);

            // assert
            directoryMock.Verify(m => m.Create(path), Times.Once);
        }

        [Fact(DisplayName = "FSS-005: Should not create directory on save session state when directory already exists.")]
        public void FSS005()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var expectedPath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>();

            var directoryMock = new Mock<IDirectory>();
            directoryMock.Setup(m => m.Exists(path))
                .Returns(true);

            var fileMock = new Mock<IFile>();
            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            storage.Save(sessionKey, sessionState);

            // assert
            directoryMock.Verify(m => m.Create(path), Times.Never);
        }


        [Fact(DisplayName = "FSS-006: Should delete correct file when it exists when deleting session.")]
        public void FSS006()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var filePath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>();

            var directoryMock = new Mock<IDirectory>();

            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Exists(filePath))
                .Returns(true);
            
            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            storage.Delete(sessionKey);

            // assert
            fileMock.Verify(m => m.Delete(filePath), Times.Once);
        }


        [Fact(DisplayName = "FSS-007: Should not delete file when it does not exists when deleting session.")]
        public void FSS007()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKey = fixture.Create<string>();
            var path = fixture.Create<string>();
            var filePath = Path.Combine(path, sessionKey);

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>();

            var directoryMock = new Mock<IDirectory>();

            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Exists(filePath))
                .Returns(false);

            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            storage.Delete(sessionKey);

            // assert
            fileMock.Verify(m => m.Delete(filePath), Times.Never);
        }

        [Fact(DisplayName = "FSS-008: Should return all files as session keys.")]
        public void FSS008()
        {
            // arrange
            var fixture = new Fixture();
            var sessionKeys = fixture.CreateMany<string>(2);
            var path = fixture.Create<string>();
            var filePaths = sessionKeys
                .Select( sessionKey => Path.Combine(path, sessionKey))
                .ToList();

            var sessionStateSerializer = new BinarySessionStateSerializer();
            var sessionState = new Dictionary<string, object>();

            var directoryMock = new Mock<IDirectory>();
            directoryMock
                .Setup(m => m.Exists(path))
                .Returns(true);

            directoryMock
                .Setup(m => m.GetFileNames(path))
                .Returns(filePaths);

            var fileMock = new Mock<IFile>();

            var storage = new FileSessionStorage(directoryMock.Object, fileMock.Object, sessionStateSerializer, path);

            // act
            var storedSessions = storage.StoredSessions;

            // assert
            storedSessions.Should().BeEquivalentTo(sessionKeys);
        }
    }
}
