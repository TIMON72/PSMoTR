@echo off
REM Try to use DOTNET_ROOT if set, otherwise use dotnet from PATH
if defined DOTNET_ROOT (
    "%DOTNET_ROOT%\dotnet.exe" %*
) else (
    dotnet %*
)
