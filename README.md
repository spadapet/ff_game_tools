# Ferret Face Game Tools
This is a set of tools to go along with the ff_game_library.

git clone --recursive https://github.com/spadapet/ff_game_tools.git

## Build from Visual Studio 2019
1) Open solution __game_tools.sln__
2) Build Solution (just Ctrl-Shift-B)

## Build from the command line
1) Download [nuget.exe](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
2) Open a __Visual Studio 2019__ developer command prompt
2) Run: __nuget.exe restore game_tools.sln__
3) Run: __msbuild.exe game_tools.sln__
    * Or for a specific type of build: __msbuild.exe /p:Configuration=Release|Debug game_tools.sln__

## Coding style
* Follow the code formatting as defined in .editorconfig
* Casing
    * snake_case for all functions, classes, variables, etc.
    * PascalCase for all template parameter names
* Generally don't use "var" to be lazy about variable type names
