using GameNetcodeStuff;
using HarmonyLib;
using System;
using HesuLC;
using System.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;
using BepInEx;
using RemoteTerminal;

namespace LethalCheater.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        static Terminal terminal;
        static ShipTeleporter[] shipTeleporters;

        [HarmonyPatch("SubmitChat_performed")]
        [HarmonyPrefix]
        static void interceptTextChat(ref TMPro.TMP_InputField ___chatTextField)
        {
            String[] chatInput = ___chatTextField.text.Split(' ');

            switch (chatInput[0])
            {
                case "/credits":
                    addCredits(chatInput, ref ___chatTextField);
                    break;
                case "/tp":
                    teleportOut(ref ___chatTextField);
                    break;
                case "/lights":
                    toggleLights(ref ___chatTextField);
                    break;
                case "/fire":
                    killPlayer(chatInput, ref ___chatTextField);
                    break;
                case "/quota":
                    autoQuota(ref ___chatTextField);
                    break;
                case "/god":
                    toggleGodMode(ref ___chatTextField);
                    break;
                case "/is":
                    toggleInfiniteSprint(ref ___chatTextField);
                    break;
            }
        }

        static bool menuOpen = false;
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void updatePatch()
        {
            if (UnityInput.Current.GetKeyDown("F10"))
            {
                if (!menuOpen)
                {
                    UI.openMenu();
                    menuOpen = true;
                }
                else
                {
                    UI.closeMenu();
                    menuOpen = false;
                }
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
        #endregion

        static void addCredits(string[] chatInput, ref TMPro.TMP_InputField ___chatTextField)
        {
            if (terminal == null)
                terminal = (Terminal)UnityEngine.Object.FindObjectOfType(typeof(Terminal));

            int currentCredits = terminal.groupCredits;
            int addCredits = 0;
            int.TryParse(chatInput[1], out addCredits);

            terminal.SyncGroupCreditsServerRpc(currentCredits + addCredits, terminal.numberOfItemsInDropship);

            Utils.displayMessage("Credits added", $"Total Credits: {currentCredits + addCredits}");

            ___chatTextField.text = "";
        }

        static void toggleLights(ref TMPro.TMP_InputField ___chatTextField)
        {
            InteractTrigger[] interactables = UnityEngine.Object.FindObjectsOfType<InteractTrigger>();
            InteractTrigger lightSwitch = interactables.FirstOrDefault(i => i.name == "LightSwitch");

            lightSwitch.onInteract.Invoke(Utils.getPlayerClient());

            ___chatTextField.text = "";
        }

        static void killPlayer(string[] chatInput, ref TMPro.TMP_InputField ___chatTextField)
        {
            if (chatInput.Length <= 1)
            {
                ___chatTextField.text = "";
                return;
            }

            PlayerControllerB player = Utils.getPlayerObject(chatInput[1]);
            if (player == null)
            {
                ___chatTextField.text = "";
                return;
            }

            player.KillPlayer(UnityEngine.Vector3.zero);
            ___chatTextField.text = "";
        }

        static void autoQuota(ref TMPro.TMP_InputField ___chatTextField)
        {
            TimeOfDay timeOfDay = (TimeOfDay)UnityEngine.Object.FindObjectOfType(typeof(TimeOfDay));

            timeOfDay.quotaFulfilled = (int)timeOfDay.profitQuota;
            timeOfDay.UpdateProfitQuotaCurrentTime();

            Utils.displayMessage("Quota", "Quota auto fulfilled");

            ___chatTextField.text = "";
        }

        static void toggleGodMode(ref TMPro.TMP_InputField ___chatTextField)
        {
            PlayerControllerBPatch.toggleGodMode();

            ___chatTextField.text = "";
        }

        static void toggleInfiniteSprint(ref TMPro.TMP_InputField ___chatTextField)
        {
            PlayerControllerBPatch.toggleInfiniteSprint();

            ___chatTextField.text = "";
        }
    }
}
