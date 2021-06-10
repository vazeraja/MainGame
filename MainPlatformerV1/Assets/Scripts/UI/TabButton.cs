using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MainGame {
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
        public TabGroup tabGroup;
        public int buttonID;

        [HideInInspector] public Image background;

        private void Awake() {
            background = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData) => tabGroup.OnTabSelected(this);
        public void OnPointerEnter(PointerEventData eventData) => tabGroup.OnTabEnter(this);
        public void OnPointerExit(PointerEventData eventData) => tabGroup.OnTabExit(this);
        public void Select(Action action) => action?.Invoke();
        public void Deselect(Action action) => action?.Invoke();
    }
}