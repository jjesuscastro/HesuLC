using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HesuLC;
using LethalCheater.Patches;
using System;
using UnityEngine;

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
        }

        public static void ToggleUI()
        {
           
        }

        public static void DisableUI()
        {
        }

    }
}
