./nuget restore ../DataGrid.sln
msbuild /p:Configuration=Release ../DataGrid.sln
./xamarin-component package ./release
./nuget pack ./release.nuspec

