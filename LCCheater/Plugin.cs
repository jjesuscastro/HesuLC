using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HesuLC;
using LethalCheater.Patches;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace LethalCheater
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LethalCheaterBase : BaseUnityPlugin
    {
        public const string modGUID = "hesukastro.LethalCheater";
        public const string modName = "Lethal Cheater";
        public const string modVersion = "2.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        public static ConfigEntry<string> playerName;

        private static LethalCheaterBase Instance;
        public static ManualLogSource mls;

        AssetBundle assetBundle;
        static GameObject UIPrefab;
        static GameObject UIGameObject;

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

            LoadAssetBundle();
        }

        void LoadAssetBundle()
        {
            string sAeemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(sAeemblyLocation, "lcuibundle/uibundle"));

            if (assetBundle == null)
            {
                mls.LogError("Cannot Load Asset");
                return;
            }

            UIPrefab = assetBundle.LoadAsset<GameObject>("Assets/LethalCheaterUI.prefab");
        }

        public static void ToggleUI()
        {
           if(UIGameObject == null)
            {
                UIGameObject = Instantiate(UIPrefab);
            }
           else
            {
                UIGameObject.SetActive(!UIGameObject.activeInHierarchy);
            }

            PlayerControllerBPatch.SetLookInputLock(UIGameObject.activeInHierarchy);
            Cursor.visible = UIGameObject.activeInHierarchy;
            Cursor.lockState = UIGameObject.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static void DisableUI()
        {
            if (UIGameObject == null)
                return;

            UIGameObject.SetActive(false);
            PlayerControllerBPatch.SetLookInputLock(UIGameObject.activeInHierarchy);
        }

    }
}
