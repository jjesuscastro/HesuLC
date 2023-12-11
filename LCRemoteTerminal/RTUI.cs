using HesuLC;
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
        public override Vector2 DefaultAnchorMax => new Vector2(0.28f, 0.33f);
        public override Vector2 DefaultPosition => new Vector2(100, 220);
        public override bool CanDragAndResize => true;

        Dropdown moonDropdown;
        string[] moonList;
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

            UIUtils.CreateButtonWithLabel(vLayout, "MoonWeather", "Show weather of moons", "Show Weather").OnClick = () =>
            {
                string[] chatInput = { "/moons" };
                HUDManagerPatch.userTerminal(chatInput);
            };

            UIUtils.CreateDropdownWithSubmit(vLayout, out moonDropdown, "GoToMoon", "Go To Moon", OnMoonValueChanged).OnClick = () => {
                string[] chatInput = { "/moons", HUDManagerPatch.moons[selectedMoon] };
                HUDManagerPatch.userTerminal(chatInput);
            };

            UIUtils.CreateButtonWithLabel(vLayout, "Scan", "Scan scraps in current moon", "Scan").OnClick = () => {
                HUDManagerPatch.scanItems();
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

        public void UpdateMoonDropdown()
        {
            moonList = HUDManagerPatch.moons;
            moonDropdown.ClearOptions();

            List<Dropdown.OptionData> dropdownItems = new List<Dropdown.OptionData>();

            foreach (string s in moonList)
            {
                dropdownItems.Add(new Dropdown.OptionData(s));
            }

            moonDropdown.AddOptions(dropdownItems);
        }

        void OnMoonValueChanged(int x)
        {
            selectedMoon = x;
        }
    }
}
