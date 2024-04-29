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
        while (true)
        {
            // Search for new plugins and then go through each plugin and reload the config file if necessary
            UpdatePluginList();
            foreach (var helper in PluginHelpers)
            {
                if (helper.NeedToReloadConfig())
                {
                    helper.ReloadConfig();
                }
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
