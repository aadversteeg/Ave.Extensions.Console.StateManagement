using System;

namespace Ave.Extensions.Console.StateManagement
{
    public class StateManager : IStateManager
    {
        public StateManager(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public string ApplicationName { get; }
    }
}
