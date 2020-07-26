# spectabis-next

Cross-platform graphical frontend for PCSX2 emulator to replace [.NET Framework Spectabis](https://github.com/FaithLV/Spectabis). Currently developing in my free time, feel free to contribute.
Plan is to implement most used and useful features of original Spectabis, improve code quality to encourage more contributions but most importantly, let me use PCSX2 again.
[Began as a command-line frontend for the emulator](https://github.com/FaithLV/spectabis-cli), but that's too much effort for too little return.

Currently, **this is not usable for end-users**.

[![Build status](https://ci.appveyor.com/api/projects/status/nk9bp0m8ak2wm2e3/branch/master?svg=true)](https://ci.appveyor.com/project/FaithLV/spectabis-next/branch/master)
[![Build Status](https://dev.azure.com/Spectabis/SpectabisNext/_apis/build/status/FaithLV.spectabis-next?branchName=master)](https://dev.azure.com/Spectabis/SpectabisNext/_build/latest?definitionId=1&branchName=master)

## Current Technology Stack

* .NET Core 3.1
* Avalonia UI 0.9.7
* Avalona ReactiveUI 0.9.7
* AutoFac 4.9.2
* Azure Functions v3

## Platforms

* Linux (the point of the whole project)
* Windows (obviously)

Support for Mac is possible and is taken into account, but *is not* currently in scope.

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

## Coding Standarts

I've yet to come up with exact guidelines, but I try to follow SOLID principles wherever it's sensible. There's also full dependency injection with AutoFac, that includes Avalonia constructor injection etc.

Let's keep third party dependencies to a minimum and use them sensibly.

### Credits & Attributions

* Graphics - [Pixro](https://www.instagram.com/artcallspixro/)

* Game Art - [GiantBomb](https://www.giantbomb.com/api/)
