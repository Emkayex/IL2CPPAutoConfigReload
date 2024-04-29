# IL2CPPAutoConfigReload
This is a [BepInEx](https://github.com/BepInEx/BepInEx) plugin for Unity IL2CPP games that will automatically reload changes to plugin config options if the associated config file changes. The plugin will scan for changes once every three seconds.

Since the [ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) plugin does not work for IL2CPP games at the time of writing, this was created as a way to allow config options to be changed without needing to restart the game each time.

## Requirements to Build
- .NET 6 SDK
- Python 3
    - Optional

## Building
Clone the repository and enter the directory.

```bash
git clone https://github.com/Emkayex/IL2CPPAutoConfigReload.git
cd IL2CPPAutoConfigReload
```

Call [Build.py](./Build.py) with the location where the DLL should be output. For testing purposes, it's useful to point this to a game's plugin directory.

```bash
python ./Build.py <Path to Game>/BepInEx/plugins/IL2CPPAutoConfigReload
```

Otherwise you can just call `dotnet build`, `dotnet publish`, etc.

## Credits
The code for discovering plugins was adapted from the official BepInEx [ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) plugin.

- Original: https://github.com/BepInEx/BepInEx.ConfigurationManager/blob/master/ConfigurationManager.IL2CPP/SettingSearcher.cs
    - License: https://github.com/BepInEx/BepInEx.ConfigurationManager/blob/master/LICENSE
- Adapted: https://github.com/Emkayex/IL2CPPAutoConfigReload/blob/master/PluginFinder.cs
