@echo off
cls

ECHO.
ECHO Building Primary Nuget Package xCache
ECHO =======================================

nuget pack src\xCache\xCache.csproj -build -Prop Configuration=Release -IncludeReferencedProjects -OutputDirectory artifacts

ECHO.
ECHO Building xCache Aop Package for Unity 
ECHO =======================================

nuget pack src\xCache.Aop.Unity\xCache.Aop.Unity.csproj -build -Prop Configuration=Release -IncludeReferencedProjects -OutputDirectory artifacts

ECHO.
ECHO All done
