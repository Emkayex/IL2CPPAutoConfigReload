using System.Collections.Generic;
using System.Linq;
using BepInEx.Unity.IL2CPP;

namespace IL2CPPAutoConfigReload;
public class PluginFinder
{
    public static IEnumerable<BasePlugin> Search()
    {
        // Find all loaded plugins (they must inherit from BasePlugin)
        var pluginInfos = IL2CPPChainloader.Instance.Plugins.Values.Where(x => x.Instance is BasePlugin).ToList();
        foreach (var pluginInfo in pluginInfos)
        {
            // Get the instance of the plugin so the bound ConfigEntry objects can be discovered
            var basePlugin = (BasePlugin)pluginInfo.Instance;
            if (basePlugin is not null)
            {
                yield return basePlugin;
            }
        }
    }
}
