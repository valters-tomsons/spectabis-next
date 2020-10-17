# EmuConfig

Library for reading and writing PCSX2 configuration files.

## Notes

* Configurations are specified in `Configs` and properties require `IniKey` attribute to be parsed
* `Enums` contain abstractions over PCSX2 configuration types

## Credits

Uses [`IniFileParserStandard`](https://github.com/lukazh/ini-parser-standard) for deserializing INI files to CLR objects
