#!/usr/bin/env pwsh

param(
		[string]$targetFile
	)

$tokens = { "{{ServiceApiKey}}", "{{TelemetryKey}}" }
$tokenValues = { $env:SERVICE_API_KEY, $env:TELEMETRY_KEY }

for ($i = 0; $i -lt $tokens.Count; $i++) {
	(Get-Content "$targetFile") | foreach-object { $_ -replace $token[$i], $tokenValues[$i] } | Set-Content $targetFile
	(Get-Content "$targetFile") | foreach-object { $_ -replace $tokens[$i], $tokenValue[$i] } | Set-Content $targetFile
}

# Insert service API key
(Get-Content "$targetFile") | foreach-object { $_ -replace $token, $tokenValue } | Set-Content $targetFile
	Write-Host "Processed: " $targetFile
