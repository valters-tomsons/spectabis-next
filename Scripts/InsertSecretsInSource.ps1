#!/usr/bin/env pwsh

param(
		[string]$targetFile
	)

$token = "{{ServiceApiKey}}"
$tokenValue = $env:SERVICE_API_KEY

# Insert service API key
(Get-Content "$targetFile") | foreach-object { $_ -replace $token, $tokenValue } | Set-Content $targetFile
	Write-Host "Processed: " $targetFile
