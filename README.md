# spectabis-next

[![Build Status](https://dev.azure.com/Spectabis/SpectabisNext/_apis/build/status/FaithLV.spectabis-next?branchName=master)](https://dev.azure.com/Spectabis/SpectabisNext/_build/latest?definitionId=1&branchName=master)

Spectabis is a free and open-source graphical frontend for [`PCSX2`](https://pcsx2.net/). Its purpose is to add aditional features which make using the PlayStation 2 (PS2) emulator easier and faster. This allows you to use PCSX2 with per-game configuration and many additional features and benefits. This source code is available to everyone under the standard [MIT license](LICENSE).

Currently usable but not useful yet.

*Please Note:* If you're building the desktop client from source, you will **not** be able to connect to the service and online functionality will not work. You can host the service locally though.

![Screenshot](https://i.imgur.com/RcbMegH.png)

`! Games are not included and must be dumped from original game disc and imported.`

## Platforms

* Linux (the point of the whole project)
* Windows (obviously)

`! Builds for Mac OS are also available but are not tested at all.`

## Current Features

* Game Profiles
* Online service (API aggregator)
* Automated builds for Linux/Windows/Mac
* Discord Rich Presence

## Must Have Features before a "stable" release

* Profile Configuration
* Global Controller profile
* ROM serial discovery (.gz, .cso)
* CLI support
* Playtime counter
* Licensing and GDPR compliance

## Nice to have (in no particular order)

* "steam-like" library view
* Plugin configuration
* Controller support / Couch Mode
* Extension (Plugin) System
* Dark Mode
* Emulator version tracking
* Multilingual support
* Isolated PCSX2 installation profiles
* Seperate plugin updates from Spectabis releases
* Game favourites list
* Advanced sorting options
* Incorporate - <https://github.com/Zombeaver/PCSX2-Configs>

### Planned sorting options

* Playtime
* Date Added
* Alphabetical
* Genre

## Open Source Technologies

### Desktop

* .NET Core 3.1
* AutoFac 4.9.2
* Avalonia UI 0.9.7
* Avalo

### Service

* .NET Core 3.1
* Azure Functions v3

### Attributions

* Graphics design by [Piksro](https://www.instagram.com/piksro/)
* Game art delivered by [GiantBomb API](https://www.giantbomb.com/api/)
* AvaloniaGif by [Jumar Macato](https://github.com/jmacato/AvaloniaGif)

## Contributing

Please see [CONTIRUBTING.md](CONTRIBUTING.md)
