# spectabis-next

Cross-platform graphical frontend for PCSX2 emulator to replace [.NET Framework Spectabis](https://github.com/FaithLV/Spectabis). Currently developing in my free time, feel free to contribute.
Plan is to implement most used and useful features of original Spectabis, improve code quality to encourage more contributions but most importantly, let me use PCSX2 again.
[Began as a command-line frontend for the emulator](https://github.com/FaithLV/spectabis-cli), but that's too much effort for too little return.

Currently, **this is not usable for end-users**. There's a graphical interface with game profile generation and boxart tile loading in UI.

## Current Technology Stack
* .NET Core
* Avalonia UI

## Platforms
* Linux (the point of the whole project)
* Windows (obviously)

Support for Mac OS is possible. I won't go out of my way to make sure it works there though.

## Must Have Features
* Per-game configurations
* Box-art Scraping
* Basic emulator setting configuration
* Global Controller profiles
* ROM serial discovery (including .iso, .gz, .cso, .cue)

## Nice to have
* Plugin configuration
* Controller support
* Couch mode
* Playstation 1 game support and anything that comes with it
* Extension (Plugin) System
* Visual theming (at least, a dark mode)

## Coding Standarts
I've yet to come up with exact guidelines, but I try to follow SOLID principles wherever it's sensible. There's also full dependency injection with AutoFac, that includes Avalonia constructor injection etc.

Let's keep third party dependencies to a minimum and use them sensibly.
