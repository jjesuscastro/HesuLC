using RemoteTerminal.Patches;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace RemoteTerminal
{
    internal class RTUI : UniverseLib.UI.Panels.PanelBase
    {
        public RTUI(UIBase owner) : base(owner) { }

        public override string Name => $"{RemoteTerminalBase.modName} v{RemoteTerminalBase.modVersion}";
        public override int MinWidth => 200;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.1f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.28f, 0.37f);
        public override Vector2 DefaultPosition => new Vector2(100, 220);
        public override bool CanDragAndResize => true;

        int selectedMoon = 0;

        protected override void ConstructPanelContent()
        {
            Rect.offsetMin = new Vector2(15, 20);
            Rect.offsetMax = new Vector2(15, 20);

            //Vertical Layout Group
            GameObject vLayout = UIFactory.CreateVerticalGroup(ContentRoot, "Contents", true, false, true, true, 15, new Vector4(10, 0, 10, 0));

            //Open/Close Doors/Turrets/Mines
            GameObject terminal = UIFactory.CreateHorizontalGroup(vLayout, "RTerminal", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            InputFieldRef terminalInput = UIFactory.CreateInputField(terminal, "TerminalInput", "");
            ButtonRef terminalButton = UIFactory.CreateButton(terminal, "TerminalSubmit", "Toggle Door/Disable Turret/Mine");

            CreateButtonWithLabel(vLayout, "MoonWeather", "Show weather of moons", "Show Weather").OnClick = () =>
            {
                string[] chatInput = { "/moons" };
                HUDManagerPatch.userTerminal(chatInput);
            };

            CreateDropdownWithSubmit(vLayout, "RouteToMoon", "Go To Moon",HUDManagerPatch.moons, OnMoonValueChanged, HUDManagerPatch.moons[selectedMoon]).OnClick = () => {
                string[] chatInput = { "/moons", HUDManagerPatch.moons[selectedMoon] };
                HUDManagerPatch.userTerminal(chatInput);
            };

            CreateButtonWithLabel(vLayout, "Scan", "Scan scraps in current moon", "Scan").OnClick = () => {
                HUDManagerPatch.scanItems();
            };
            CreateButtonWithLabel(vLayout, "Help", "Shows available commands for RemoteTerminal", "Help").OnClick = () => {
                HUDManagerPatch.help();
            };

            terminalButton.OnClick = () =>
            {
                string[] chatInput = { "/rt", terminalInput.Text };
                HUDManagerPatch.userTerminal(chatInput);
                terminalInput.Text = string.Empty;
            };

            UIFactory.SetLayoutElement(terminalButton.GameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(terminalInput.GameObject, minWidth: 50, minHeight: 30);
        }

        public override void ConstructUI()
        {
            base.ConstructUI();

            GameObject closeBUtton = GameObject.Find("CloseButton");
            closeBUtton.SetActive(false);
        }

        ButtonRef CreateButtonWithLabel(GameObject parent, string name, string text, string buttonLabel)
        {
            //Moon weather
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            Text label = UIFactory.CreateLabel(gameObject, $"{name}Label", $"{text}");
            ButtonRef buttonRef = UIFactory.CreateButton(gameObject, $"{name}Button", $"{buttonLabel}");

            UIFactory.SetLayoutElement(label.gameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(buttonRef.GameObject, minWidth: 50, minHeight: 30);

            return buttonRef;
        }

        ButtonRef CreateDropdownWithSubmit(GameObject parent, string name, string label, string[] items, System.Action<int> OnValueChanged, string defaultItem = "")
        {
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 0, new Vector4(0, 5, 0, 5));

            Dropdown dropdown;
            GameObject dropdownGameObject = UIFactory.CreateDropdown(gameObject, $"{name}Dropdown", out dropdown, "Company", 12, OnValueChanged);
            

            List<Dropdown.OptionData> moonItems = new List<Dropdown.OptionData>();

            foreach(string s in items)
            {
                moonItems.Add(new Dropdown.OptionData(s));
            }

            dropdown.AddOptions(moonItems);
            ButtonRef button = UIFactory.CreateButton(gameObject, $"{name}Button", $"{label}");

            UIFactory.SetLayoutElement(dropdownGameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(button.GameObject, minWidth: 50, minHeight: 30);
            return button;
        }

        void OnMoonValueChanged(int x)
        {
            selectedMoon = x;
        }

        // override other methods as desired
    }
}
