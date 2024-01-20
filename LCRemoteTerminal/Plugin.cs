using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RemoteTerminal.Patches;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace RemoteTerminal
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class RemoteTerminalBase : BaseUnityPlugin
    {
        public const string modGUID = "hesukastro.RemoteTerminal";
        public const string modName = "Remote Terminal";
        public const string modVersion = "1.2.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static RemoteTerminalBase Instance;
        public static ManualLogSource mls;

        AssetBundle assetBundle;
        static GameObject ui;

        void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Start RemoteTerminal Mod");

            harmony.PatchAll(typeof(RemoteTerminalBase));
            harmony.PatchAll(typeof(HUDManagerPatch));

            float startupDelay = 1f;

            LoadAssetBundle();

        }

        void LoadAssetBundle()
        {
            string sAeemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(sAeemblyLocation, "lcremter"));

            if(assetBundle == null)
            {
                mls.LogError("Cannot Load Asset");
                return;
            }

            ui = assetBundle.LoadAsset<GameObject>("Assets/UI.prefab");
            //Maybe you can instantiate the ui from here
        }

        public static void ToggleUI()
        {
            Instantiate(ui);
        }

        public static void DisableUI()
        {
        }

        void UiUpdate()
        {
            // Called once per frame when your UI is being displayed.
        }
    }
}
