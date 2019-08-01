Set-Location $env:USERPROFILE\.nuget\packages\nswag.msbuild\12.2.3\tools\NetCore22

$projectName = "WebShop.Users.Api"
$binFolder = "D:\Repos\Azure\WebShop\src\"+ $projectName +"\bin\Debug\netcoreapp2.2\publish"
$outFolder = $binFolder + "\wwwroot"
$versions = @("v1")
foreach ($version in $versions) {

Write-Host "GENERATING CLIENTS FOR"$version
dotnet dotnet-nswag.dll aspnetcore2swagger /assembly:$binFolder\$projectName.dll /documentName:$version /output:$outFolder\$projectName.$version.json
dotnet dotnet-nswag.dll swagger2csclient /output:$outFolder\$projectName.Clients.$version.Client.cs /namespace:$projectName.$version /InjectHttpClient:true /input:$outFolder\$projectName.$version.json
dotnet dotnet-nswag.dll swagger2tsclient /output:$outFolder\$projectName.Clients.$version.Client.ts /namespace:$projectName.$version /input:$outFolder\$projectName.$version.json

Write-Host "GENERATING ARCHIVE FOR"$version
Compress-Archive -LiteralPath $outFolder\$projectName.Clients.$version.Client.cs, $outFolder\$projectName.Clients.$version.Client.ts, $outFolder\$projectName.$version.json -DestinationPath $outFolder\$projectName.Clients.$version.zip -Force

Write-Host "REMOVING VERSION"$version" FILES"
Remove-Item –path $outFolder\$projectName.Clients.$version.Client.cs
Remove-Item –path $outFolder\$projectName.Clients.$version.Client.ts 
Remove-Item –path $outFolder\$projectName.$version.json

}