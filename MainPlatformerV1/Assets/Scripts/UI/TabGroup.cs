using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainGame {
    public class TabGroup : MonoBehaviour {
        
        public List<TabButton> tabButtons;
        
        [HideInInspector] public TabButton selectedTab;
        [HideInInspector] public TabButton nextTab;
        [HideInInspector] public TabButton previousTab;

        public Color tabIdle = new Color(0.07f, 0.99f, 1f, 0.14f);
        public Color tabHover = new Color(0.22f, 0.44f, 1f);
        public Color tabActive = new Color(1f, 0.51f, 0.15f);
        
        public List<GameObject> pagesToSwap;


        public void OnTabRightButton() {
            if (selectedTab == null) return;
            
            if (selectedTab.buttonID == tabButtons.Count) {
                OnTabSelected(tabButtons.Find(x => x.buttonID == 1));
                return;
            }
            
            nextTab = tabButtons.Find(x => x.buttonID == selectedTab.buttonID + 1);
            OnTabSelected(nextTab);
        }
        public void OnTabLeftButton() {
            if (selectedTab == null) return;

            if (selectedTab.buttonID == 1) {
                OnTabSelected(tabButtons.Find(x => x.buttonID == tabButtons.Count));
                return;
            }
            
            previousTab = tabButtons.Find(x => x.buttonID == selectedTab.buttonID - 1);
            OnTabSelected(previousTab);
        }

        public void Subscribe(TabButton button) {
            tabButtons ??= new List<TabButton>();
            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button) {
            ResetTabs();
            if (selectedTab == null || button != selectedTab) 
                button.background.color = tabHover;
        }

        public void OnTabExit(TabButton button) {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button) {
            // if (selectedTab != null)
            //     selectedTab.Deselect(() => { Debug.Log("deselected action"); });
            
            selectedTab = button;
            //selectedTab.Select(() => { Debug.Log("selected action"); });

            ResetTabs();
            button.background.color = tabActive;
            int index = button.transform.GetSiblingIndex();
            for (var i = 0; i < pagesToSwap.Count; i++) {
                pagesToSwap[i].SetActive(i == index);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void ResetTabs() {
            foreach (var button in tabButtons.Where(button => selectedTab == null || button != selectedTab)) {
                button.background.color = tabIdle;
            }
        }
    }
}