using Ave.Extensions.Console.StateManagement;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // determine root folder for saving session data:
            var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SessionState");
            var sessionStorage = new FileSessionStorage(new SystemDirectory(), new SystemFile(), new BinarySessionStateSerializer(), path);

            var session = new Session();

            var stateManager = new StateManager("SampleApp", session, sessionStorage);

            if(args.Length == 1)
            {
                Console.WriteLine($"Value for {args[0]} : {stateManager.GetValue<string>( args[0], "")}");
            } 
            else if(args.Length == 2)
            {
                stateManager.SetValue(args[0], args[1]);
                stateManager.Save();
                Console.WriteLine($"Setting value for {args[0]} to {args[1]}");
            }
        }
    }
}
