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
        public const string modVersion = "2.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static RemoteTerminalBase Instance;
        public static ManualLogSource mls;

        AssetBundle assetBundle;
        static GameObject UIPrefab;
        static GameObject UIGameObject;

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
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(sAeemblyLocation, "rtuibundle/uibundle"));

            if(assetBundle == null)
            {
                mls.LogError("Cannot Load Asset");
                return;
            }

            UIPrefab = assetBundle.LoadAsset<GameObject>("Assets/RemoteTerminalUI.prefab");
        }

        public static void ToggleUI()
        {
            if (UIGameObject == null)
            {
                UIGameObject = Instantiate(UIPrefab);
            }
            else
            {
                UIGameObject.SetActive(!UIGameObject.activeInHierarchy);
            }

            Cursor.visible = UIGameObject.activeInHierarchy;
            Cursor.lockState = UIGameObject.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static void DisableUI()
        {
            if (UIGameObject == null)
                return;

            UIGameObject.SetActive(false);
        }
    }
}
