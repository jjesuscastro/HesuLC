using GameNetcodeStuff;
using HarmonyLib;
using System;
using HesuLC;
using System.Threading.Tasks;
using System.Linq;
using BepInEx;
using TMPro;

namespace LethalCheater.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        static Terminal terminal;
        static ShipTeleporter[] shipTeleporters;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void updatePatch()
        {
            if (UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.F10))
            {
                LethalCheaterBase.ToggleUI();
            }

            if(UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.Escape))
            {
                LethalCheaterBase.DisableUI();
            }
        }

        #region Teleport
        static void teleportOut(ref TMPro.TMP_InputField ___chatTextField)
        {
            delayedTeleport();
            ___chatTextField.text = "";
        }

        static async void delayedTeleport()
        {
            shipTeleporters = (ShipTeleporter[])UnityEngine.Object.FindObjectsOfType(typeof(ShipTeleporter));

            int index = Utils.getPlayerIndex(LethalCheaterBase.playerName.Value);
            if (index == -1)
                return;

            StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(index);
            await Task.Delay(250);

            ShipTeleporter[] array = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();
            foreach (ShipTeleporter teleporter in array)
            {
                if (!teleporter.isInverseTeleporter && teleporter.buttonTrigger.interactable)
                    teleporter.buttonTrigger.onInteract.Invoke(Utils.getPlayerClient());
            }
        }

        public static async void delayedTeleport(string playerName)
        {
            shipTeleporters = (ShipTeleporter[])UnityEngine.Object.FindObjectsOfType(typeof(ShipTeleporter));

            int index = Utils.getPlayerIndex(playerName);
            if (index == -1)
            {
                LethalCheaterBase.mls.LogInfo($"Cannot find player {playerName}");
                return;
            }

            StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(index);
            await Task.Delay(250);

            ShipTeleporter[] array = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();
            foreach (ShipTeleporter teleporter in array)
            {
                if (!teleporter.isInverseTeleporter && teleporter.buttonTrigger.interactable)
                    teleporter.buttonTrigger.onInteract.Invoke(Utils.getPlayerClient());
            }
        }

        #endregion

        public static void addCredits(string[] chatInput)
        {
            TMP_InputField temp = null;
            addCredits(chatInput, ref temp);
        }

        static void addCredits(string[] chatInput, ref TMPro.TMP_InputField ___chatTextField)
        {
            if (terminal == null)
                terminal = (Terminal)UnityEngine.Object.FindObjectOfType(typeof(Terminal));

            int currentCredits = terminal.groupCredits;
            int addCredits = 0;
            int.TryParse(chatInput[1], out addCredits);

            terminal.SyncGroupCreditsServerRpc(currentCredits + addCredits, terminal.numberOfItemsInDropship);

            Utils.displayMessage("Credits added", $"Total Credits: {currentCredits + addCredits}");

            if(___chatTextField != null)
                ___chatTextField.text = "";
        }

        public static void toggleLights()
        {
            TMP_InputField temp = null;
            toggleLights(ref temp);
        }

        static void toggleLights(ref TMPro.TMP_InputField ___chatTextField)
        {
            InteractTrigger[] interactables = UnityEngine.Object.FindObjectsOfType<InteractTrigger>();
            InteractTrigger lightSwitch = interactables.FirstOrDefault(i => i.name == "LightSwitch");

            lightSwitch.onInteract.Invoke(Utils.getPlayerClient());

            if (___chatTextField != null)
                ___chatTextField.text = "";
        }

        public static void killPlayer()
        {
            TMP_InputField temp = null;
            killPlayer(ref temp);
        }

        static void killPlayer(ref TMPro.TMP_InputField ___chatTextField)
        {
            PlayerControllerB player = Utils.getPlayerClient();
            if (player == null)
            {
                if (___chatTextField != null)
                    ___chatTextField.text = "";
                return;
            }

            player.KillPlayer(UnityEngine.Vector3.zero);
            if (___chatTextField != null)
                ___chatTextField.text = "";
        }

        public static void autoQuota()
        {
            TMP_InputField temp = null;
            autoQuota(ref temp);
        }

        static void autoQuota(ref TMPro.TMP_InputField ___chatTextField)
        {
            TimeOfDay timeOfDay = (TimeOfDay)UnityEngine.Object.FindObjectOfType(typeof(TimeOfDay));

            timeOfDay.quotaFulfilled = (int)timeOfDay.profitQuota;
            timeOfDay.UpdateProfitQuotaCurrentTime();

            Utils.displayMessage("Quota", "Quota auto fulfilled");

            if (___chatTextField != null)
                ___chatTextField.text = "";
        }
    }
}
