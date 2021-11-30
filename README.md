# spectabis-next

[![Build Status](https://dev.azure.com/Spectabis/SpectabisNext/_apis/build/status/FaithLV.spectabis-next?branchName=master)](https://dev.azure.com/Spectabis/SpectabisNext/_build/latest?definitionId=1&branchName=master)

Spectabis is a free and open-source graphical frontend for [`PCSX2`](https://pcsx2.net/). Its purpose is to add additional features which make using the PlayStation 2 (PS2) emulator easier and faster. This allows you to use PCSX2 with per-game configuration and many additional features and benefits. This source code is available to everyone under the standard [MIT license](LICENSE).

Currently usable and a little useful yet.

> *Please Note:* If you're building the desktop client from source, you will **not** be able to connect to the service and online functionality will not work. You can host the service locally though.

![Screenshot](https://i.imgur.com/IGSp3YO.png)

> Games are not included and must be copied from physical media you own

## Platforms

* Linux (the point of the whole project)
* Windows (obviously)

> Builds for Mac OS are also available but are not tested at all

## Feature set

## Current Features

* Game Profiles
* Game file parsing (cue & iso)
* Art online service
* Discord Rich Presence
* Automated builds
* Telemetry

## Goals for 1.0

* Global controller profile
* Game file parsing (gz, cso)
* Playtime counter
* Licensing and GDPR compliance
* CLI support

## Long-term goals (in no particular order)

* Cloud save games
* Library sorting (Playtime, Added, Alphabetical)
* Separate plugin updates from Spectabis releases
* "steam-like" library view
* Plugin configuration
* Controller support / Couch Mode
* Extension (Plugin) System
* Dark Mode
* Emulator version tracking
* Multilingual support
* Game favorites list
* Advanced sorting options
* Incorporate - <https://github.com/Zombeaver/PCSX2-Configs>
* Unit tests

## Desktop Client Development

Documentation for API Service [here](./docs/run-service-locally.md).

### Pre-requisites

* .NET 5.0 SDK

### Run

```
cd src/SpectabisNext
dotnet run
```

### Contributing

Please see [CONTRIBUTING.md](CONTRIBUTING.md)

### Thanks to

## Third-Party Dependencies

* AutoFac
* AvaloniaUI
* ReactiveUI
* Azure Functions
* LunrCore
* IniFileParserStandard
* DiscUtils
* TinyCsvParser
* Newtonsoft.Json
* Newtonsoft.Json.Bson
* DiscordRichPresence

## Attributions

* Graphics design by [Piksro](https://www.instagram.com/piksro/)
* Game art delivered by [GiantBomb API](https://www.giantbomb.com/api/)
* AvaloniaGif by [Jumar Macato](src/SpectabisUI/Controls/AnimatedImage/README.md)
