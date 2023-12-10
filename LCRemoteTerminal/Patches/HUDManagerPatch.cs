using HarmonyLib;
using System;
using HesuLC;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using BepInEx;
using TMPro;

namespace RemoteTerminal.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        public static string[] moons = new string[] { "company", "experimentation", "assurance", "vow", "offense", "march", "rend", "dine", "titan" };

        static Terminal terminal;

        [HarmonyPatch("SubmitChat_performed")]
        [HarmonyPrefix]
        static void interceptTextChat(ref TMPro.TMP_InputField ___chatTextField)
        {
            String[] chatInput = ___chatTextField.text.Split(' ');

            switch (chatInput[0])
            {
                case "/rt":
                case "/moons":
                    useTerminal(chatInput, ref ___chatTextField);
                    break;
                case "/scan":
                    scanItems(ref ___chatTextField);
                    break;
                case "/help":
                    help(ref ___chatTextField);
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
                } else
                {
                    UI.closeMenu();
                    menuOpen = false;
                }
            }
        }

        #region TerminalCommands

        public static void userTerminal(string[] chatInput)
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

        async static void delayedInput(Terminal terminal, string action, string input)
        {
            terminal.BeginUsingTerminal();

            terminal.currentText = "";
            terminal.screenText.text = input;

            terminal.OnSubmit();

            if (action.Equals("/moons"))
            {
                if (moons.Any(input.Contains))
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

        public static void help()
        {
            TMP_InputField temp = null;
            help(ref temp);
        }

        public static void help(ref TMPro.TMP_InputField ___chatTextField)
        {
            Utils.displayMessage("Remote Terminal 1.1.0", "<size=60%>/rt [door/turret] </size><size=50%>Open/close doors; Disable turret</size>\n" +
                "<size=60%>/moons </size><size=50%>List moon weathers</size>\n" +
                "<size=60%>/moons [moonName] </size><size=50%>Reroute to moon</size>\n" +
                "<size=60%>/scan </size><size=50%>Scan objects</size>\n" +
                "<size=60%>/tp </size><size=50%>Activate teleporter</size>");

            if (___chatTextField != null)
                ___chatTextField.text = "";
        }
    }
}
