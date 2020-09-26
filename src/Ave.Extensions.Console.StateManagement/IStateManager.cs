﻿namespace Ave.Extensions.Console.StateManagement
{
    public interface IStateManager
    {
        bool HasValueFor(string key);

        T GetValue<T>(string key, T defaultValue = default(T));

        void SetValue<T>(string key, T value);
    }
}
