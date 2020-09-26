namespace Ave.Extensions.Console.StateManagement
{
    public interface IStateManager
    {
        bool HasValueFor(StateScope scope, string key);

        T GetValue<T>(StateScope scope, string key, T defaultValue = default(T));

        void SetValue<T>(StateScope scope, string key, T value);
    }
}
