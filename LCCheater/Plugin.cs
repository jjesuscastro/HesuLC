using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HesuLC;
using LethalCheater.Patches;
using System;
using UnityEngine;
using UniverseLib.UI;

namespace LethalCheater
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LethalCheaterBase : BaseUnityPlugin
    {
        public const string modGUID = "hesukastro.LethalCheater";
        public const string modName = "Lethal Cheater";
        public const string modVersion = "1.1.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        public static ConfigEntry<string> playerName;

        private static LethalCheaterBase Instance;
        public static ManualLogSource mls;

        public static UIBase UiBase { get; private set; }
        internal static LCUI LcUI { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Start LethalCheater Mod");

            playerName = Config.Bind<string>("Player", "playerName", "", "Player name used for teleporting");

            harmony.PatchAll(typeof(LethalCheaterBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
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
            UiBase = UniversalUI.RegisterUI("LethalCheater.UI", UiUpdate);
            LcUI = new LCUI(UiBase);

            UiBase.Enabled = false;
        }

        public static void ToggleUI()
        {
            LcUI.UpdatePlayerDropdown();
            UiBase.Enabled = !UiBase.Enabled;

            if (UiBase.Enabled)
            {
                LcUI.SetActive(true);
            }
        }

        public static void DisableUI()
        {
            UiBase.Enabled = false;
        }

        void UiUpdate()
        {
        }
    }
}
