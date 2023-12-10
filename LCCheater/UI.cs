using LethalCheater;
using LethalCheater.Patches;
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
            LethalCheaterBase.mls.LogWarning("F10 pressed: open");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (menuContainer == null)
            {
                LethalCheaterBase.mls.LogWarning("Menu not initialized, initializing...");
                GameObject canvas = GameObject.Find("Canvas");
                menuContainer = new GameObject("LCMenuContainer");
                RectTransform rTransform = menuContainer.AddComponent<RectTransform>();
                rTransform.anchoredPosition = new Vector2(200, 0);
                rTransform.sizeDelta = new Vector2(150, 300);
                VerticalLayoutGroup vLayoutGroup = menuContainer.AddComponent<VerticalLayoutGroup>();
                vLayoutGroup.padding = new RectOffset(15, 15, 15, 15);
                vLayoutGroup.childControlHeight = true;
                vLayoutGroup.childControlWidth = true;
                vLayoutGroup.spacing = 2;
                menuContainer.AddComponent<Image>().color = new Color(83/255f, 83 / 255f, 83 / 255f);
                menuContainer.transform.SetParent(canvas.transform, false);

                createLabel("Lethal Cheater", rTransform);

                createInputWithSubmit("Add Credits", rTransform);

                createButton("Scan", rTransform).GetComponent<Button>().onClick.AddListener(delegate
                {
                });

                createButton("Help", rTransform).GetComponent<Button>().onClick.AddListener(delegate
                {
                });
            }

            menuContainer.SetActive(true);
        }

        public static void closeMenu()
        {
            LethalCheaterBase.mls.LogWarning("F10 pressed: close");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            menuContainer.SetActive(false);
        }

        static GameObject createInputWithSubmit(string text, Transform parent)
        {
            GameObject textInputGroup = new GameObject($"{text}InputGroup");
            textInputGroup.transform.SetParent (parent);
            HorizontalLayoutGroup hLayoutGroup = textInputGroup.AddComponent<HorizontalLayoutGroup>();
            hLayoutGroup.padding = new RectOffset(0,0,0,0);
            hLayoutGroup.childControlHeight = true;
            hLayoutGroup.childControlWidth = true;
            hLayoutGroup.spacing = 2;

            RectTransform rTransform = textInputGroup.AddComponent<RectTransform>();
            GameObject textInput = new GameObject($"{text}Input");
            textInput.AddComponent<RectTransform>();
            textInput.AddComponent<TMP_InputField>();

            textInput.transform.SetParent(rTransform, false);

            createButton("Add Credits", rTransform);

            return textInputGroup;
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
