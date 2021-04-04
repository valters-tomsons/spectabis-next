#!/usr/bin/env pwsh

param(
		[string]$databaseUrl = "https://forums.pcsx2.net/data/data.csv",
        [string]$destinationFile = "./data.csv"
	)

Write-Output "Downloading latest PCSX2 game database..."
Invoke-RestMethod -Uri "$databaseUrl" -OutFile "$destinationFile"

Write-Output "Database saved at '$destinationFile'"
