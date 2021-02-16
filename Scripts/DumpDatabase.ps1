#!/usr/bin/env pwsh

param(
		[string]$databaseFile = "data.csv",
        [string]$apiEndpoint,
        [bool]$dumpImages = $false,
        [string]$dumpsDir = "artDump"
	)

$serials = Import-Csv -Delimiter "`t" -Path $databaseFile -Header 'Serial' | Select-Object -ExpandProperty 'Serial'
$urlWithQuery = $apiEndpoint + "&serial="

foreach ($serial in $serials) {
    $fullUri = "$urlWithQuery$serial"

    try{
        if($dumpImages)
        {
            New-Item -Path $dumpsDir -ItemType Directory
            $result = Invoke-WebRequest -Uri $fullUri -OutFile "$dumpsDir/$serial.png"

            Write-Host "Image written to $serial"
        }
        else{
            $result = Invoke-WebRequest -Uri $fullUri
            Write-Host "Successful request for $serial"
        }
    }
    catch{
        Write-Warning "Request failed $fullUri"
        $_.Exception.Message

        Write-Host
    }
}