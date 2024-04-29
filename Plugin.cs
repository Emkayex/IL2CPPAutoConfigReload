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

        // Start the tracker's main loop on a separate thread
        Task.Run(() => {
            var tracker = new Tracker();
            tracker.MainLoop();
        });

        // Print a message right away to indicate the plugin is loaded
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded.");
    }
}
