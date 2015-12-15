./nuget restore ../DataGrid.sln
msbuild /p:Configuration=Release ../DataGrid.sln
./xamarin-component package ./rel
./nuget pack ./release.nuspec

