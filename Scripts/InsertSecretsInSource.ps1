#!/usr/bin/env pwsh

param(
		[string]$targetFile
	)

$tokens = "{{ServiceApiKey}}", "{{TelemetryKey}}"
$tokenValues = "$env:SERVICE_API_KEY", "$env:TELEMETRY_KEY"

# Insert service API keys
for ($i = 0; $i -lt $tokens.Count; $i++) {
	(Get-Content "$targetFile") | foreach-object { $_ -replace $tokens[$i], $tokenValues[$i] } | Set-Content $targetFile
}

Write-Host "Processed: " $targetFile