using RemoteTerminal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteTerminal
{
    public class UI:MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField toggleInput;
        [SerializeField]
        private Button toggleButton;

        [SerializeField]
        private TMP_InputField transmitInput;
        [SerializeField]
        private Button transmitButton;

        [SerializeField]
        private Button weatherButton;
        [SerializeField]
        private Button scanButton;

        [SerializeField]
        private TMP_Dropdown moonDropdown;
        [SerializeField]
        private Button moonButton;

        void Awake()
        {
            toggleButton.onClick.AddListener(ToggleButton_OnClick);
            transmitButton.onClick.AddListener(TransmitButton_OnClick);
            weatherButton.onClick.AddListener(WeatherButton_OnClick);
            scanButton.onClick.AddListener(ScanButton_OnClick);
            moonButton.onClick.AddListener(MoonButton_OnClick);

            InstantiateMoonDropdown();
        }

        void ToggleButton_OnClick()
        {
            string[] chatInput = new string[2];
            chatInput[0] = "/rt";
            chatInput[1] = toggleInput.text;
            HUDManagerPatch.useTerminal(chatInput);
            toggleInput.text = "";
        }

        void TransmitButton_OnClick()
        {
            HUDManagerPatch.transmitMessage(transmitInput.text);
            transmitInput.text = "";
        }

        void WeatherButton_OnClick()
        {
            string[] chatInput = new string[1];
            chatInput[0] = "/moons";
            HUDManagerPatch.useTerminal(chatInput);
        }

        void MoonButton_OnClick()
        {
            string[] chatInput = new string[2];
            chatInput[0] = "/moons";
            chatInput[1] = moonDropdown.options[moonDropdown.value].text;
            RemoteTerminalBase.mls.LogInfo(chatInput[1]);
            HUDManagerPatch.useTerminal(chatInput);
        }

        void ScanButton_OnClick()
        {
            HUDManagerPatch.scanItems();
        }

        void InstantiateMoonDropdown()
        {
            moonDropdown.ClearOptions();
            moonDropdown.AddOptions(new List<string>(HUDManagerPatch.moons));
        }
    }
}
