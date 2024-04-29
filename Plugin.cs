using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace IL2CPPAutoConfigReload;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    public static ManualLogSource Logger { get => Log; }

    public override void Load()
    {
        Log = base.Log;

        // Start a task to delay the startup of this plugin for 10 seconds after loading to ensure all other plugins are loaded
        const int START_DELAY_SECONDS = 10;
        Task.Run(async () => {
            await Task.Delay(START_DELAY_SECONDS * 1000);
        });

        // Print a message right away to indicate the plugin is loaded.
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded. Initial plugin search will occur in {START_DELAY_SECONDS} seconds.");
    }
}
