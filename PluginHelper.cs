using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;

namespace IL2CPPAutoConfigReload;
public class PluginHelper
{
    public BasePlugin Plugin { get; }
    public string ConfigFilePath => Config.ConfigFilePath;

    private ConfigFile Config => Plugin.Config;
    private byte[] LastHash = Array.Empty<byte>();

    public PluginHelper(BasePlugin plugin)
    {
        _ = this; // Remove IDE warning about using a primary constructor
        Plugin = plugin;
    }

    private byte[] CalculateConfigHash()
    {
        // Read the bytes from the config file
        var cfgBytes = File.ReadAllBytes(ConfigFilePath);

        // Calculate an MD5 hash for the bytes (since this isn't being used for cryptographic purposes, this is acceptable)
        using var md5 = MD5.Create();
        return md5.ComputeHash(cfgBytes);
    }

    public bool NeedToReloadConfig()
    {
        // If the config file doesn't exist (due to having no config options), then always return false
        if (!File.Exists(ConfigFilePath))
        {
            return false;
        }

        // The config should be reloaded if the new hash doesn't match the old one
        var newHash = CalculateConfigHash();
        return !newHash.SequenceEqual(LastHash);
    }

    public PluginHelper ReloadConfig()
    {
        // Calculate and save a new hash, and then reload the actual config settings
        // There are definitely ways this could be optimized to avoid computing hashes twice, but I'm not going to worry since configs won't be changing often
        LastHash = CalculateConfigHash();
        Config.Reload();

        // Also unload and reload the plugin in case any settings only apply at startup
        Plugin.Unload();
        Plugin.Load();

        IL2CPPAutoConfigReload.Plugin.Logger.LogInfo($"Reloaded from {ConfigFilePath}");

        return Clone();
    }

    private PluginHelper Clone() => new(Plugin);
}
