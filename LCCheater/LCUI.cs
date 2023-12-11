using HesuLC;
using LethalCheater.Patches;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace LethalCheater
{
    internal class LCUI : UniverseLib.UI.Panels.PanelBase
    {
        public LCUI(UIBase owner) : base(owner) { }

        public override string Name => "Lethal Cheater";
        public override int MinWidth => 200;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.1f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.28f, 0.6f);
        public override Vector2 DefaultPosition => new Vector2(-250, 220);
        public override bool CanDragAndResize => false;

        Dropdown playerDropdown;
        string[] playerList;
        int selectedPlayer = 0;

        protected override void ConstructPanelContent()
        {
            Rect.offsetMin = new Vector2(15, 20);
            Rect.offsetMax = new Vector2(15, 20);

            //Title
            Text panelTitle = UIFactory.CreateLabel(ContentRoot, "LethalCheaterLabel", "\tLethal Cheater");
            UIFactory.SetLayoutElement(panelTitle.gameObject, minWidth: 200, minHeight: 30);

            //Vertical Layout Group
            GameObject vLayout = UIFactory.CreateVerticalGroup(ContentRoot, "Contents", true, false, true, true, 15, new Vector4(10, 0, 10, 0));

            Text clientSidedHeader = UIFactory.CreateLabel(vLayout, "ClientSidedHeader", "Client Sided Cheats");
            UIFactory.SetLayoutElement(clientSidedHeader.gameObject, minWidth: 200, minHeight: 30);

            Toggle godMode = CreateToggle(vLayout, "GodMode", "God Mode");
            Toggle infSprint = CreateToggle(vLayout, "InfSprint", "Infinite Sprint");

            Text serverSidedHeader = UIFactory.CreateLabel(vLayout, "ServerSidedHeader", "Server Sided Cheats");
            UIFactory.SetLayoutElement(serverSidedHeader.gameObject, minWidth: 200, minHeight: 30);

            //Kill Player
            CreateButtonWithLabel(vLayout, "Suicide", "Kill Self", "Suicide").OnClick = () => {
                HUDManagerPatch.killPlayer();
            };

            CreateDropdownWithSubmit(vLayout, out playerDropdown, "TeleportPlayer", "Teleport", OnPlayerValueChanged).OnClick = () => {
                HUDManagerPatch.delayedTeleport(playerList[selectedPlayer]);
            };

            //Add Credits
            GameObject addCredits = UIFactory.CreateHorizontalGroup(vLayout, "AddCredits", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            InputFieldRef addCreditsInput = UIFactory.CreateInputField(addCredits, "AddCreditsInput", "");
            ButtonRef addCreditsButton = UIFactory.CreateButton(addCredits, "AddCreditsSubmit", "Add Credits");

            CreateButtonWithLabel(vLayout, "ToggleLights", "Toggle Ship Lights On/Off", "Toggle Lights").OnClick = () => {
                HUDManagerPatch.toggleLights();
            };

            Text hostHeader = UIFactory.CreateLabel(vLayout, "HostHeader", "Host-only Cheats");
            UIFactory.SetLayoutElement(hostHeader.gameObject, minWidth: 200, minHeight: 30);
            CreateButtonWithLabel(vLayout, "AutoQuota", "Automatically fulfill quota", "Auto Quota").OnClick = () => {
                HUDManagerPatch.autoQuota();
            };

            addCreditsButton.OnClick = () =>
            {
                string[] chatInput = { "/credits", addCreditsInput.Text };
                HUDManagerPatch.addCredits(chatInput);
                addCreditsInput.Text = string.Empty;
            };

            godMode.onValueChanged.AddListener(delegate
            {
                PlayerControllerBPatch.setGodMode(godMode.isOn);
            });

            infSprint.onValueChanged.AddListener(delegate
            {
                PlayerControllerBPatch.setInfSprint(infSprint.isOn);
            });

            UIFactory.SetLayoutElement(addCreditsInput.GameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(addCreditsButton.GameObject, minWidth: 50, minHeight: 30);
        }


        Toggle CreateToggle(GameObject parent, string name, string text)
        {
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 10, new Vector4(0, 5, 0, 5));
            Toggle toggle;
            Text label = UIFactory.CreateLabel(gameObject, $"{name}Text", $"{text}");
            label.alignment = TextAnchor.MiddleCenter;
            GameObject GO = UIFactory.CreateToggle(gameObject, $"{name}Toggle", out toggle, out label);

            toggle.isOn = false;

            UIFactory.SetLayoutElement(label.gameObject, minWidth: 180, minHeight: 30, preferredWidth: 180);
            UIFactory.SetLayoutElement(toggle.gameObject, minWidth: 30, minHeight: 30);
            UIFactory.SetLayoutElement(GO, minWidth: 200, minHeight: 30);

            return toggle;
        }

        ButtonRef CreateDropdownWithSubmit(GameObject parent, out Dropdown dropdown, string name, string label, System.Action<int> OnValueChanged)
        {
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 0, new Vector4(0, 5, 0, 5));

            GameObject dropdownGameObject = UIFactory.CreateDropdown(gameObject, $"{name}Dropdown", out dropdown, "", 12, OnValueChanged);
            ButtonRef button = UIFactory.CreateButton(gameObject, $"{name}Button", $"{label}");

            UIFactory.SetLayoutElement(dropdownGameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(button.GameObject, minWidth: 50, minHeight: 30);
            return button;
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

        public void UpdatePlayerDropdown()
        {
            playerList = Utils.getPlayerList();
            playerDropdown.ClearOptions();

            List<Dropdown.OptionData> dropdownItems = new List<Dropdown.OptionData>();

            foreach (string s in playerList)
            {
                dropdownItems.Add(new Dropdown.OptionData(s));
            }

            playerDropdown.AddOptions(dropdownItems);
        }

        void OnPlayerValueChanged(int i)
        {
            selectedPlayer = i;
        }

        // override other methods as desired
    }
}
