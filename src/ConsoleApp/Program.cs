using Ave.Extensions.Console.StateManagement;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // determine root folder for saving session data
            var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SessionState");

            var sessionStateSerializer = new SessionStateProtector(new BinarySessionStateSerializer());

            var sessionStorage = new FileSessionStorage(new SystemDirectory(), new SystemFile(), sessionStateSerializer, path);

            // create Session for generating correct session key
            var sessionMananager = new SessionManager(sessionStorage, new SystemProcessIdProvider());

            // create state manager
            var stateManager = new StateManager(sessionMananager);

            if(args.Length == 2)
            {
                // read a value from state
                var scope = Enum.Parse<StateScope>(args[0]);
                Console.WriteLine($"Value for {args[1]} : {stateManager.GetValue<string>(scope, args[1], "")}");
            } 
            else if(args.Length == 3)
            {
                // set a value to state
                var scope = Enum.Parse<StateScope>(args[0]);
                stateManager.SetValue(scope, args[1], args[2]);
                stateManager.Save();
                Console.WriteLine($"Setting value for {args[1]} to {args[2]}");
            }
        }
    }
}
