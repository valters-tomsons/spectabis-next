#!/usr/bin/env pwsh

param(
		[string]$targetFile
	)

$env = (Get-ChildItem env:)
$targetContent = Get-Content -Path $targetFile

foreach($var in $env)
{
	$targetContent = $targetContent.Replace("{{$($var.Name)}}", $var.Value)
}

Set-Content -Path $targetFile -Value $targetContent
Write-Host "Finished processing $targetFile" -ForegroundColor Green