using RemoteTerminal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using static UnityEngine.InputSystem.HID.HID;

namespace RemoteTerminal
{
    internal class RTUI : UniverseLib.UI.Panels.PanelBase
    {
        public RTUI(UIBase owner) : base(owner) { }

        public override string Name => "Remote Terminal";
        public override int MinWidth => 200;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.1f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.28f, 0.33f);
        public override Vector2 DefaultPosition => new Vector2(100, 200);
        public override bool CanDragAndResize => false;

        int selectedMoon = 0;

        protected override void ConstructPanelContent()
        {
            Rect.offsetMin = new Vector2(15, 20);
            Rect.offsetMax = new Vector2(15, 20);

            //Title
            Text panelTitle = UIFactory.CreateLabel(ContentRoot, "RemoteTerminalLabel", "\tRemote Terminal");
            UIFactory.SetLayoutElement(panelTitle.gameObject, minWidth: 200, minHeight: 30);

            //Vertical Layout Group
            GameObject vLayout = UIFactory.CreateVerticalGroup(ContentRoot, "Contents", true, false, true, true, 15, new Vector4(10, 0, 10, 0));

            //Open/Close Doors/Turrets/Mines
            GameObject terminal = UIFactory.CreateHorizontalGroup(vLayout, "RouteMoon", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            InputFieldRef terminalInput = UIFactory.CreateInputField(terminal, "TerminalInput", "");
            ButtonRef terminalButton = UIFactory.CreateButton(terminal, "TerminalSubmit", "Toggle Door/Disable Turret/Mine");

            //Route to Moon
            GameObject routeMoon = UIFactory.CreateHorizontalGroup(vLayout, "RouteMoon", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            CreateMoonDropdown(routeMoon);
            ButtonRef routeButton = UIFactory.CreateButton(routeMoon, "GoToMoon","Go To Moon");

            //Scan
            GameObject scan = UIFactory.CreateHorizontalGroup(vLayout, "Scan", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            Text scanLabel = UIFactory.CreateLabel(scan, "ScanLabel", "Scan scraps in current moon");
            ButtonRef scanButton = UIFactory.CreateButton(scan, "ScanButton", "Scan");

            //Help
            GameObject help = UIFactory.CreateHorizontalGroup(vLayout, "Scan", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            Text helpLabel = UIFactory.CreateLabel(help, "HelpLabel", "Shows available commands for RemoteTerminal");
            ButtonRef helpButton = UIFactory.CreateButton(help, "HelpButton", "Help");

            UIFactory.SetLayoutElement(terminalInput.GameObject, minWidth: 50, minHeight: 30);
            UIFactory.SetLayoutElement(terminalButton.GameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(routeButton.GameObject, minWidth: 50, minHeight: 30);
            UIFactory.SetLayoutElement(scanLabel.gameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(scanButton.GameObject, minWidth: 50, minHeight: 30);
            UIFactory.SetLayoutElement(helpLabel.gameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(helpButton.GameObject, minWidth: 50, minHeight: 30);

            terminalButton.OnClick = () =>
            {
                string[] chatInput = { "/rt", terminalInput.Text };
                HUDManagerPatch.userTerminal(chatInput);
                terminalInput.Text = string.Empty;
            };

            routeButton.OnClick = () => {
                string[] chatInput = { "/moons", HUDManagerPatch.moons[selectedMoon] };
                HUDManagerPatch.userTerminal(chatInput);
            };

            scanButton.OnClick = () => {
                HUDManagerPatch.scanItems();
            };

            helpButton.OnClick = () => {
                HUDManagerPatch.help();
            };
        }

        GameObject CreateMoonDropdown(GameObject routeMoon)
        {
            Dropdown moonDropdown;
            GameObject gameObject = UIFactory.CreateDropdown(routeMoon, "MoonDropdown", out moonDropdown, "Company", 12, OnMoonValueChanged);
            UIFactory.SetLayoutElement(gameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);

            List<Dropdown.OptionData> moonItems = new List<Dropdown.OptionData>();

            foreach(string moon in HUDManagerPatch.moons)
            {
                moonItems.Add(new Dropdown.OptionData(moon));
            }

            moonDropdown.AddOptions(moonItems);
            return gameObject;
        }

        void OnMoonValueChanged(int x)
        {
            selectedMoon = x;
        }

        // override other methods as desired
    }
}
