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
            var session = new SessionManager(sessionStorage, new SystemProcessIdProvider());

            // create state manager
            var stateManager = new StateManager("SampleApp", session, sessionStorage);

            if(args.Length == 1)
            {
                // read a value from state
                Console.WriteLine($"Value for {args[0]} : {stateManager.GetValue<string>( args[0], "")}");
            } 
            else if(args.Length == 2)
            {
                // set a value to state
                stateManager.SetValue(args[0], args[1]);
                stateManager.Save();
                Console.WriteLine($"Setting value for {args[0]} to {args[1]}");
            }
        }
    }
}
