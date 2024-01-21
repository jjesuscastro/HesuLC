using HesuLC;
using LethalCheater.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalCheater
{
    public class UI : MonoBehaviour
    {
        string[] playerList;

        [SerializeField]
        private Toggle godModeToggle;
        [SerializeField]
        private Toggle ignoreDeathToggle;
        [SerializeField]
        private Toggle infiniteStaminaToggle;

        [SerializeField]
        private TMP_Dropdown playerListDropDown;
        [SerializeField]
        private Button teleportButton;

        [SerializeField]
        private Button killSelfButton;
        [SerializeField]
        private Button lightsButton;

        [SerializeField]
        private TMP_InputField addCreditsInput;
        [SerializeField]
        private Button addCreditsButton;

        [SerializeField]
        private Button autoQuotaButton;

        void Awake()
        {
            godModeToggle.onValueChanged.AddListener(GodMode_OnChange);
            ignoreDeathToggle.onValueChanged.AddListener(IgnoreDeath_OnChange);
            infiniteStaminaToggle.onValueChanged.AddListener(InfiniteSprint_OnChange);
            teleportButton.onClick.AddListener(Teleport_OnClick);
            killSelfButton.onClick.AddListener(KillSelf_OnClick);
            lightsButton.onClick.AddListener(Lights_OnClick);
            addCreditsButton.onClick.AddListener(AddCredits_OnClick);
            autoQuotaButton.onClick.AddListener(AutoQuota_OnClick);
        }

        void OnEnable()
        {
            InstantiatePlayerList();
        }

        void GodMode_OnChange(bool _value)
        {
            PlayerControllerBPatch.setGodMode(_value);
        }

        void IgnoreDeath_OnChange(bool _value)
        {
            PlayerControllerBPatch.setIgnoreDeath(_value);
        }

        void InfiniteSprint_OnChange(bool _value)
        {
            PlayerControllerBPatch.setInfSprint(_value);
        }

        void Teleport_OnClick()
        {
            HUDManagerPatch.delayedTeleport(playerList[playerListDropDown.value]);
        }

        void KillSelf_OnClick()
        {
            HUDManagerPatch.killPlayer();
        }

        void Lights_OnClick()
        {
            HUDManagerPatch.toggleLights();
        }

        void AddCredits_OnClick()
        {
            string[] chatInput = new string[2];
            chatInput[0] = "/credits";
            chatInput[1] = addCreditsInput.text;

            HUDManagerPatch.addCredits(chatInput);
            addCreditsInput.text = "";
        }

        void AutoQuota_OnClick()
        {
            HUDManagerPatch.autoQuota();
        }

        void InstantiatePlayerList()
        {
            playerList = Utils.getPlayerList();

            playerListDropDown.ClearOptions();
            playerListDropDown.AddOptions(new List<string>(playerList));
        }
    }
}
