using Ave.Extensions.Console.StateManagement;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Extensions.Console.StateManagement
{
    public class BinarySessionStateSerializerTests
    {
        [Fact(DisplayName = "BSSS-001: Serializing state and deserializing should return state.")]
        public void BSSS001()
        {
            // arrange
            var sessionState = new Dictionary<string, object>()
            {
                { "one", "value one"},
                {  "two", 2.0}
            };

            var serializer = new BinarySessionStateSerializer();

            // act
            var bytes = serializer.Serialize(sessionState);
            var sessionStateFromBytes = serializer.Deserialize(bytes);

            // verify

            var expectedSessionState = new Dictionary<string, object>()
            {
                { "one", "value one"},
                {  "two", 2.0}
            };

            sessionStateFromBytes.Should().BeEquivalentTo(expectedSessionState);
        }
    }
}
