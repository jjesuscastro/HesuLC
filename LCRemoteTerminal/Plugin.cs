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
        public const string modGUID = "hesukastro.RemoteTerminal";
        public const string modName = "Remote Terminal";
        public const string modVersion = "1.1.5";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static RemoteTerminalBase Instance;
        public static ManualLogSource mls;

        public static UIBase UiBase { get; private set; }
        internal static RTUI rtUI { get; private set; }
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
            rtUI = new RTUI(UiBase);

            UiBase.Enabled = false;
        }

        public static void ToggleUI()
        {
            rtUI.UpdateMoonDropdown();
            UiBase.Enabled = !UiBase.Enabled;

            if(UiBase.Enabled)
            {
                rtUI.SetActive(true);
            }
        }

        public static void DisableUI()
        {
            UiBase.Enabled = false;
        }

        void UiUpdate()
        {
            // Called once per frame when your UI is being displayed.
        }
    }
}
