using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RemoteTerminal.Patches;
using UnityEngine;
using UniverseLib.UI;

namespace RemoteTerminal
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class RemoteTerminalBase : BaseUnityPlugin
    {
        UnityEngine.GameObject menu;
        private const string modGUID = "hesukastro.RemoteTerminal";
        private const string modName = "Remote Terminal";
        private const string modVersion = "1.1.2";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static RemoteTerminalBase Instance;
        public static ManualLogSource mls;

        public static UIBase UiBase { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Start RemoteTerminal Mod");

            harmony.PatchAll(typeof(RemoteTerminalBase));
            harmony.PatchAll(typeof(HUDManagerPatch));

            float startupDelay = 1f;
            UniverseLib.Config.UniverseLibConfig config = new UniverseLib.Config.UniverseLibConfig
            {
                Disable_EventSystem_Override = false,
                Force_Unlock_Mouse = true,
            };

            UniverseLib.Universe.Init(startupDelay, OnInitialized, LogHandler, config);
        }

        void LogHandler(string message, LogType type)
        {
            // ...
        }

        void OnInitialized()
        {
            UiBase = UniversalUI.RegisterUI("RemoteTerminal.UI", UiUpdate);
            RTUI rtUI = new RTUI(UiBase);

            //UiBase.Enabled = false;
        }

        public static void ToggleUI()
        {
            UiBase.Enabled = !UiBase.Enabled;
        }

        void UiUpdate()
        {
            // Called once per frame when your UI is being displayed.
        }
    }
}
