using HarmonyLib;
using System;
using HesuLC;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using TMPro;

namespace RemoteTerminal.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        public static string[] moons = new string[] { "Company", "Experimentation", "Assurance", "Vow", "Offense", "March", "Rend", "Dine", "Titan" };

        static Terminal terminal;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void updatePatch()
        {
            if (UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.F10))
            {
                RemoteTerminalBase.ToggleUI();
            }

            if (UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.Escape))
            {
                RemoteTerminalBase.DisableUI();
            }
        }

        #region TerminalCommands

        public static void useTerminal(string[] chatInput)
        {
            TMP_InputField temp = null;
            useTerminal(chatInput, ref temp);
        }

        static void useTerminal(string[] chatInput, ref TMPro.TMP_InputField ___chatTextField)
        {
            if (terminal == null)
                terminal = (Terminal)UnityEngine.Object.FindObjectOfType(typeof(Terminal));

            string action = chatInput[0];
            string[] inputArr = chatInput.Where(val => val != "/rt" && val != "/moons").ToArray();
            string input = string.Join(" ", inputArr);

            delayedInput(terminal, action, input);
            if (___chatTextField != null)
                ___chatTextField.text = "";
        }

        public static void transmitMessage(string message)
        {
            HUDManager.Instance.UseSignalTranslatorServerRpc(message);
        }

        async static void delayedInput(Terminal terminal, string action, string input)
        {
            terminal.BeginUsingTerminal();

            terminal.currentText = "";
            terminal.screenText.text = input;

            terminal.OnSubmit();

            if (action.Equals("/moons"))
            {
                if (moons.Contains(input, StringComparer.OrdinalIgnoreCase))
                {
                    await Task.Delay(250);
                    terminal.currentText = "";
                    input = "confirm";
                    terminal.screenText.text = input;
                    terminal.OnSubmit();
                }
                else
                {
                    SelectableLevel[] moons = terminal.moonsCatalogueList;
                    StringBuilder sb = new StringBuilder();

                    int i = 0;
                    foreach (SelectableLevel moon in moons)
                    {
                        sb.Append("<size=80%>");
                        sb.Append(moon.PlanetName.Split(' ')[1]);
                        sb.Append(" ");
                        if (!moon.currentWeather.ToString().Equals("None"))
                            sb.Append($"({moon.currentWeather})");

                        if (i != moons.Length - 1)
                            sb.Append(" | ");

                        sb.Append("</size>");
                        i++;
                    }
                    Utils.displayMessage("Moons", sb.ToString());
                }
            }

            await Task.Delay(250);
            terminal.QuitTerminal();
        }

        public static void scanItems()
        {
            TMP_InputField temp = null;
            scanItems(ref temp);
        }

        static void scanItems(ref TMPro.TMP_InputField ___chatTextField)
        {
            GrabbableObject[] objects = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            System.Random random = new System.Random(StartOfRound.Instance.randomMapSeed + 91);
            int numObjects = 0;
            int totalValue = 0;

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].itemProperties.isScrap && !objects[i].isInShipRoom && !objects[i].isInElevator)
                {
                    totalValue += Mathf.Clamp(random.Next(objects[i].itemProperties.minValue, objects[i].itemProperties.maxValue), objects[i].scrapValue - 6 * i, objects[i].scrapValue + 9 * i);
                    numObjects++;
                }
            }

            Utils.displayMessage("Scan", $"There are {numObjects} objects outside the ship, totalling at an approximate value of ${totalValue}.");

            if(___chatTextField != null)
                ___chatTextField.text = "";
        }
        #endregion
    }
}
