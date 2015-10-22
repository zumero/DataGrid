./nuget restore ../DataGrid.sln
devenv.com  ../DataGrid.sln /rebuild release
./xamarin-component package ./rel
./nuget pack ./release.nuspec

