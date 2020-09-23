#!/usr/bin/env pwsh

# Used to tokenize sensitive variables in ServiceClient library to be used in desktop client

param(
		[string]$targetFile
	)

$tokens = "{{ServiceApiKey}}", "{{TelemetryKey}}", "{{ServiceBaseUrl}}"
$tokenValues = "$env:SERVICE_API_KEY", "$env:TELEMETRY_KEY", "$env:SERVICE_BASE_URL"

# Insert service API keys
for ($i = 0; $i -lt $tokens.Count; $i++) {
	(Get-Content "$targetFile") | foreach-object { $_ -replace $tokens[$i], $tokenValues[$i] } | Set-Content $targetFile
}

Write-Host "Processed: " $targetFile