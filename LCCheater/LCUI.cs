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

        public override string Name => $"{LethalCheaterBase.modName} v{LethalCheaterBase.modVersion}";
        public override int MinWidth => 200;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.1f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.28f, 0.63f);
        public override Vector2 DefaultPosition => new Vector2(-250, 220);
        public override bool CanDragAndResize => true;

        Dropdown playerDropdown;
        string[] playerList;
        int selectedPlayer = 0;

        protected override void ConstructPanelContent()
        {
            Rect.offsetMin = new Vector2(15, 20);
            Rect.offsetMax = new Vector2(15, 20);

            //Vertical Layout Group
            GameObject vLayout = UIFactory.CreateVerticalGroup(ContentRoot, "Contents", true, false, true, true, 15, new Vector4(10, 0, 10, 0));

            Text clientSidedHeader = UIFactory.CreateLabel(vLayout, "ClientSidedHeader", "Client Sided Cheats");
            UIFactory.SetLayoutElement(clientSidedHeader.gameObject, minWidth: 200, minHeight: 30);

            Toggle godMode = UIUtils.CreateToggle(vLayout, "GodMode", "God Mode");
            Toggle ignoreDeath = UIUtils.CreateToggle(vLayout, "IgnoreDeat", "Ignore Death");
            Toggle infSprint = UIUtils.CreateToggle(vLayout, "InfSprint", "Infinite Sprint");

            Text serverSidedHeader = UIFactory.CreateLabel(vLayout, "ServerSidedHeader", "Server Sided Cheats");
            UIFactory.SetLayoutElement(serverSidedHeader.gameObject, minWidth: 200, minHeight: 30);

            //Kill Player
            UIUtils.CreateButtonWithLabel(vLayout, "Suicide", "Kill Self", "Suicide").OnClick = () => {
                HUDManagerPatch.killPlayer();
            };

            UIUtils.CreateDropdownWithSubmit(vLayout, out playerDropdown, "TeleportPlayer", "Teleport", OnPlayerValueChanged).OnClick = () => {
                HUDManagerPatch.delayedTeleport(playerList[selectedPlayer]);
            };

            //Add Credits
            GameObject addCredits = UIFactory.CreateHorizontalGroup(vLayout, "AddCredits", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            InputFieldRef addCreditsInput = UIFactory.CreateInputField(addCredits, "AddCreditsInput", "");
            ButtonRef addCreditsButton = UIFactory.CreateButton(addCredits, "AddCreditsSubmit", "Add Credits");

            UIUtils.CreateButtonWithLabel(vLayout, "ToggleLights", "Toggle Ship Lights On/Off", "Toggle Lights").OnClick = () => {
                HUDManagerPatch.toggleLights();
            };

            Text hostHeader = UIFactory.CreateLabel(vLayout, "HostHeader", "Host-only Cheats");
            UIFactory.SetLayoutElement(hostHeader.gameObject, minWidth: 200, minHeight: 30);
            UIUtils.CreateButtonWithLabel(vLayout, "AutoQuota", "Automatically fulfill quota", "Auto Quota").OnClick = () => {
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

            ignoreDeath.onValueChanged.AddListener(delegate
            {
                PlayerControllerBPatch.setIgnoreDeath(ignoreDeath.isOn);
            });

            infSprint.onValueChanged.AddListener(delegate
            {
                PlayerControllerBPatch.setInfSprint(infSprint.isOn);
            });

            UIFactory.SetLayoutElement(addCreditsInput.GameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(addCreditsButton.GameObject, minWidth: 50, minHeight: 30);
        }

        public override void ConstructUI()
        {
            base.ConstructUI();

            GameObject closeBUtton = GameObject.Find("CloseButton");
            closeBUtton.SetActive(false);
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
    }
}
