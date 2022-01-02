# Hosting service for local development

## Pre-requisites

* [GiantBomb API Key](https://www.giantbomb.com/api/)
* .NET Core 6.0 SDK
* [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#v2)
* [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite#install-azurite) (blob service)

## Setting Up

Create `local.settings.json` in `src/SpectabisService/` with following, make sure to replace GiantBomb API key with your own.

```json
{
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "ApiKey_GiantBomb": "yourkeyhere"
}
```

## Running

* Start [Blob storage emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite#run-azurite)
* Run `func host start --csharp -p 7071` at `src/SpectabisService`

You can run integration tests in `Tests/ServiceTests` to make sure everything is configured correctly. Default settings for tests are pointed to `localhost:7071`.

If test succeeds, Spectabis should connect to local service **when built with debug configuration**.