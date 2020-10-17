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

Write-Host "Finished processing $targetFile" -ForegroundColor Green