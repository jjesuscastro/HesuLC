using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalCheater.Patches;
using System;

namespace LethalCheater
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LethalCheaterBase : BaseUnityPlugin
    {
        private const String modGUID = "hesukastro.LethalCheater";
        private const String modName = "Cheater";
        private const String modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        public static ConfigEntry<string> playerName;

        private static LethalCheaterBase Instance;
        public static ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Start RT Mod");

            playerName = Config.Bind<string>("Player", "playerName", "", "Player name used for teleporting");

            harmony.PatchAll(typeof(LethalCheaterBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            harmony.PatchAll(typeof(HUDManagerPatch));
        }
    }
}
