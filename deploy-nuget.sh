ApiKey=$1
Source=$2

dotnet pack -c Release --no-restore --no-build -o ../../ ./src/ClickHouse.Net.Migrations/ClickHouse.Net.Migrations.csproj 
dotnet nuget push -k $ApiKey -s $Source ./ClickHouse.Net.*.nupkg 