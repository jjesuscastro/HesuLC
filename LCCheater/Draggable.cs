using BepInEx;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LethalCheater
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private GameObject draggableObject;

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            draggableObject.transform.position = UnityInput.Current.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }
    }
}
