# Hosting service for local development

## Required

* .NET Core 6.0 SDK
* [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#v2)
* [GiantBomb API Key](https://www.giantbomb.com/api/)

## Setting Up

Create `local.settings.json` in `src/SpectabisService/` with following, make sure to replace GiantBomb API key with your own.

```json
{
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "ApiKey_GiantBomb": "yourkeyhere"
}
```

## Starting

* Go to `src/SpectabisService`
* Run `func host start --csharp -p 7071`

You can run integration tests in `Tests/ServiceTests` to make sure everything is configured correctly. Default settings for tests are pointed to `localhost:7071`.

If test succeeds, Spectabis should connect to local service when built with debug.
