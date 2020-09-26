# Ave.Extensions.Console.StateManagement

[![Build Status](https://versteeg-its.visualstudio.com/Ave/_apis/build/status/CI%20-%20Ave.Extensions.Console.StateManagement?branchName=master)](https://versteeg-its.visualstudio.com/Ave/_build/latest?definitionId=130&branchName=master)
[![Nuget downloads](https://img.shields.io/nuget/v/Ave.Extensions.Console.StateManagement)](https://www.nuget.org/packages/Ave.Extensions.Console.StateManagement/)
[![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/aadversteeg/ave.extensions.console.stateManagement/blob/master/LICENSE)

Nuget package for managing state of console application.

Console applications lose state after terminating. This package enables to preserve state between multiple invocations of a console application in the same console window.

The process id of the parent, ussually the console window, is used as a key to store the state. The state is stored as a binary file. The contents of this file can be protected using the Data Protection API (DPAPI).

## StateScope

Values can be saved ore retrieved using state scope ```user``` or ```session```.

## Session

Values are stored using the parent process id as key. As long as the terminal window is open, stored values are available.

## User

Values are stored using a fixed key. The values are available in all open terminal windows and are preserved after all terminal windows are closed. It as assumed that the path used for ```FileSessionStorage``` is only used by the current user.
