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
    internal class UI
    {
        static GameObject menuContainer;

        public static void openMenu()
        {
            RemoteTerminalBase.mls.LogWarning("F10 pressed: open");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (menuContainer == null)
            {
                RemoteTerminalBase.mls.LogWarning("Menu not initialized, initializing...");
                GameObject canvas = GameObject.Find("Canvas");
                menuContainer = new GameObject("RTMenuContainer");
                RectTransform rTransform = menuContainer.AddComponent<RectTransform>();
                rTransform.sizeDelta = new Vector2(150, 300);
                VerticalLayoutGroup vLayoutGroup = menuContainer.AddComponent<VerticalLayoutGroup>();
                vLayoutGroup.padding = new RectOffset(15, 15, 15, 15);
                vLayoutGroup.childControlHeight = true;
                vLayoutGroup.childControlWidth = true;
                vLayoutGroup.spacing = 2;
                menuContainer.AddComponent<Image>().color = new Color(83/255f, 83 / 255f, 83 / 255f);
                menuContainer.transform.SetParent(canvas.transform, false);

                createLabel("Remote Terminal", rTransform);

                foreach (string moon in HUDManagerPatch.moons)
                {
                    createButton($"Route to {moon}", rTransform).GetComponent<Button>().onClick.AddListener(delegate
                    {
                        string[] chatInput = { "/moons", moon };
                        HUDManagerPatch.userTerminal(chatInput);
                    });
                }

                createButton("Scan", rTransform).GetComponent<Button>().onClick.AddListener(delegate
                {
                    HUDManagerPatch.scanItems();
                });

                createButton("Help", rTransform).GetComponent<Button>().onClick.AddListener(delegate
                {
                    HUDManagerPatch.help();
                });
            }

            menuContainer.SetActive(true);
        }

        public static void closeMenu()
        {
            RemoteTerminalBase.mls.LogWarning("F10 pressed: close");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            menuContainer.SetActive(false);
        }

        static Button createButton(string text, Transform parent)
        {
            //Create button and target image
            GameObject buttonGO = new GameObject(text);
            RectTransform buttonGOrectTransform = buttonGO.AddComponent<RectTransform>();
            Button button = buttonGO.AddComponent<Button>();

            button.image = buttonGO.AddComponent<Image>();

            createLabel(text, buttonGOrectTransform);

            buttonGO.transform.SetParent(parent, false);

            return button;
        }

        static GameObject createLabel(string text, RectTransform parent)
        { //Create button label
            GameObject labelGO
                = new GameObject($"{text}Label");
            RectTransform labelRTransform = labelGO.AddComponent<RectTransform>();
            labelRTransform.sizeDelta = new Vector2(parent.sizeDelta.x, 0);
            TMP_Text label = labelGO.AddComponent<TextMeshProUGUI>();
            label.fontSize = 10;
            label.alignment = TextAlignmentOptions.Center;
            label.text = $"<color=black>{text}</color>";

            labelGO.transform.SetParent(parent, false);

            return labelGO;
        }
    }
}
