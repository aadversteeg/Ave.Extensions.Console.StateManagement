using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ave.Extensions.Console.StateManagement
{
    public class SessionStateProtector : ISessionStateSerializer
    {
        // Create byte array for additional entropy when using Protect method.
        static byte[] _additionalEntropy = { 9, 8, 7, 6, 5 };

        private ISessionStateSerializer _innerSessionStateSerializer;

        public SessionStateProtector(ISessionStateSerializer innerSessionStateSerializer)
        {
            _innerSessionStateSerializer = innerSessionStateSerializer;
        }

        public IDictionary<string, object> Deserialize(byte[] bytes)
        {
            var unprotectedBytes = ProtectedData.Unprotect(bytes, _additionalEntropy, DataProtectionScope.CurrentUser);
            return _innerSessionStateSerializer.Deserialize(unprotectedBytes);
        }

        public byte[] Serialize(IDictionary<string, object> sessionState)
        {
            var unprotectedBytes = _innerSessionStateSerializer.Serialize(sessionState);
            return  ProtectedData.Protect(unprotectedBytes, _additionalEntropy, DataProtectionScope.CurrentUser);
        }
    }
}
