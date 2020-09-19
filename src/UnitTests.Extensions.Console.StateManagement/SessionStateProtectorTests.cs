using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class SessionStateProtectorTests
    {
        [Fact(DisplayName ="SSP-001: Protected session state is not readable without protector ")]
        public void SSP001()
        {
            // arrange
            var sessionState = new Dictionary<string, object>()
            {
                { "one", 1 }
            };

            var sessionSerializer = new BinarySessionStateSerializer();
            var sessionStateProtector = new SessionStateProtector(sessionSerializer);

            var protectedSessionState = sessionStateProtector.Serialize(sessionState);

            Action action = () => sessionSerializer.Deserialize(protectedSessionState);

            // act, assert
            action.Should().Throw<System.Runtime.Serialization.SerializationException>();
        }

        [Fact(DisplayName ="SSP-002: Protected session state is reradable with protector")]
        public void SSP002()
        {
            // arrange
            var sessionState = new Dictionary<string, object>()
            {
                { "one", 1 }
            };

            var sessionSerializer = new BinarySessionStateSerializer();
            var sessionStateProtector = new SessionStateProtector(sessionSerializer);

            var protectedSessionState = sessionStateProtector.Serialize(sessionState);

            // act
            var deserializedSessionState = sessionStateProtector.Deserialize(protectedSessionState);

            // assert
            var expectedSessionState = new Dictionary<string, object>()
            {
                { "one", 1 }
            };

            deserializedSessionState.Should().BeEquivalentTo(expectedSessionState);
        }
    }
}
