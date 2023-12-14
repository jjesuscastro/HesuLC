using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace HesuLC
{
    public class UIUtils
    {
        public static Toggle CreateToggle(GameObject parent, string name, string text)
        {
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 10, new Vector4(0, 5, 0, 5));
            Toggle toggle;
            Text label;
            GameObject GO = UIFactory.CreateToggle(gameObject, $"{name}Toggle", out toggle, out label);
            label.alignment = TextAnchor.MiddleLeft;
            
            label.text = $"\t{text}";
            toggle.isOn = false;

            UIFactory.SetLayoutElement(GO, minWidth: 200, minHeight: 30);

            return toggle;
        }

        public static ButtonRef CreateDropdownWithSubmit(GameObject parent, out Dropdown dropdown, string name, string label, System.Action<int> OnValueChanged)
        {
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 0, new Vector4(0, 5, 0, 5));

            GameObject dropdownGameObject = UIFactory.CreateDropdown(gameObject, $"{name}Dropdown", out dropdown, "", 12, OnValueChanged);
            ButtonRef button = UIFactory.CreateButton(gameObject, $"{name}Button", $"{label}");

            UIFactory.SetLayoutElement(dropdownGameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(button.GameObject, minWidth: 50, minHeight: 30);
            return button;
        }

        public static ButtonRef CreateButtonWithLabel(GameObject parent, string name, string text, string buttonLabel)
        {
            //Moon weather
            GameObject gameObject = UIFactory.CreateHorizontalGroup(parent, $"{name}", true, false, true, true, 0, new Vector4(0, 5, 0, 5));
            Text label = UIFactory.CreateLabel(gameObject, $"{name}Label", $"{text}");
            ButtonRef buttonRef = UIFactory.CreateButton(gameObject, $"{name}Button", $"{buttonLabel}");

            UIFactory.SetLayoutElement(label.gameObject, minWidth: 150, minHeight: 30, preferredWidth: 150);
            UIFactory.SetLayoutElement(buttonRef.GameObject, minWidth: 50, minHeight: 30);

            return buttonRef;
        }
    }
}
