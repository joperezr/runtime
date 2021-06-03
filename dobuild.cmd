call dotnet build src\libraries\System.Drawing.Common\src\System.Drawing.Common.csproj /p:BuildProjectReferences=false /p:RunApiCompat=false
call dotnet msbuild src\libraries\src.proj /t:ILLinkTrimOOBAssemblies /p:BuildingNetCoreAppVertical=true /p:RuntimeConfiguration=Release /bl
