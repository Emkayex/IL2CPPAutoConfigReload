using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IL2CPPAutoConfigReload;
public class Tracker
{
    private readonly List<PluginHelper> PluginHelpers = new();

    public void MainLoop()
    {
        // Since this will run on a separate thread, run the following loop infinitely
        var helpersToRemove = new List<PluginHelper>();
        var helpersToAdd = new List<PluginHelper>();
        while (true)
        {
            helpersToRemove.Clear();
            helpersToAdd.Clear();

            // Search for new plugins and then go through each plugin and reload the config file if necessary
            UpdatePluginList();
            foreach (var helper in PluginHelpers)
            {
                try
                {
                    if (helper.NeedToReloadConfig())
                    {
                        var newHelper = helper.ReloadConfig();
                        helpersToRemove.Add(helper);
                        helpersToAdd.Add(newHelper);
                    }
                }
                catch (Exception e)
                {
                    // Catch all exceptions to prevent a bad config or file lock from crashing the plugin
                    Plugin.Logger.LogError(e.ToString());
                }
            }

            if (helpersToRemove.Count > 0)
            {
                foreach (var helper in helpersToRemove)
                {
                    PluginHelpers.Remove(helper);
                }

                PluginHelpers.AddRange(helpersToAdd);
            }

            // Wait 3 seconds between reloading configs to avoid constantly reading from the disk
            Thread.Sleep(3 * 1000);
        }
    }

    internal void UpdatePluginList()
    {
        // Use the config file paths as an identifier to determine whether a plugin has already been added since that value should never change while running
        var newPlugins = PluginFinder.Search().Where(p => !IterateConfigFilePaths().Contains(p.Config.ConfigFilePath));
        foreach (var plugin in newPlugins)
        {
            var helper = new PluginHelper(plugin);
            PluginHelpers.Add(helper);
            Plugin.Logger.LogInfo($"Found config file: {helper.Plugin.Config.ConfigFilePath}");
        }
    }

    private IEnumerable<string> IterateConfigFilePaths()
    {
        foreach (var plugin in PluginHelpers)
        {
            yield return plugin.ConfigFilePath;
        }
    }
}
