using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RemoteTerminal.Patches;

namespace RemoteTerminal
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class RemoteTerminalBase : BaseUnityPlugin
    {
        UnityEngine.GameObject menu;
        private const string modGUID = "hesukastro.RemoteTerminal";
        private const string modName = "Remote Terminal";
        private const string modVersion = "1.1.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static RemoteTerminalBase Instance;

        public static ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Start RemoteTerminal Mod");

            harmony.PatchAll(typeof(RemoteTerminalBase));
            harmony.PatchAll(typeof(HUDManagerPatch));
        }
    }
}
